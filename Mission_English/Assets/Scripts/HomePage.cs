using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HomePage : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text greetingText;

    private const string PlayerNameKey = "PlayerName";

    void Start()
    {
        // Retrieve and display the player's name with a greeting
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            string playerName = PlayerPrefs.GetString(PlayerNameKey);
            greetingText.text = GetGreeting(playerName);
        }
        else
        {
            greetingText.text = GetGreeting("Player");
        }
    }

    // Method to generate the greeting message with a new line
    string GetGreeting(string playerName)
    {
        int hour = System.DateTime.Now.Hour;
        string greeting;

        if (hour < 12)
            greeting = "Good morning";
        else if (hour < 18)
            greeting = "Good afternoon";
        else
            greeting = "Good evening";

        // Return the greeting with player's name and a new line
        return $"{greeting}, {playerName}!\n";
    }

    // Load a mini-game scene by scene name
    public void LoadMiniGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Load a mini-game scene by scene index
    public void LoadMiniGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
