using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnalogGame : MonoBehaviour
{
    public TMP_Text questionText;
    public Button[] optionButtons; // Buttons with TextMeshPro child texts
    public TMP_Text feedbackText;
    public TMP_Text scoreText;
    public AnalogyQuestionData analogyQuestionData; // Reference to your ScriptableObject
    public GameObject endGamePanel; // Reference to the End Game UI Panel
    public Button retryButton; // Retry button
    public Button exitButton; // Exit button to go to home page

    [Header("Button Colors")]
    public Color correctAnswerColor = Color.green;
    public Color incorrectAnswerColor = Color.red;
    public Color defaultButtonColor = Color.white;

    [Header("Audio Clips")]
    public AudioClip correctAnswerClip; // Audio for correct answers
    public AudioClip incorrectAnswerClip; // Audio for incorrect answers
    public AudioClip buttonClickClip; // Audio for button clicks
    public AudioClip endGameClip; // Audio for game completion

    private AudioSource audioSource;
    private List<AnalogyQuestionData.Question3> questions = new List<AnalogyQuestionData.Question3>();
    private int currentQuestionIndex = 0;
    private int score = 0;

    private void Start()
    {
        feedbackText.text = "";
        score = 0;

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        LoadQuestions();
        DisplayQuestions();
        endGamePanel.SetActive(false); // Hide the end game panel initially

        // Add listeners to the buttons
        retryButton.onClick.AddListener(() =>
        {
            PlaySound(buttonClickClip);
            RetryGame();
        });
        exitButton.onClick.AddListener(() =>
        {
            PlaySound(buttonClickClip);
            GoToHomePage();
        });
    }

    private void LoadQuestions()
    {
        // Load questions from the ScriptableObject
        questions = new List<AnalogyQuestionData.Question3>(analogyQuestionData.questions);
    }

    private void DisplayQuestions()
    {
        if (currentQuestionIndex < questions.Count)
        {
            var current = questions[currentQuestionIndex];
            questionText.text = current.question;

            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < current.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true); // Show all buttons
                    TMP_Text buttonText = optionButtons[i].GetComponentInChildren<TMP_Text>();
                    buttonText.text = current.options[i];
                    optionButtons[i].image.color = defaultButtonColor; // Reset button color

                    int index = i; // Capture index for the lambda
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() =>
                    {
                        PlaySound(buttonClickClip);
                        CheckAnswer(index);
                    });
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false); // Hide extra buttons
                }
            }
        }
        else
        {
            EndGame(); // Call EndGame when all questions are answered
        }
    }

    private void CheckAnswer(int selected)
    {
        var current = questions[currentQuestionIndex];

        // Highlight the correct and incorrect buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i == current.correctAnswerIndex)
            {
                optionButtons[i].image.color = correctAnswerColor; // Use correct answer color
            }
            else if (i == selected)
            {
                optionButtons[i].image.color = incorrectAnswerColor; // Use incorrect answer color
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false); // Hide other buttons
            }
        }

        // Update feedback, score, and play audio
        if (selected == current.correctAnswerIndex)
        {
            feedbackText.text = "Correct!!";
            score += 10;
            PlaySound(correctAnswerClip);
        }
        else
        {
            feedbackText.text = "Wrong!";
            PlaySound(incorrectAnswerClip);
        }

        UpdateScore();
        Invoke(nameof(NextQuestion), 2f); // Delay before moving to the next question
    }

    private void NextQuestion()
    {
        feedbackText.text = "";
        currentQuestionIndex++;
        DisplayQuestions();
    }

    private void UpdateScore()
    {
        scoreText.text = $"Score: {score}";
    }

    private void EndGame()
    {
        questionText.text = "Congratulations! You've completed the game.";
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Show the End Game UI Panel
        endGamePanel.SetActive(true);

        // Update the end game panel with the score
        TMP_Text endGameText = endGamePanel.GetComponentInChildren<TMP_Text>();
        endGameText.text = $"Game Over!\nYour Score: {score}";

        // Play end game sound
        PlaySound(endGameClip);

        // Hide the retry and exit buttons initially
        retryButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    // Method to play a sound
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Method to restart the game
    private void RetryGame()
    {
        // Reset variables and reload the current scene
        currentQuestionIndex = 0;
        score = 0;
        feedbackText.text = "";
        UpdateScore();
        endGamePanel.SetActive(false); // Hide the end game panel
        DisplayQuestions();
    }

    // Method to go back to the Home Page (Main Menu)
    private void GoToHomePage()
    {
        // Load the home page scene (assuming it's named "HomePage")
        SceneManager.LoadScene("HomePage");
    }
}

