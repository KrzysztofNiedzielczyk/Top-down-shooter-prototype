using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodExplosion : MonoBehaviour
{
    private protected BattleManager battleManager;

    private void Awake()
    {
        battleManager = BattleManager.Instance;
    }

    void OnEnable()
    {
        StartCoroutine(WaitAndRelease());
    }

    public virtual IEnumerator WaitAndRelease()
    {
        yield return new WaitForSeconds(3);
        battleManager.ghoulSpawner.ghoulExplosionPool.Release(gameObject);
        battleManager.ghoulSpawner.DeactivateOnReturnedToPool(gameObject);
    }
}
