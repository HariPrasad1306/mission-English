using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WordSearchManager : MonoBehaviour
{
    public int gridSize = 10;
    public GameObject letterButtonPrefab;
    public Transform gridParent;
    public Transform wordListparent;
    public TextMeshProUGUI timerText;
    public List<string> wordList;

    private char[,] grid;
    private float timeRemaining = 180f;
    private bool gameActive = true;
    void Start()
    {
        GenerateGrid();
        PlaceWords();
        FillEmptySpaces();
        CreateGridUI();
        DisplayWordList();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            UpdateTimer();
        }
    }

    private void GenerateGrid()
    {
        grid =new char[gridSize, gridSize];
    }

    private void PlaceWords()
    {
        foreach (string word in wordList)
        {

        }
    }
    private void FillEmptySpaces()
    {
        for(int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (grid[i,j] == '\0')
                {
                    grid[i, j] = (char)Random.Range(65, 91);
                }
            }
        }
    }

    private void CreateGridUI()
    {
        for (int i = 0; i < gridSize;i++)
        {
            for (int j =0; j < gridSize; j++)
            {
                GameObject button = Instantiate(letterButtonPrefab, gridParent);
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = grid[i,j].ToString();
            }
        }
    }

    private void DisplayWordList()
    {
        foreach(string word in wordList)
        {
            GameObject wordText = new GameObject(word);
            wordText.transform.SetParent(wordListparent);
            TextMeshProUGUI textComponent = wordText.AddComponent<TextMeshProUGUI>();
            textComponent.text = word;
            textComponent.fontSize = 18;
        }
    }

    private void UpdateTimer()
    {
        if(timeRemaining > 0)
        {
            timeRemaining-= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeRemaining/60);
            int seconds = Mathf.FloorToInt(timeRemaining%60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            gameActive = false;
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("Game Over!");
    }
}
