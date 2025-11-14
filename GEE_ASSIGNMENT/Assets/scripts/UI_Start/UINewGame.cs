using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private levelling_logic logicScript;

    private void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("gameManager");
        if (manager != null)
        {
           
            logicScript = manager.GetComponent<levelling_logic>();
        }
        }
    public void StartNewGame()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            logicScript.DeleteData();
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("StartGame: sceneToLoad is empty");
        }
    }


    public void ContinueGame()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            logicScript.SaveData();
            logicScript.loadData();
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("StartGame: sceneToLoad is empty");
        }
    }
}