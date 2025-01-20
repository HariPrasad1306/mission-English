using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropZone : MonoBehaviour, IDropHandler
{
    [Header("Question Data")]
    public QuestionData questionData;
    public int questionIndex;
    public int subQuestionIndex;

    [Header("Feedback Settings")]
    public TextMeshProUGUI feedbackText;

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

                    if (questionIndex >= 0 && questionIndex < questionData.questions.Count)
                    {
                        if (subQuestionIndex >= 0 && subQuestionIndex < questionData.questions[questionIndex].correctAnswers.Length)
                        {
                            string correctAnswer = questionData.questions[questionIndex].correctAnswers[subQuestionIndex];

                            if (answer == correctAnswer)
                            {
                                feedbackText.text = "Correct!";
                                feedbackText.color = Color.green;

                                draggedItem.transform.SetParent(this.transform); // Lock the answer to the drop zone
                                draggedItem.LockItem(); // Lock the item so it can't be moved.
                                draggedItem.rectTransform.localPosition = Vector3.zero; // Center the item.
                            }
                            else
                            {
                                feedbackText.text = "Incorrect!";
                                feedbackText.color = Color.red;

                                // Reset the item position and put it back in the original layout group
                                draggedItem.ResetItem();
                            }
                        }
                        else
                        {
                            feedbackText.text = "Sub-question index out of range!";
                        }
                    }
                    else
                    {
                        feedbackText.text = "Question index out of range!";
                    }
                }
                else
                {
                    feedbackText.text = "No TextMeshPro found on dragged item!";
                }
            }
        }
    }
}
