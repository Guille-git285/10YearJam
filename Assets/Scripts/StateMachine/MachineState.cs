using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MachineState : MonoBehaviour
{
    [SerializeField] private string stateName;
    [SerializeField] protected StateMachineRoot root;

    protected virtual void Awake()
    {
        root = GetComponentInParent<StateMachineRoot>();
        this.enabled = false;
    }

    public virtual string StateName { get => stateName; set => stateName = value; }

    public virtual void OnStateEnter() { }

    public virtual void OnStateUpdate() { }

    public virtual void OnStateExit() { }

    public void ChangeState(string newStateName)
    {
        GetComponentInParent<StateMachineRoot>().ChangeState(newStateName);
    }
}