using UnityEngine;

public class enemyWalk : MonoBehaviour
{
    public float speed = 2.0f;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ignore self
        if (other.transform == transform) return;

        // destroy everything else
        if(other.isTrigger)
        {
            Destroy(other.gameObject);
        }
    }
}
