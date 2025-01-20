using UnityEngine;
using TMPro;

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

    private int currentSetIndex = 0; // Tracks which set of 4 questions is active.

    void Start()
    {
        LoadQuestions();
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
        }
        else
        {
            Debug.Log("All questions completed!");
            // Handle end-of-game logic here (e.g., show summary).
        }
    }

    public void NextSet()
    {
        currentSetIndex++;
        LoadQuestions();
    }
}
