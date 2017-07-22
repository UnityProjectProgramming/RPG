
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

using System.Linq;

using System.Collections;
using System.Collections.Generic;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace MultiAssetRenamer
    {

        // =================================	
        // Classes.
        // =================================

        public class MultiAssetRenamer : EditorWindow
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // The paths of the selected assets.

            List<string> selectedAssetPaths = new List<string>();

            // Scrolling view position for selected assets.

            Vector2 scrollPosition;

            // Find and replace.

            string findString = "find";
            string replaceString = "replace";

            string addToStartString = "addToStart";
            string addToEndString = "addToEnd";

            int removeFromStartCount = 0;
            int removeFromEndCount = 0;

            // For labeling and tooltips.

            GUIContent guiContentLabel;

            // =================================	
            // Functions.
            // =================================

            // ...

            [MenuItem("Window/Mirza Beig/Multi-Asset Renamer")]
            static void showEditor()
            {
                EditorWindow.GetWindow<MultiAssetRenamer>(false, "Mirza Beig - Multi-Asset Renamer");
            }

            // ...

            void updateSelectedAssetPaths()
            {
                // Clear for updated selection.

                selectedAssetPaths.Clear();

                // Populate asset paths selection.

                for (int i = 0; i < Selection.assetGUIDs.Length; i++)
                {
                    selectedAssetPaths.Add(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]));
                }

                Repaint();
            }

            // ...

            void OnSelectionChange()
            {
                updateSelectedAssetPaths();
            }

            // ...

            string getAssetNameFromPath(string path)
            {
                // Get name from after folder path.

                int lastIndexOfFolderSlash = path.LastIndexOf('/') + 1;

                string assetName = path.Substring(
                    path.LastIndexOf('/') + 1, path.Length - lastIndexOfFolderSlash);

                // Get name from before asset type (*.prefab, *.png, etc...).
                // Folders also count as assets in Unity. Check if path has an extension.

                if (assetName.Contains('.'))
                {
                    assetName = assetName.Substring(0, assetName.LastIndexOf('.'));
                }

                return assetName;
            }

            // ...

            void OnGUI()
            {
                // Find and replace settings.

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("- Find & Replace Settings:", EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                guiContentLabel = new GUIContent("Find String", "Find string in selected assets.");
                findString = EditorGUILayout.TextField(guiContentLabel, findString);

                guiContentLabel = new GUIContent("Replace String", "Replace string in selected assets.");
                replaceString = EditorGUILayout.TextField(guiContentLabel, replaceString);

                EditorGUILayout.Separator();

                // Button to replace string in selected assets.

                guiContentLabel = new GUIContent("Find and Replace",
                    "Find and replace string in selected assets.");

                if (GUILayout.Button(guiContentLabel))
                {
                    for (int i = 0; i < selectedAssetPaths.Count; i++)
                    {
                        string assetName =
                            getAssetNameFromPath(selectedAssetPaths[i]);

                        if (assetName.Contains(findString))
                        {
                           AssetDatabase.RenameAsset(selectedAssetPaths[i], assetName.Replace(findString, replaceString));
                        }
                    }

                    updateSelectedAssetPaths();
                }

                // Add to start settings.

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("- Add to Start Settings:", EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                guiContentLabel = new GUIContent("Add to Start String", "Add to start.");
                addToStartString = EditorGUILayout.TextField(guiContentLabel, addToStartString);

                EditorGUILayout.Separator();

                // Button to add string in selected assets.

                guiContentLabel = new GUIContent("Add to Start",
                    "Add string to start in selected assets.");

                if (GUILayout.Button(guiContentLabel))
                {
                    for (int i = 0; i < selectedAssetPaths.Count; i++)
                    {
                        string assetName =
                            getAssetNameFromPath(selectedAssetPaths[i]);

                        AssetDatabase.RenameAsset(selectedAssetPaths[i], assetName.Insert(0, addToStartString));
                    }

                    updateSelectedAssetPaths();
                }

                // Add to end settings.

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("- Add to End Settings:", EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                guiContentLabel = new GUIContent("Add to End String", "Add to end.");
                addToEndString = EditorGUILayout.TextField(guiContentLabel, addToEndString);

                EditorGUILayout.Separator();

                // Button to add string in selected assets.

                guiContentLabel = new GUIContent("Add to End",
                    "Add string to end in selected assets.");

                if (GUILayout.Button(guiContentLabel))
                {
                    for (int i = 0; i < selectedAssetPaths.Count; i++)
                    {
                        string assetName =
                            getAssetNameFromPath(selectedAssetPaths[i]);

                        AssetDatabase.RenameAsset(selectedAssetPaths[i], assetName + addToEndString);
                    }

                    updateSelectedAssetPaths();
                }

                // Remove from Start settings.

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("- Remove from Start Settings:", EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                guiContentLabel = new GUIContent("Remove From Start Count", "Remove from Start.");
                removeFromStartCount = EditorGUILayout.IntField(guiContentLabel, removeFromStartCount);

                EditorGUILayout.Separator();

                // Button to remove string at end from selected assets.

                guiContentLabel = new GUIContent("Remove From Start",
                    "Remove these many characters from the start.");

                if (GUILayout.Button(guiContentLabel))
                {
                    for (int i = 0; i < selectedAssetPaths.Count; i++)
                    {
                        string assetName =
                            getAssetNameFromPath(selectedAssetPaths[i]);

                        AssetDatabase.RenameAsset(selectedAssetPaths[i], assetName.Substring(removeFromStartCount, assetName.Length - removeFromStartCount));
                    }

                    updateSelectedAssetPaths();
                }

                EditorGUILayout.Separator();

                // Remove from end settings (truncate).

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("- Remove from End Settings:", EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                guiContentLabel = new GUIContent("Remove From End Count", "Remove from end.");
                removeFromEndCount = EditorGUILayout.IntField(guiContentLabel, removeFromEndCount);

                EditorGUILayout.Separator();

                // Button to remove string at end from selected assets.

                guiContentLabel = new GUIContent("Remove From End",
                    "Remove these many characters from the end.");

                if (GUILayout.Button(guiContentLabel))
                {
                    for (int i = 0; i < selectedAssetPaths.Count; i++)
                    {
                        string assetName =
                            getAssetNameFromPath(selectedAssetPaths[i]);

                        AssetDatabase.RenameAsset(selectedAssetPaths[i], assetName.Substring(0, assetName.Length - removeFromEndCount));
                    }

                    updateSelectedAssetPaths();
                }

                EditorGUILayout.Separator();

                // Selected objects.

                guiContentLabel = new GUIContent("- Selected:",
                    "Valid folder and typed assets selected in the PROJECT view.");

                EditorGUILayout.LabelField(guiContentLabel, EditorStyles.boldLabel);
                EditorGUILayout.Separator();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                {
                    if (selectedAssetPaths.Count == 0)
                    {
                        EditorGUILayout.LabelField("Please select some asset(s) in the project view.", EditorStyles.miniBoldLabel);
                    }
                    else
                    {
                        for (int i = 0; i < selectedAssetPaths.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                //EditorGUILayout.LabelField(">   " + selectedAssetPaths[i], EditorStyles.miniLabel);
                                EditorGUILayout.LabelField(">   " + getAssetNameFromPath(selectedAssetPaths[i]), EditorStyles.miniLabel);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            // ...

            void OnFocus()
            {
                updateSelectedAssetPaths();
            }

            // =================================	
            // End functions.
            // =================================

        }

        // =================================	
        // End namespace.
        // =================================

    }

}

// =================================	
// --END-- //
// =================================
