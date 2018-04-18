using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class Shooting : NetworkBehaviour
{
    //Determines active weapon
    [Tooltip("pistol = 1; rifle = 2; shotgun = 3")]
    public int weaponEquipped = 1;
    [Tooltip("Range of pistol")]
    public float pistolRange = 100;
    [Tooltip("Range of rifle")]
    public float rifleRange = 100;
    [Tooltip("Range of shotgun")]
    public float shotgunRange = 100;

    //Raycast stuff
    GameObject hitObject;
    private Quaternion hitObjectRotation;

    //Visual FX
    public Camera playerCamera; //Camera attached to player prefab
    private GameObject bulletImpact;
    [Tooltip("Prefab for pistol bullet impact. Instantiated when the pistol raycast hits something.")]
    public GameObject pistolBulletImpactPrefab;
    [Tooltip("How long the pistol bullet impact remains in the scene.")]
    public float pistolBulletImpactLifetime = 0.5f;
    [Tooltip("Prefab for rifle bullet impact. Instantiated when the rifle raycast hits something.")]
    public GameObject rifleBulletImpactPrefab;
    [Tooltip("How long the rifle bullet impact remains in the scene.")]
    public float rifleBulletImpactLifetime = 0.5f;
    [Tooltip("Prefab for shotgun bullet impact. Instantiated when a shotgun raycast hits something.")]
    public GameObject shotgunBulletImpactPrefab;
    [Tooltip("How long a shotgun bullet impact remains in the scene.")]
    public float shotgunBulletImpactLifetime = 0.5f;
    [Tooltip("Multiplier for how wide of an angle a shotgun shot can be at.")]
    public float shotgunSpread = 0.75f;
    [Tooltip("Number of shots fired by the shotgun.")]
    public int shotgunShots = 8;

    [Command]
    //Fires equipped weapon
    private void CmdFireWeapon()
    {
        if (weaponEquipped == 1) //Pistol
        {
            RaycastHit hit;
            Vector3 hitPoint = Vector3.zero;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pistolRange)) //Shoot raycast down the forward vector of the main camera; range is pistolRange
            {
                //Get specs about object that was hit
                hitPoint = hit.point;
                Collider hitCol = hit.collider;
                hitObject = hitCol.gameObject;
                hitObjectRotation = hitObject.transform.rotation;

                checkRaycast(hitCol); //Check if Hidden was hit

                //Makes visual FX visible
                if (pistolBulletImpactPrefab != null)
                {
                    bulletImpact = Instantiate(pistolBulletImpactPrefab, hitPoint, hitObjectRotation); //Instantiate pistolBulletImpactPrefab at hitPoint
                    StartCoroutine(destroyVisualFX(bulletImpact, pistolBulletImpactLifetime)); //Destroys a visual effect after a delay
                    NetworkServer.Spawn(bulletImpact);
                }
            }
        }
        else if (weaponEquipped == 2) //Rifle
        {
            RaycastHit hit;
            Vector3 hitPoint = Vector3.zero;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rifleRange)) //Shoot raycast down the forward vector of the player camera; range is rifleRange
            {
                //Get specs about object that was hit
                hitPoint = hit.point;
                Collider hitCol = hit.collider;
                hitObject = hitCol.gameObject;
                hitObjectRotation = hitObject.transform.rotation;

                checkRaycast(hitCol); //Check if Hidden was hit

                //Makes visual FX visible
                if (rifleBulletImpactPrefab != null)
                {
                    bulletImpact = Instantiate(rifleBulletImpactPrefab, hitPoint, hitObjectRotation); //Instantiate rifleBulletImpactPrefab at hitPoint
                    StartCoroutine(destroyVisualFX(bulletImpact, rifleBulletImpactLifetime)); //Destroys a visual effect after a delay
                    NetworkServer.Spawn(bulletImpact);
                }
            }
        }
        else if (weaponEquipped == 3) //Shoots multiple raycasts for shotgun 
        {
            RaycastHit mainHit;
            Vector3 mainHitPoint;

            //Shoot raycast down the forward vector of the player camera; range is shotgunRange
            //Used as a reference for the other shotgun shots
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out mainHit, shotgunRange)) //Fire main shot
            {
                mainHitPoint = mainHit.point;
                Collider hitCol = mainHit.collider;
                hitObject = hitCol.gameObject;
                hitObjectRotation = hitObject.transform.rotation;

                //Makes visual FX visible
                bulletImpact = Instantiate(shotgunBulletImpactPrefab, mainHitPoint, hitObjectRotation);
                StartCoroutine(destroyVisualFX(bulletImpact, shotgunBulletImpactLifetime)); //Destroys a visual effect after a delay
                NetworkServer.Spawn(bulletImpact);

                checkRaycast(hitCol); //Check if Hidden was hit

                for (int i = 0; i < shotgunShots - 1; i++) //Fire other shotgun shots (number specified by (shotgunShots - 1) to account for main shot
                {
                    Vector3 randomPoint = Random.insideUnitCircle * shotgunSpread; //Calculate random point within the unit circle
                    Vector3 newHitPoint = mainHitPoint + randomPoint; //Add the random point to the mainHitPoint
                    Vector3 hitPoint = Vector3.zero;
                    RaycastHit hit;

                    //Shoot a raycast in the direction of the newHitPoint; range is shotgunRange
                    if (Physics.Raycast(playerCamera.transform.position, newHitPoint - playerCamera.transform.position, out hit, shotgunRange))
                    {
                        //Get specs about object that was hit
                        hitPoint = hit.point;
                        hitCol = hit.collider;
                        hitObject = hitCol.gameObject;
                        hitObjectRotation = hitObject.transform.rotation;

                        //Makes visual FX visible
                        if (shotgunBulletImpactPrefab != null)
                        {
                            bulletImpact = Instantiate(shotgunBulletImpactPrefab, hitPoint, hitObjectRotation); //Instantiate shotgunBulletImpactPrefab at hitPoint
                            StartCoroutine(destroyVisualFX(bulletImpact, shotgunBulletImpactLifetime)); //Destroys a visual effect after a delay
                            NetworkServer.Spawn(bulletImpact);
                        }
                    }

                    checkRaycast(hitCol); //Check if Hidden was hit
                }
            }
        }
    }

    //Checks if the Hidden was hit (requires that Hidden to be tagged as "Hidden"
    private void checkRaycast(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Health>().DecreaseHealth(25);
            //Damage Hidden
            Debug.Log("Damaged the Hidden");
        }
    }

    //Destroys visual FX after specified time to clear up the scene and prevent unintended FX from showing up
    private IEnumerator destroyVisualFX(GameObject effect, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(effect);
    }

    void Start()
    {

    }

    //Using fixed update to keep fire rate independent of player's framerate
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            //Fires pistol and shotgun (semiautomatic)
            if (Input.GetMouseButtonDown(0)) //Left Click
            {
                CmdFireWeapon();
            }
            if (Input.GetMouseButton(0) && weaponEquipped == 2) //Left mouse button down && the rifle is equipped (automatic fire)
            {
                CmdFireWeapon();
            }
        }
    }
}