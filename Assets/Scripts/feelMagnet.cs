using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feelMagnet : MonoBehaviour
{

    public GameObject magnetObject;
    public Magnet magnet;
    private Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {

        magnetObject = GameObject.FindGameObjectWithTag("Magnet");
        magnet = magnetObject.GetComponentInChildren<Magnet>();
        magnet.magnetFiredEvent += Magnet_MagnetFiredEvent;
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Magnet_MagnetFiredEvent(float magnitude)
    {
        Vector3 distVect = magnetObject.transform.position - this.transform.position;

        Vector3 mag = distVect.normalized * (1 / (distVect.sqrMagnitude)) * magnitude;

        //Debug.Log("Magnitude: " + mag.magnitude);
        if (mag.magnitude > magnet.MaxMagnetMagnitude)
            mag = distVect.normalized * magnet.MaxMagnetMagnitude;

        rb.AddForce(mag);
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {



    }
}
