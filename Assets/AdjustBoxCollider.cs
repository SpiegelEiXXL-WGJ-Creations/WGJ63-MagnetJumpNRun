using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustBoxCollider : MonoBehaviour
{
    public BoxCollider2D bc;
    public SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        bc.size = sr.size;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
