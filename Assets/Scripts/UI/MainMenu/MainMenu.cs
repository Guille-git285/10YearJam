using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameController gameController;

    void Awake()
    {
        gameController = Component.FindObjectOfType<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue ()
    {
        gameController.LoadCheckPoint();
    }

    public void StartGame ()
    {
        gameController.StartNewGame();
    }

    public void Options ()
    {

    }

    public void ExitGame ()
    {
        gameController.ExitGame();
    }
}
