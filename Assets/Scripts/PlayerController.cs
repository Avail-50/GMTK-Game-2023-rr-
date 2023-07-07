using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float force;
    public float move_drag;
    public float stop_drag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.drag = stop_drag;
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, MathF.Max(rb.velocity.y, 0));
            rb.AddRelativeForce(new Vector2(0, force * Time.deltaTime), ForceMode2D.Force);
            rb.drag = move_drag;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(MathF.Min(rb.velocity.x, 0), rb.velocity.y);
            rb.AddRelativeForce(new Vector2(-force * Time.deltaTime, 0), ForceMode2D.Force);
            rb.drag = move_drag;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, MathF.Min(rb.velocity.y, 0));
            rb.AddRelativeForce(new Vector2(0, -force * Time.deltaTime), ForceMode2D.Force);
            rb.drag = move_drag;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(MathF.Max(rb.velocity.x, 0), rb.velocity.y);
            rb.AddRelativeForce(new Vector2(force * Time.deltaTime, 0), ForceMode2D.Force);
            rb.drag = move_drag;
        }
    }
}
