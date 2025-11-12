using UnityEngine;

public class stats : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxhealth;
    [SerializeField] private int temphealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void setTempHealth(int add)
    {
        temphealth += add;
        setmaxhealth();
    }
    private void setmaxhealth()
    {
        maxhealth = health + temphealth;
    }

    public void takedmg(int dmg)
    {
        maxhealth += dmg;
        if (maxhealth <= 0)
        {
           //playerDie
        }
    }
    }

    // Update is called once per frame


