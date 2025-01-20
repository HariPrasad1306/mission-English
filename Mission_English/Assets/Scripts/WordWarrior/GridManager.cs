using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSize = 10; // 10x10 grid
    public GameObject cellPrefab; // GridCell prefab
    public Transform gridParent; // Parent for grid cells

    [Header("Word Settings")]
    public string[] wordList = { "CAT", "DOG", "BIRD", "FISH", "HOUSE" }; // List of words to hide
    private char[,] gridData; // Grid array to hold letters

    [Header("UI Settings")]
    public TextMeshProUGUI timerText; // Timer display
    public TextMeshProUGUI messageText; // Message display
    public Button retryButton; // Retry button (optional)

    private float timeRemaining = 180f; // 3 minutes
    private bool isGameRunning = true;

    void Start()
    {
        gridData = new char[gridSize, gridSize];
        CreateGrid();
        PlaceWords();
    }

    void Update()
    {
        if (isGameRunning)
        {
            UpdateTimer();
        }
    }

    void CreateGrid()
    {
        // Create grid UI
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject cell = Instantiate(cellPrefab, gridParent);
                cell.name = $"Cell_{row}_{col}";
                cell.GetComponentInChildren<TextMeshProUGUI>().text = "."; // Placeholder
            }
        }
    }

    void PlaceWords()
    {
        foreach (string word in wordList)
        {
            bool placed = false;

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

        UpdateGridUI();
    }

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

    void PlaceWord(string word, int startX, int startY, int direction)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int x = startX, y = startY;

            if (direction == 0) y += i; // Horizontal
            else if (direction == 1) x += i; // Vertical
            else if (direction == 2) { x += i; y += i; } // Diagonal

            gridData[x, y] = word[i];
        }
    }

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

    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            isGameRunning = false;
            timerText.text = "00:00";
            EndGame(false);
            return;
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void EndGame(bool won)
    {
        isGameRunning = false;
        messageText.text = won ? "You Win!" : "Time's Up! Try Again.";
        retryButton.gameObject.SetActive(true);
    }

    public void RetryGame()
    {
        // Reload the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
