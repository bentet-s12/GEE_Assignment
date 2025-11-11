using UnityEngine;

public class gamelogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int currentRoom = 0;
    [SerializeField] private int spawncount;
    public enemyspawn enemyscript;

    private void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("SpawnPoint");
        enemyscript = manager.GetComponent<enemyspawn>();
    }
    public void roomIncrease()
    {
        currentRoom += 1;
        Debug.Log(currentRoom);
    }
    public void spawnEnemies()
    {
        if (enemyscript != null) {
            if (currentRoom % 10 != 0)
            {
                spawncount = Random.Range(0, 10);
                spawncount = spawncount * currentRoom;
                int enemyspawned = Random.Range(0, spawncount);
                enemyscript.spawnEnemy(enemyspawned);
                if (enemyspawned < spawncount)
                {
                    int elitespawned = spawncount - enemyspawned;
                    enemyscript.spawnElite(elitespawned);
                }
            }
            else
            {
                spawncount = Random.Range(0, 10);
                spawncount = spawncount * currentRoom;
                enemyscript.spawnBoss(1 * (currentRoom / 10));
                spawncount = spawncount - (1 * (currentRoom / 10));
                int enemyspawned = Random.Range(0, (spawncount - (1 * (currentRoom / 10))));
                enemyscript.spawnEnemy(enemyspawned);
                if (enemyspawned < spawncount)
                {
                    int elitespawned = spawncount - enemyspawned;
                    enemyscript.spawnElite(elitespawned);
                }
            }
        }
    }
 }
    
