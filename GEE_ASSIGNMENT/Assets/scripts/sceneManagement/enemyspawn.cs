using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class enemyspawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Collider spawnspace;
    [SerializeField] private GameObject enemyprefab;
    [SerializeField] private GameObject eliteprefab;
    [SerializeField] private GameObject bossprefab;
    [SerializeField] private int spawnedEnemies;
    [SerializeField] private int defeatedEnemies;
    [SerializeField] private DoorSlide[] doorScripts;
    private bool canCheckDoors = false;

    void Start()
    {
        StartCoroutine(EnableDoorCheckAfterDelay(5f));
    }
    IEnumerator EnableDoorCheckAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canCheckDoors = true;
        Debug.Log("Door checking enabled!");
    }

    public void enemydefeated()
    {
        defeatedEnemies++;
    }
    public void spawnEnemy(int spawnamt)
    {
        if (spawnspace != null)
        {
            for (int i = 0; i < spawnamt; i++)
            {
                Vector3 spawnPos = GetRandomPointInCollider(spawnspace);
                Instantiate(enemyprefab, spawnPos, Quaternion.identity);
            }

        }
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
        if (canCheckDoors)
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
        else
        {
            return;
        }
    }
    private Vector3 GetRandomPointInCollider(Collider col)
    {
        Bounds bounds = col.bounds;
        Vector3 point = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );

        // Optionally adjust Y to ground level or raycast down
        return point;
    }
}
