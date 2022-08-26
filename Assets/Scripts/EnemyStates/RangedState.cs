using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeState : IEnemyState
{
    private Enemy enemy;

    private float throwTimer;
    private float throwCoolDown = 3;
    private bool canThrow = true;
    public string GetStateName()
    {
        return "RangedState";
    }
    public void Execute()
    {
        ThrowKnife();
        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
        if (enemy.Target != null)
        {
            enemy.Move();
        }
        else
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

    private void ThrowKnife()
    {
        throwTimer += Time.deltaTime;
        if (throwTimer >= throwCoolDown)
        {
            canThrow = true;
            throwTimer = 0;
        }
        if (canThrow)
        {
            enemy.MyAnimator.SetTrigger("throw");
            canThrow = false;
        }
    }
}
