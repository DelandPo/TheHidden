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

        //Instantiate(explosion, transform.position, transform.rotation);       //Uncomment what particle effect is added

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearbyPlayer in colliders)
        {
            if(nearbyPlayer.gameObject.tag == "Hidden")
            {
                Health targetHealth = nearbyPlayer.GetComponent<Health>();

                if (targetHealth != null)
                {
                    targetHealth.DecreaseHealth(damage);
                }
            }
            

        }//end foreach

        Destroy(gameObject);

    }//end OnCollisionEnter

}//end GrenadeExplosion
