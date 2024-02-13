using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public CubeMovement cubeMovement;

    void OnCollisionEnter(Collision collisionInfo) 
    {
        if (collisionInfo.collider.CompareTag("maze"))
        {
            //print("ENTER");
            cubeMovement.EnableW = false;
        }
    }

    void OnCollisionStay(Collision collisionInfo) 
    {
        if (collisionInfo.collider.CompareTag("maze"))
        {
            //print("STAY");
            cubeMovement.EnableW = false;
        }
    }

    void OnCollisionExit(Collision collisionInfo) 
    {
        if (collisionInfo.collider.CompareTag("maze"))
        {
            //print("ENTER");
            cubeMovement.EnableW = true;
        }
    }
}
