using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof (AudioSource))]
[RequireComponent(typeof (NetworkIdentity))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Rigidbody))]
public class Mine : NetworkBehaviour {
    [Tooltip("Sound the mine will play when the hidden is within a certian distance.")]
    public AudioClip beepSound;
    AudioSource audioSource;
    bool playingSound = false;
    [Tooltip("How long between beeps when the mine is triggered.")]
    public float beepInterval = 2f;
    [Tooltip("Distance at which mine sound will play.")]
    public float triggerDistance;
    GameObject[] hiddenPlayers; //Will probably be only 1 player
    bool triggered = false; //Mine has been triggered
    NetworkTransform nTransform;


    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = beepSound;
        GetComponent<Rigidbody>().useGravity = true;
        nTransform = GetComponent<NetworkTransform>();
        nTransform.sendInterval = 1f;
        nTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
        hiddenPlayers = GameObject.FindGameObjectsWithTag("Hidden");
	}

    void checkDistance()
    {
        if (!playingSound)
        {
            triggered = false;
            for (int i = 0; i < hiddenPlayers.Length; i++)
            {
                if (Vector3.Distance(hiddenPlayers[i].transform.position, transform.position) < triggerDistance)
                {
                    triggered = true;
                    break;
                }
            }
        }
    }

    //Play beep sound
    IEnumerator beep()
    {
        playingSound = true;
        if (beepSound != null)
        {
            audioSource.Play();
        }
        yield return new WaitForSeconds(beepInterval);
        playingSound = false;
    }
	
	// Update is called once per frame
	void Update () {
        checkDistance();

		if (triggered && !playingSound)
        {
            StartCoroutine(beep());
        }
	}
}
