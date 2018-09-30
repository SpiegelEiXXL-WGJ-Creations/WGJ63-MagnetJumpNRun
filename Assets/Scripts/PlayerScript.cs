using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    [Header("Settings")]
    public Rigidbody2D rigidBody;
    public float jumpForce = 10f;
    public float moveForce = 10f;
    public bool WASDMovement = false;
    public int lives = 5;
    public Image respawnFader;
    public float respawnTime = 10f;
    public GameObject TextScreen;

    [Header("Read only values")]
    public bool isFalling = false;
    public bool isDying = false;
    public bool isSpawning = false;
    public bool isFinished = false;


    // Use this for initialization
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isDying)
        {
            respawnFader.color = new Color(respawnFader.color.r, respawnFader.color.g, respawnFader.color.b, Mathf.Lerp(respawnFader.color.a, 1, respawnTime * Time.deltaTime));
            if (respawnFader.color.a >= 0.99f)
            {
                this.transform.localPosition = new Vector3(-4, -1, 0);
                isDying = false;
                if (!isFinished)
                    isSpawning = true;
            }
        }
        if (isFinished && !isDying)
        {
            TextScreen.SetActive(true);
            isSpawning = true;
            TextScreen.GetComponentInChildren<TextTyperByTheLetter>().startPrintingText();
        }
        if (isSpawning)
        {
            respawnFader.color = new Color(respawnFader.color.r, respawnFader.color.g, respawnFader.color.b, Mathf.Lerp(respawnFader.color.a, 0, respawnTime * Time.deltaTime));
            if (respawnFader.color.a <= 0.001f)
            {
                isSpawning = false;
            }
        }

        if (WASDMovement && !isSpawning && !isDying)
        {
            if (Input.GetAxis("Horizontal") > 0f)
            {
                rigidBody.velocity = new Vector2(moveForce, rigidBody.velocity.y);
            }
            if (Input.GetAxis("Horizontal") < 0f)
            {

                rigidBody.velocity = new Vector2(-moveForce, rigidBody.velocity.y);
            }
            if (Input.GetAxis("Vertical") > 0f && !isFalling)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
                isFalling = true;
            }
        }
        Camera.main.transform.rotation = this.transform.rotation;
        Camera.main.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles * -1);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isFalling = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathPlane")
        {
            isDying = true;
            /*
            this.transform.localPosition = new Vector3(-4, -1, 0);
            lives--;
            */
        }
        if (collision.tag == "NextLevelTag")
        {
            isDying = true;
            isFinished = true;
        }
    }
}
