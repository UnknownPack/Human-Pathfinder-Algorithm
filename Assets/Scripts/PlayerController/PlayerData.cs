using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject antPrefab;
    public int numberOfAnts;
    private float spawnRateCounter = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnRateCounter +=  Time.deltaTime;
        //Debug.Log($"{spawnRateCounter}/1");
        if (spawnRateCounter >= 1)
        {
            numberOfAnts++;
            spawnRateCounter = 0;
        }
    }

    public void spawnAnts(Vector3 destination)
    {
        
    }
}
