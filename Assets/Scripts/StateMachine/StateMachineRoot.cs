using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineRoot : MonoBehaviour
{
    [SerializeField] private MachineState[] states;
    [SerializeField] private MachineState entry_state;
    [SerializeField] private MachineState current_state;
    private MachineState prior_state;
    [SerializeField] private GameObject target;

    public GameObject Target { get => target; }

    void Awake()
    {
        states = GetComponentsInChildren<MachineState>();
    }

    void Start()
    {
        foreach (MachineState state in states)
        {
            state.enabled = false;
        }

        if (entry_state != null)
        {
            current_state = entry_state;
            current_state.OnStateEnter();
        } else
        {
            Debug.Log("No Entry State assigned. Defaulting to first state found.");
            if (states.Length != 0)
            {
                current_state = states[0];
            } else
            {
                Debug.LogError("This state machine has no states.");
                this.enabled = false;
            }
        }
    }

    void Update()
    {
        current_state.OnStateUpdate();
    }

    private void ChangeState(MachineState new_state)
    {
        if (states.Length != 0 && new_state != null)
        {
            current_state.OnStateExit();
            prior_state = current_state;
            current_state = new_state;
            current_state.OnStateEnter();
            prior_state.enabled = false;
        }
    }

    public void ChangeState(string state_name)
    {
        ChangeState(GetState(state_name));
    }

    public string CurrentStateName()
    {
        return current_state.StateName;
    }

    public MachineState CurrentState()
    {
        return current_state;
    }

    public MachineState GetState(string state_name)
    {
        foreach (MachineState state in states)
        {
            if (state.StateName == state_name) return state;
        }
        return null;
    }

}