using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GhoulSpawner : MonoBehaviour
{
    public GameObject ghoul;
    public GameObject ghoulExplosion;
    public ObjectPool<GameObject> ghoulPool;
    public ObjectPool<GameObject> ghoulExplosionPool;
    [SerializeField] private float ghoulSpawnTime;

    private void Awake()
    {
        //initiate a new pool
        ghoulPool = new ObjectPool<GameObject>(() => Instantiate(ghoul), ghoul => { }, ghoul => { }, ghoul => Destroy(ghoul), true, 300, 10000);

        ghoulExplosionPool = new ObjectPool<GameObject>(() => Instantiate(ghoulExplosion), ghoulExplosion => { }, ghoulExplosion => { }, ghoulExplosion => Destroy(ghoulExplosion), true, 300, 10000);
    }

    private void Start()
    {
        StartCoroutine(GhoulSpawningCycle());
    }

    IEnumerator GhoulSpawningCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(ghoulSpawnTime);

            GameObject newGhould = ghoulPool.Get();
            ActivateOnTakeFromPool(newGhould);

            float randomPosX = Random.Range(-10f, 10f);
            float randomPosY = Random.Range(0f, 0.5f);
            float randomPosZ = Random.Range(-10f, 10f);

            float randomRotY = Random.Range(0f, 360f);

            newGhould.transform.position = transform.position + new Vector3(randomPosX, randomPosY, randomPosZ);
            newGhould.transform.rotation = transform.rotation * Quaternion.Euler(0, randomRotY, 0);
        }
    }

    public void DeactivateOnReturnedToPool(GameObject item)
    {
        item.SetActive(false);
    }
    public void ActivateOnTakeFromPool(GameObject item)
    {
        item.SetActive(true);
    }
}
