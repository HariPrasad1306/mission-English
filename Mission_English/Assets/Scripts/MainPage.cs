using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainPage : MonoBehaviour
{
    public GameObject welcomePanel; // Panel for name input
    public GameObject mainMenuPanel; // Panel with greeting and play button
    public TMP_InputField nameInputField; // Input field for name
    public TextMeshProUGUI greetingText; // Greeting text for main menu
    public Button editNameButton;

    private const string PlayerNameKey = "PlayerName";

    void Start()
    {
        // Check if a name is already saved
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            string playerName = PlayerPrefs.GetString(PlayerNameKey);
            ShowMainMenu(playerName);
        }
        else
        {
            ShowWelcomePanel();
        }
        editNameButton.onClick.AddListener(OnEditNameClicked);
    }

    public void SubmitName()
    {
        string playerName = nameInputField.text.Trim();
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString(PlayerNameKey, playerName);
            PlayerPrefs.Save();
            ShowMainMenu(playerName);
        }
    }

    private void ShowWelcomePanel()
    {
        welcomePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    private void ShowMainMenu(string playerName)
    {
        welcomePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        greetingText.text = $"Welcome back,\n {playerName}!";
    }

    public void PlayGame()
    {
        // Load the game selection screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameSelection");
    }

    private void OnEditNameClicked()
    {
        // Enable the Welcome Panel for editing
        nameInputField.text = PlayerPrefs.GetString(PlayerNameKey); // Pre-fill the current name
        ShowWelcomePanel();
    }

    public void LoadHomePage(int sceneindex)
    {
        SceneManager.LoadScene(sceneindex);
    }
    
}
