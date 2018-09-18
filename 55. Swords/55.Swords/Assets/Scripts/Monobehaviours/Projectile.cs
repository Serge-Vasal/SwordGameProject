using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject caster;
    private float speed;
    private float range;
    private Vector3 travelDirection;

    private float distanceTraveled;

    public event Action<GameObject, GameObject> ProjectileCollided;

    public void Fire(GameObject Caster,Vector3 target,float Speed,float Range)
    {
        caster = Caster;
        speed = Speed;
        range = Range;

        travelDirection = target - transform.position;
        travelDirection.y = 0f;
        travelDirection.Normalize();

        distanceTraveled = 0f;
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            float distanceToTravel = speed * Time.deltaTime;

            transform.Translate(travelDirection * distanceToTravel);

            distanceTraveled += distanceToTravel;
            if (distanceTraveled > range)
            {
                gameObject.SetActive(false);
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {            
            if (ProjectileCollided != null)
            {
                ProjectileCollided(caster, other.gameObject);
            }
            gameObject.SetActive(false);
        }
        
    }
}
