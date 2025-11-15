using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private levelling_logic logicScript;

    [Header("Audio")]
    public AudioSource bgmSource;       // background music
    public AudioSource sfxSource;       // click sound source
    public AudioClip clickSound;        // click sound

    private void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("gameManager");
        if (manager != null)
        {
            logicScript = manager.GetComponent<levelling_logic>();
        }

        // Play BGM only in Start Menu
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    private void PlayClick()
    {
        if (sfxSource != null && clickSound != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
    }

    public void StartNewGame()
    {
        PlayClick();

        if (bgmSource != null)
            bgmSource.Stop(); // stop music before changing scene

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
        PlayClick();

        if (bgmSource != null)
            bgmSource.Stop();

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

    public void OpenSettings()
    {
        PlayClick();
        // your settings UI open code here
    }

    public void QuitGame()
    {
        PlayClick();
        Application.Quit();
    }
}
