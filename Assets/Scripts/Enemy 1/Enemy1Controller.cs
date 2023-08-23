using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    public float life;
    // Start is called before the first frame update

    public void ReciveDamage(float damage)
    {
        life -= damage;

        CheckDeath();
    }

    void CheckDeath()
    {
        if (life <= 0)
        {
            GameManager.Instance.UpdateEnemiesArray();
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Fireball")
            ReciveDamage(collision.gameObject.GetComponent<ProjectileLogic>().damage);

        //CheckDeath();
    }
}
