using System;
using UnityEngine;

public class levelling_logic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int expNeeded;
    [SerializeField] private int currentexp;
    [SerializeField] private int level;
    [SerializeField] private stats statScript;
    [SerializeField] private shootScript gunScript;
    [SerializeField] private gamelogic logicScript;
    [SerializeField] private PlayerStateManager spdScript;

    [SerializeField] private int health;
    [SerializeField] private float basespeed;
    [SerializeField] private float currentspeed;
    [SerializeField] private float Tempspeed;

    [SerializeField] private int damage;
    [SerializeField] private int damagetemp;
    [SerializeField] private int currentdamage;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            statScript = player.GetComponent<stats>();
            spdScript = player.GetComponent<PlayerStateManager>();
            basespeed = spdScript.getspeed();
            health = statScript.getHealth();
        }
        else
        {
            Debug.LogError("cannot find stats");
        }
        GameObject weapon = GameObject.FindGameObjectWithTag("weapon");
        if (weapon != null)
        {
            gunScript = weapon.GetComponent<shootScript>();
            damage = gunScript.getdamage();
        }
        else
        {
            Debug.LogError("cannot find gun");
        }
        
        GameObject manager = GameObject.FindGameObjectWithTag("gameManager");
        if (weapon != null)
        {
            logicScript = manager.GetComponent<gamelogic>();
        }
        else
        {
            Debug.LogError("cannot find stats");
        }
    }
    public void calculateExpNeeded()
    {
        if (level == 0)
        {
            expNeeded = 10;
        }
        else
        {
            expNeeded = (level + 1) * 10;
        }
    }
    public void lvlup()
    {
        currentexp++;
        if (currentexp >= expNeeded)
        {
            currentexp = 0;
            level++;
            calculateExpNeeded();
        }
    }
    
    public void adddmg(int dmg)
    {
        if (gunScript != null)
        {
            gunScript.setdmg(dmg);
        }
    }
    public void addmulti(int multi)
    {
        if (gunScript != null)
        {
            gunScript.setmultishot(multi);
        }
    }
    public void addtempspd(float add)
    {
        Tempspeed += add;
    }
    private void Update()
    {
        int currentRoom = logicScript.getCurrentRoom();
        health = health * currentRoom;
        if (statScript.getHealth() != health)
        {
            statScript.setHealth(health);
            statScript.setmaxhealth();
        }
        currentspeed = spdScript.getspeed();
        if (currentspeed != basespeed + Tempspeed)
        {
            float addspd = basespeed + Tempspeed - currentspeed;
            spdScript.setspeed(addspd);
            
        }
        currentdamage = gunScript.getdamage();
        if (currentdamage != damage + damagetemp)
        {
            int adddmg = damage + damagetemp;
            gunScript.setdmg(adddmg);
            
        }
    }

}
