using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{

    [Header("Input")]
    [SerializeField] private string interactButton = "Interact";

    [Header("Interactions")]
    [SerializeField] private float interactionDistance = 3.0f;
    [SerializeField] private LayerMask interactableMask;

    void Update()
    {
        if (Input.GetButtonDown(interactButton))
        {
            RaycastHit result;
            if (Physics.Raycast(transform.position, transform.forward, out result, interactionDistance, interactableMask))
            {
                result.transform.gameObject.SendMessage("Activate");
            }
        }
    }
}
