using UnityEngine;

public class stats : MonoBehaviour
{
    [SerializeField] private int basehealth;
    [SerializeField] private int health;
    [SerializeField] private int maxhealth;
    [SerializeField] private int currenthealth;
    [SerializeField] private int temphealth;
    [SerializeField] private PlayerStateManager pm;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        basehealth = 20;

        // Initialize correctly
        health = basehealth;
        temphealth = 0;

        maxhealth = health + temphealth;
        currenthealth = maxhealth;   
    }


    public int getHealth()
    {
        return health;
    }
    public int getmaxHealth()
    {
        return maxhealth;
    }
    public void setHealth(int add)
    {
        health = add;
    }
    public void setTempHealth(int add)
    {
        temphealth = add;
      
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
            currenthealth = 0;
            PlayerStateManager pm = GetComponent<PlayerStateManager>();
            if (pm != null)
                pm.Die();
        }

    }

}

// Update is called once per frame


