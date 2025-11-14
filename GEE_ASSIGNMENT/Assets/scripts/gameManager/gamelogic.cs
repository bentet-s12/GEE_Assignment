using UnityEngine;

public class gamelogic : MonoBehaviour
{
    [SerializeField] private int currentRoom = 0;
    [SerializeField] private int spawnCount;
    [SerializeField] private GameObject manager;
    private enemyspawn enemyScript;

    void Update()
    {
        manager = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (manager != null)
        {
            enemyScript = manager.GetComponent<enemyspawn>();
        }
        else
        {
            Debug.Log("SpawnPoint not found! Please tag your enemy spawner GameObject correctly.");
        }
    }
    
    

    public void roomIncrease()
    {
        currentRoom++;
        Debug.Log($"Room: {currentRoom}");
    }
    public int getCurrentRoom()
    {
        return currentRoom;
    }

    public void SpawnEnemies()
    {
        spawnCount = Random.Range(1, 10) * currentRoom;
        if (enemyScript == null || currentRoom == 0) return;

        spawnCount = Random.Range(1, 10) * currentRoom;

        if (currentRoom % 10 == 0)
        {
            // Boss room
            int bossCount = currentRoom / 10;
            enemyScript.spawnBoss(bossCount);

            int adjustedSpawn = Mathf.Max(0, spawnCount - bossCount);
            int enemiesSpawned = Random.Range(0, adjustedSpawn);
            enemyScript.spawnEnemy(enemiesSpawned);

            int elitesSpawned = adjustedSpawn - enemiesSpawned;
            if (elitesSpawned > 0)
                enemyScript.spawnElite(elitesSpawned);
        }
        else
        {
            // Normal room
            int enemiesSpawned = Random.Range(1, spawnCount);
            enemyScript.spawnEnemy(enemiesSpawned);

            int elitesSpawned = spawnCount - enemiesSpawned;
            if (elitesSpawned > 0)
                enemyScript.spawnElite(elitesSpawned);
        }
    }
}
