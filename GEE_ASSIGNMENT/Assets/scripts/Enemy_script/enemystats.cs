using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemystats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private int health;
    [SerializeField] private enemyspawn spawnscript;
    [SerializeField] private gamelogic roomscaler;
    [SerializeField] private int currentroom;
    private bool isDead = false;
    private void Start()
    {
        spawnscript = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<enemyspawn>();
        roomscaler = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gamelogic>();
        if (roomscaler != null)
        {
            health = health + (currentroom * 10);
        }
        else
        {
            return;
        }
    }
    public void takedmg(int dmg)
    {
        if (isDead) return;  // prevents double-death

        health -= dmg;
        Debug.Log(health.ToString());

        if (health <= 0)
        {
            Die();
        }



    }
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (spawnscript != null)
            spawnscript.enemydefeated();

        Destroy(gameObject);
    }
}
