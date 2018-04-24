using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class Throwing : NetworkBehaviour {

    public GameObject mine;

    public GameObject grenade;

    public Camera cam;

    [Tooltip("Force behind throwing grenade and mine")]
    public float force = 40f;

    GameObject throwable;

    Vector3 grenadePos;

	void FixedUpdate () {

        if (CrossPlatformInputManager.GetButtonDown("Throw"))
        {

            if (gameObject.tag == "Hidden")    //If player is Hidden
            {
                throwable = Instantiate(grenade, cam.transform.position, Quaternion.Euler(cam.transform.forward));
                Rigidbody rb = throwable.GetComponent<Rigidbody>();
                rb.AddForce(cam.transform.forward * force);
            }
            else     //If player is security
            {
                throwable = Instantiate(mine, cam.transform.position, Quaternion.Euler(cam.transform.forward));
                Rigidbody rb = throwable.GetComponent<Rigidbody>();
                rb.AddForce(cam.transform.forward * force);
            }
            
        }//end keydown

    }//end Update

}//end Throwing
