using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GridManager1 : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSize = 10; // Define grid size (10x10)
    public GameObject cellPrefab; // Prefab for the grid cell (button)
    public Transform gridParent; // Parent object to hold grid cells

    [Header("Word List")]
    public string[] wordList = { "CAT", "DOG", "BIRD", "FISH", "HOUSE" }; // Example words

    [Header("UI Settings")]
    public TextMeshProUGUI timerText; // Timer UI
    public TextMeshProUGUI messageText; // Game message UI
    public Button retryButton; // Retry button to restart the game

    private char[,] gridData; // Array to store grid data
    private float timeRemaining = 180f; // 3 minutes timer
    private bool isGameRunning = true;

    void Start()
    {
        gridData = new char[gridSize, gridSize]; // Initialize the grid
        retryButton.gameObject.SetActive(false); // Hide retry button initially
        CreateGrid(); // Create the grid UI
        PlaceWords(); // Place words in the grid
    }

    void Update()
    {
        if (isGameRunning)
        {
            UpdateTimer(); // Update the timer each frame
        }
    }

    // Method to create the grid layout
    void CreateGrid()
    {
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject cell = Instantiate(cellPrefab, gridParent);
                cell.name = $"Cell_{row}_{col}";

                Button button = cell.GetComponent<Button>();
                button.onClick.AddListener(() => OnCellClicked(row, col));

                TextMeshProUGUI text = cell.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "."; // Initially set the cell text to '.'
            }
        }
    }

    // Method to place words in the grid randomly
    void PlaceWords()
    {
        foreach (string word in wordList)
        {
            bool placed = false;

            // Try to place word up to 100 times
            for (int attempts = 0; attempts < 100 && !placed; attempts++)
            {
                int startX = Random.Range(0, gridSize);
                int startY = Random.Range(0, gridSize);
                int direction = Random.Range(0, 3); // 0: Horizontal, 1: Vertical, 2: Diagonal

                if (CanPlaceWord(word, startX, startY, direction))
                {
                    PlaceWord(word, startX, startY, direction);
                    placed = true;
                }
            }

            if (!placed)
            {
                Debug.LogWarning($"Failed to place word: {word}");
            }
        }

        UpdateGridUI(); // Update the grid after placing the words
    }

    // Method to check if the word can be placed
    bool CanPlaceWord(string word, int startX, int startY, int direction)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int x = startX, y = startY;

            if (direction == 0) y += i; // Horizontal
            else if (direction == 1) x += i; // Vertical
            else if (direction == 2) { x += i; y += i; } // Diagonal

            if (x < 0 || x >= gridSize || y < 0 || y >= gridSize) return false;
            if (gridData[x, y] != '\0' && gridData[x, y] != word[i]) return false;
        }

        return true;
    }

    // Method to place the word in the grid
    void PlaceWord(string word, int startX, int startY, int direction)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int x = startX, y = startY;

            if (direction == 0) y += i; // Horizontal
            else if (direction == 1) x += i; // Vertical
            else if (direction == 2) { x += i; y += i; } // Diagonal

            gridData[x, y] = word[i]; // Store the letter in the grid
        }
    }

    // Method to update the grid display in the UI
    void UpdateGridUI()
    {
        int index = 0;
        foreach (Transform child in gridParent)
        {
            int row = index / gridSize;
            int col = index % gridSize;

            char letter = gridData[row, col];
            child.GetComponentInChildren<TextMeshProUGUI>().text = letter == '\0' ? "." : letter.ToString();
            index++;
        }
    }

    // Timer update method
    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isGameRunning = false;
            timerText.text = "00:00";
            EndGame(false); // Game over
            return;
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Game Over method
    void EndGame(bool won)
    {
        messageText.text = won ? "You Win!" : "Time's Up! Try Again.";
        retryButton.gameObject.SetActive(true); // Show retry button
    }

    // Retry method to restart the game
    public void RetryGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); // Reload the scene
    }

    // Handle when a grid cell is clicked
    void OnCellClicked(int row, int col)
    {
        // Logic for selecting a word (you can implement word finding here)
        Debug.Log($"Cell clicked: {row}, {col}");
    }
}

