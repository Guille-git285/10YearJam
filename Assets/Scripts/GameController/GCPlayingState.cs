using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCPlayingState : GCState
{
    public override void OnStateEnter()
    {

    }

    public override void OnStateUpdate()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameControl.PauseGame();
        }
    }

    public override void OnStateExit()
    {

    }
}
