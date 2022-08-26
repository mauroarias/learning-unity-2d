using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    private float attackTimer;
    private float attackCoolDown = 3;
    private bool canAttack = true;
    private Enemy enemy;
    public string GetStateName()
    {
        return "MeleeState";
    }
    public void Execute()
    {
        Attack();
        if (enemy.InThrowRange && !enemy.InMeleeRange)
        {
            enemy.ChangeState(new RangeState());
        } else if (enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void Exit()
    {

    }
    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCoolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }
        if (canAttack)
        {
            enemy.MyAnimator.SetTrigger("attack");
            canAttack = false;
        }
    }
}
