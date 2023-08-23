using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    float timePerRoom;
    float totalLifePointsInRoom;
    float abilitiesUsedPerRoom;

    int enemiesInScene;
    bool roomEnded;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        timePerRoom = 0;
        enemiesInScene = FindObjectsOfType<Enemy1Controller>().Length;
        roomEnded = false;
        print("Enemigos en escena:" + enemiesInScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesInScene > 0)
        {
            timePerRoom += Time.unscaledDeltaTime;
            var ts = TimeSpan.FromSeconds(timePerRoom);
            print("Tiempo: " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
        }
        
        if(enemiesInScene <= 0) roomEnded = true;
        

        if (roomEnded) { 

            ShowStats();
            NextRoomCreation();
        }
    }

    public void UpdateEnemiesArray()
    {
        enemiesInScene -= 1;
        //print("Enemigos en escena:" + enemiesInScene);
    }

    public void UpdateLife(float life)
    {
        totalLifePointsInRoom = life;
    }
    void ShowStats()
    {
        print("Todos los enemigos derrotados");
        var ts = TimeSpan.FromSeconds(timePerRoom);
        print("Tiempo final de la sala: " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
        UpdateLife(FindObjectOfType<PlayerController>().life);
        print("Vida final de la sala: " + totalLifePointsInRoom);
    }

    void NextRoomCreation()
    {
        
    }

}
