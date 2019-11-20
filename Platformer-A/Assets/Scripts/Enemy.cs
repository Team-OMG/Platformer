using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //Where the ennemy will move if he is alone.
    //He will move to XMin and to XMax
    public float xMin, XMax;
    Rigidbody2D rb;
    //The speed he can't reach
    public float limite;
    //Where she look
    public string Direction = "R";

    private void Start()
    {
        //Make it ignore collision between the ennemy and the player
        Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), Player.GetComponent<BoxCollider2D>());
        rb = GetComponent<Rigidbody2D>();
    }

    //The text that will be showed when the ennemy will be shoot
    public Text KnockedText;

    private void Update()
    {
        if (OnGround)
        {
            Knocked -= 1 * Time.deltaTime;
        }

        if (Knocked > 0)
        {
            KnockedText.gameObject.SetActive(true);
        }
        else
        {
            KnockedText.gameObject.SetActive(false);
        }

        if (IdleMove)
        {
            DetectPlayer();
            IdleMoveVoid();
        }


        if (rb.velocity.x > limite)
        {
            rb.velocity = new Vector2(limite, rb.velocity.y);
        }
        else if (rb.velocity.x < -limite)
        {
            rb.velocity = new Vector2(-limite, rb.velocity.y);
        }

        //If ennemy have see the player go to him
        if (MoveToPlayer == true && Knocked <= 0)
        {
            MoveEnnemyToPose(Player.transform);
        }

        Animation();
    }

    void MoveEnnemyToPose(Transform Destination)
    {
        //Cooldown before he can jump again
        CoolDown -= 1 * Time.deltaTime;
        //Check where to goes by checking the x value
        //If the player is to the left of the ennemy so his x position is < to the x position of the ennemy
        if (Destination.transform.position.x < transform.position.x)
        {
            rb.AddForce(new Vector2(-50, 0));
            Direction = "L";
        }
        else
        {
            rb.AddForce(new Vector2(50, 0));
            Direction = "R";
        }

        //Ray are invisible gameobject that can check if a gameobject is within is trait
        RaycastHit2D hit;

        if (Direction == "R")
        {
            //(First parameter, a vector2)Make the Ray have his position equal to the ennemy + 2 on the x axis for be in front of the ennemy.
            //(Second parameter, a transform)Make the Ray goes to the down
            //(thirs parameter, a float) Make the ray have a max distance of 5
            hit = Physics2D.Raycast(new Vector2(transform.position.x + 2, transform.position.y), -transform.up, 5);
        }
        else
        {
            //(First parameter, a vector2)Make the Ray have his position equal to the ennemy - 2 on the x axis for be in front of the ennemy but because he turn to the left.
            hit = Physics2D.Raycast(new Vector2(transform.position.x - 2, transform.position.y), -transform.up, 5);
        }

        //Check if the collider is not hiting something.
        //If it's the case that mean he need to jump
        if (hit.collider == null && OnGround && CoolDown <= 0)
        {
            rb.AddForce(new Vector2(0, Force));
            //Cooldown before he can jump again
            CoolDown = 1;

            //I wanted to make him jump in fuction of the Y distance for him to jump correctly.
            //If someone want to try here's my code for this

            //float diffY = Player.transform.position.y - transform.position.y;

            //If the player jump so ennemy need to jump

            //rb.AddForce(new Vector2(0, diffY * Force));
            //Cooldown before he can jump again
            //CoolDown = 1;
        }
    }

    public float CoolDown, Force, Limite;
    bool OnGround;

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
    }

    public GameObject Player;
    public float RayDetection;

    void DetectPlayer()
    {
        //That will detect player in a circle. "RayDetection" is the length of this circle
        float X = Player.transform.position.x - transform.position.x, Y = Player.transform.position.y - transform.position.y;
        bool XIsNegative = false;


        if (X < 0)
        {
            XIsNegative = true;
            X *= -1;
        }

        if (Y < 0)
        {
            Y *= -1;
        }

        if (X + Y < RayDetection)
        {
            IdleMove = false;
            StopAllCoroutines();
            StartCoroutine(DetectionOfPlayer(XIsNegative));
        }

    }

    public GameObject warning;

    IEnumerator DetectionOfPlayer(bool PlayerIsInTheLeft)
    {
        if (PlayerIsInTheLeft)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        warning.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, 0);
        Instantiate(warning);
        IdleMove = false;
        yield return new WaitForSeconds(1.15f);

        while (IdleMove == false)
        {
            float X = Player.transform.position.x - transform.position.x, Y = Player.transform.position.y - transform.position.y;

            if (X < 0)
            {
                X *= -1;
            }

            if (Y < 0)
            {
                Y *= -1;
            }

            //Detect if the player is too far away for the ennemy
            if (X + Y > RayDetection + 20)
            {
                Debug.Log("Stop");
                StopAllCoroutines();
                IdleMove = true;
                MoveToPlayer = false;
            }
            else
            {
                MoveToPlayer = true;
                yield return new WaitForSeconds(1f);
            }
        }
    }

    bool IdleMove = true, MoveToPlayer;

    void IdleMoveVoid()
    {
        //If the ennemy don't go to the player
        if (Direction == "R")
        {
            rb.AddForce(new Vector3(10, 0));
            if (transform.position.x > XMax)
            {
                Direction = "L";
                rb.velocity = new Vector2(rb.velocity.x * -0.9f, rb.velocity.y);
            }
        }
        else
        {
            rb.AddForce(new Vector3(-10, 0));
            if (transform.position.x < xMin)
            {
                Direction = "R";
                rb.velocity = new Vector2(rb.velocity.x * -0.9f, rb.velocity.y);
            }
        }
    }

    void Animation()
    {
        //Just make him look to the right or to the left
        if (Direction == "R")
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    float Knocked;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Player is die
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (collision.gameObject.tag == "Ball")
        {
            //Ennemy is knocked
            Knocked = 1f;
            Destroy(collision.gameObject);
        }
    }
}
