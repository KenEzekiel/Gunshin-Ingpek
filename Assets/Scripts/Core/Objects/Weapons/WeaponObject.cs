using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponObject : MonoBehaviour 
{
    // Attributes
    protected AttackObjectType bearerType;
    protected IArmed bearer;
    [SerializeField] protected float BaseDamage;
    [SerializeField] protected float KnockbackPower;
    [SerializeField] protected float AttackInterval;
    [SerializeField] protected float AlternateAttackInterval;
    [SerializeField] private bool canAttack;

    // Events
    public event Action OnAttackEvent;
    public event Action OnAlternateAttackEvent;

    // Constructor
    protected void Start()
    {
        bearer = GetComponentInParent<IArmed>();
        
        // Switch requires a constant, so can't use that here
        if(bearer is Player)
        {
            bearerType = AttackObjectType.PLAYER;
        }
        else if(bearer is EnemyEntity)
        {
            bearerType = AttackObjectType.ENEMY;
        } 
        else
        {
            bearerType = AttackObjectType.ENVIRONMENT;
        } 

        canAttack = true;
        OnAttackEvent += OnAttack;
        OnAlternateAttackEvent += OnAlternateAttack;
    }

    // Functions
    public virtual bool Attack()
    {
        if(!canAttack)
        {
            return false;
        }
        
        canAttack = false;
        StartCoroutine(DelayAttack(AttackInterval));
        OnAttackEvent?.Invoke();

        return true;
    }

    public virtual bool AlternateAttack()
    {
        if(!canAttack)
        {
            return false;
        }

        canAttack = false;
        StartCoroutine(DelayAttack(AlternateAttackInterval));
        OnAlternateAttackEvent?.Invoke();
        
        return true;
    }
    
    private IEnumerator DelayAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    // Abstract Functions
    protected abstract void OnAttack();
    protected abstract void OnAlternateAttack();
}
