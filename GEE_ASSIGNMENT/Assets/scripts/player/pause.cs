using UnityEngine;

public class pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
