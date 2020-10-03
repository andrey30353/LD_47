using UnityEngine;

public class Game : MonoBehaviour
{
    public Snake WormPrefab;

    public Transform SpawnPoint;

    public Snake CurrentWorm;
      

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CurrentWorm.Dead();

            CurrentWorm = Instantiate(WormPrefab, SpawnPoint);
        }
    }


}
