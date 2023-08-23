using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeanAbilitie : MonoBehaviour
{
    public GameObject bean;
    public float cooldown;

    private void Start()
    {
        cooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0)
            cooldown -= Time.deltaTime;

        if (cooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Instantiate(bean, transform.position, Quaternion.identity);
                //Instantiate(bean, transform.position, Quaternion.identity);

                cooldown = 15;
            }
        }
    }
}
