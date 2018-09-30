using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feelMagnet : MonoBehaviour
{

    public GameObject magnetObject;
    public Magnet magnet;
    private Rigidbody2D rb;
    private LineRenderer lr;
    // Use this for initialization
    void Start()
    {

        magnetObject = GameObject.FindGameObjectWithTag("Magnet");
        magnet = magnetObject.GetComponentInChildren<Magnet>();
        magnet.magnetFiredEvent += Magnet_MagnetFiredEvent;
        rb = this.GetComponent<Rigidbody2D>();
        lr = this.gameObject.GetComponentInChildren<LineRenderer>();
    }

    void Magnet_MagnetFiredEvent(float magnitude)
    {
        Magnet_MagnetFiredEvent(magnitude, rb);
    }


    void Magnet_MagnetFiredEvent(float magnitude, Rigidbody2D rib = null)
    {
        if (rib == null)
            rib = rb;
        Vector3 distVect = magnetObject.transform.position - this.transform.position;

        Vector3 mag = distVect.normalized * (1 / (distVect.sqrMagnitude)) * magnitude;

        //Debug.Log("Magnitude: " + mag.magnitude);
        if (mag.magnitude > magnet.MaxMagnetMagnitude)
            mag = distVect.normalized * magnet.MaxMagnetMagnitude;



        rib.AddForce(mag);
    }


    // Update is called once per frame
    void Update()
    {

    }
    public float timeInSec = 0f;
    public float maxTime = 1f;
    public float cooldown = 10f;
    public float currentCooldown = 0f;
    public float stepSize = 2f;

    private void FixedUpdate()
    {
        if (currentCooldown > 0f)
        {
            currentCooldown = currentCooldown - .2f;
            return;
        }

        if (magnet.mode == Magnet.PlayMode.WheelToCharge)
        {
            if (timeInSec > 0f) return;
            timeInSec = maxTime;

            Physics2D.autoSimulation = false;

            GameObject o = GameObject.Instantiate(this.gameObject);
            Rigidbody2D d = o.GetComponent<Rigidbody2D>();
            rb.simulated = false;
            List<Vector3> points = new List<Vector3>();

            //Debug.Log("Starting simulating...");
            Magnet_MagnetFiredEvent(magnet.CurrentMagnetStrength, d);
            //Simulate where it will be in x seconds
            while (timeInSec > 0f)
            {
                points.Add(d.transform.localPosition);

                timeInSec -= Time.fixedDeltaTime * stepSize;
                Physics2D.Simulate(Time.fixedDeltaTime);
                //Debug.Log("Simulating...");
            }
            //Debug.Log("Done simulating...");
            lr.positionCount = points.Count;
            lr.SetPositions(points.ToArray());
            //Re-enable Physics AutoSimulation and Reset position
            GameObject.DestroyImmediate(o);
            rb.simulated = true;
            Physics2D.autoSimulation = true;
            currentCooldown = cooldown;
        }

    }
}
