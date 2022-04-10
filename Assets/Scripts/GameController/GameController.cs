using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private static GameController instance = null;
    public static GameController Instance { get { return instance; } }

    public bool IsPaused { get; set; }
    
    [SerializeField] private string priorScene;
    [SerializeField] private string currentScene;

    [SerializeField] private StateMachineRoot stateMachine;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPrefab;

    public UnityEvent OnGameOver;

    public void TransitionTo(string scene, string marker)
    {
        if (player != null)
        {
            player.SetActive(false);
        }
        StartCoroutine(Transitioning(scene, marker));
    }

    private IEnumerator Transitioning(string scene, string marker)
    {
        priorScene = currentScene;
        currentScene = scene;

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(priorScene);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (player != null)
        {
            Transform markerTransform = GameObject.Find(marker).transform;
            player.transform.SetPositionAndRotation(markerTransform.position, markerTransform.rotation);
            player.SetActive(true);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }

    public void GameOver()
    {

    }
}