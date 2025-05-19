using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

public class MissingComponentFinder : EditorWindow
{
    private string componentTypeName = "Collider";
    private List<GameObject> missingObjects = new();
    private bool showResults = false;

    private string componentSearch = "";
    private string selectedComponent = "";
    private string selectedTag = "Untagged";
    private string[] allTags;
    private Vector2 scroll;

    private List<string> cachedComponentTypes;

    [MenuItem("Tools/Missing Component Finder")]
    public static void ShowWindow()
    {
        GetWindow<MissingComponentFinder>("Missing Component Finder");
    }


    private void OnEnable()
    {
        allTags = UnityEditorInternal.InternalEditorUtility.tags;

        cachedComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract)
            .Select(t => t.FullName)
            .Distinct()
            .OrderBy(n => n)
            .ToList();
    }


    private void OnGUI()
    {
        GUILayout.Label("Find GameObjects Missing a Specific Component", EditorStyles.boldLabel);

        selectedTag = EditorGUILayout.TagField("Tag Filter", selectedTag);

        componentSearch = EditorGUILayout.TextField("Component", componentSearch);


        if (!string.IsNullOrEmpty(componentSearch))
        {
            var matches = cachedComponentTypes
                .FindAll(name => name.IndexOf(componentSearch, StringComparison.OrdinalIgnoreCase) >= 0);

            if (matches.Count > 0)
            {
                EditorGUILayout.LabelField("Suggestions:");
                foreach (var match in matches)
                {
                    if (GUILayout.Button(match, EditorStyles.miniButton))
                    {
                        selectedComponent = match;
                        componentSearch = match;
                        GUI.FocusControl(null); // Unfocus text field
                    }
                }
            }
        }

        if (GUILayout.Button("Scan Scene"))
        {
            ScanForMissingComponents(selectedComponent, selectedTag);
        }

        if (showResults)
        {
            GUILayout.Space(10);
            GUILayout.Label($"Found {missingObjects.Count} objects missing '{selectedComponent}' (Tag: {selectedTag})", EditorStyles.boldLabel);

            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
            foreach (var go in missingObjects)
            {
                if (GUILayout.Button(go.name, EditorStyles.linkLabel))
                {
                    Selection.activeGameObject = go;
                    EditorGUIUtility.PingObject(go);
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }

 

    private void ScanForMissingComponents(string componentName, string tagFilter)
    {
        missingObjects.Clear();
        showResults = false;

        if (string.IsNullOrEmpty(componentName))
        {
            EditorUtility.DisplayDialog("Invalid Component", "Please select a component.", "OK");
            return;
        }

        Type componentType = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .FirstOrDefault(t => t.FullName == componentName || t.Name == componentName);

        if (componentType == null || !componentType.IsSubclassOf(typeof(Component)))
        {
            EditorUtility.DisplayDialog("Invalid Type", $"'{componentName}' is not a valid Component type.", "OK");
            return;
        }

        var allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (var go in allObjects)
        {
            if ((tagFilter == "Untagged" || go.CompareTag(tagFilter)) &&
                go.GetComponent(componentType) == null)
            {
                missingObjects.Add(go);
            }
        }

        showResults = true;
        Debug.Log($"Scan complete � {missingObjects.Count} GameObjects missing {componentName} with tag '{tagFilter}'");
    }
}
