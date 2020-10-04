using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Snake WormPrefab;

    public Transform SpawnPoint;

    public Snake CurrentWorm;

    public static Game Instance;

    public int StrawberyCount;

    public Button Pause;
    public Image WinScreen;

    public void Awake()
    {
        Instance = this;
    }

    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CurrentWorm.Dead();

            CurrentWorm = Instantiate(WormPrefab, SpawnPoint);
        }
    } */
    


}
