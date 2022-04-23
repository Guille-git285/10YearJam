using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCPauseState : GCState
{
    public override void OnStateEnter()
    {
        GameController.isGamePaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        GameControl.PauseMenu.SetActive(true);
    }

    public override void OnStateUpdate()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameControl.ResumeGame();
        }
    }

    public override void OnStateExit()
    {
        GameControl.PauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        GameController.isGamePaused = false;
    }
}
