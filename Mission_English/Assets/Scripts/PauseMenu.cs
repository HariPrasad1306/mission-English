using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseMenuUI; // The pause menu UI panel
    public Button resumeButton;    // Resume button
    public Button restartButton;   // Restart button
    public Button quitButton;      // Quit button
    public Button pauseButton;     // Pause button in the game UI

    private bool isPaused = false;

    void Start()
    {
        // Initially hide the pause menu
        pauseMenuUI.SetActive(false);

        // Button listeners
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        quitButton.onClick.AddListener(Quit);
        pauseButton.onClick.AddListener(Pause); // Add listener for the pause button

        // Optionally, hide the Pause button during the game start if you want
        pauseButton.gameObject.SetActive(true);
    }

    void Update()
    {
        // Optional: Toggle pause when Escape is pressed (can be removed if not needed)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    // Method to pause the game and show the pause menu
    void Pause()
    {
        // Pause the game and show the pause menu
        Time.timeScale = 0f; // Freeze game time
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    // Method to resume the game and hide the pause menu
    void Resume()
    {
        // Resume the game and hide the pause menu
        Time.timeScale = 1f; // Resume game time
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    // Method to restart the current scene
    void Restart()
    {
        Time.timeScale = 1f; // Ensure time is resumed before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Method to quit to the main menu (or any other desired scene)
    void Quit()
    {
        Time.timeScale = 1f; // Ensure time is resumed before quitting
        SceneManager.LoadScene("HomePage"); // Modify this to your desired scene
    }

    public void TogglePauseMenu()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

}
