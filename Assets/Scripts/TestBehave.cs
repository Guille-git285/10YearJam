using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehave : MonoBehaviour
{
    private GameController gameController;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GameOver", 4f);
    }

    public void GameOver ()
    {

    }

    public void PauseGame ()
    {

    }

    public void ResumeGame ()
    {

    }

    public void NextLevel ()
    {

    }

    public void MainMenu ()
    {

    }
}
