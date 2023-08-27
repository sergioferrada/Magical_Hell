using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float life;
    private Animator animator;
    // Start is called before the first frame update

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ReciveDamage(float damage)
    {
        animator.SetTrigger("Injured");
        life -= damage;
        CheckDeath();
    }

    void CheckDeath()
    {
        if (life <= 0)
        {
            animator.SetBool("Death", true);
            GameManager.Instance.UpdateEnemiesArray();
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, .75f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if (collision.gameObject.name == "Fireball")
            ReciveDamage(collision.gameObject.GetComponent<ProjectileLogic>().damage);
        

        //CheckDeath();
    }
}
