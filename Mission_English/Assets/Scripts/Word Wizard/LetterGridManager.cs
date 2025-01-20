using UnityEngine;

public class LetterGridManager : MonoBehaviour
{
    public GameObject letterButtonPrefab;
    public Transform gridParent;
    public RiddleManager riddleManager;

    void Start()
    {
        Debug.Log("Start method called");

        // Clear previous buttons
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        foreach (char letter in alphabet)
        {
            GameObject button = Instantiate(letterButtonPrefab, gridParent);
            LetterButton letterButton = button.GetComponent<LetterButton>();

            letterButton.letter = letter;
            letterButton.riddleManager = riddleManager;
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = letter.ToString();
        }
    }
}
