﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This will acces me to the sceneManager so i can switch scene in this script
using UnityEngine.SceneManagement;

//I acces to the package "Hotween" that i installed from the asset store. Here's a link for more information http://dotween.demigiant.com/
using DG.Tweening;

public class MovementScript : MonoBehaviour
{
    private int _score = 0;
    private Score _uiManager;

    [SerializeField] float Speed = 10;
    [SerializeField] float jumpForce = 5;
    Rigidbody2D rb;

    private bool OnGround = true;
    private bool IsAlive = true;

    [SerializeField] AudioClip JumpSFX;
    [SerializeField] AudioClip LandSFX;
    [SerializeField] AudioClip WalkSFX;

    public AudioSource Source;

    [SerializeField] float Health = 100;
    public Text HealthTxt;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        IsAlive = true;
        Source = GetComponent<AudioSource>();
        //Setting Bools And Getting Components
    }

    // Update is called once per frame
    void Update()
    {

        if (IsAlive)
        {
            Move();
        }
        //Checking If The Player Is Alive Before Running The Movement Code

        Jump();
        CheckHealth();


    }

    public Sprite AstroStay;

    void Move()
    {
        //This take the animator component for verifying if he need to walk or not
        Animator thisAnimator = this.GetComponent<Animator>();

        float x = Input.GetAxisRaw("Horizontal");

        float moveBy = x * Speed;

        rb.velocity = new Vector2(moveBy, rb.velocity.y);

        _uiManager = GameObject.Find("Canvas").GetComponent<Score>();

        //This check if the player don't move
        if (x > -0.1f && x < 0.1f)
        {
            //if the player move make him stop running by desactiving the animator component
            thisAnimator.enabled = false;
            //Get the render of the sprites
            SpriteRenderer render = this.GetComponent<SpriteRenderer>();
            //And make him be the sprites assigned
            render.sprite = AstroStay;
        }
        if (x < 0)
        {
            //Make the animator active so he run
            thisAnimator.enabled = true;
            //Make the player look to the left because he walk to the left
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (x > 0)
        {
            //Make the animator active so he run
            thisAnimator.enabled = true;
            //Make the player look to the right because he walk to the right
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //check if the player in not in ground
        if (OnGround == false)
        {
            //if it's true that active the animator component even if he don't move
            thisAnimator.enabled = true;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnGround && IsAlive)
        //Checking If The Player Is On The Ground (To Prevent Double Jumping) Before Allowing Them To Jump
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
        //Debug.Log("Collision Detected");

        if (collision.gameObject.CompareTag("Ground"))
        {
            OnGround = true;
            //Allowing The Player To Jump Again

            Source.PlayOneShot(LandSFX, 2.2f);
            //Playing The Landing Sound FX


        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Adding the door's event

        if (collision.gameObject.tag == "Door") //Check if we touch the door
        {
            //Access to the collider of the player
            BoxCollider2D box = this.GetComponent<BoxCollider2D>();
            //Remove the box component ; that he's for avoiding some bugs that could happens
            box.enabled = false;
            //Make the rigidbody not simulated ; for avoiding the player to fall down when he try to reach the door
            rb.simulated = false;

            //I acces to the hotween component that make things move good
            this.transform.DOMove(collision.gameObject.transform.position, 1);
            //I make the player rotate in 1 seconds
            this.transform.DORotate(new Vector3(0, 0, 180), 1);
            //I make the player's scale = 0 in 1 seconds
            this.transform.DOScale(0, 1);

            //I acces to the Door's script. It's only to know wich scene we need to load  by the variable
            Door DoorScript = collision.GetComponent<Door>();

            //I start the coroutine. It's like a void but it can do more things like wait seconds, wait the end of frame etc..
            //We always launch a couroutine this ways.
            StartCoroutine(LoadSceneAfterXSecs(DoorScript.SceneToLoad, 1)); // I tell to the couroutine to switch scene in 1 seconds
        }

        //The Code Below Is For The Health System Not The Door System
        if (collision.gameObject.CompareTag("Damage"))
        {
            if (Health > 0)
            {
                int damage = Random.Range(9, 22);

                Health -= damage;
                //Removing The Damage From The Health Status

                //Creating a gamobject that will make the animation of taking a coin:

                //Making the gameobject have the coordinates of the point
                CoinAnimation.transform.position = collision.transform.position;
                //Creating the animation's gameobject
                Instantiate(CoinAnimation);

                Destroy(collision.gameObject);
                //Destroying The Mushroom After A Player Has Collided With It To Stop The Double Damage Problem I Had When Testing

            }
        }

        //If the gameobject detect a coin
        if (collision.gameObject.tag == "CoinTypeOne")
        {
            //Add 1 to the score
            AddScore(1);

            //Creating a gamobject that will make the animation of taking a coin:

            //Making the gameobject have the coordinates of the point
            CoinAnimation.transform.position = collision.transform.position;
            //Creating the animation's gameobject
            Instantiate(CoinAnimation);

            Destroy(collision.gameObject);
            //Destroying The Coin After A Player Has Collided With It.
        }
    }

    public GameObject CoinAnimation;

    public void CheckHealth()
    {

        if (Health < 1)
        {

            Debug.Log("Game Over, Player Is Dead");
            IsAlive = false;
            //Stopping The Player From Jumping And Moving By Changing The Alive Bool To False

            //Adding a restart system :

            //The scene reload
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            //The player regain maximum healt
            Health = 100;
        }

        HealthTxt.text = "Health: " + Health + "%";
        //Updating The Health Text To Specify The Health, I'm Currently Going To Do This In A Percentage Based System Rather Than A Points Based System
    }






    //This is the coroutine
    public IEnumerator LoadSceneAfterXSecs(string NameToLoad, float XSecs)
    {
        yield return new WaitForSeconds(XSecs); //We make the couroutine wait X seconds
        SceneManager.LoadScene(NameToLoad); //We make the couroutine load a scene
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdatePlayerScore(_score);
    }
}