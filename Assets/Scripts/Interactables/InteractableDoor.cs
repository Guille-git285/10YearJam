using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : MonoBehaviour, IInteractable
{

    private bool isDoorOpen;

    void IInteractable.Activate()
    {
        isDoorOpen = !isDoorOpen;

        Transform parent = transform.parent;
        parent.LeanRotateAround(Vector3.up, isDoorOpen ? -90.0f : 90.0f, 1.0f).setEase(LeanTweenType.easeOutSine)
            .setOnStart(() => GetComponent<Collider>().enabled = false)
            .setOnComplete(() => GetComponent<Collider>().enabled = true);
    }
}
