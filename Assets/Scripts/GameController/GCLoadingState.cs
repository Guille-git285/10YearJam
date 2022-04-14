using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GCLoadingState : GCState
{

    public override void OnStateEnter()
    {
        GameControl.OnGameLoading.Invoke();
        StartCoroutine(LoadingScene(GameControl.CurrentScene, GameControl.NextScene));
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        
    }

    private IEnumerator LoadingScene(string currentScene, string targetScene)
    {
        GameControl.MenuCamera.SetActive(true);
        GameControl.LoadingScreen.SetActive(true);
        Scene sceneToUnload = SceneManager.GetSceneByName(currentScene);

        if (sceneToUnload.IsValid() && sceneToUnload.isLoaded)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToUnload);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        } else
        {
            Debug.Log("No current scene assign.");
        }

        int sceneToLoadIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + targetScene + ".unity");
        if (sceneToLoadIndex != -1)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadIndex, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        } else
        {
            Debug.Log("Scene name does not have a matching scene at Assets/Scenes/" + targetScene + ".unity");
        }
        GameControl.LoadingScreen.SetActive(false);
        string nextState;
        if (GameControl.SceneType == SceneType.MENU)
        {
            nextState = "MenuState";
        }
        else
        {
            nextState = "PlayingState";
            GameControl.MenuCamera.SetActive(false);
        }
        ChangeState(nextState);
        Debug.Log("End Loading");
    }
}
