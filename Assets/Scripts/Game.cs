using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Worm WormPrefab;

    public Transform SpawnPoint;

    public Worm CurrentWorm;

    public static Game Instance;

    public int currentStrawberryAmount;
    public int StrawberyCount;

    public Button Pause;
    public Image WinScreen;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentWorm = Instantiate(WormPrefab, SpawnPoint);
    }

    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.F))
        {
            WormDead();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }        
    }

    public void WormDead()
    {
        CurrentWorm.Dead();

        CurrentWorm = Instantiate(WormPrefab, SpawnPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(SpawnPoint.position, SpawnPoint.position + Vector3.right * 10);
    }
}
