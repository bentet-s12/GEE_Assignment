using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private int basedamage = 1;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject prefabs;
    private void Start()
    {
        damage = basedamage;
    }
    private void Update()
    {
        // update damage from player stats here
    }
    public int getdmg()
    {
       return damage;
    }
    public void setdmg(int add)
    {
        this.damage = add;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        //getscript here
        enemystats dmgScript = target.GetComponent<enemystats>();
        //damage here using the enemy script
        if (dmgScript != null)
        {
            if (collision.gameObject.CompareTag("Dragon"))
            {
                collision.gameObject.GetComponent<Dragon>().TakeDamage(this.damage / 2);

            }
            else
            {
                dmgScript.takedmg(this.damage);
            }
        }

        Destroy(prefabs);
    }

}
