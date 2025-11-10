using UnityEngine;

public class gamelogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int currentRoom = 0;
    [SerializeField] private int x;

    public void roomIncrease()
    {
        currentRoom += 1;
        Debug.Log(currentRoom);
    }
}
