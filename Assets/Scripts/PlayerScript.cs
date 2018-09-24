using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public Rigidbody2D rb;
    public float vForce = 10f;
    public float hForce = 10f;
    public bool isFalling = false;

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0f)
        {
            rb.velocity = new Vector2(hForce, rb.velocity.y);
        }
        if (Input.GetAxis("Horizontal") < 0f)
        {

            rb.velocity = new Vector2(-hForce, rb.velocity.y);
        }
        if (Input.GetAxis("Vertical") > 0f && !isFalling)
        {
            rb.velocity = new Vector2(rb.velocity.x, vForce);
            isFalling = true;
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
            this.transform.localPosition = new Vector3(-4, -1, 0);
        }
    }
}
