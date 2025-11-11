using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    public gamelogic logicscript;

    private void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("gameManager");
        logicscript = manager.GetComponent<gamelogic>();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(MovePlayerToSpawnNextFrame());
    }

    private IEnumerator MovePlayerToSpawnNextFrame()
    {
        // Wait one frame to ensure scene objects are ready
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player found: " + player);

        if (player != null)
        {
            if (spawn == null)
            {
                // Try finding a spawn automatically if not assigned
                spawn = GameObject.FindWithTag("SpawnPoint");
                Debug.Log(spawn.name);
            }

            if (spawn != null)
            {
                player.transform.position = spawn.transform.position;
                Debug.Log($"Player moved to spawn point at {spawn.transform.position}");
                logicscript.SpawnEnemies();
            }
            else
            {
                Debug.LogWarning(" No spawn point assigned or found in scene!");
            }
        }
    }
}