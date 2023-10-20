using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    public float maxEnergy;
    public float energy;

    [SerializeField] private GameObject deathParticle;
    public ObjectPool<GameObject> playerExplosionPool;

    private void OnEnable()
    {
        health = maxHealth;
        energy = maxEnergy;
    }

    private void Awake()
    {
        health = maxHealth;
        energy = maxEnergy;

        playerExplosionPool = new ObjectPool<GameObject>(() => Instantiate(deathParticle), deathParticle => { }, deathParticle => { }, deathParticle => Destroy(deathParticle), true, 5, 25);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        GameObject newDeathParticle = playerExplosionPool.Get();
        ActivateOnTakeFromPool(newDeathParticle);
        newDeathParticle.transform.position = transform.position + new Vector3(0, 1, 0);
        newDeathParticle.transform.rotation = Quaternion.identity;

        // +++ need improvement to pooling +++
        Destroy(gameObject);
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
