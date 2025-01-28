using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LogicLadder : MonoBehaviour
{
    public TMP_Text premise1Text;
    public TMP_Text premise2Text;
    public TMP_Text conclusionText;
    public TMP_Text feedbackText;
    public Button trueButton;
    public Button falseButton;
    public TMP_Text progressText;

    // Reference to the ScriptableObject holding questions
    public SyllogismQuestionData questionData;

    private int currentStep = 0;

    // Game Over Panel and Text reference
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;  // Reference to Game Over Text

    // Audio clips for feedback
    public AudioSource audioSource;  // Audio source component
    public AudioClip correctSound;   // Sound for correct answers
    public AudioClip incorrectSound; // Sound for incorrect answers
    public AudioClip hintSound;// Optional sound for hint
    public AudioClip GameOverPanel;

    // Sprite-related variables
    public Image stepImage;           // Reference to the UI Image
    public Sprite[] stepSprites;      // Array of sprites for steps

    void Start()
    {
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (currentStep < questionData.questions.Length)
        {
            // Update question text
            premise1Text.text = questionData.questions[currentStep].premise1;
            premise2Text.text = questionData.questions[currentStep].premise2;
            conclusionText.text = questionData.questions[currentStep].conclusion;

            // Update progress text
            progressText.text = "Step \n" + (currentStep + 1) + " of " + questionData.questions.Length;

            // Update step image every 2 questions
            int spriteIndex = currentStep / 2; // Divide step by 2 to determine sprite index
            if (spriteIndex < stepSprites.Length)
            {
                stepImage.sprite = stepSprites[spriteIndex];
            }
        }
        else
        {
            GameOver();
        }
    }

    public void CheckAnswer(bool playerAnswer)
    {
        bool correctAnswer = questionData.questions[currentStep].isConclusionValid;

        if (playerAnswer == correctAnswer)
        {
            feedbackText.text = "Correct!\n Climb up the ladder.";
            audioSource.PlayOneShot(correctSound); // Play correct answer sound
            currentStep++;
        }
        else
        {
            feedbackText.text = "Incorrect.\n Try again!";
            audioSource.PlayOneShot(incorrectSound); // Play incorrect answer sound
        }

        LoadQuestion();  // Load the next question or show Game Over
    }

    public void ProvideHint()
    {
        feedbackText.text = questionData.questions[currentStep].hint;
        if (hintSound != null)
        {
            audioSource.PlayOneShot(hintSound); // Play hint sound (if assigned)
        }
    }

    void GameOver()
    {
        // Show the Game Over Panel
        gameOverPanel.SetActive(true);

        // Update the Game Over Text
        gameOverText.text = "Congratulations! \n You've completed all the steps.\n";
        
        audioSource.PlayOneShot(GameOverPanel);
        // Disable the question UI
        premise1Text.gameObject.SetActive(false);
        premise2Text.gameObject.SetActive(false);
        conclusionText.gameObject.SetActive(false);
        trueButton.gameObject.SetActive(false);
        falseButton.gameObject.SetActive(false);
        progressText.gameObject.SetActive(false);
        feedbackText.text = "";  // Clear feedback
    }

    public void RestartGame()
    {
        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(int num)
    {
        SceneManager.LoadScene(num);
    }
}
