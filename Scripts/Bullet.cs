using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    public float damage;
    [SerializeField] private float releaseTime;
    private Rigidbody rb;

    private Coroutine coroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        coroutine = StartCoroutine(ReleaseAfterTime());
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    IEnumerator ReleaseAfterTime()
    {
        yield return new WaitForSeconds(releaseTime);

        BattleManager.Instance.weaponController.BulletRelease(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player") { return;  }

        if(collision.gameObject.TryGetComponent(out IDamageable hit))
        {
            hit.TakeDamage(damage);

            StopCoroutine(coroutine);

            BattleManager.Instance.weaponController.BulletRelease(gameObject);
        }
        else
        {
            StopCoroutine(coroutine);

            BattleManager.Instance.weaponController.BulletRelease(gameObject);
        }
    }
}
