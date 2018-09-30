using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public delegate void magnetFired(float magnitude);
public class Magnet : MonoBehaviour
{
    public enum PlayMode
    {
        ClickToCharge = 0,
        WheelToCharge = 1
    }

    [Header("Settings")]
    public float MaxMagnetStrength = 10000f;
    public float MaxMagnetMagnitude = 1000f;
    public float chargeFactor = 10f;
    public AudioClip magnetActionWeak;
    public AudioClip magnetActionStrong;
    public AudioClip magnetCharging;
    public AudioClip magnetFullyCharged;
    public AudioSource audioSource;

    public PlayMode mode;

    [Header("Read-Only")]
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
    public bool isCharging = false;
    public event magnetFired magnetFiredEvent;


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
        if (mode == PlayMode.ClickToCharge)
            HealthBarFilling.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (mode == PlayMode.ClickToCharge)
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
        else if (mode == PlayMode.WheelToCharge)
        {
            CurrentMagnetStrength += Input.GetAxis("Mouse ScrollWheel") * 100;
            if (Input.GetMouseButtonUp(0))
            {
                audioSource.clip = CurrentMagnetStrength < MaxMagnetStrength ? magnetActionWeak : magnetActionStrong;

                audioSource.Stop();
                audioSource.Play();

                isCharging = false;

                if (magnetFiredEvent != null)
                    magnetFiredEvent(this.CurrentMagnetStrength);

                return;
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
