using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float basedamage = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private GameObject prefabs;
    private void Start()
    {
        damage = basedamage;
    }
    private void Update()
    {
        // update damage from player stats here
    }
    public float getdmg()
    {
       return damage;
    }
    public void setdmg(float add)
    {
        this.damage = add;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        //getscript here

        //damage here using the enemy script

        Destroy(prefabs);
    }
}
