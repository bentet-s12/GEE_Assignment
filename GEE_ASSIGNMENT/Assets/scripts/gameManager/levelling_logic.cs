using JetBrains.Annotations;
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

    [SerializeField] private int currentmultishot;
    [SerializeField] private int multishot;

    private GameObject player;
    //playerPrefs
    public void SaveData()
    {
        PlayerPrefs.SetInt("multi", multishot);
        PlayerPrefs.SetFloat("spd", Tempspeed);
        PlayerPrefs.Save();
    }
    public void loadData()
    {
        multishot = PlayerPrefs.GetInt("multi");
        Tempspeed = PlayerPrefs.GetFloat("spd");
 
        //maybe ability
    }
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (player != null)
            {
                statScript = player.GetComponent<stats>();
                spdScript = player.GetComponent<PlayerStateManager>();
                basespeed = spdScript.getspeed();
                health = statScript.getHealth();
            }
            else
            {
                Debug.Log("cannot find stats");
            }
            GameObject weapon = GameObject.FindGameObjectWithTag("weapon");
            if (weapon != null)
            {
                gunScript = weapon.GetComponent<shootScript>();
                damage = gunScript.getdamage();
            }
            else
            {
                Debug.Log("cannot find gun");
            }

            GameObject manager = GameObject.FindGameObjectWithTag("gameManager");
            if (weapon != null)
            {
                logicScript = manager.GetComponent<gamelogic>();
            }
            else
            {
                Debug.Log("cannot find stats");
            }
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
    public void addmulti()
    {
        currentmultishot++;
        if (gunScript != null)
        {
            gunScript.setmultishot(currentmultishot);
        }
    }
    public void addtempspd(float add)
    {
        Tempspeed += add;
    }
    private void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                statScript = player.GetComponent<stats>();
                spdScript = player.GetComponent<PlayerStateManager>();
                basespeed = spdScript.getspeed();
                health = statScript.getHealth();
            }
            else
            {
                Debug.Log("cannot find stats");
            }
            GameObject weapon = GameObject.FindGameObjectWithTag("weapon");
            if (weapon != null)
            {
                gunScript = weapon.GetComponent<shootScript>();
                damage = gunScript.getdamage();
            }
            else
            {
                Debug.Log("cannot find gun");
            }

            GameObject manager = GameObject.FindGameObjectWithTag("gameManager");
            if (weapon != null)
            {
                logicScript = manager.GetComponent<gamelogic>();
            }
            else
            {
                Debug.Log("cannot find stats");
            }
        }
        if (logicScript != null)
        {
            int currentRoom = logicScript.getCurrentRoom();
            health = health * currentRoom;
        }
        else
        {
            health = 20;

        }
        if (statScript != null)
        {
            if (statScript.getHealth() != health)
            {
                statScript.setHealth(health);
                statScript.setmaxhealth();
            }
        }
        if (spdScript != null)
        {
            currentspeed = spdScript.getspeed();
            if (currentspeed != basespeed + Tempspeed)
            {
                float addspd = basespeed + Tempspeed - currentspeed;
                spdScript.setspeed(addspd);

            }
        }
        if (gunScript != null)
        {
            currentdamage = gunScript.getdamage();
            if (currentdamage != damage + damagetemp)
            {
                int adddmg = damage + damagetemp;
                gunScript.setdmg(adddmg);

            }

            currentmultishot = gunScript.getmultishot();
            if (currentmultishot != multishot)
            {
                gunScript.setmultishot(multishot);
            }
        }
        

        }
    }


