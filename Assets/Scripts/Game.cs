using UnityEngine;

public class Game : MonoBehaviour
{
    public Snake WormPrefab;

    public Transform SpawnPoint;

    public Snake CurrentWorm;

    public static Game Instance;

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
