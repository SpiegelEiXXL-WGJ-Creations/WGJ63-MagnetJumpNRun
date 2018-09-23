using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public delegate void magnetFired(float magnitude);
public class Magnet : MonoBehaviour
{
    public float MaxMagnetStrength = 10000f;
    public float MaxMagnetMagnitude = 1000f;
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
    public event magnetFired magnetFiredEvent;
    public bool isCharging = false;


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

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            audioSource.clip = CurrentMagnetStrength < MaxMagnetStrength ? magnetActionWeak : magnetActionStrong;

            audioSource.Stop();
            audioSource.Play();

            isCharging = false;

            if (magnetFiredEvent != null)
                magnetFiredEvent(this.CurrentMagnetStrength);

            return;
        }

        if (Input.GetMouseButtonDown(0) && !isCharging && CurrentMagnetStrength <= 0f)
        {
            audioSource.clip = magnetCharging;
            audioSource.Play();
            isCharging = true;
            HealthBarFilling.gameObject.SetActive(true);
            return;
        }

        float charger = Time.deltaTime * chargeFactor;
        if (isCharging && CurrentMagnetStrength < MaxMagnetStrength)
        {
            // start Charging
            CurrentMagnetStrength = CurrentMagnetStrength + charger;

            //Debug.Log("Mah Chargelevel is: " + CurrentMagnetStrength);
        }
        else if (isCharging && CurrentMagnetStrength >= MaxMagnetStrength)
        {
            if (audioSource.clip.name != magnetFullyCharged.name)
            {
                audioSource.clip = magnetFullyCharged;
                audioSource.Play();
            }
        }
        else
        {
            if (CurrentMagnetStrength > 0f)
                CurrentMagnetStrength = CurrentMagnetStrength - charger;
            else
            {
                CurrentMagnetStrength = 0f;
                HealthBarFilling.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 mPos = Camera.main.ScreenToWorldPoint(
            Input.mousePosition);
        mPos.z = magnetObject.transform.position.z;
        magnetObject.transform.position = mPos;

    }
}
