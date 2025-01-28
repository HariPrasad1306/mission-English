using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionManager : MonoBehaviour
{
    [Header("Question Texts")]
    public TextMeshProUGUI[] questionTexts; // Array for 4 questions.

    [Header("Answer Parent")]
    public Transform answerParent; // Parent object to hold answer buttons.

    [Header("Answer Prefab")]
    public GameObject answerPrefab; // Prefab for draggable answers.

    [Header("Question Data")]
    public QuestionData questionData; // ScriptableObject reference.

    [Header("Drop Zones")]
    public DropZone[] dropZones; // Array for the 4 drop zones.

    [Header("Next Button")]
    public GameObject nextButton; // Reference to the next button.

    [Header("Game Over Panel")]
    public GameObject gameOverPanel; // Reference to the Game Over panel.
    public TextMeshProUGUI gameOverText; // Reference to the Game Over text.

    [Header("Questions Left Text")]
    public TextMeshProUGUI questionsLeftText; // Reference to the questions left text.

    [Header("Audio Clips")]
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip buttonClickSound;
    public AudioClip gameOverSound;

    private AudioSource audioSource;

    private int currentSetIndex = 0; // Tracks which set of 4 questions is active.

    public int CurrentQuestionIndex => currentSetIndex; // Expose the current question set index as a read-only property

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateQuestionsLeftText();
        LoadQuestions();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void LoadQuestions()
    {
        if (currentSetIndex < questionData.questions.Count)
        {
            // Load 4 questions into the UI.
            for (int i = 0; i < questionTexts.Length; i++)
            {
                questionTexts[i].text = questionData.questions[currentSetIndex].subQuestions[i];
            }

            // Spawn answers dynamically.
            foreach (Transform child in answerParent)
            {
                Destroy(child.gameObject); // Clear previous answers.
            }

            foreach (string answer in questionData.questions[currentSetIndex].correctAnswers)
            {
                GameObject answerObj = Instantiate(answerPrefab, answerParent);
                answerObj.GetComponentInChildren<TextMeshProUGUI>().text = answer;
            }

            // Reset drop zones
            foreach (DropZone dropZone in dropZones)
            {
                dropZone.ResetDropZone();
            }

            // Disable the next button initially
            nextButton.SetActive(false);

            // Update the questions left text
            UpdateQuestionsLeftText();
        }
        else
        {
            GameOver(); // Trigger the Game Over when all questions are answered
        }
    }

    private void UpdateQuestionsLeftText()
    {
        int totalSets = questionData.questions.Count;
        questionsLeftText.text = $"Questions\n{currentSetIndex + 1}/{totalSets}";
    }

    public void CheckAllDropZonesFilled()
    {
        // Check if all drop zones are filled
        bool allFilled = true;
        foreach (DropZone dropZone in dropZones)
        {
            if (!dropZone.IsFilled())
            {
                allFilled = false;
                break;
            }
        }

        // Enable next button if all drop zones are filled
        nextButton.SetActive(allFilled);
    }

    public void NextSet()
    {
        PlaySound(buttonClickSound);
        currentSetIndex++;
        LoadQuestions();
    }

    public void GameOver()
    {
        PlaySound(gameOverSound);
        // Disable the question UI and next button
        foreach (TextMeshProUGUI questionText in questionTexts)
        {
            questionText.gameObject.SetActive(false);
        }

        foreach (Transform child in answerParent)
        {
            child.gameObject.SetActive(false);
        }

        // Show the Game Over panel
        gameOverPanel.SetActive(true);
        gameOverText.text = "Congratulations!! \n You've completed all the questions!";
    }

    // Optionally, you can have a restart function
    public void RestartGame()
    {
        currentSetIndex = 0;
        gameOverPanel.SetActive(false);
        LoadQuestions();
    }

    public void LoadScreen(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
