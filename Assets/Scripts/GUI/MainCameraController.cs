using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner;
    private GameObject cameraBound;
    private void Awake()
    {
        DontDestroyOnLoad(this);    
    }

    public void Start()
    {
        //cameraBound = GameObject.FindGameObjectWithTag("CameraBounds");

        //if (cinemachineConfiner.m_BoundingShape2D == null)
        //    cinemachineConfiner.m_BoundingShape2D = cameraBound.GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Start();
    }
}
