using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropZone : MonoBehaviour, IDropHandler
{
    [Header("Question Data")]
    public QuestionData questionData; // Reference to the QuestionData.
    public int dropZoneIndex; // Index to identify which drop zone this is in the current question set.
    public QuestionManager questionManager; // Reference to the QuestionManager.

    [Header("Feedback Settings")]
    public TextMeshProUGUI feedbackText; // For showing feedback (correct/incorrect).

    private bool isFilled = false; // To check if the drop zone is filled.

    // Method to get the drop zone state
    public bool IsFilled() => isFilled;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

            if (draggedItem != null)
            {
                TextMeshProUGUI draggedText = draggedItem.GetComponentInChildren<TextMeshProUGUI>();

                if (draggedText != null)
                {
                    string answer = draggedText.text;

                    // Check for correct answer based on the dropZoneIndex
                    if (dropZoneIndex >= 0 && dropZoneIndex < questionData.questions[questionManager.CurrentQuestionIndex].correctAnswers.Length)
                    {
                        string correctAnswer = questionData.questions[questionManager.CurrentQuestionIndex].correctAnswers[dropZoneIndex];

                        if (answer == correctAnswer)
                        {
                            feedbackText.text = "Correct!";
                            feedbackText.color = Color.green;

                            draggedItem.transform.SetParent(this.transform); // Lock the answer to the drop zone
                            draggedItem.LockItem(); // Lock the item so it can't be moved.
                            draggedItem.rectTransform.localPosition = Vector3.zero; // Center the item.

                            isFilled = true; // Mark the drop zone as filled
                            questionManager.PlaySound(questionManager.correctSound);
                            // Notify the QuestionManager that this drop zone has been filled
                            questionManager.CheckAllDropZonesFilled();
                        }
                        else
                        {
                            feedbackText.text = "Incorrect!";
                            feedbackText.color = Color.red;

                            // Reset the item position and put it back in the original layout group
                            draggedItem.ResetItem();
                            isFilled = false; // Reset the drop zone to not filled
                            questionManager.PlaySound(questionManager.incorrectSound);
                        }
                    }
                    else
                    {
                        feedbackText.text = "Index out of range!";
                    }
                }
                else
                {
                    feedbackText.text = "No TextMeshPro found on dragged item!";
                }
            }
        }
    }

    // Reset the drop zone when transitioning to the next question
    public void ResetDropZone()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject); // Remove the dropped item
        }

        isFilled = false; // Reset the filled state
    }

}
