using UnityEngine;

public class lvlup_UI : MonoBehaviour
{
    [SerializeField]private GameObject[] cards;
    [SerializeField] private GameObject[] spawnarea;
    [SerializeField] private GameObject UI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void activeUI()
    {
        Time.timeScale = 0f;
        
        AudioListener.pause = true;
        UI.SetActive(true);
    }
    public void deactivateUI()
    {
        Time.timeScale = 1f;
       
        AudioListener.pause = false;
        UI.SetActive(false);
    }
    public void upgrade_refresh()
    {
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
