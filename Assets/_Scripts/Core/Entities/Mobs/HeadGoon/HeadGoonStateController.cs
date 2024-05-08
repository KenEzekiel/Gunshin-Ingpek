using System;
using UnityEngine;

// TODO: Refactor a bit as EnemyStateController
// Lotsa codes are reusable
[Serializable]
public class HeadGoonStateController : EntityStateController
{
    // Attributes
    private HeadGoon headGoon;
    [HideInInspector] public WeaponState weaponState = WeaponState.IDLE;
    public float detectionDistance = 15f;
    public float attackDistance = 5f;
    public float attackCloseDistance = 1.5f;

    // Events
    public event Action OnPlayerEnterDetectionEvent;

    // Constructor
    public void Init(HeadGoon headGoon)
    {
        this.headGoon = headGoon;
        headGoon.OnDeathEvent += OnDeath;
    }

    // Functions
    protected override int DetectState()
    {
        // Get states
        int movementState = DetectMovementState();
        int aiState = DetectAIState();
        int attackState = DetectAttackState();

        // Combine states
        state = movementState | aiState | attackState;

        return state;
    }

    // State functions
    private int DetectMovementState()
    {
        if(DetectJumping())
        {
            return HeadGoonState.JUMPING;
        }
        else if(DetectFalling())
        {
            return HeadGoonState.FALLING;
        }
        else if(DetectSprinting())
        {
            return HeadGoonState.SPRINTING;
        }
        else
        {
            return HeadGoonState.IDLE;
        }
    }
    private int DetectAIState()
    {
        if(GameController.Instance.player.Dead)
        {
            headGoon.aiController.nav.speed = headGoon.aiController.patrolSpeed;
            return HeadGoonState.AI_PATROL_STATE;
        }

        if(Vector3.Distance(headGoon.Position, GameController.Instance.player.Position) < attackCloseDistance)
        {
            return HeadGoonState.AI_IN_RANGE_CLOSE_STATE;
        }
        else if(Vector3.Distance(headGoon.Position, GameController.Instance.player.Position) < attackDistance)
        {
            return HeadGoonState.AI_IN_RANGE_STATE;
        }
        else if(Vector3.Distance(headGoon.Position, GameController.Instance.player.Position) < detectionDistance)
        {
            if(HeadGoonState.GetAIState(state) < HeadGoonState.AI_DETECTED_STATE)
            {
                headGoon.audioController.Play(HeadGoon.AUDIO_CRY_KEY);
            }
            headGoon.aiController.nav.speed = headGoon.Speed;
            return HeadGoonState.AI_DETECTED_STATE;
        }
        else
        {
            headGoon.aiController.nav.speed = headGoon.aiController.patrolSpeed;
            return HeadGoonState.AI_PATROL_STATE;
        }
    }
    private int DetectAttackState()
    {
        int attackState = 0;
        if(DetectAttacking())
        {
            AttackType attackType = weaponState switch
            {
                WeaponState.ATTACK => headGoon.Weapon.attackType,
                WeaponState.ALTERNATE_ATTACK => headGoon.Weapon.alternateAttackType,
                _ => AttackType.NULL
            };

            attackState = attackType switch
            {
                AttackType.RANGED => KingState.ATTACK_RANGED,
                AttackType.MELEE => KingState.ATTACK_MELEE,
                _ => HeadGoonState.NULL
            };
        }
        return attackState;
    }
    public void ClearWeaponState()
    {
        weaponState = WeaponState.IDLE;
    }
    public void SetWeaponState(WeaponState state)
    {
        weaponState = state;
    }
    private bool DetectSprinting()
    {
        return headGoon.aiController.nav.velocity.magnitude > 0.5;
    }
    private bool DetectJumping()
    {
        return !headGoon.Grounded && headGoon.Rigidbody.velocity.y > 0;
    }
    private bool DetectFalling()
    {
        return !headGoon.Grounded && headGoon.Rigidbody.velocity.y < 0;
    }
    private bool DetectAttacking()
    {
        return !headGoon.Weapon.CanAttack;
    }
    private void OnDeath()
    {
        state = HeadGoonState.DEAD;
    }


    // Debugging functions
    public void VisualizeDetection(MonoBehaviour monoBehaviour)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(monoBehaviour.transform.position, detectionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(monoBehaviour.transform.position, attackDistance);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(monoBehaviour.transform.position, attackCloseDistance);
    }

    public void VisualizePatrolRoute(HeadGoon headGoon)
    {
        if(headGoon.aiController.patrolRoute.Count > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < headGoon.aiController.patrolRoute.Count - 1; i++)
            {
                Gizmos.DrawLine(headGoon.aiController.patrolRoute[i].position, headGoon.aiController.patrolRoute[i + 1].position);
            }
            Gizmos.DrawLine(headGoon.aiController.patrolRoute[^1].position, headGoon.aiController.patrolRoute[0].position);
        }
    }
}
