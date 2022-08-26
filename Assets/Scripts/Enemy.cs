using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private IEnemyState currentState;
    public GameObject Target { get; set; }
    [SerializeField]    
    private float meleeRange;
    [SerializeField]    
    private float throwRange;
    [SerializeField]    
    private Transform leftEdge;
    [SerializeField]    
    private Transform rightEdge;
    private Vector3 startPos;
    public bool InMeleeRange 
    { 
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        } 
    }
    public bool InThrowRange 
    { 
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }
            return false;
        } 
    }
    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
    }

    public void Update() 
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();  
            }
            LookAtTarget();  
        }
    }

    public void ChangeState(IEnemyState newState) 
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void Move() 
    {
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                MyAnimator.SetFloat("speed", 1);
                transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
            }
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    } 
    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if ((xDir < 0 && facingRight) || (xDir > 0 && !facingRight))
            {
                ChangeDirection();
            }
        }
    }

    public override IEnumerator TakeDamage() 
    {
        health -= 10;
        Debug.Log("taking");
        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
        // MyAnimator.SetTrigger("idle");
        // MyAnimator.ResetTrigger("die");
        // health = 30;
        // transform.position = startPos;
    }
}
