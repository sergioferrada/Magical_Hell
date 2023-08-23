using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLogic : ProjectileLogic
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Fireball";
        rb = GetComponent<Rigidbody2D>();     
        //Se selecciona una direccion aleatoria lrededor del jugador
        direction = Random.insideUnitSphere;
        direction = direction.normalized;
        rb.velocity = speed * direction;
    }

    public void ExploteInMore()
    {
        //Se crean entre 4 a 7 nuevos proyectiles
        for (int i = 0; i < Random.Range(4, 7); i++)
        {
            GameObject projectile = Instantiate(this.gameObject, transform.position, Quaternion.identity);
            //Reduce el tamaño de los projectiles cada vez que se elimna a un enemigo
            projectile.transform.localScale = projectile.transform.localScale * 0.75f;
            //Aumenta el daño en uno cada vez que se eleimina un enemigo
            ////projectile.gameObject.GetComponent<ProjectileLogic>().damage = damage + 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Cuando colisione con un objeto en el layer de "Enemies"
        if (collision.gameObject.layer == 7)
        {
            //Si el objeto colisonado tiene menos o la misma cantidad de vida que el daño
            if (collision.gameObject.GetComponent<Enemy1Controller>().life <= damage)
                ExploteInMore();
                   
        }
        Destroy(gameObject);
    }
}
