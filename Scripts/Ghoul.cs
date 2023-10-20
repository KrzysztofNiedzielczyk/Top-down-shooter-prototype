using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : Enemy
{
    public override void Dying()
    {
        GameObject newDeathParticle = BattleManager.Instance.ghoulSpawner.ghoulExplosionPool.Get();
        battleManager.ghoulSpawner.ActivateOnTakeFromPool(newDeathParticle);
        newDeathParticle.transform.position = transform.position + new Vector3(0, 1, 0);
        newDeathParticle.transform.rotation = Quaternion.identity;

        battleManager.ghoulSpawner.ghoulPool.Release(gameObject);
        battleManager.ghoulSpawner.DeactivateOnReturnedToPool(gameObject);
    }
}
