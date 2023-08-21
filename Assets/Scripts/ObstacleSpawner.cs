using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    
    public List<GameObject> ObstacleList;
    public float Timer;
    public float TimerMax;

    public Vector3 SpawnPositionMin;
    public Vector3 SpawnPositionMax;

    void Start()
    {
        
    }

    void Spawn(){
        Vector3 spawnPosition = new Vector3(Random.Range(SpawnPositionMin.x, SpawnPositionMax.x),
                                            Random.Range(SpawnPositionMin.y, SpawnPositionMax.y),
                                            Random.Range(SpawnPositionMin.z, SpawnPositionMax.z)
        );
        int spawnIndex = Random.Range(0, ObstacleList.Count);       
        GameObject go = Instantiate(ObstacleList[spawnIndex], spawnPosition, Quaternion.identity);

    }
    
    void Update()
    {
        if(Timer > 0){
            Timer -= Time.deltaTime;
        }
        else{
            Spawn();
            Timer = TimerMax;
        }
    }
}
