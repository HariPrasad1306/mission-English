using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    private Vector2 originalPosition;
    private bool isLocked = false;
    public AudioClip dragStartSound;


    public Transform originalParent; // Reference to the original parent (answer panel).

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent; // Set the original parent when the game starts.
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        var questionManager = FindObjectOfType<QuestionManager>();
        questionManager.PlaySound(dragStartSound);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Reset position if not correctly dropped
        if (transform.parent == null || transform.parent.GetComponent<DropZone>() == null)
        {
            transform.SetParent(originalParent); // Move the answer back to its original parent.
            rectTransform.localPosition = Vector3.zero; // Reset to the Grid Layout Group position.
        }
    }

    public void LockItem()
    {
        isLocked = true;
        canvasGroup.blocksRaycasts = false; // Prevent further dragging.
    }

    public void ResetItem()
    {
        isLocked = false;
        transform.SetParent(originalParent); // Return to the original parent.
        rectTransform.localPosition = Vector3.zero; // Reset position within the layout group.
    }
}
