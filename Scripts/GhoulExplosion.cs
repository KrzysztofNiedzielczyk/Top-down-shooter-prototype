using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulExplosion : BloodExplosion
{
    public override IEnumerator WaitAndRelease()
    {
        yield return new WaitForSeconds(3);
        battleManager.ghoulSpawner.ghoulExplosionPool.Release(gameObject);
        battleManager.ghoulSpawner.DeactivateOnReturnedToPool(gameObject);
    }
}
