using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiddleManager : MonoBehaviour
{
    [System.Serializable]
    public class Riddle
    {
        public string question;
        public string answer;
    }

    public List<Riddle> riddles;  // List of riddles
    private Riddle currentRiddle;  // Active riddle
    private string guessedWord;   // Player's progress
    private int wrongGuesses;     // Track the number of wrong guesses
    private const int maxWrongGuesses = 5;  // Maximum number of wrong guesses

    public TMPro.TextMeshProUGUI riddleText;
    public TMPro.TextMeshProUGUI wordDisplay;
    public TMPro.TextMeshProUGUI messageText;  // To display messages like "Wrong Guess" or "Next Riddle"
    public Transform gridParent; // Reference to the parent of all letter buttons

    void Start()
    {
        LoadRandomRiddle();
    }

    public void LoadRandomRiddle()
    {
        if (riddles.Count == 0) return;  // No riddles to load

        currentRiddle = riddles[Random.Range(0, riddles.Count)];
        guessedWord = new string('_', currentRiddle.answer.Length);
        wrongGuesses = 0;  // Reset wrong guesses
        messageText.text = "";  // Clear any previous messages

        riddleText.text = currentRiddle.question;
        UpdateWordDisplay();
        EnableAllButtons();
    }

    public void GuessLetter(char letter)
    {
        if (wrongGuesses >= maxWrongGuesses)
        {
            messageText.text = "Try again!\n You have reached the max wrong guesses! ";
            return;
        }

        char[] guessedArray = guessedWord.ToCharArray();
        bool correctGuess = false;

        for (int i = 0; i < currentRiddle.answer.Length; i++)
        {
            if (char.ToUpper(currentRiddle.answer[i]) == char.ToUpper(letter))
            {
                guessedArray[i] = currentRiddle.answer[i];
                correctGuess = true;
            }
        }

        guessedWord = new string(guessedArray);
        UpdateWordDisplay();

        if (correctGuess)
        {
            // Check if the player has solved the riddle
            if (!guessedWord.Contains("_"))
            {
                messageText.text = "Correct!\n Moving to the next riddle!";
                Invoke("LoadRandomRiddle", 2f);  // Wait for 2 seconds before loading next riddle
            }
        }
        else
        {
            wrongGuesses++;
            messageText.text = $"Wrong guess! \n You have {maxWrongGuesses - wrongGuesses} tries left.";
            if (wrongGuesses >= maxWrongGuesses)
            {
                Invoke("ResetRiddle", 2f);  // Wait for 2 seconds before resetting the riddle
            }
        }
    }

    private void UpdateWordDisplay()
    {
        wordDisplay.text = string.Join(" ", guessedWord.ToCharArray());
    }


    public void EnableAllButtons()
    {
        // Check if gridParent is assigned
        if (gridParent == null)
        {
            Debug.LogError("Grid Parent is not assigned in the Inspector!");
            return;
        }

        // Loop through each button and re-enable it
        Button[] buttons = gridParent.GetComponentsInChildren<Button>();  // Assumes the buttons are children of the 'gridParent'

        foreach (Button button in buttons)
        {
            button.interactable = true;  // Enable the button
        }
    }
    private void ResetRiddle()
    {
        messageText.text = "You failed the riddle!  Try again!";
        EnableAllButtons();  // Re-enable buttons when retrying the riddle
        Invoke("LoadRandomRiddle", 2f);  // Wait for 2 seconds before reloading the same riddle
    }
}
