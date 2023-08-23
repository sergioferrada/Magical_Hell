using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbilitieItem : MonoBehaviour
{
    public GameObject projectileAbilitie;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { 

            collision.gameObject.AddComponent<FireballAttackAbilitie>();
            collision.gameObject.GetComponent<FireballAttackAbilitie>().fireball = projectileAbilitie;
            Destroy(gameObject);
        }
        
    }
}
