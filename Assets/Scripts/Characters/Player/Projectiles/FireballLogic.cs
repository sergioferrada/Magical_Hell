using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLogic : ProjectileLogic
{
    [SerializeField] public int timesItcanExplode;
    [SerializeField] private int minFireballRange, maxFireballRange;

    public int timesExploded;

    // Start is called before the first frame update
    protected override void Start()
    {
        //Se selecciona una direccion aleatoria alrededor del jugador
        direction = Random.insideUnitCircle.normalized;
        base.Start();
        gameObject.name = "Fireball";
    }

    public void ExploteInMore()
    {
        if (timesExploded < timesItcanExplode)
        {   
            for (int i = 0; i < Random.Range(minFireballRange, maxFireballRange); i++)
            {
                GameObject projectile = Instantiate(gameObject, transform.position, Quaternion.Euler(direction));
                var fireballScript = projectile.gameObject.GetComponent<FireballLogic>();

                fireballScript.timesExploded += 1;
                fireballScript.Damage = Damage * .5f;
                projectile.transform.localScale *= .5f;
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        //Cuando colisione con un objeto en el layer de "Enemies"
        if (collision.gameObject.layer == 7)
        {
            //Si el objeto colisonado tiene menos o la misma cantidad de vida que el daño
            if (collision.gameObject.GetComponent<Enemy>().Life <= Damage)
                ExploteInMore();
                   
        }
        animator.Play("Fireball_Explosion");
    }

    private void Explote()
    {
        rb2d.velocity = Vector2.zero;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, .4f);
    }
}
