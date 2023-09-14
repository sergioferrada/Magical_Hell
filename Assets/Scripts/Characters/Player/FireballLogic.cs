using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLogic : ProjectileLogic
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gameObject.name = "Fireball";
        //Se selecciona una direccion aleatoria alrededor del jugador
        direction = Random.insideUnitCircle.normalized;
        rb2d.velocity = speed * direction;
    }

    public void ExploteInMore()
    {
        //Se crean entre 4 a 7 nuevos proyectiles
        for (int i = 0; i < Random.Range(4, 7); i++)
        {
            GameObject projectile = Instantiate(gameObject, transform.position, Quaternion.Euler(direction));
            //Reduce el tamaño de los projectiles cada vez que se elimna a un enemigo
            projectile.transform.localScale = projectile.transform.localScale * 0.75f;
            //Aumenta el daño en uno cada vez que se eleimina un enemigo
            //projectile.GetComponent<FireballLogic>().damage = damage + 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        rb2d.velocity = new Vector2(0,0);
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, .4f);
    }
}
