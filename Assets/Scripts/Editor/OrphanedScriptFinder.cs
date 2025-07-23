using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Quest3VR.Editor
{
    /// <summary>
    /// Editor utility to find MonoBehaviour scripts that are not attached to any GameObjects in scenes
    /// </summary>
    public class OrphanedScriptFinder : EditorWindow
    {
        [MenuItem("Quest3VR/Find Orphaned MonoBehaviours")]
        public static void ShowWindow()
        {
            var window = GetWindow<OrphanedScriptFinder>("Orphaned Script Finder");
            window.Show();
        }
        
        private Vector2 scrollPosition;
        private List<OrphanedScriptInfo> orphanedScripts = new List<OrphanedScriptInfo>();
        private bool hasScanned = false;
        
        private class OrphanedScriptInfo
        {
            public string scriptName;
            public string scriptPath;
            public string guid;
            public bool isAttachedToScene;
            public System.Type scriptType;
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Orphaned MonoBehaviour Finder", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            GUILayout.Label("This tool finds MonoBehaviour scripts that are not attached to any GameObjects in your scenes.", EditorStyles.helpBox);
            GUILayout.Space(10);
            
            if (GUILayout.Button("Scan for Orphaned Scripts", GUILayout.Height(30)))
            {
                ScanForOrphanedScripts();
            }
            
            if (hasScanned)
            {
                GUILayout.Space(10);
                GUILayout.Label($"Found {orphanedScripts.Count} MonoBehaviour scripts", EditorStyles.boldLabel);
                
                var attachedScripts = orphanedScripts.Where(s => s.isAttachedToScene).ToList();
                var unattachedScripts = orphanedScripts.Where(s => !s.isAttachedToScene).ToList();
                
                GUILayout.Label($"✅ Attached to scenes: {attachedScripts.Count}", EditorStyles.label);
                GUILayout.Label($"⚠️ Not attached to scenes: {unattachedScripts.Count}", EditorStyles.label);
                
                GUILayout.Space(10);
                
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                
                if (unattachedScripts.Count > 0)
                {
                    GUILayout.Label("🔍 ORPHANED SCRIPTS (Not attached to any scene):", EditorStyles.boldLabel);
                    
                    foreach (var script in unattachedScripts)
                    {
                        GUILayout.BeginHorizontal("box");
                        
                        GUILayout.BeginVertical();
                        GUILayout.Label($"📄 {script.scriptName}", EditorStyles.boldLabel);
                        GUILayout.Label($"Path: {script.scriptPath}", EditorStyles.miniLabel);
                        GUILayout.Label($"GUID: {script.guid}", EditorStyles.miniLabel);
                        GUILayout.EndVertical();
                        
                        if (GUILayout.Button("Select", GUILayout.Width(60)))
                        {
                            var scriptAsset = AssetDatabase.LoadAssetAtPath<MonoScript>(script.scriptPath);
                            if (scriptAsset != null)
                            {
                                Selection.activeObject = scriptAsset;
                                EditorGUIUtility.PingObject(scriptAsset);
                            }
                        }
                        
                        GUILayout.EndHorizontal();
                        GUILayout.Space(5);
                    }
                }
                
                if (attachedScripts.Count > 0)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("✅ ATTACHED SCRIPTS (Found in scenes):", EditorStyles.boldLabel);
                    
                    foreach (var script in attachedScripts)
                    {
                        GUILayout.BeginHorizontal("box");
                        
                        GUILayout.BeginVertical();
                        GUILayout.Label($"📄 {script.scriptName}", EditorStyles.label);
                        GUILayout.Label($"Path: {script.scriptPath}", EditorStyles.miniLabel);
                        GUILayout.EndVertical();
                        
                        if (GUILayout.Button("Select", GUILayout.Width(60)))
                        {
                            var scriptAsset = AssetDatabase.LoadAssetAtPath<MonoScript>(script.scriptPath);
                            if (scriptAsset != null)
                            {
                                Selection.activeObject = scriptAsset;
                                EditorGUIUtility.PingObject(scriptAsset);
                            }
                        }
                        
                        GUILayout.EndHorizontal();
                        GUILayout.Space(2);
                    }
                }
                
                GUILayout.EndScrollView();
            }
        }
        
        private void ScanForOrphanedScripts()
        {
            orphanedScripts.Clear();
            
            Debug.Log("=== Scanning for Orphaned MonoBehaviour Scripts ===");
            
            // Get all script files
            string[] scriptPaths = AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets/Scripts" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.EndsWith(".cs"))
                .ToArray();
            
            Debug.Log($"Found {scriptPaths.Length} script files to analyze");
            
            // Get all scene files
            string[] scenePaths = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
            
            Debug.Log($"Found {scenePaths.Length} scene files to check");
            
            // Get all script GUIDs used in scenes
            HashSet<string> usedScriptGUIDs = new HashSet<string>();
            
            foreach (string scenePath in scenePaths)
            {
                Debug.Log($"Checking scene: {scenePath}");
                string sceneContent = File.ReadAllText(scenePath);
                
                // Find all script references in scene files
                var matches = System.Text.RegularExpressions.Regex.Matches(
                    sceneContent, 
                    @"m_Script:\s*\{fileID:\s*11500000,\s*guid:\s*([a-f0-9]{32}),\s*type:\s*3\}"
                );
                
                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    string guid = match.Groups[1].Value;
                    usedScriptGUIDs.Add(guid);
                    Debug.Log($"  Found script GUID in scene: {guid}");
                }
            }
            
            Debug.Log($"Total unique script GUIDs found in scenes: {usedScriptGUIDs.Count}");
            
            // Analyze each script
            foreach (string scriptPath in scriptPaths)
            {
                var monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
                if (monoScript == null) continue;
                
                var scriptClass = monoScript.GetClass();
                if (scriptClass == null) continue;
                
                // Check if it's a MonoBehaviour
                if (!typeof(MonoBehaviour).IsAssignableFrom(scriptClass)) continue;
                
                // Skip Editor scripts
                if (scriptPath.Contains("/Editor/")) continue;
                
                string scriptGUID = AssetDatabase.AssetPathToGUID(scriptPath);
                bool isAttached = usedScriptGUIDs.Contains(scriptGUID);
                
                var scriptInfo = new OrphanedScriptInfo
                {
                    scriptName = scriptClass.Name,
                    scriptPath = scriptPath,
                    guid = scriptGUID,
                    isAttachedToScene = isAttached,
                    scriptType = scriptClass
                };
                
                orphanedScripts.Add(scriptInfo);
                
                Debug.Log($"Script: {scriptInfo.scriptName} | GUID: {scriptGUID} | Attached: {isAttached}");
            }
            
            hasScanned = true;
            
            var orphaned = orphanedScripts.Where(s => !s.isAttachedToScene).ToList();
            Debug.Log($"=== Scan Complete ===");
            Debug.Log($"Total MonoBehaviour scripts: {orphanedScripts.Count}");
            Debug.Log($"Attached to scenes: {orphanedScripts.Count - orphaned.Count}");
            Debug.Log($"Orphaned scripts: {orphaned.Count}");
            
            if (orphaned.Count > 0)
            {
                Debug.LogWarning("⚠️ Orphaned MonoBehaviour scripts found:");
                foreach (var script in orphaned)
                {
                    Debug.LogWarning($"  - {script.scriptName} ({script.scriptPath})");
                }
            }
            else
            {
                Debug.Log("✅ All MonoBehaviour scripts are attached to scenes!");
            }
        }
    }
}
