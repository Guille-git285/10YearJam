using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCState : MachineState
{
    private GameController gameControl;

    protected GameController GameControl { get => gameControl; set => gameControl = value; }

    protected override void Awake()
    {
        base.Awake();
        GameControl = GetComponentInParent<GameController>();
    }

}
