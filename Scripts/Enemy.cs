using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    public float speed;
    public float damage;

    private RichAI richAi;
    private AIDestinationSetter aIDestinationSetter;

    [SerializeField] private protected Rigidbody rb;
    [SerializeField] private protected Animator anim;
    [SerializeField] private protected BattleManager battleManager;

    [SerializeField] private protected List<Collider> meleeAttackColliders;
    [SerializeField] private protected bool isAttackingMelee = false;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Dying();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        richAi = GetComponent<RichAI>();
        battleManager = BattleManager.Instance;
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Start()
    {
        //set the speed of enemy
        richAi.maxSpeed = speed;
        //set the player as a target
        if(battleManager.player != null)
        {
            aIDestinationSetter.target = battleManager.player.transform;
        }
    }

    private void Update()
    {
        MoveAnimations();
        MeleeAttack();
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public virtual void MoveAnimations()
    {
        if (rb.velocity.magnitude > 0)
        {
            anim.SetFloat("VerticalMovement", 1);
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetFloat("VerticalMovement", 0);
            anim.SetBool("IsMoving", false);
        }
    }

    public virtual void MeleeAttack()
    {
        if (battleManager.player == null) return;

        MeleeAttackAnimations(2f);
        MeleeAttackDamage(2f, battleManager.playerComponent);
    }

    public virtual void MeleeAttackAnimations(float distance)
    {
        if (Vector3.Distance(battleManager.playerTransform.position, transform.position) < distance)
        {
            anim.SetBool("IsAttacking", true);
        }
        else
        {
            anim.SetBool("IsAttacking", false);
        }
    }

    public virtual void MeleeAttackDamage(float distance, Player player)
    {
        if(isAttackingMelee == true)
        {
            if (Vector3.Distance(battleManager.playerTransform.position, transform.position) < distance)
            {
                if (player.TryGetComponent(out IDamageable hit))
                {
                    isAttackingMelee = false;
                    hit.TakeDamage(damage);
                }
            }
        }
    }

    public void MeleeHitAnimationEventOn()
    {
        isAttackingMelee = true;
    }

    public void MeleeHitAnimationEventOff()
    {
        isAttackingMelee = false;
    }

    public virtual void Dying()
    {
        Destroy(gameObject);
    }
}
