using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boid : MonoBehaviour
{

    //variables
    public Rigidbody rb;

    public float speed;
    public Vector3 target;   //holds the target of the boid
    public List<Collider> neighbors;   //hold all of this boids neihbors
    public List<Collider> neighborsSep;  //holds the neigbors in seperation radius
    public float viewDist;   //holds how far the boid can see other boids and objects
    public float sepDist;  //holds the veiw distance used for avoidance

    //weights for each behaviour
    public int cohesionWeight;
    public int alignmentWeight;
    public int seperationWeight;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(Random.value, Random.value, Random.value).normalized * speed;
    }

    //moves the boid
    public void Move()
    { 
        //collect all colliders within our view distance
        neighbors = new List<Collider>(Physics.OverlapSphere(rb.position, viewDist));

        //remove any collider that is not a boid
        for (int i = 0; i < neighbors.Count; i++)
            if (neighbors[i].tag != "Boid")
                neighbors.RemoveAt(i);

        //collect all colliders within our view distance
        neighborsSep = new List<Collider>(Physics.OverlapSphere(rb.position, sepDist));

        //remove any collider that is not a boid
        for (int i = 0; i < neighborsSep.Count; i++)
            if (neighborsSep[i].gameObject.layer != 7 && neighborsSep[i].gameObject.layer != 6)
                neighborsSep.RemoveAt(i);

        Cohesion();
        Alignment();
        Seperation();
        Tracking();

        if (rb.velocity.magnitude > speed)
            rb.velocity = rb.velocity.normalized * speed;

        rb.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void FixedUpdate()
    {
        //Move(); 
        
    }

    //returns the direction the boid needs to travel to get to the target
    private void Tracking()
    {
        Vector3 result = Vector3.zero;

        result = (target - rb.position);


        rb.velocity += Vector3.Lerp(Vector3.zero, result, Time.deltaTime) *2;
    }

    //implementation of the cohesion behaviour
    private void Cohesion()
    {
        Vector3 result = Vector3.zero;

        //add up all the vectors from the neighbors
        foreach (Collider col in neighbors)
            result += col.gameObject.GetComponent<Rigidbody>().position - rb.position; 

        //take the average if there are neighbors
        if (neighbors.Count > 0)
            result /= neighbors.Count;

        rb.velocity += Vector3.Lerp(Vector3.zero, result, result.magnitude / viewDist) * (cohesionWeight / 25);
    }

    //implementation of the alignment behaviour
    private void Alignment()
    {
        Vector3 result = Vector3.zero;

        //add up all the velocities from the neighbors
        foreach (Collider col in neighbors)
            result += col.gameObject.GetComponent<Rigidbody>().velocity;

        //take the average if there are neighbors
        if (neighbors.Count > 0)
            result /= neighbors.Count;

        rb.velocity += Vector3.Lerp(rb.velocity, result, Time.deltaTime) * (alignmentWeight / 25);
    }

    //implementation of the seperation behaviour
    private void Seperation()
    {
        Vector3 result = Vector3.zero;

        //add up all the vectors from the neighbors
        foreach (Collider col in neighborsSep)
            result += col.gameObject.GetComponent<Rigidbody>().position - rb.position;

        //take the average if there are neighbors
        if (neighborsSep.Count > 0)
            result /= neighborsSep.Count;

        rb.velocity -= Vector3.Lerp(Vector3.zero, result, result.magnitude / viewDist) * (seperationWeight / 25);
    }

    //set the weights for each of the behaviours
    public void SetWeights(int coWei, int alWei, int seWei)
    {
        cohesionWeight = coWei;
        alignmentWeight = alWei;
        seperationWeight = seWei;
    }

    //sets the target of this boid
    public void SetTarget(Vector3 tar)
    {
        target = tar;
    }

    //sets the view distances
    public void SetViewDistance(int viewDistance, int viewDistanceSep)
    {
        viewDist = viewDistance;
        sepDist = viewDistanceSep;
    }

    
}

