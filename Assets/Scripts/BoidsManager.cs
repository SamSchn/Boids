using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    private struct BoidData
    {
        public Vector3 velocity;
        public Vector3 position;
    }

    private List<Boid> boids;

    //create an array of the structs
    private BoidData[] boidArray;

    //weights for each behaviour
    public int cohesionWeight;
    public int alignmentWeight;
    public int seperationWeight;

    public int spawnAmount;

    public GameObject boidPrefab;

    //flags for what boid flight is active
    public bool followLeader = false;
    public bool lazyFlight = true;

    public Rigidbody player;

    public FishFood food;

    //Comute Buffers
    private ComputeBuffer buffer;
    public ComputeShader compute;

    private void Awake()
    {

        boids = new List<Boid>();

        //spawn the boids
        for (int i = 0; i < spawnAmount; i++)
        {
            boids.Add(GameObject.Instantiate(boidPrefab).GetComponent<Boid>());
            boids[i].SetWeights(cohesionWeight, alignmentWeight, seperationWeight);
        }

        player = GameObject.FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>();

        food = GameObject.FindObjectOfType<FishFood>();

        CreatBoidDataArray(boids);
        buffer = MakeBoidBuffer(boidArray, 1);

            
    }

    private void FixedUpdate()
    {
        compute.SetInt("length", buffer.count);
        compute.SetFloat("cohesionDist", 56);
        compute.SetFloat("aligmentDist", 56);
        compute.SetBuffer(0, "boids", buffer);
        compute.Dispatch(compute.FindKernel("CSMain"), buffer.count, 1, 1);
    }

    public void SetWeights(int coWei, int alWei, int seWei)
    {
        for (int i = 0; i < spawnAmount; i++)
            boids[i].SetWeights(coWei, alWei, seWei);
    }

    public void SetViewDistance(int viewDistance, int viewDistanceSep)
    {
        for (int i = 0; i < spawnAmount; i++)
            boids[i].SetViewDistance(viewDistance, viewDistanceSep);
    }

    //sets the targets for the boids
    public void SetTarget()
    {
        if (followLeader)
        {
            for (int i = 0; i < spawnAmount; i++)
                boids[i].SetTarget(player.position);
        }
        else if (lazyFlight)
        {
            for (int i = 0; i < spawnAmount; i++)
                boids[i].SetTarget(food.gameObject.transform.position);
        }
    }

    //changes the flight mode
    public void SetFlightMode(bool mode)
    {
        followLeader = mode;
        lazyFlight = !mode;
    }

    private void CreatBoidDataArray(List<Boid> boids)
    {
        //create the array of BoidData
        boidArray = new BoidData[boids.Count];

        for (int i = 0; i < boids.Count; i++)
        {
            boidArray[i] = new BoidData();
            boidArray[i].velocity = boids[i].rb.velocity;
            boidArray[i].position = boids[i].rb.position;
        }
    }

    private ComputeBuffer MakeBoidBuffer(BoidData[] boids, int stride)
    {
        ComputeBuffer buffer = new ComputeBuffer(boids.Length, stride, ComputeBufferType.Raw);
        buffer.SetData(boids);
        return buffer;
    }

}
