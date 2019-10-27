using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] float Speed = 10;
    [SerializeField] float jumpForce = 5;
    Rigidbody2D rb;

    private bool OnGround = true;

    [SerializeField] AudioClip JumpSFX;
    [SerializeField] AudioClip LandSFX;
    [SerializeField] AudioClip WalkSFX;

    public AudioSource Source; 


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");

        float moveBy = x * Speed;

        rb.velocity = new Vector2(moveBy, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //Moving The Player

            OnGround = false;
            //Making Sure The Player Can't Jump While In The Air (Double Jump)

            Source.PlayOneShot(JumpSFX, 0.5f);
            //Playing The Hump SoundFX

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected");

        if(collision.gameObject.CompareTag("Ground"))
        { 
            OnGround = true;
            //Allowing The Player To Jump Again

            Source.PlayOneShot(LandSFX, 2.2f);
            //Playing The Landing Sound FX


        }

    }
}
