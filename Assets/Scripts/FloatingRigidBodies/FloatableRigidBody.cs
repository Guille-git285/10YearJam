using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatableRigidBody : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsWater;
    //private bool touchingWater = false;
    private Rigidbody rgdbdy;
    private Collider col;
    //private float submergence = 0f;

    void Awake()
    {
        rgdbdy = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if ((whatIsWater & (1 << other.gameObject.layer)) != 0)
        {
            //touchingWater = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if ((whatIsWater & (1 << other.gameObject.layer)) != 0)
        {

        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((whatIsWater & (1 << other.gameObject.layer)) != 0)
        {
            //touchingWater = false;

        }
    }

    private void EvaluateSubmergence()
    {

    }
}
