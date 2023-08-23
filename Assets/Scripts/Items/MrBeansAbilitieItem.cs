using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeansAbilitieItem : MonoBehaviour
{
    public GameObject bean;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.AddComponent<MrBeanAbilitie>();
            collision.gameObject.GetComponent<MrBeanAbilitie>().bean = bean;
            Destroy(gameObject);
        }
        
    }
}
