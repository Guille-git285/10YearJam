using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{

    [SerializeField] private UnityEvent interactionEvent;

    //private bool isDoorOpen;

    void IInteractable.Activate()
    {
        if (interactionEvent != null)
            interactionEvent.Invoke();

        //transform.parent.LeanRotateAround(Vector3.up, isDoorOpen ? -90.0f : 90.0f, 1.0f).setEase(LeanTweenType.easeOutSine)
        //    .setOnStart(() => GetComponent<Collider>().enabled = false)
        //    .setOnComplete(() => GetComponent<Collider>().enabled = true)
        //    .setOnComplete(() => isDoorOpen = !isDoorOpen);
    }
}
