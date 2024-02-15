/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangLogic : ProjectileBase
{
    public Transform playerLocation;
    private float distanceTraveled = 0f;
    public float minRange = 0.5f;
    public bool isComingBack = false;

    protected override void Awake()
    {
        base.Awake();
        direction = Random.insideUnitSphere;
        direction = direction.normalized;
    }

    protected override void Start()
    {
        base.Start();
    }

     private void Update()
    {
        TrackPlayer();
    }

    protected override void DestroyProjectile()
    {
        rb2d.velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 0.5f);
    }

    public void TrackPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerLocation.position);

        if (distanceTraveled <= maxRange)
        {
            distanceTraveled += speed * Time.deltaTime;
        }
        else
        {
            // Reverse direction when maxRange is reached
            direction = (playerLocation.position - transform.position).normalized;
            isComingBack = true;
            if (isComingBack && distanceTraveled >= maxRange && distance <= minRange)
            {
                Debug.Log("Boomerang returned to player and got destroyed");
                DestroyProjectile();
            }

        }

    }
} */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangLogic : ProjectileBase
{
    public Transform playerLocation;
    private float distanceTraveled = 0f;
    public float minRange = 1f;
    public bool isComingBack = false;

    protected override void Awake()
    {
        base.Awake();
        direction = Random.insideUnitSphere;
        direction = direction.normalized;
    } 

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        TrackPlayer();
        if (isComingBack)
        {
            direction = (playerLocation.position - transform.position).normalized;
            SetProyectileVelocity(speed, direction);
        }
    }

    protected override void DestroyProjectile()
    {
        rb2d.velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 0.1f);
    }

    public void TrackPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerLocation.position);
      //  Debug.Log($"Distance Traveled: {distanceTraveled}, Distance to Player: {distance}, Coming Back: {isComingBack}, Direction :{direction}, maxRange: {maxRange}");
        if (distanceTraveled <= maxRange)
        {
            distanceTraveled += speed * Time.deltaTime;
        }

        // vuelve cuando llega a maxrange
        if (distance >= maxRange)
        {
            direction = (playerLocation.position - transform.position).normalized;
            SetProyectileVelocity(speed, direction);

            Debug.Log($"Speed: {speed}, direction: {direction}");

            isComingBack = true;
        }

        // ver si esta volviendo y esta lo suficientemente cerca para destruir el proyectil
        if (isComingBack && distance <= minRange)
        {
            Debug.Log("Boomerang returned to player and got destroyed");
            DestroyProjectile();
        }
    }
}

