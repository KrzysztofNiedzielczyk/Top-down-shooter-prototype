using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public GameObject player;
    public Player playerComponent;
    public Transform playerTransform;
    public WeaponController weaponController;
    public GhoulSpawner ghoulSpawner;

    private void Awake()
    {
        ghoulSpawner = GameObject.FindGameObjectWithTag("GhoulSpawner").GetComponent<GhoulSpawner>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        playerComponent = player.GetComponent<Player>();
        weaponController = player.GetComponent<WeaponController>();
    }
}
