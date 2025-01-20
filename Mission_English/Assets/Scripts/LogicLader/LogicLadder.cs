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

    void Start()
    {
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (currentStep < questionData.questions.Length)
        {
            premise1Text.text = questionData.questions[currentStep].premise1;
            premise2Text.text = questionData.questions[currentStep].premise2;
            conclusionText.text = questionData.questions[currentStep].conclusion;

            // Update progress text
            progressText.text = "Step \n" + (currentStep + 1) + " of " + questionData.questions.Length;
        }
        else
        {
            // Show the Game Over Panel
            GameOver();
        }
    }

    public void CheckAnswer(bool playerAnswer)
    {
        bool correctAnswer = questionData.questions[currentStep].isConclusionValid;

        if (playerAnswer == correctAnswer)
        {
            feedbackText.text = "Correct!\n Climb up the ladder.";
            currentStep++;
        }
        else
        {
            feedbackText.text = "Incorrect.\n Try again!";
        }

        LoadQuestion();  // Load the next question or show Game Over
    }

    public void ProvideHint()
    {
        feedbackText.text = questionData.questions[currentStep].hint;
    }

    void GameOver()
    {
        // Show the Game Over Panel
        gameOverPanel.SetActive(true);

        // Update the Game Over Text
        gameOverText.text = "Congratulations! \n You've completed all the steps.\n";

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
