using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feelMagnet : MonoBehaviour
{

    public GameObject magnetObject;
    public Magnet magnet;
    private Rigidbody2D rb;
    public bool isCharging = false;

    // Use this for initialization
    void Start()
    {

        magnetObject = GameObject.FindGameObjectWithTag("Magnet");
        magnet = magnetObject.GetComponentInChildren<Magnet>();

        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButton(0))
        {
            Vector3 distVect = magnetObject.transform.position - this.transform.position;
            Vector3 mag = distVect.normalized * (1 / (distVect.sqrMagnitude)) * magnet.magnetStrength;
            Debug.Log("button down; distVect: " + distVect + " - mag: " + mag);

            this.gameObject.GetComponent<Rigidbody2D>().AddForce(mag);
        }*/

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            if (magnet.CurrentMagnetStrength < magnet.MaxMagnetStrength)
                magnet.audioSource.clip = magnet.magnetActionWeak;
            else
                magnet.audioSource.clip = magnet.magnetActionStrong;

            magnet.audioSource.Stop();
            magnet.audioSource.Play();

            Vector3 distVect = magnetObject.transform.position - this.transform.position;
            Vector3 mag = distVect.normalized * (1 / (distVect.sqrMagnitude)) * magnet.CurrentMagnetStrength;
            rb.AddForce(mag);
            isCharging = false;
            //magnet.CurrentMagnetStrength = 0f;
            //magnet.HealthBarFilling.gameObject.SetActive(false);
            return;
        }

        if (Input.GetMouseButtonDown(0) && !isCharging && magnet.CurrentMagnetStrength <= 0f)
        {
            magnet.audioSource.clip = magnet.magnetCharging;
            magnet.audioSource.Play();
            isCharging = true;
            magnet.HealthBarFilling.gameObject.SetActive(true);
            return;
        }

        float charger = Time.deltaTime * magnet.chargeFactor;
        if (isCharging && magnet.CurrentMagnetStrength < magnet.MaxMagnetStrength)
        {
            // start Charging
            magnet.CurrentMagnetStrength = magnet.CurrentMagnetStrength + charger;

            //Debug.Log("Mah Chargelevel is: " + magnet.CurrentMagnetStrength);
        }
        else if (isCharging && magnet.CurrentMagnetStrength >= magnet.MaxMagnetStrength)
        {
            if (magnet.audioSource.clip.name != magnet.magnetFullyCharged.name)
            {
                magnet.audioSource.clip = magnet.magnetFullyCharged;
                magnet.audioSource.Play();
            }
        }
        else
        {
            if (magnet.CurrentMagnetStrength > 0f)
                magnet.CurrentMagnetStrength = magnet.CurrentMagnetStrength - charger;
            else
            {
                magnet.CurrentMagnetStrength = 0f;
                magnet.HealthBarFilling.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {



    }
}
