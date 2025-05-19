using UnityEditor;
using UnityEngine;
using System.Collections.Generic;



public class MissingReferenceFinder : EditorWindow
{
    private class ReferenceResult
    {
        public string message;
        public GameObject gameObject;
        public ReferenceResult(string message, GameObject gameObject)
        {
            this.message = message;
            this.gameObject = gameObject;
        }
    }

    private bool showMissingScripts = true;
    private bool showBrokenRefs = true;
    private Dictionary<GameObject, List<string>> groupedResults = new();
    private Dictionary<GameObject, bool> foldouts = new();

    private List<ReferenceResult> results = new();
    private int missingScriptCount = 0;
    private int nullRefCount = 0;

    [MenuItem("Tools/Missing Reference Finder")]
    public static void ShowWindow()
    {
        GetWindow<MissingReferenceFinder>("Missing Reference Finder");
    }

    private void OnGUI()
    {
        GUILayout.Label(" Missing Reference Scanner", EditorStyles.boldLabel);

        GUILayout.Space(5);

        if (GUILayout.Button(" Scan Scene", GUILayout.Height(30)))
        {
            ScanScene();
        }

        GUILayout.Space(10);
        GUILayout.Label(" Results Summary", EditorStyles.boldLabel);
        GUILayout.Label($" Missing Scripts: {missingScriptCount}");
        GUILayout.Label($" Broken Object References: {nullRefCount}");

        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        showMissingScripts = EditorGUILayout.ToggleLeft("Show Missing Scripts", showMissingScripts);
        showBrokenRefs = EditorGUILayout.ToggleLeft("Show Broken References", showBrokenRefs);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        EditorGUILayout.LabelField(" Click on a result to select the GameObject", EditorStyles.helpBox);

        GUILayout.Space(5);

        if (groupedResults.Count == 0)
        {
            EditorGUILayout.HelpBox("No issues found. Hooray! ", MessageType.Info);
            return;
        }

        // Results grouped by GameObject
        foreach (var kvp in groupedResults)
        {
            GameObject go = kvp.Key;
            List<string> issues = kvp.Value;

            // Check if any of the issues match filters
            bool showGroup = false;
            foreach (string issue in issues)
            {
                if ((issue.Contains("[Missing Script]") && showMissingScripts) ||
                    (issue.Contains("[Broken Ref]") && showBrokenRefs))
                {
                    showGroup = true;
                    break;
                }
            }

            if (!showGroup) continue;

            // Foldout UI
            if (!foldouts.ContainsKey(go))
                foldouts[go] = false;

            EditorGUILayout.BeginVertical("box");
            foldouts[go] = EditorGUILayout.Foldout(foldouts[go], go.name, true);

            if (foldouts[go])
            {
                EditorGUI.indentLevel++;
                foreach (string issue in issues)
                {
                    if ((issue.Contains("[Missing Script]") && !showMissingScripts) ||
                        (issue.Contains("[Broken Ref]") && !showBrokenRefs))
                        continue;

                    if (GUILayout.Button("• " + issue, EditorStyles.linkLabel))
                    {
                        Selection.activeGameObject = go;
                        EditorGUIUtility.PingObject(go);
                    }
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(3);
        }
    }


    private void ScanScene()
    {
        // Reset results
        groupedResults.Clear();
        foldouts.Clear();
        missingScriptCount = 0;
        nullRefCount = 0;

        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                //  MISSING SCRIPT CHECK
                if (components[i] == null)
                {
                    string issue = "[Missing Script]";

                    if (!groupedResults.ContainsKey(go))
                        groupedResults[go] = new List<string>();

                    groupedResults[go].Add(issue);
                    missingScriptCount++;

                    continue;
                }

                //  BROKEN REFERENCE CHECK
                SerializedObject so = new SerializedObject(components[i]);
                SerializedProperty prop = so.GetIterator();

                while (prop.NextVisible(true))
                {
                    if (prop.propertyType == SerializedPropertyType.ObjectReference &&
                        prop.objectReferenceValue == null &&
                        prop.objectReferenceInstanceIDValue != 0)
                    {
                        string issue = $"[Broken Ref] Component: {components[i].GetType().Name} | Field: {prop.displayName}";

                        if (!groupedResults.ContainsKey(go))
                            groupedResults[go] = new List<string>();

                        groupedResults[go].Add(issue);
                        nullRefCount++;
                    }
                }
            }
        }

        Debug.Log($"Reference Scan Complete — Missing Scripts: {missingScriptCount}, Broken References: {nullRefCount}");
    }


}
