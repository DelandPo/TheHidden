using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class AssignState : NetworkBehaviour
{

    // Server assigns the state of player prefab
    
    [SyncVar(hook = "playerState")]
    public static bool hiddenState;

    void Update()
    {
        if(hiddenState == true)
        {
            hiddenState = false;
        }
    }


}

