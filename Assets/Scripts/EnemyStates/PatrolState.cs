using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimmer;
    private float patrolDuration;
    public string GetStateName()
    {
        return "PatrolState";
    }
    public void Execute()
    {
        Patrol();
        enemy.Move();
        if (enemy.Target != null && enemy.InThrowRange)
        {
            enemy.ChangeState(new RangeState());
        } 
    }
    public void Enter(Enemy enemy)
    {
        patrolDuration = UnityEngine.Random.Range(3,10);
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
    private void Patrol()
    {
        patrolTimmer += Time.deltaTime;
        if (patrolTimmer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
