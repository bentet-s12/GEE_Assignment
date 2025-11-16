using System.Collections.Generic;
using UnityEngine;

public class lvlup_UI : MonoBehaviour
{
    [SerializeField]private GameObject[] cards;
    [SerializeField] private GameObject[] spawnarea;
    [SerializeField] private GameObject UI;
    [SerializeField] levelling_logic lvlingscript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        lvlingscript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<levelling_logic>();
        if (lvlingscript != null && lvlingscript.getDJ() == true)
        {
            List<GameObject> cardList = new List<GameObject>(cards);

            
            cardList.RemoveAll(x => x.name == "MultiJump");

        
            cards = cardList.ToArray();
        }
    }
    public void activeUI()
    {
        Time.timeScale = 0f;
        
        AudioListener.pause = true;
        UI.SetActive(true);
    }
    public void deactivateUI()
    {
        Time.timeScale = 1f;
        foreach (GameObject spawn in spawnarea)
        {
            foreach (Transform child in spawn.transform)
            {
                Destroy(child.gameObject);
            }
        }
        AudioListener.pause = false;
        UI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void upgrade_refresh()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //ensure that there are no other cards
        foreach (GameObject spawn in spawnarea)
        {
            foreach (Transform child in spawn.transform)
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < spawnarea.Length; i++)
        {
            // Pick a random card prefab from your array
            GameObject cardToSpawn = cards[Random.Range(0, cards.Length)];

            // Instantiate the card as a child of the spawnarea
            GameObject newCard = Instantiate(cardToSpawn, spawnarea[i].transform);

            // Optional: reset local position, rotation, and scale
            newCard.transform.localPosition = Vector3.zero;
            newCard.transform.localRotation = Quaternion.identity;
            newCard.transform.localScale = Vector3.one;
        }
    }
}
