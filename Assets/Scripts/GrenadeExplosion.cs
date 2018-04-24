using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrenadeExplosion : NetworkBehaviour {

    [Tooltip("Particle Effect for Explosion")]
    public ParticleSystem explosion;

    [Tooltip("Radius of Explosion Damage")]
    public float radius;

    [Tooltip("Damage Value of Grenade")]
    public int damage;

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("CollisionEnter");

        ContactPoint contact = collision.contacts[0];
        Vector3 center = contact.point;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;

       
        while (i < hitColliders.Length)
        {

            hitColliders[i].GetComponent<Health>().DecreaseHealth(damage);
            i++;

            /*
            if(hitColliders[i].tag != "Hidden")     //Only damages Security players
            {

            }//end if
            */
        }//end while
        
        Destroy(gameObject);

    }//end OnCollisionEnter

}//end GrenadeExplosion
