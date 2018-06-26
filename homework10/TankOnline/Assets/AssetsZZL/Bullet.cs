﻿using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var hitPlayer = hit.GetComponent<TankMovement>();
        if (hitPlayer != null)
        {
            // Subscribe and Publish model may be good here!
            var combat = hit.GetComponent<Combat>();
            combat.TakeDamage(30);

            Destroy(gameObject);
        }
        
    }
}