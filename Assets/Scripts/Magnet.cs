using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public delegate void magnetFired(float magnitude);
public class Magnet : MonoBehaviour
{
    public float MaxMagnetStrength = 10000f;
    public float chargeFactor = 10f;
    public AudioClip magnetActionWeak;
    public AudioClip magnetActionStrong;
    public AudioClip magnetCharging;
    public AudioClip magnetFullyCharged;
    public AudioSource audioSource;
    private float _CurrentMagnetStrength = 0f;
    public float CurrentMagnetStrength
    {
        get
        {
            return _CurrentMagnetStrength;
        }
        set
        {
            setStrength(value);
        }
    }
    public GameObject magnetObject;
    public Image HealthBarFilling;
    public magnetFired magnetFiredEvent;


    public void setStrength(float strength)
    {
        _CurrentMagnetStrength = strength;

        HealthBarFilling.fillAmount = _CurrentMagnetStrength / MaxMagnetStrength;

    }

    // Use this for initialization
    void Start()
    {
        audioSource = this.GetComponentInChildren<AudioSource>();
        magnetObject = GameObject.FindGameObjectWithTag("Magnet");
        Image[] i = this.GetComponentsInChildren<Image>();
        foreach (Image ii in i)
        {
            if (ii.name == "HealthBarFilling")
                HealthBarFilling = ii;
        }
        HealthBarFilling.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 mPos = Camera.main.ScreenToWorldPoint(
            Input.mousePosition);
        mPos.z = magnetObject.transform.position.z;
        magnetObject.transform.position = mPos;

    }
}
