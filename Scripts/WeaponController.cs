using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform muzzle;
    public Transform gun;

    public GameObject bullet;
    public ObjectPool<GameObject> bulletPool;

    private bool fireing;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;
    [SerializeField] private float damage = 10f;

    private void Awake()
    {
        //initiate a new pool
        bulletPool = new ObjectPool<GameObject>(() => Instantiate(bullet), bullet => { }, bullet => { }, bullet => Destroy(bullet), true, 300, 1000);
        bullet.GetComponent<Bullet>().damage = damage;
    }

    private void Update()
    {
        Fire();
        AdjustGunRotation();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        //get if fire input was cancelled or else allow fireing
        if (context.canceled)
            fireing = false;
        else
            fireing = true;
    }

    void Fire()
    {
        //if input is stopped then do not execute code
        if (fireing == false) return;

        //if statement for rate of fire implementation
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            //get the gameobject from the pool
            GameObject newBullet = bulletPool.Get();
            ActivateOnTakeFromPool(newBullet);

            // randomize position and angles for spread of fire
            float randomPosX = Random.Range(-0.05f, 0.05f);
            float randomPosZ = Random.Range(-0.05f, 0.05f);
            float randomRotX = Random.Range(-1.7f, 1.7f);
            float randomRotY = Random.Range(-1.7f, 1.7f);
            float randomRotZ = Random.Range(-1.7f, 1.7f);

            //set the position and rotation of the gameobject
            newBullet.transform.position = muzzle.position + new Vector3(randomPosX, 0, randomPosZ);
            newBullet.transform.rotation = muzzle.rotation * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(randomRotX, randomRotZ, randomRotY);
        }
    }

    void AdjustGunRotation()
    {
        //rotate a rigidbody torwards a mouse position using raycast but without rotating on y axis
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, 1000))
        {
            //offset the aiming point when aiming at the environmenet layer
            if (hit.transform.gameObject.layer == 10)
            {
                gun.LookAt(hit.point + new Vector3(0, 0.5f, 0));
            }
            //shoot directly at the enemy
            else
            {
                gun.LookAt(hit.point);
            }
        }
    }

    public void BulletRelease(GameObject bullet)
    {
        if (!bullet.activeInHierarchy) { return; }

        bulletPool.Release(bullet);
        DeactivateOnReturnedToPool(bullet);
    }

    public void DeactivateOnReturnedToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
    public void ActivateOnTakeFromPool(GameObject bullet)
    {
        bullet.SetActive(true);
    }
}
