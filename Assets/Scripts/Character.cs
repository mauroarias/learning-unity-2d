using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected Transform knifePosition;
    [SerializeField]
    protected float movementSpeed;
    protected bool facingRight;
    [SerializeField]
    private GameObject knifePrefab;
    [SerializeField]
    protected int health;
    [SerializeField]
    private EdgeCollider2D swordCollider;
    [SerializeField]
    private List<string> damageSources;
    public abstract bool IsDead { get; }
    public bool TakingDamage { get; set; }
    public bool Attack { get; set; }
    public Animator MyAnimator {get; private set; }

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    public abstract IEnumerator TakeDamage(); 
    public abstract void Death();

    public virtual void Start()
    {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
    }

    public void ChangeDirection() 
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void ThrowKnife(int value) 
    {
    if (facingRight)
        {
            GameObject tmp = Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0,0,-90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0,0,90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = !SwordCollider.enabled;
    }

    public virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if (damageSources.Contains(other.tag))  
        {
            StartCoroutine(TakeDamage());
        }  
    }
}
