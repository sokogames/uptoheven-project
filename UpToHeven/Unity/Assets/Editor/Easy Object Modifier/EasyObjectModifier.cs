using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Assets.Editor
{
    public class EasyObjectModifier : EditorWindow
    {
        public GameObject[] ObjectsToReplaceTable = null; // for grouped objects
        public GameObject ObjectToReplace = null;

        private List<Transform> selectedObjectTransforms = new List<Transform>();
        private List<GameObject> selectedGameObjects = new List<GameObject>();
        private List<Object> selectedFoldersFromProject = new List<Object>();
        private List<string> selectedFoldersPathTable = new List<string>();
        private List<string> tagTable = new List<string>();
        private Dictionary<string, int> objectInfoTable = new Dictionary<string, int>();
        private Dictionary<int, List<GameObject>> groupedObjectsTable = new Dictionary<int, List<GameObject>>();
        private List<string> labelTable = new List<string>();
        private bool[] showFoldoutTable = null;
        private bool[] resetTransformTable = null;
        private bool[] maintainTransformTable = null;
        private bool[] maintainHierarchyTable = null;
        private bool[] changePosTable = null;
        private bool[] changeRotTable = null;
        private bool[] changeScaleTable = null;
        private bool[] dialogForNoObjectDisplayed = null;
        private bool permissionToModifyGiven = false;
        private Vector3[] offsetPosTable = null;
        private Vector3[] offsetRotTable = null;
        private Vector3[] offsetScaleTable = null;
        private string[] changedTagTable = null;
        private List<int> modifiedObjectIndices = new List<int>();

        private Vector2 scrollView = Vector2.zero;
        private string message = "";

        private int numberOfItemsToGroup;
        private static EditorWindow window;

        Vector3 offsetPos = Vector3.zero;
        Vector3 offsetRot = Vector3.zero;
        Vector3 offsetScale = Vector3.zero;
        private string searchString = "";
        private string[] searchByOptions = new string[] { "Tag", "Name" };
        private string[] searchFromOptions = new string[] { "Scene", "Project" };
        private int toSearchBy = 0;
        private int toSearchFrom = 0;
        private bool selectedProjectSearch = false;
        private bool showLabel = false;
        private bool searchInvoked = false;
        private bool objectsGrouped = false;
        private bool hasReplacementPrefab = true;
        private bool foldersFromProjectAdded = false;
        private bool maintainTransforms = true;
        private bool continueWithChanges = false;
        private int currentGroupCount = 0;
        private int numberOfObjectsModified = 0;
        private int originalObjectsToReplaceCount = 0;

        [MenuItem("GameObject/Easy Object Modifier %E", false, 102)]
        static void Init()
        {
            window = GetWindow(typeof(EasyObjectModifier), false, "Object Modifier");
            window.position = new Rect(100, 100, 700, 500);
            window.minSize = new Vector2(600f, 400f);
            window.Show();
            Selection.activeObject = null;
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void GetObjectTransforms()
        {
            //selectedObjectTransforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab).Where(x => x.parent.CompareTag("Test")).ToList();
            selectedObjectTransforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab).ToList();
            message = selectedObjectTransforms.Count + " objects selected.";
            Repaint();
        }

        void OnSelectionChange()
        {
            // selection for scene/hierarchy search/group.
            if (!showLabel && !searchInvoked && !selectedProjectSearch)
                GetObjectTransforms();

            // selection for Asset search/group.
            if (selectedProjectSearch && !searchInvoked && !objectsGrouped)
                selectedFoldersFromProject = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel | SelectionMode.DeepAssets | SelectionMode.ExcludePrefab).ToList();
        }

        void ClearSelection(bool clearSelectedObjects = true, bool fromProject = false)
        {
            if (clearSelectedObjects)
                selectedObjectTransforms.Clear();

            tagTable.Clear();
            numberOfItemsToGroup = 0;
            modifiedObjectIndices.Clear();
            objectInfoTable.Clear();
            groupedObjectsTable.Clear();
            labelTable.Clear();
            message = "";

            if (!fromProject)
            {
                selectedProjectSearch = false;
                searchInvoked = false;
                objectsGrouped = false;
                toSearchBy = toSearchFrom = 0;
                searchString = "";
            }
        }

        void AddItemsToTables(Transform item)
        {
            if (!tagTable.Contains(item.tag))
            {
                tagTable.Add(item.tag);
                int tempIndex = tagTable.FindIndex(x => x == item.tag);
                objectInfoTable.Add(item.tag, 1);
                groupedObjectsTable.Add(tempIndex, new List<GameObject>());
                groupedObjectsTable[tempIndex].Add(item.gameObject);
            }
            else
            {
                int tempIndex = tagTable.FindIndex(x => x == item.tag);
                groupedObjectsTable[tempIndex].Add(item.gameObject);
                objectInfoTable[item.tag] += 1;
            }
        }

        void ExtractIntoGroups(bool groupUntaggedItems = false, bool fromProject = false, bool groupChildren = false)
        {
            if (fromProject)
                ClearSelection(false, true);
            else
                ClearSelection(false);

            // add tags to the Tag table, tags and index to the Object info dictionary, and index and list of gameobjects to the Grouped objects dictionary.
            if (selectedObjectTransforms.Count > 0)
            {
                foreach (var item in selectedObjectTransforms)
                {
                    if ((!groupUntaggedItems && item.tag != "Untagged") || groupUntaggedItems)
                    {
                        AddItemsToTables(item);

                        if (groupChildren)
                        {
                            if (item.childCount > 0)
                            {
                                for (int i = 0; i < item.childCount; i++)
                                    AddItemsToTables(item.GetChild(i));
                            }
                        }
                    }
                }

                numberOfItemsToGroup = tagTable.Count;
                ObjectsToReplaceTable = new GameObject[numberOfItemsToGroup];
                originalObjectsToReplaceCount = ObjectsToReplaceTable.Length;
                showFoldoutTable = new bool[numberOfItemsToGroup];
                dialogForNoObjectDisplayed = new bool[numberOfItemsToGroup];

                // instantiate and ready up the different tables
                offsetPosTable = new Vector3[numberOfItemsToGroup];
                offsetRotTable = new Vector3[numberOfItemsToGroup];
                offsetScaleTable = new Vector3[numberOfItemsToGroup];

                resetTransformTable = new bool[numberOfItemsToGroup];
                resetTransformTable.PopulateTable(false);

                maintainHierarchyTable = new bool[numberOfItemsToGroup];
                maintainHierarchyTable.PopulateTable(true);

                maintainTransformTable = new bool[numberOfItemsToGroup];
                changePosTable = new bool[numberOfItemsToGroup];
                changeRotTable = new bool[numberOfItemsToGroup];
                changeScaleTable = new bool[numberOfItemsToGroup];
                maintainTransformTable.PopulateTable(true);
                changePosTable.PopulateTable(false);
                changeRotTable.PopulateTable(false);
                changeScaleTable.PopulateTable(false);

                changedTagTable = new string[numberOfItemsToGroup];

                searchInvoked = false;
                objectsGrouped = true;
            }


            // add labels to the table that get displayed next to the object field, after the objects are grouped in the editor.
            foreach (var item in objectInfoTable)
                labelTable.Add(item.Value + " item(s) with tag " + item.Key);

            showLabel = true;
        }

        void InitChanges()
        {
            if (groupedObjectsTable.Count > 0)
            {
                if (!permissionToModifyGiven)
                {
                    string message = "";

                    if (toSearchFrom == 0)
                        message = "Are you sure you want to proceed with the modifications on these objects?";
                    else
                        message = "Are you sure you want to proceed with the modifications on these assets from your Assets folder?";

                    if (EditorUtility.DisplayDialog("Need permission", message, "Yes", "No"))
                        permissionToModifyGiven = true;
                    else
                        return;
                }

                int progressCount = 0;

                foreach (var item in groupedObjectsTable)
                {
                    currentGroupCount = item.Value.Count;

                    if (!item.Value.Contains(null))
                    {
                        foreach (var obj in item.Value)
                        {
                            if (modifiedObjectIndices.Count > 0 && modifiedObjectIndices.Contains(item.Key))
                                continue;

                            EditorUtility.DisplayProgressBar("Processing...", "Working on " + obj.name, (progressCount / (float)item.Value.Count));
                            MakeChange(obj.transform, item.Key);
                            ++progressCount;
                        }
                    }
                }

                EditorUtility.ClearProgressBar();

                if (currentGroupCount != -1)
                {
                    string message = "";

                    if (toSearchFrom == 1)
                        message = " object(s) from the project panel were successfully modified.";
                    else
                        message = " object(s) from the hierarchy were successfully modified.";

                    ShowNotification(new GUIContent(numberOfObjectsModified + message));

                    if (EditorUtility.DisplayDialog("Replacement complete", numberOfObjectsModified + message, "Ok"))
                    {
                        if (ObjectsToReplaceTable.Length > 0)
                        {
                            for (int i = 0; i < ObjectsToReplaceTable.Length; i++)
                            {
                                if (ObjectsToReplaceTable[i] != null)
                                {
                                    ObjectsToReplaceTable[i] = null;
                                    modifiedObjectIndices.Add(i);
                                }
                                else
                                {
                                    ObjectsToReplaceTable[i] = null;
                                }
                            }

                            if (modifiedObjectIndices.Count == ObjectsToReplaceTable.Length)
                            {
                                modifiedObjectIndices.Clear();
                                resetTransformTable.ToList().Clear();
                                maintainTransformTable.ToList().Clear();
                                changePosTable.ToList().Clear();
                                changeRotTable.ToList().Clear();
                                changeScaleTable.ToList().Clear();
                                maintainHierarchyTable.ToList().Clear();
                                offsetPosTable.ToList().Clear();
                                offsetRotTable.ToList().Clear();
                                offsetScaleTable.ToList().Clear();
                                showLabel = false;
                                ClearSelection();
                                return;
                            }
                        }

                        for (int i = 0; i < dialogForNoObjectDisplayed.Length; i++)
                            dialogForNoObjectDisplayed[i] = false;

                        numberOfObjectsModified = 0;

                        return;
                    }
                }
            }
        }

        void SearchThroughChildren(GameObject loadedObject, ref int count)
        {
            if (loadedObject.transform.childCount > 0)
            {
                List<Transform> loadedObjectChildren = new List<Transform>();

                for (int i = 0; i < loadedObject.transform.childCount; i++)
                    loadedObjectChildren.Add(loadedObject.transform.GetChild(i));

                if (loadedObjectChildren.Count > 0)
                {
                    foreach (var child in loadedObjectChildren)
                    {
                        bool addChild = false;

                        if (toSearchBy == 0)
                        {
                            if (child.CompareTag(searchString) && !selectedGameObjects.Contains(child.gameObject))
                                addChild = true;
                        }
                        else
                        {
                            if (child.name == searchString && !selectedGameObjects.Contains(child.gameObject))
                                addChild = true;
                        }

                        if (addChild)
                        {
                            selectedGameObjects.Add(child.gameObject);
                            selectedObjectTransforms.Add(child);
                            ++count;
                        }
                    }
                }
            }
        }

        void MakeChange(Transform item, int index)
        {
            if (currentGroupCount >= 0)
            {
                if (ObjectsToReplaceTable[index] == null || toSearchFrom == 1)
                {
                    if (toSearchFrom == 0)
                    {
                        if (message != "")
                            message += "\nNo replacement object selected for " + objectInfoTable.Keys.ToList()[index];
                        else
                            message = "No replacement object selected for " + objectInfoTable.Keys.ToList()[index];

                        hasReplacementPrefab = false;

                        if (!continueWithChanges)
                        {
                            if (!dialogForNoObjectDisplayed[index])
                            {
                                if (!EditorUtility.DisplayDialog("Replacement object", "No replacement object selected for " + objectInfoTable.Keys.ToList()[index] + ".\nWould you like to continue with the other changes(if any)?", "Yes", "No"))
                                {
                                    //currentGroupCount = -1;
                                    dialogForNoObjectDisplayed[index] = true;

                                    if (currentGroupCount > 0)
                                        currentGroupCount--;

                                    if (currentGroupCount == 0)
                                        currentGroupCount = 0;

                                    return;
                                }
                                else
                                {
                                    continueWithChanges = true;
                                }
                            }
                            else
                            {
                                continueWithChanges = true;
                            }
                        }
                    }

                    if (continueWithChanges || toSearchFrom == 1)
                    {
                        if (changedTagTable[index] != null)
                        {
                            Undo.RecordObject(item.gameObject, "changes to tag");
                            item.tag = changedTagTable[index];
                            EditorUtility.SetDirty(item);
                        }

                        Undo.RecordObject(item.transform, "object modifications");
                        if (resetTransformTable[index])
                        {
                            item.transform.position = Vector3.zero;
                            item.transform.rotation = Quaternion.identity;
                            item.transform.localScale = Vector3.one;
                            EditorUtility.SetDirty(item.transform);
                        }

                        if (!maintainTransformTable[index])
                        {
                            item.transform.position += offsetPosTable[index];
                            item.transform.localEulerAngles += offsetRotTable[index];
                            item.transform.localScale += offsetScaleTable[index];
                            EditorUtility.SetDirty(item.transform);
                        }

                        numberOfObjectsModified++;
                        permissionToModifyGiven = false;

                        if (toSearchFrom == 0)
                        {
                            if (currentGroupCount > 0)
                                currentGroupCount--;

                            if (currentGroupCount == 0)
                                continueWithChanges = false;

                            return;
                        }
                    }
                }

                if (toSearchFrom == 0)
                {
                    GameObject newGameObject = PrefabUtility.InstantiatePrefab(ObjectsToReplaceTable[index]) as GameObject;

                    if (maintainHierarchyTable[index])
                        newGameObject.transform.parent = item.parent;

                    if (changedTagTable[index] != null)
                    {
                        Undo.RecordObject(newGameObject, "changes to tag");
                        newGameObject.tag = changedTagTable[index];
                        EditorUtility.SetDirty(newGameObject);
                    }

                    Undo.RecordObject(newGameObject.transform, "object modifications");
                    if (resetTransformTable[index])
                    {
                        newGameObject.transform.position = Vector3.zero;
                        newGameObject.transform.rotation = Quaternion.identity;
                        newGameObject.transform.localScale = Vector3.one;
                        EditorUtility.SetDirty(newGameObject.transform);
                    }

                    if (!maintainTransformTable[index])
                    {
                        newGameObject.transform.position += offsetPosTable[index];
                        newGameObject.transform.localEulerAngles += offsetRotTable[index];
                        newGameObject.transform.localScale += offsetScaleTable[index];
                        EditorUtility.SetDirty(newGameObject.transform);
                    }
                    else
                    {
                        if (!resetTransformTable[index])
                        {
                            newGameObject.transform.position = item.position;
                            newGameObject.transform.localEulerAngles = item.localEulerAngles;
                            newGameObject.transform.localScale = item.localScale;
                            EditorUtility.SetDirty(newGameObject.transform);
                        }
                    }

                    Undo.RegisterCreatedObjectUndo(newGameObject, "Replaced object");
                    Undo.DestroyObjectImmediate(item.gameObject);

                    hasReplacementPrefab = true;
                    //ObjectsToReplaceTable[index] = null;

                    if (currentGroupCount > 0)
                        currentGroupCount--;

                    if (currentGroupCount == 0)
                        currentGroupCount = 0;

                    numberOfObjectsModified++;
                    permissionToModifyGiven = false;
                }
            }
        }

        void OnGUI()
        {
            #region Search bar
            GUILayout.Space(10f);
            if (showLabel)
                GUI.enabled = false;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Search using", EditorStyles.boldLabel);
                GUILayout.Space(-110f);
                toSearchBy = EditorGUILayout.Popup(toSearchBy, searchByOptions);
                GUILayout.Space(-2f);
                EditorGUILayout.LabelField("from", EditorStyles.boldLabel);
                GUILayout.Space(-170f);
                toSearchFrom = EditorGUILayout.Popup(toSearchFrom, searchFromOptions);

                if (toSearchFrom == 1 && !selectedProjectSearch && /*!searchInvoked && !objectsGrouped*/ !foldersFromProjectAdded)
                {
                    if (EditorUtility.DisplayDialog("Select Folder(s)", "Select one or more folders from the Project Panel to search from.", "Select Folders"))
                        message = "Select one or more folders from the Project Panel to search from.";

                    Selection.activeObject = null;
                    showLabel = false;
                    searchInvoked = false;
                    selectedProjectSearch = true;
                    selectedFoldersFromProject.Clear();
                    selectedGameObjects.ToList().Clear();
                    selectedObjectTransforms.Clear();
                    objectsGrouped = false;
                }
                else if (toSearchFrom == 0 && selectedProjectSearch)
                {
                    message = "No Object Selected";
                    Selection.activeObject = null;
                    selectedProjectSearch = false;
                    selectedFoldersFromProject.Clear();
                    ClearSelection(true);
                }

                if (selectedProjectSearch)
                {
                    string buttonString = foldersFromProjectAdded == true ? "Reset" : "Add " + selectedFoldersFromProject.Count + " items.";

                    if (!showLabel)
                        GUI.enabled = true;
                    if (selectedFoldersFromProject.Count < 1)
                    {
                        buttonString = "Select folders";
                        GUI.enabled = false;
                    }

                    if (GUILayout.Button(buttonString))
                    {
                        if (selectedFoldersFromProject.Count < 1)
                        {
                            if (EditorUtility.DisplayDialog("Select Folder(s)", "You have not selected any folder from the Project Panel!", "Ok"))
                                return;
                        }

                        message = "Selected Folders : ";

                        if (!foldersFromProjectAdded)
                        {
                            foreach (var item in selectedFoldersFromProject)
                            {
                                if (!selectedFoldersPathTable.Contains(AssetDatabase.GetAssetPath(item)))
                                {
                                    selectedFoldersPathTable.Add(AssetDatabase.GetAssetPath(item));

                                    if (selectedFoldersFromProject.IndexOf(item) > 0)
                                        message += ", " + item.name;
                                    else
                                        message += item.name;
                                }
                            }

                            foldersFromProjectAdded = true;
                        }
                        else
                        {
                            message = "Select one or more folders from the Project Panel to search from.";
                            Selection.activeGameObject = null;
                            selectedFoldersFromProject.Clear();
                            selectedFoldersPathTable.Clear();
                            foldersFromProjectAdded = false;
                            objectsGrouped = false;
                        }
                    }
                }

                if (toSearchBy == 0)
                    searchString = EditorGUILayout.TagField(searchString);
                else
                    searchString = EditorGUILayout.TextField(searchString, EditorStyles.toolbarTextField);

                #region Search Button
                if (GUILayout.Button("Search") && searchString != "")
                {
                    Selection.activeObject = null;
                    selectedObjectTransforms.Clear();

                    if (selectedFoldersFromProject.Count < 1 && toSearchFrom == 1)
                    {
                        if (EditorUtility.DisplayDialog("Select Folder(s)", "You have not selected any folder from the Project Panel!", "Ok"))
                            return;
                    }

                    int count = 0;
                    bool searchThroughChildren = false;

                    if (EditorUtility.DisplayDialog("Search type", "Would you like to search for " + searchString + " through the child objects as well?", "Yes", "No"))
                        searchThroughChildren = true;

                    if (!selectedProjectSearch)
                    {
                        switch (toSearchBy)
                        {
                            case 0:
                                foreach (GameObject item in GameObject.FindObjectsOfType<GameObject>())
                                {
                                    if (item.CompareTag(searchString) && !selectedGameObjects.Contains(item))
                                    {
                                        selectedGameObjects.Add(item);
                                        selectedObjectTransforms.Add(item.transform);
                                        count++;
                                    }

                                    if (searchThroughChildren)
                                    {
                                        if (item.transform.childCount > 0)
                                            SearchThroughChildren(item, ref count);
                                    }
                                }

                                if (selectedGameObjects.Count > 0)
                                {
                                    Selection.objects = selectedGameObjects.ToArray();
                                    message = count + " objects of the tag " + searchString + " found and selected.\nClick the Group Objects button for options to modify the objects.";
                                    searchInvoked = true;
                                }
                                else
                                {
                                    message = "No objects of the tag " + searchString + " could be found.\nTry a different search string.";
                                    Selection.activeObject = null;
                                }
                                break;

                            case 1:
                                foreach (GameObject item in GameObject.FindObjectsOfType<GameObject>())
                                {
                                    if (item.name == searchString && !selectedGameObjects.Contains(item))
                                    {
                                        selectedGameObjects.Add(item);
                                        selectedObjectTransforms.Add(item.transform);
                                        count++;
                                    }

                                    if (searchThroughChildren)
                                    {
                                        if (item.transform.childCount > 0)
                                            SearchThroughChildren(item, ref count);
                                    }
                                }

                                if (selectedGameObjects.Count > 0)
                                {
                                    Selection.objects = selectedGameObjects.ToArray();
                                    message = count + " objects of the name " + searchString + " found and selected.\nClick the Group Objects button for options to modify the objects.";
                                    searchInvoked = true;
                                }
                                else
                                {
                                    message = "No objects of the name " + searchString + " could be found.\nTry a different search string.";
                                }
                                break;

                            default:
                                break;
                        }

                        selectedGameObjects.Clear();
                    }
                    else
                    {
                        List<string> assetsFound = new List<string>();

                        foreach (var folderPath in selectedFoldersPathTable)
                        {
                            string[] lookFor = new string[] { folderPath };
                            assetsFound.AddRange(AssetDatabase.FindAssets("t:GameObject", lookFor).ToList());
                        }

                        switch (toSearchBy)
                        {
                            case 0:
                                foreach (var item in assetsFound)
                                {
                                    GameObject loadedObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(item)) as GameObject;

                                    if (loadedObject.CompareTag(searchString) && !selectedGameObjects.Contains(loadedObject))
                                    {
                                        selectedGameObjects.Add(loadedObject);
                                        selectedObjectTransforms.Add(loadedObject.transform);
                                        ++count;
                                    }

                                    if (searchThroughChildren)
                                    {
                                        if (loadedObject.transform.childCount > 0)
                                            SearchThroughChildren(loadedObject, ref count);
                                    }
                                }

                                if (selectedGameObjects.Count > 0)
                                {
                                    Selection.objects = selectedGameObjects.ToArray();
                                    message = count + " objects of the tag " + searchString + " found and selected.\nClick the Group Objects button for options to modify the objects.";
                                    searchInvoked = true;
                                }
                                else
                                {
                                    message = "No objects of the tag " + searchString + " could be found.\nTry a different search string.";
                                }
                                break;

                            case 1:
                                foreach (var item in assetsFound)
                                {
                                    GameObject loadedObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(item)) as GameObject;

                                    if (loadedObject.name == searchString && !selectedGameObjects.Contains(loadedObject))
                                    {
                                        selectedGameObjects.Add(loadedObject);
                                        selectedObjectTransforms.Add(loadedObject.transform);
                                        count++;
                                    }

                                    if (searchThroughChildren)
                                    {
                                        if (loadedObject.transform.childCount > 0)
                                            SearchThroughChildren(loadedObject, ref count);
                                    }
                                }

                                if (selectedGameObjects.Count > 0)
                                {
                                    Selection.objects = selectedGameObjects.ToArray();
                                    message = count + " objects of the name " + searchString + " found and selected.\nClick the Group Objects button for options to modify the objects.";
                                    searchInvoked = true;
                                }
                                else
                                {
                                    message = "No objects of the name " + searchString + " could be found.\nTry a different search string.";
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    selectedGameObjects.Clear();
                    message += "\n\n\t\t\t\tHit the RESET button above to start a new search!";
                    foldersFromProjectAdded = false;
                }
                #endregion
            }
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;
            #endregion

            #region Simple Replace
            if (showLabel || toSearchFrom == 1)
                GUI.enabled = false;

            GUILayout.Space(10f);
            GUILayout.Label("Simple Replace", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            {
                string simpleReplaceMessage = "";

                if (selectedObjectTransforms.Count < 1)
                {
                    simpleReplaceMessage = "Select objects to replace";
                    GUI.enabled = false;
                }
                else
                {
                    simpleReplaceMessage = "Replace " + selectedObjectTransforms.Count + " object(s) with ";
                }

                ObjectToReplace = EditorGUILayout.ObjectField(simpleReplaceMessage, ObjectToReplace, typeof(GameObject), false) as GameObject;

                #region Replace Button
                if (GUILayout.Button("Replace"))
                {
                    if (ObjectToReplace != null)
                    {
                        if (selectedObjectTransforms.Count > 0)
                        {
                            foreach (var item in selectedObjectTransforms)
                            {
                                GameObject newGameObject = PrefabUtility.InstantiatePrefab(ObjectToReplace) as GameObject;

                                newGameObject.transform.position = item.position;
                                newGameObject.transform.localEulerAngles = item.localEulerAngles;
                                newGameObject.transform.localScale = item.localScale;
                                newGameObject.transform.parent = item.parent;

                                Undo.RegisterCreatedObjectUndo(newGameObject.gameObject, "Replaced object");
                                Undo.DestroyObjectImmediate(item.gameObject);
                            }


                            if (EditorUtility.DisplayDialog("Replacement complete", selectedObjectTransforms.Count + " object(s) from the hierarchy were successfully replaced with " + ObjectToReplace.name, "Ok"))
                                ClearSelection();
                        }
                        else
                        {
                            message = "No objects selected to be replaced!";
                        }
                    }
                    else
                    {
                        message = "Replacement prefab not found! Please provide one, and try again!";
                    }
                }
                #endregion
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(30f);
            #endregion

            #region Buttons
            GUI.enabled = true;
            if ((showLabel || selectedObjectTransforms.Count < 1) && !searchInvoked)
                GUI.enabled = false;

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Group obects"))
                {
                    bool groupUntaggedItems = false;
                    bool groupChildren = false;
                    numberOfObjectsModified = 0;

                    if (toSearchFrom == 0)
                    {
                        if (!searchInvoked)
                        {
                            if (EditorUtility.DisplayDialog("Tag type", "Would you also like to group the 'Untagged' items?", "Yes", "No"))
                                groupUntaggedItems = true;

                            if (EditorUtility.DisplayDialog("Search type", "Would you also like to group the child objects, if any?", "Yes", "No"))
                                groupChildren = true;
                        }

                        ExtractIntoGroups(groupUntaggedItems, bool.Equals(toSearchFrom, 1), groupChildren);
                    }
                    else
                    {
                        ExtractIntoGroups(groupUntaggedItems, true);
                    }

                    selectedGameObjects.Clear();
                    Selection.activeGameObject = null;
                }

                GUI.enabled = true;
                if (GUILayout.Button("RESET"))
                {
                    showLabel = false;
                    ObjectToReplace = null;
                    ClearSelection();
                }

                if (!showLabel)
                    GUI.enabled = false;

                if (GUILayout.Button("Make Changes"))
                    InitChanges();

                if (Selection.objects.Length > 0)
                    GUI.enabled = true;

                if (GUILayout.Button("Clear selection"))
                    Selection.activeObject = null;
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region Help Boxes
            GUI.enabled = true;
            GUILayout.Space(10f);
            EditorGUILayout.BeginVertical();
            {
                if (Selection.gameObjects.Length == 0 && !showLabel && !selectedProjectSearch)
                    EditorGUILayout.HelpBox(message, MessageType.Error);
                else if (Selection.gameObjects.Length > 0 && !showLabel)
                    EditorGUILayout.HelpBox(message, MessageType.Info);
                else if (!hasReplacementPrefab)
                    EditorGUILayout.HelpBox(message, MessageType.Error);
                else if (searchInvoked || selectedProjectSearch)
                    EditorGUILayout.HelpBox(message, MessageType.Info);
            }
            EditorGUILayout.EndVertical();
            #endregion

            #region Grouped Objects
            GUI.enabled = true;
            if (showLabel && selectedObjectTransforms.Count > 0)
            {
                GUILayout.Label("Grouped objects", EditorStyles.boldLabel);

                if (toSearchFrom == 0)
                {
                    EditorGUILayout.BeginHorizontal("Label");
                    {
                        GUILayout.Label("Selected Objects", EditorStyles.boldLabel);
                        GUILayout.Label("Replace with", EditorStyles.boldLabel);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginVertical();
                {
                    scrollView = EditorGUILayout.BeginScrollView(scrollView);
                    {
                        if (numberOfItemsToGroup > 0)
                        {
                            for (int i = 0; i < numberOfItemsToGroup; i++)
                            {
                                if (modifiedObjectIndices.Count > 0 && modifiedObjectIndices.Contains(i))
                                    continue;

                                if (toSearchFrom == 0)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField(labelTable[i]);
                                        GUILayout.Space(-150f);
                                        ObjectsToReplaceTable[i] = EditorGUILayout.ObjectField(ObjectsToReplaceTable[i], typeof(GameObject), false) as GameObject;

                                        //GUILayout.Space(30f);
                                        if (GUILayout.Button("Highlight in heirarchy", GUILayout.Width(150f), GUILayout.Height(20f)))
                                        {
                                            Selection.objects = groupedObjectsTable[i].ToArray();
                                            message = Selection.objects.Length + " selected in the hierarchy panel.";
                                        }

                                        if (GUILayout.Button("Remove", GUILayout.Width(60f), GUILayout.Height(20f)))
                                        {
                                            modifiedObjectIndices.Add(i);

                                            if ((modifiedObjectIndices.Count > 0 && modifiedObjectIndices.Count == originalObjectsToReplaceCount))
                                            {
                                                showLabel = false;
                                                numberOfItemsToGroup = 0;
                                                originalObjectsToReplaceCount = 0;
                                                ClearSelection();
                                            }

                                            return;
                                        }
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                else
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField(labelTable[i], EditorStyles.boldLabel);

                                        if (GUILayout.Button("Highlight in Project", GUILayout.Width(150f), GUILayout.Height(20f)))
                                        {
                                            Selection.objects = groupedObjectsTable[i].ToArray();
                                            message = Selection.objects.Length + " selected in the Project panel.";
                                        }
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }

                                showFoldoutTable[i] = EditorGUILayout.Foldout(showFoldoutTable[i], "Change settings for " + tagTable[i]);

                                if (showFoldoutTable[i])
                                {
                                    EditorGUILayout.BeginVertical();
                                    {
                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            maintainTransformTable[i] = EditorGUILayout.Toggle("Keep object(s) transform", maintainTransformTable[i]);
                                            resetTransformTable[i] = EditorGUILayout.Toggle("Reset object(s) transform", resetTransformTable[i]);

                                            if (toSearchFrom == 0)
                                                maintainHierarchyTable[i] = EditorGUILayout.Toggle("Maintain heirarchy", maintainHierarchyTable[i]);
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        if (!maintainTransformTable[i])
                                        {
                                            changePosTable[i] = EditorGUILayout.Toggle("Offset Position", changePosTable[i]);
                                            changeRotTable[i] = EditorGUILayout.Toggle("Offset Rotation", changeRotTable[i]);
                                            changeScaleTable[i] = EditorGUILayout.Toggle("Offset Scale", changeScaleTable[i]);
                                        }

                                        if (maintainTransformTable[i])
                                        {
                                            changePosTable[i] = changeRotTable[i] = changeScaleTable[i] = false;
                                            offsetPosTable.ToList().Clear();
                                            offsetRotTable.ToList().Clear();
                                            offsetScaleTable.ToList().Clear();
                                        }

                                        //resetTransformTable[i] = resetTransforms;
                                        //maintainTransformTable[i] = maintainTransforms;

                                        //if (toSearchFrom == 0)
                                        //    maintainHierarchyTable[i] = maintainHierarchy;
                                    }
                                    EditorGUILayout.EndVertical();

                                    if (changePosTable[i])
                                    {
                                        offsetPos = EditorGUILayout.Vector3Field("Position", offsetPos);
                                        offsetPosTable[i] = offsetPos;
                                    }

                                    if (changeRotTable[i])
                                    {
                                        offsetRot = EditorGUILayout.Vector3Field("Rotation", offsetRot);
                                        offsetRotTable[i] = offsetRot;
                                    }

                                    if (changeScaleTable[i])
                                    {
                                        offsetScale = EditorGUILayout.Vector3Field("Scale", offsetScale);
                                        offsetScaleTable[i] = offsetScale;
                                    }

                                    if (toSearchBy == 0)
                                    {
                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            changedTagTable[i] = EditorGUILayout.TagField("Change tag " + tagTable[i] + " to ", changedTagTable[i]);
                                        }
                                        EditorGUILayout.EndHorizontal();
                                    }
                                }
                                GUILayout.Space(10f);
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            #endregion
        }
    }
}

