using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetGameButton_Memory))]
[CanEditMultipleObjects]
[System.Serializable]
public class SetGameButton_MemoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SetGameButton_Memory myScript = target as SetGameButton_Memory;
        switch (myScript.buttonType)
        {
            case SetGameButton_Memory.EButtonType.PairNumberBtn:
                myScript.PairNumber = (GameSettings_Memory.EpairNumber)EditorGUILayout.EnumPopup("Pair Numbers", myScript.PairNumber);
                break;

            case SetGameButton_Memory.EButtonType.PuzzleCategoryBtn:
                myScript.PuzzleCategories = (GameSettings_Memory.EPuzzleCategories)EditorGUILayout.EnumPopup("Puzzle Categories", myScript.PuzzleCategories);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
