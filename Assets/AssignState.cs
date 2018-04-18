using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class AssignState : NetworkBehaviour
{

    // Server assigns the state of player prefab
    
    [SyncVar(hook = "playerState")]
    public static int state;

    [SyncVar(hook = "consumedState")]
    public static bool consumed = false;

    void Update()
    {
        if(consumed == true)
        {
            System.Random rnd = new System.Random();
            state = rnd.Next(0, 2);
        }
    }


}

