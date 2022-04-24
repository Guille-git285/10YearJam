using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum SceneType
{
    MENU,
    LEVEL
}

public struct LoadData
{
    public string SceneName;
    public SceneType Type;
    public string MarkerName;
}

public class GameController : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField] private string currentScene;
    [SerializeField] private string nextScene;
    [SerializeField] private SceneType sceneType;

    [SerializeField] private StateMachineRoot stateMachine;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject pauseMenu;

    public UnityEvent OnGameStart;
    public UnityEvent OnGameLoading;

    public UnityEvent OnSceneLoading;
    public UnityEvent OnSceneLoaded;

    public UnityEvent OnMainMenu;
    public UnityEvent OnCheckPointLoad;

    public UnityEvent OnGamePause;
    public UnityEvent OnGamePauseResume;

    public UnityEvent OnGameOver;

    public string CurrentScene { get => currentScene; }
    public string NextScene { get => nextScene; }
    public GameObject LoadingScreen { get => loadingScreen; }
    public SceneType SceneType { get => sceneType; }
    public GameObject MenuCamera { get => menuCamera; }
    public GameObject PauseMenu { get => pauseMenu; }

    void Awake()
    {
        UpdateScenes();
    }

    void Start()
    {

    }

    public void LoadMainMenu ()
    {
        nextScene = "MainMenu";
        sceneType = SceneType.MENU;
        stateMachine.ChangeState("LoadingState");
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartNewGame ()
    {
        nextScene = "EntryArea";
        sceneType = SceneType.LEVEL;
        stateMachine.ChangeState("LoadingState");
    }

    public void LoadCheckPoint ()
    {
        Debug.Log("Load Checkpoint");
        stateMachine.ChangeState("LoadingState");
    }

    public void LoadLevel (string levelName, string markerName)
    {
        nextScene = levelName;
        sceneType = SceneType.LEVEL;
        stateMachine.ChangeState("LoadingState");
    }

    public void SaveCheckPoint ()
    {
        Debug.Log("Save Checkpoint");
    }

    public void PauseGame()
    {
        //Time.timeScale = 0;
        //Cursor.lockState = CursorLockMode.Confined;
        stateMachine.ChangeState("PauseState");
    }

    public void ResumeGame()
    {
        //Time.timeScale = 1;
        //Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState("PlayingState");
    }

    public void ExitGame()
    {
        stateMachine.ChangeState("ExitingState");
    }

    public void GameOver ()
    {
        stateMachine.ChangeState("GameOverState");
    }

    public void UpdateScenes ()
    {
        if (SceneManager.sceneCount > 1)
        {
            currentScene = SceneManager.GetSceneAt(1).name;
        }
    }

    private IEnumerator LoadingScene(string currentScene, string targetScene)
    {
        MenuCamera.SetActive(true);
        LoadingScreen.SetActive(true);
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
        LoadingScreen.SetActive(false);
    }
}