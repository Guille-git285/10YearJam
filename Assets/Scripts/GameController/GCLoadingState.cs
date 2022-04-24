using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GCLoadingState : GCState
{

    public override void OnStateEnter()
    {
        GameControl.OnGameLoading.Invoke();
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        GameControl.UpdateScenes();
    }
}
