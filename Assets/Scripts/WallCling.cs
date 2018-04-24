using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

//****************************************************************************
// Notes: 
// WallCling assumes that all walls that can be clung to have a tag
// associated with them showing that they can be clung to.
//
// At the time this was written there was no way to tell the difference between
// the Hidden and Security so all players have this ability. Once player states
// are in I will adjust the script accordingly.
//
// There is no super jump atm so the script will be edited to allow for super
// jumping from a wall cling once it is available.
//****************************************************************************

public class WallCling : NetworkBehaviour
{

    [Tooltip("Player Controller Camera")]
    public Camera cam;

    [Tooltip("Maximum range Hidden can grab wall from")]
    public float range = 1;

    [Tooltip("The Player GameObject")]
    public GameObject player;

    public bool cling;      //whether player is currently clinging to a wall

    Vector3 posLock;        //the position of the player when they first cling to a wall

    void Start()
    {

        cling = false;

    }//end Start


    void Update()
    {

        if (CrossPlatformInputManager.GetButton("Cling"))
        {
            //Press key again to get off wall
            if (cling)
                cling = false;

            //Check if player is within range of wall to cling, if yes set cling to true and save current position
            else
            {
                RaycastHit ray;

                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out ray, range) && !cling && ray.transform.tag != "Nonstick")
                {

                    cling = true;
                    posLock = player.transform.position;
                }//end distance/tag check

            }//end else

        }//end if keydown

        //if cling is true keep player position where they had clinged to
        if (cling)
        {
            player.transform.position = posLock;

        }//end cling

    }//end update

}//end WallCling
