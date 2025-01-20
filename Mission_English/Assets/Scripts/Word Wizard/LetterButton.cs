using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    public char letter;
    public RiddleManager riddleManager;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        riddleManager.GuessLetter(letter);
        GetComponent<Button>().interactable = false;  // Disable the button after it's clicked
    }
}
