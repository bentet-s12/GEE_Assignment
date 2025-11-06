using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float damage = 1;
    [SerializeField] private GameObject prefabs; 
    private void Update()
    {
        // update damage from player stats here
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        //getscript here

        //damage here using the enemy script

        Destroy(prefabs);
    }
}
