using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosion : BloodExplosion
{
    private Player player;

    private void Awake()
    {
        player = battleManager.playerComponent;
    }

    public override IEnumerator WaitAndRelease()
    {
        yield return new WaitForSeconds(3);
        player.playerExplosionPool.Release(gameObject);
        player.DeactivateOnReturnedToPool(gameObject);
    }
}
