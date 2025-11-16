using UnityEngine;

public class teleportback : MonoBehaviour
{
    [SerializeField] private GameObject spawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionExit(Collision collision)
    {
        if (spawn != null)
        {
            spawn = GameObject.FindWithTag("SpawnPoint");
            collision.gameObject.transform.position = spawn.transform.position;
        }
       
        }
    }
