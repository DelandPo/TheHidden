using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    public float damageStrength = 5.0f;
    public float attactDistance = 1.0f;
    public bool DebugMode = true;
    private Camera cam;
    private GameObject attactLocator;
    private GameObject tran;
    private int count = 0;
    private int angle = 10;
    private bool clawed;
    

    private void Start()
    {
        cam = Camera.main;
        attactLocator = new GameObject("locator");
        tran = Instantiate(attactLocator, cam.transform.position, Quaternion.Euler((cam.transform.right)), cam.transform);
    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;

        Ray attackRay = new Ray(transform.position, tran.gameObject.transform.right);

        if (Input.GetButton("Fire1"))
        {
            clawed = true;
        }

        if(clawed && count <= 20)
        {
            tran.gameObject.transform.Rotate(Vector3.up * -angle);
            count++;
        }
        if(count == 20)
        {
            clawed = false;
        }
        if(clawed == false && count > 0)
        {
            tran.gameObject.transform.Rotate(Vector3.up * angle);
            count--;
        }

        if (DebugMode)
        {
            Debug.DrawRay(transform.position, tran.gameObject.transform.right);
        }

        if(Physics.Raycast(attackRay, out hit, attactDistance))
        {
            if(hit.collider.name == "Security")
            {
                print("That's a hit!!");
            }
        }
	}
}