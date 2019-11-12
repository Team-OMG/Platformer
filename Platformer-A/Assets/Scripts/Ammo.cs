using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [HideInInspector] //Hide this varialbe("Direction") in the inspector
    public string Direction;
    public float Speed; //The speed of the ball into the air
    public GameObject CoinAnimation; //The animation

    private void Start()
    {
        if (Direction == "Left") //Check if the ball must go to the left or the right
        {
            Speed *= -1;
        }

        Destroy(this.gameObject, 30); //Destroy this gameobject after 30 seconds : for optimisation
    }

    private void Update()
    {
        //Make the ball avancing forward
        transform.Translate(this.transform.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Damage") //Detect if the ball hits a mushroom
        {
            //Making the gameobject have the coordinates of the ball
            CoinAnimation.transform.position = transform.position;
            //Creating the animation's gameobject
            Instantiate(CoinAnimation);

            //Destroy the ball and the mushroom
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
