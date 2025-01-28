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
    private int currentRiddleIndex = 0;  // Track which riddle is being used
    private const int maxWrongGuesses = 12;  // Maximum number of wrong guesses (to trigger 12 animations)

    public TMPro.TextMeshProUGUI riddleText;
    public TMPro.TextMeshProUGUI wordDisplay;
    public TMPro.TextMeshProUGUI messageText;  // To display messages like "Wrong Guess" or "Next Riddle"
    public Transform gridParent; // Reference to the parent of all letter buttons

    // Audio variables
    public AudioSource audioSource;  // The AudioSource to play sounds
    public AudioClip correctClip;    // Sound to play when the guess is correct
    public AudioClip wrongClip;      // Sound to play when the guess is wrong
    public AudioClip nextRiddleClip; // Sound to play when moving to the next riddle
    public AudioClip failClip;       // Sound to play when failing a riddle

    // Animator for animations on wrong guesses
    public Animator animator;  // Reference to the Animator
    public string wrongGuessAnimationParameter = "WrongGuess";  // The name of the Animator trigger for wrong guesses

    void Start()
    {
        LoadNextRiddle();  // Start with the first riddle
    }

    public void LoadNextRiddle()
    {
        if (currentRiddleIndex >= riddles.Count)
        {
            messageText.text = "Congratulations! You've solved all riddles!";
            PlaySound(failClip);  // Play sound on game completion
            return;
        }

        currentRiddle = riddles[currentRiddleIndex];
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
            PlaySound(failClip);  // Play failure sound
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
            PlaySound(correctClip);  // Play correct guess sound
            // Check if the player has solved the riddle
            if (!guessedWord.Contains("_"))
            {
                messageText.text = "Correct!\n Moving to the next riddle!";
                currentRiddleIndex++;  // Increment to the next riddle
                PlaySound(nextRiddleClip);  // Play next riddle sound
                Invoke("LoadNextRiddle", 2f);  // Wait for 2 seconds before loading next riddle
            }
        }
        else
        {
            wrongGuesses++;
            PlaySound(wrongClip);  // Play wrong guess sound
            TriggerWrongGuessAnimation();  // Trigger the animation for wrong guesses
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
        messageText.text = "You failed the riddle! Try again!";
        PlaySound(failClip);  // Play failure sound
        EnableAllButtons();  // Re-enable buttons when retrying the riddle
        Invoke("LoadNextRiddle", 2f);  // Wait for 2 seconds before reloading the same riddle
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);  // Play the sound once
        }
    }

    private void TriggerWrongGuessAnimation()
    {
        // Make sure the animator is assigned and has the "WrongGuess" trigger parameter.
        if (animator != null)
        {
            animator.SetTrigger(wrongGuessAnimationParameter + wrongGuesses);  // Trigger the correct animation based on wrong guess count
        }
    }
}
