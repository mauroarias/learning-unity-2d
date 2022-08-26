using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;
    private float idleTimmer;
    private float idleDuration;
    public string GetStateName()
    {
        return "IdleState";
    }
    public void Execute()
    {
        Idle();
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
    public void Enter(Enemy enemy)
    {
        idleDuration = UnityEngine.Random.Range(1,5);
        this.enemy = enemy;
    }
    public void Exit()
    {

    }
    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Knife")
        {
            enemy.Target = Player.Instance.gameObject;
        }
    }

    private void Idle()
    {
        enemy.MyAnimator.SetFloat("speed", 0);
        idleTimmer += Time.deltaTime;
        if (idleTimmer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
