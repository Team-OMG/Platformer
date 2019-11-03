using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Player; //The player's data
    public Vector3 Offset;

    void Update()
    {
        //Set the position of the camera
        this.transform.position = new Vector3(Player.transform.position.x + Offset.x, Player.transform.position.y + Offset.y, -10);
    }
}
