using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  public GameObject spawnObj;

//   public float minX, maxX, minZ, maxZ, minSpawnTime2, maxSpawnTime2, minSpawnTime3, maxSpawnTime3, maxEnemy, enemyAmount;
  private bool isSpawning;
  public float minSpawnTime, maxSpawnTime, maxEnemy, enemyAmount, difficultyMultiplier;
//   private float timer, timer2, timer3;
    float timer, minX, maxX, minZ, maxZ, timeSinceStart;

  // !Is spawn ska vara False från 1-frame
  void Awake()
  {
    isSpawning = false;
    timeSinceStart = 0;
  }
  void Update()
  {

    timeSinceStart = Time.deltaTime / 5;
    // !Ger en liten random delay efter varje Spawn
    if (!isSpawning && maxEnemy >= enemyAmount)
    {
      timer = Random.Range(minSpawnTime, maxSpawnTime);
      Invoke("SpawnObject1", timer);

    //   timer2 = Random.Range(minSpawnTime2, maxSpawnTime2);
    //   Invoke("SpawnObject2", timer2);

    //   timer3 = Random.Range(minSpawnTime3, maxSpawnTime3);
    //   Invoke("SpawnObject3", timer3);

      isSpawning = true;
    }
  }
  void SpawnObject1()
  {
    // !Spawnar fienden i Random plats
    float x = Random.Range(minX, maxX);
    float z = Random.Range(minZ, maxZ);

    Instantiate(spawnObj, transform.position, Quaternion.identity);
    isSpawning = false;
    enemyAmount += 1;

  }
//   void SpawnObject2()
//   {
//     // !Spawnar fienden i Random plats
//     float x2 = Random.Range(minX, maxX);
//     float z2 = Random.Range(minZ, maxZ);

//     Instantiate(spawnObj2, new Vector3(x2, 7, z2), Quaternion.identity);
//     isSpawning = false;
//     enemyAmount += 1;
//   }
//   void SpawnObject3()
//   {
//     // !Spawnar fienden i Random plats
//     float x3 = Random.Range(minX, maxX);
//     float z3 = Random.Range(minZ, maxZ);

//     Instantiate(spawnObj3, new Vector3(x3, 4, z3), Quaternion.identity);
//     isSpawning = false;
//     enemyAmount += 1;
//   }
}