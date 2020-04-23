using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;

public class SelectionCountTool : EditorWindow
{
    [MenuItem("Window/Selection Count")]
    public static void ShowWindow()
    {
        GetWindow<SelectionCountTool>("Selection Count");
    }

    public void OnGUI()
    {
        GUILayout.Label("Number Of Objects Selected: " + Selection.gameObjects.Length, EditorStyles.boldLabel);
    }
}
