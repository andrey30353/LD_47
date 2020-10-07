using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Snake WormPrefab;

    public Transform SpawnPoint;

    public Snake CurrentWorm;

    public static Game Instance;

    public int currentStrawberryAmount;
    public int StrawberyCount;

    public Button Pause;
    public Image WinScreen;

 


    public void Awake()
    {
        Instance = this;
    }

   

    
    void Update()
    {
        
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    CurrentWorm.Dead();

        //    CurrentWorm = Instantiate(WormPrefab, SpawnPoint);
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
        
    } 

}
