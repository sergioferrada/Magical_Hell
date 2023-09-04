using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Test : MonoBehaviour
{
    //List<GameObject> enemiesPosition;
    Vector2 currentPosition;
    float originalTime, originalFixedTime;
    public float timeSlowTimer;
    bool timeSlow;
    // Start is called before the first frame update
    void Start()
    {
        timeSlowTimer = 0;
        originalTime = Time.timeScale;
        originalFixedTime = Time.fixedDeltaTime;
        timeSlow = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Reiniciar el tiempo a la normalidad
        if(timeSlowTimer <= 0 && timeSlow)
        {
            Time.timeScale = originalTime;
            Time.fixedDeltaTime = originalFixedTime;
            Camera.main.transform.position = new Vector3(0,0,-10);
            Camera.main.orthographicSize = 7f;
            gameObject.GetComponent<Collider2D>().enabled = true;
            timeSlow = false;
        }

        //Cooldown de relentizar tiempo
        if(timeSlowTimer > 0)
        {
            timeSlowTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            timeSlow = true;
            timeSlowTimer = 1.5f;
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

            currentPosition = gameObject.transform.position;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");          
            Vector2 nextPosition = enemies[Random.Range(0, enemies.Length - 1)].transform.position;

            while(currentPosition == nextPosition)
            {
                nextPosition = enemies[Random.Range(0, enemies.Length - 1)].transform.position;
            }

            gameObject.GetComponent<Collider2D>().enabled = false;

            gameObject.transform.position = nextPosition + new Vector2(0,1);
            currentPosition = nextPosition + new Vector2(0, 1);
            Camera.main.transform.position = gameObject.transform.position + new Vector3(0, 0, -10);
            Camera.main.orthographicSize = 5f;
        }
    }
}
