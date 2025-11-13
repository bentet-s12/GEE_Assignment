using UnityEngine;

public class stats : MonoBehaviour
{
    [SerializeField] private int basehealth;
    [SerializeField] private int health;
    [SerializeField] private int maxhealth;
    [SerializeField] private int currenthealth;
    [SerializeField] private int temphealth;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
       
        basehealth = 20;
        health = basehealth;
    }

    public int getHealth()
    {
        return health;
    }
    public void setHealth(int add)
    {
        health = add;
    }
    public void setTempHealth(int add)
    {
        temphealth += add;
        setmaxhealth();
    }
    public void setmaxhealth()
    {
        maxhealth = health + temphealth;
        currenthealth = maxhealth;
    }

    public void takedmg(int dmg)
    {
        currenthealth -= dmg;
        if (currenthealth <= 0)
        {
           //playerDie
           //after animation add UI
           //disable DDU/ do not destroy
        }
    }
    }

    // Update is called once per frame


