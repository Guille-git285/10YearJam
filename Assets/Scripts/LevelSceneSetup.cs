using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Callbacks;

public class LevelSceneSetup : ScriptableObject
{
    [SerializeField] public SceneSetup[] scenes;

    [OnOpenAssetAttribute(1)]
    public static bool LoadSceneSetup (int instanceID, int line)
    {
        LevelSceneSetup levelSetup = (LevelSceneSetup)EditorUtility.InstanceIDToObject(instanceID);
        EditorSceneManager.RestoreSceneManagerSetup(levelSetup.scenes);
        return true;
    }
}
