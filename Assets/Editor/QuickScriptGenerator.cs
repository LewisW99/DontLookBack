using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class QuickScriptGenerator : EditorWindow
{
    private string scriptName = "NewScript";
    private enum ScriptType { MonoBehaviour, ScriptableObject, Singleton }
    private ScriptType selectedType = ScriptType.MonoBehaviour;
    private bool useNamespace = false;

    private string targetFolder = "Assets";

    private bool showInterfaces = false;

    private Dictionary<string, bool> interfaceOptions = new()
{
    { "IDisposable", false },
    { "IComparable", false },
    { "UnityEngine.EventSystems.IPointerClickHandler", false },
    { "System.IEquatable", false },
    { "IInteractable", false }

};


    [MenuItem("Tools/Quick Script Generator")]
    public static void ShowWindow()
    {
        GetWindow<QuickScriptGenerator>("Script Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Quick Script Generator", EditorStyles.boldLabel);

        scriptName = EditorGUILayout.TextField("Script Name", scriptName);
        selectedType = (ScriptType)EditorGUILayout.EnumPopup("Script Type", selectedType);
        useNamespace = EditorGUILayout.Toggle("Use Namespace", useNamespace);

        GUILayout.Space(10);
        GUILayout.Space(5);
        EditorGUILayout.BeginVertical("box");
        showInterfaces = EditorGUILayout.Foldout(showInterfaces, "Implement Interfaces", true);
        if (showInterfaces)
        {
            EditorGUI.indentLevel++;
            foreach (var key in interfaceOptions.Keys.ToList())
            {
                interfaceOptions[key] = EditorGUILayout.ToggleLeft(key, interfaceOptions[key]);
            }
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);


        GUILayout.Label("Target Folder", EditorStyles.label);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField(targetFolder); // read-only display
        if (GUILayout.Button("Browse", GUILayout.MaxWidth(80)))
        {
            string selected = EditorUtility.OpenFolderPanel("Select Script Folder", Application.dataPath, "");
            if (!string.IsNullOrEmpty(selected))
            {
                if (selected.StartsWith(Application.dataPath))
                {
                    targetFolder = "Assets" + selected.Substring(Application.dataPath.Length);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Folder", "Folder must be inside the project Assets folder.", "OK");
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Script"))
        {
            GenerateScript();
        }
    }

    private void GenerateScript()
    {
        if (string.IsNullOrWhiteSpace(scriptName))
        {
            EditorUtility.DisplayDialog("Invalid Name", "Please enter a valid script name.", "OK");
            return;
        }

        string folderPath = targetFolder;
        string path = Path.Combine(folderPath, $"{scriptName}.cs");

        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("File Exists", "A script with that name already exists. Overwrite?", "Yes", "No"))
                return;
        }

        string content = GenerateScriptContent(scriptName, selectedType, useNamespace);
        File.WriteAllText(path, content);
        AssetDatabase.Refresh();
        EditorUtility.RevealInFinder(path);
    }

    private string GetSelectedFolderPath()
    {
        string path = "Assets";
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            string tempPath = AssetDatabase.GetAssetPath(obj);
            if (Directory.Exists(tempPath))
            {
                path = tempPath;
                break;
            }
        }
        return path;
    }

    private string GenerateScriptContent(string className, ScriptType type, bool includeNamespace)
    {
        // Get selected interfaces
        var selectedInterfaces = interfaceOptions
            .Where(i => i.Value)
            .Select(i => i.Key)
            .ToList();

        bool useBaseInteractable = selectedInterfaces.Contains("IInteractable");

        string nsOpen = includeNamespace ? "namespace YourNamespace\n{\n" : "";
        string nsClose = includeNamespace ? "\n}" : "";

        // Determine base class
        string baseClass = useBaseInteractable ? "BaseInteractable" :
            type switch
        {
            ScriptType.MonoBehaviour => "MonoBehaviour",
            ScriptType.ScriptableObject => "ScriptableObject",
            ScriptType.Singleton => className, // custom handling below
            _ => "MonoBehaviour"
        };

        // Build class declaration line with interfaces
        string inheritance = (type != ScriptType.Singleton)
            ? " : " + baseClass + (selectedInterfaces.Count > 0 ? ", " + string.Join(", ", selectedInterfaces) : "")
            : ""; // Singleton is defined differently

        // Add stub methods for selected interfaces
        string stubMethods = "";
        foreach (string iface in selectedInterfaces)
        {
            if (iface.Contains("IDisposable"))
                stubMethods += "\n    public void Dispose() { /* cleanup */ }\n";
            if (iface.Contains("IComparable"))
                stubMethods += "\n    public int CompareTo(object obj) { return 0; }\n";
            if (iface.Contains("IPointerClickHandler"))
                stubMethods += "\n    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData) { }\n";
            if (iface.Contains("IEquatable"))
                stubMethods += $"\n    public bool Equals({className} other) {{ return false; }}\n";
            if (iface == "IInteractable")
                stubMethods += "\n    public void Interact() { Debug.Log(\"Interacted.\"); }\n";
        }

        // Class body templates
        string body = type switch
        {
            ScriptType.Singleton => $@"public class {className} : MonoBehaviour
{{
    public static {className} Instance {{ get; private set; }}

    private void Awake()
    {{
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }}{stubMethods}
}}",

            _ => $@"public class {className}{inheritance}
{{
    private void Start()
    {{
        // TODO: Start logic
    }}

    private void Update()
    {{
        // TODO: Update logic
    }}{stubMethods}
}}"
        };

        return
    $@"using UnityEngine;
using System;
using UnityEngine.EventSystems;

{nsOpen}{body}{nsClose}";
    }

}
