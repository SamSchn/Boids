using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFood : MonoBehaviour
{
    private void Awake()
    {
        //initialize the position of the fish food
        transform.position = new Vector3(25, 25, 25);
    }

    //teleport the fish food somewhere else in the tank
    private void Teleport()
    {
        transform.position = new Vector3(Random.Range(-100, 100), Random.Range(1, 70), Random.Range(-100, 100));
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if it collides with a boid
        if (collision.gameObject.layer == 6)
            Teleport();//teleport it
    }
}
