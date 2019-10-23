using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Fixedpdate()
    {

        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");



    }
}
