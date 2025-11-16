using UnityEngine;

public class teleportback : MonoBehaviour
{
    [SerializeField] private Transform spawn;

    private void OnCollisionEnter(Collision collision)
    {
        // If the thing touching is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = spawn.position;
        }
    }
}
