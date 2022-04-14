using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCPauseState : GCState
{
    public override void OnStateEnter()
    {
        //Time.timeScale = 0;
        //Cursor.lockState = CursorLockMode.Confined;
        //Show Pause Menu
        Debug.Log("Pause");
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
        //Hide Pause Menu
        //Time.timeScale = 1;
        //Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Resume");
    }
}
