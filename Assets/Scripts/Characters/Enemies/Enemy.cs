using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : CharacterBase
{
    #region COMPONENETES
    protected Transform playerTransform;
    #endregion

    [SerializeField]
    protected float chaseDistance = 0;

    override protected void Awake()
    {
        base.Awake();
        playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();
    }

    protected virtual void ShortRangeAttack()
    {
        Collider2D[] hitCharacters = Physics2D.OverlapCircleAll(transform.position, attackRange, attackableLayers);
        foreach (Collider2D character in hitCharacters)
        {
            character.GetComponent<PlayerController>().ReciveDamage(Damage);
            Vector2 direction = (character.GetComponent<Collider2D>().transform.position - transform.position).normalized;
            character.GetComponent<Rigidbody2D>().AddForce(direction * 25, ForceMode2D.Impulse);
        }
    }

    protected virtual void MidRangeAttack()
    {

    }

    protected virtual void LongRangeAttack()
    {

    }

    protected void ResetAttack()
    {
        passedTime = 0;
    }

    protected void ChaseGameObject(Transform objectiveTransform)
    {
        //chasing the player
        ChangeState(State.Move);
        Vector2 direction = objectiveTransform.position - transform.position;
        direction.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, objectiveTransform.position, movementSpeed * Time.deltaTime);

        //Se redondean las coordenadas del vector para solo obtener valores de 1 o -1
        if(direction.x > 0)
            transform.localScale = new Vector2(1, transform.localScale.y);
        else if (direction.x < 0)
            transform.localScale = new Vector2(-1, transform.localScale.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (transform == null)
            return;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {   
        if (collision.gameObject.name == "Fireball")
            ReciveDamage(collision.gameObject.GetComponent<ProjectileLogic>().Damage);
    }
}
