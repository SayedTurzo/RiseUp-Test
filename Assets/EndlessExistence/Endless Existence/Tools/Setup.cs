using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static System.IO.Path;
using static System.IO.Directory;
using static UnityEditor.AssetDatabase;

public class FolderSetupWindow : EditorWindow
{
    private string rootFolderName = "";
    private List<string> folderNames = new List<string> { "Scripts", "ThirdParty", "Prefabs", "Audio", "Image" };
    private string newFolderName = "";

    // Add menu item to open the window
    [MenuItem("Endless Existence/Tools/Setup/Folder Creation Window")]
    public static void ShowWindow()
    {
        GetWindow<FolderSetupWindow>("Folder Setup");
    }

    // GUI for the window
    private void OnGUI()
    {
        GUILayout.Label("Setup Project Folders", EditorStyles.boldLabel);

        // Input field for root folder name
        rootFolderName = EditorGUILayout.TextField("Root Folder", rootFolderName);

        GUILayout.Space(10);

        // Display existing folder names and allow removing them
        GUILayout.Label("Subfolders:");
        for (int i = 0; i < folderNames.Count; i++)
        {
            GUILayout.BeginHorizontal();
            folderNames[i] = EditorGUILayout.TextField(folderNames[i]);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                folderNames.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
        }

        // Input field to add new subfolder
        GUILayout.Space(10);
        newFolderName = EditorGUILayout.TextField("Add New Folder", newFolderName);
        if (GUILayout.Button("Add Folder"))
        {
            if (!string.IsNullOrEmpty(newFolderName) && !folderNames.Contains(newFolderName))
            {
                folderNames.Add(newFolderName);
                newFolderName = ""; // Clear input after adding
            }
        }

        GUILayout.Space(20);

        // Create folders button
        if (GUILayout.Button("Create Folders"))
        {
            CreateDefaultFolders(rootFolderName, folderNames.ToArray());
        }
    }

    // Folder creation logic
    private void CreateDefaultFolders(string root, params string[] folders)
    {
        var fullpath = Combine(Application.dataPath, root);
        foreach (var folder in folders)
        {
            var path = Combine(fullpath, folder);
            if (!Exists(path))
            {
                CreateDirectory(path);
            }
        }
        Refresh(); // Refresh the AssetDatabase to see the folders in Unity immediately
        Debug.Log("Folders created successfully!");
    }
}
