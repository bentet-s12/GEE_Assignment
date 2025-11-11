using Unity.VisualScripting;
using UnityEngine;

public class enemyspawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject spawnspace;
    [SerializeField] private GameObject enemyprefab;
    [SerializeField] private GameObject eliteprefab;
    [SerializeField] private GameObject bossprefab;
    [SerializeField] private int spawnedEnemies;
    [SerializeField] private int defeatedEnemies;
    [SerializeField] private DoorSlide[] doorScripts;

    private void Start()
    {


    }
    public void enemydefeated()
    {
        defeatedEnemies++;
    }
    public void spawnEnemy(int spawnamt)
    {
        //spawn in spawn area
        spawnedEnemies += spawnamt;
    }
    public void spawnElite(int spawnamt)
    {
        spawnedEnemies += spawnamt;
    }
    public void spawnBoss(int spawnamt)
    {
        spawnedEnemies += spawnamt;
    }
    private void Update()
    {

        if (defeatedEnemies == spawnedEnemies)
        {

            foreach (DoorSlide door in doorScripts)
            {
                if (door != null)
                    door.Activate();
            }

        }

    }
}
