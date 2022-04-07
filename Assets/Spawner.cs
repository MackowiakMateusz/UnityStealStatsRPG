using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float time;
    public SaveSystem savesystem;
    [System.Serializable]
    public class SpawnerStats
    {
        public int ID;
        public float xPosition;
        public float yPosition;
        public float zPosition;
        public int maxNumberOfSpawnedEnemies;
        public int numberOfAlreadySpawnedEnemies;
        public float periodOfTimeBeetwenSpawns;
        public int idOfEnemyToSpawn;
        public float boundariesOfSpawningSquareArea;// spawning area will be square, not circle
    }
    public SpawnerStats stats;
    // Start is called before the first frame update
    void Start()
    {
        savesystem = GameObject.Find("EventSystem").GetComponent<SaveSystem>();
        savesystem.instantiatedSpawners.Add(gameObject);
        if (PlayerPrefs.HasKey("Spawner" + stats.ID))
        {
            stats = Helper.Deserialize<SpawnerStats>(PlayerPrefs.GetString("Spawner" + stats.ID));
        }
        

        time = savesystem.savegame.timeSinceGameStart % stats.periodOfTimeBeetwenSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject newEnemy;
        if(stats.numberOfAlreadySpawnedEnemies<stats.maxNumberOfSpawnedEnemies)
        {
            time = time + Time.deltaTime;
            if (time > stats.periodOfTimeBeetwenSpawns)
            {
                time = 0;
                newEnemy = Instantiate(savesystem.EnemyPrefabs[stats.idOfEnemyToSpawn], new Vector3(transform.position.x + Random.Range(-stats.boundariesOfSpawningSquareArea, stats.boundariesOfSpawningSquareArea),
                    transform.position.y + Random.Range(-stats.boundariesOfSpawningSquareArea, stats.boundariesOfSpawningSquareArea), 0), Quaternion.Euler(0, 0, 0));
                savesystem.savegame.enemies.Add(newEnemy.GetComponent<Enemy>().stats);
                savesystem.instantiatedEnemies.Add(newEnemy);
                stats.numberOfAlreadySpawnedEnemies += 1;
                newEnemy.GetComponent<Enemy>().stats.IdOfSpawnerFromWhichItWasSpawned = stats.ID;
            }
        }
    }
    public void OnApplicationQuit()
    {
        PlayerPrefs.SetString("Spawner" + stats.ID, Helper.Serialize<SpawnerStats>(stats));
    }
}
