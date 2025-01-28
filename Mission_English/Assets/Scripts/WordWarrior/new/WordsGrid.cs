using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition;
    [SerializeField] private float xOffsetv = 0.0f;
    [SerializeField] private float yOffsetv = 0.0f;

    private List<GameObject> _squareList = new List<GameObject>();

    private void Start()
    {
        SpawnGridSquares();
        SetSquaresPosition();
        UpdateTopPosition();

    }
    private void UpdateTopPosition()
    {
        topPosition = Camera.main.orthographicSize; // Adjusts to the camera size dynamically
    }
    private void SetSquaresPosition()
    {
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _squareList[0].GetComponent<Transform>();

        var offset = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y + squareOffset) * 0.01f
        };

        var startPosition = GetFirstSquarePosition();
        int columNumber = 0;
        int rowNumber = 0;

        foreach (var square in _squareList)
        {
            if (rowNumber + 1 > currentGameData.selectedBoardData.Rows)
            {
                columNumber++;
                rowNumber = 0;
            }

            var positionX = startPosition.x + offset.x * columNumber;
            var positionY = startPosition.y + offset.y * rowNumber;

            square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            rowNumber++;

        }
    }
    private Vector2 GetFirstSquarePosition()
    {
        // Check if the square list is valid and not empty
        if (_squareList == null || _squareList.Count == 0)
        {
            Debug.LogError("Square list is empty. Make sure the grid squares are initialized.");
            return Vector2.zero;
        }

        // Get the size of a single square in world units
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2
        {
            x = squareRect.width * squareTransform.localScale.x * 0.01f,
            y = squareRect.height * squareTransform.localScale.y * 0.01f
        };

        // Calculate the total dimensions of the grid
        float totalGridWidth = currentGameData.selectedBoardData.Colums * squareSize.x;
        float totalGridHeight = currentGameData.selectedBoardData.Rows * squareSize.y;

        // Get the screen dimensions in world units
        float screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        float screenHeight = Camera.main.orthographicSize * 2;

        // Default offsets for fine-tuning the grid position
        float xOffset = xOffsetv;  // Adjust this value to shift grid horizontally
        float yOffset = yOffsetv;  // Adjust this value to shift grid vertically

        // Calculate the start position of the grid
        float gridStartX = -totalGridWidth / 2 + xOffset;  // Centered horizontally with offset
        float gridStartY = screenHeight / 2 - totalGridHeight + yOffset;  // Aligned to top with offset

        // Log debug information for troubleshooting
        Debug.Log($"Grid Size: {totalGridWidth}x{totalGridHeight}");
        Debug.Log($"Screen Dimensions: {screenWidth}x{screenHeight}");
        Debug.Log($"Grid Start Position: {gridStartX}, {gridStartY}");

        return new Vector2(gridStartX, gridStartY);
    }

    //private Vector2 GetFirstSquarePosition()
    //{
    //    var startposition = new Vector2(0f, Camera.main.orthographicSize);
    //    var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
    //    var squareTransform = _squareList[0].GetComponent<Transform>();
    //    var squareSize = new Vector2(0f, 0f);

    //    squareSize.x =squareRect.width* squareTransform.localScale.x;
    //    squareSize.y =squareRect.height* squareTransform.localScale.y;

    //    var midWidthPosition = (((currentGameData.selectedBoardData.Colums - 1) * squareSize.x) / 2) * 0.01f;
    //    var midWidthHeight = (((currentGameData.selectedBoardData.Rows - 1) * squareSize.y) / 2) * 0.01f;

    //    //startposition.x = (midWidthPosition != 0f) ? midWidthPosition * -1 : midWidthPosition;
    //    //startposition.y = midWidthHeight;

    //    startposition.x = -midWidthPosition;
    //    startposition.y = Camera.main.orthographicSize - midWidthHeight;

    //    return startposition;

    //}

    private void SpawnGridSquares()
    {
        if (currentGameData != null)
        {
            var squareScale = GetSquareScale(new Vector3(2f, 2f, 0.1f));
            foreach (var squares in currentGameData.selectedBoardData.Board)
            {
                foreach (var squareLetter in squares.Row)
                {
                    var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squareLetter);
                    var SelectedLetterData = alphabetData.AlphabetHighlighted.Find(data => data.letter == squareLetter);
                    var correctLetterData = alphabetData.AlphabetWrong.Find(data => data.letter == squareLetter);

                    if (normalLetterData.image == null || correctLetterData.image == null)
                    {
                        Debug.LogError("All fields in your Array should have some letters. press Fill up with Random Button in your board data to add random letter.Letter" + squareLetter);

#if UNITY_EDITOR

                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
#endif
                    }
                    else
                    {
                        _squareList.Add(Instantiate(gridSquarePrefab));
                        _squareList[_squareList.Count - 1].GetComponent<GridSquare>().SetSprite(normalLetterData, correctLetterData, SelectedLetterData);
                        _squareList[_squareList.Count - 1].transform.SetParent(this.transform);
                        _squareList[_squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        _squareList[_squareList.Count - 1].transform.localScale = squareScale;
                        _squareList[_squareList.Count - 1].GetComponent<GridSquare>().SetIndex(_squareList.Count - 1);
                    }
                }
            }
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;

        // Ensure the squares are large enough
        float minScale = 0.5f; // Set a minimum scale to prevent the grid from being too small

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= minScale || finalScale.y <= minScale)
            {
                finalScale.x = minScale;
                finalScale.y = minScale;
                return finalScale;
            }
        }

        return finalScale;
    }



    //private Vector3 GetSquareScale(Vector3 defaultScale)
    //{
    //    var finalScale = defaultScale;
    //    var adjustment = 0.01f;

    //    while (ShouldScaleDown(finalScale))
    //    {
    //        finalScale.x -= adjustment;
    //        finalScale.y -= adjustment;

    //        if (finalScale.x <= 0 || finalScale.y <= 0)
    //        {
    //            finalScale.x = adjustment;
    //            finalScale.y = adjustment;
    //            return finalScale;
    //        }
    //    }
    //    // Scale proportionally to the camera view
    //    finalScale *= Camera.main.orthographicSize / 5f; // Adjust "5f" to your default orthographic size
    //    return finalScale;
    //}
    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midWidthPosition = ((currentGameData.selectedBoardData.Colums * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = ((currentGameData.selectedBoardData.Rows * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y = midWidthHeight;

        return startPosition.x > GetHalfScreenWidth() * -1 || startPosition.y > topPosition;

    }

    private float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width / 2;
    }
}
