using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask jumpableLayers;
    [SerializeField] private LayerMask visibleLayers;

    [Header("Data")]
    [SerializeField] private Vector2[] rays;

    private bool isGrounded;
    private Rigidbody2D rb;

    [HideInInspector]
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, jumpableLayers);

        if (rb.velocity.magnitude > 0.2f)
        {
            anim.SetBool("isMoving", true);
        }else
        {
            anim.SetBool("isMoving", false);
        }

        if (rb.velocity.x > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }else if (rb.velocity.x < -0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void Move(float direction)
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    public void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, jumpableLayers);
        anim.SetBool("isGrounded", isGrounded);
        if (!isGrounded)
        {
            return;
        }
        anim.SetTrigger("jump");

        if (GetComponent<Player>().isAlive)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public float[] GetData()
    {
        List<float> data = new List<float>();

        for (int i = 0; i < rays.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rays[i], 6, visibleLayers);
            data.Add(hit.distance);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Death"))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    data.Add(-1);
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                    data.Add(0);
                }
            }
            else
            {
                data.Add(0);
            }
        }

        return data.ToArray();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            anim.SetTrigger("die");
            speed = 0;
            rb.velocity = new Vector2(0, jumpForce / 2);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Player>().isAlive = false;
        }
    }

}
