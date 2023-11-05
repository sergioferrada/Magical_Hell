using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAreaLogic : MonoBehaviour
{
    private Animator animator;

    [SerializeField] public float Damage, attackRange;
    [SerializeField] private LayerMask attackableLayers;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Default_Animation");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void ActivateAttackArea()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, attackableLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().ReciveDamage(Damage);
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
