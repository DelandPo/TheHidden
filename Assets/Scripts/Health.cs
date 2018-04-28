using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public GameObject spawnLocation;
    [SyncVar(hook = "checkDeath")]
    public int health;

    // Use this for initialization
    void Start()
    {

        health = 100;
    }

    //Value being passed to DecreaseHealth must be a positive value
    public void DecreaseHealth(int x)
    {
        /*
        if (!isServer)
        {
            return;
        }
        */

        health = health - x;

    }//end DecreaseHealth

    //Value being passed to IncreaseHealth must be a positive value
    //Should only be accessible to the hidden - must be changed later
    public void IncreaseHealth(int x)
    {
        if (!isServer)
        {
            return;
        }
        health = health + x;

    }//end IncreaseHealth

    void checkDeath(int health)
    {
        if (health <= 0)
        {
            GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().PlayDeathCry();
            gameObject.transform.position = spawnLocation.transform.position;
            IncreaseHealth(100);

            //Destroy(gameObject);
        }
    }
}//end Health
