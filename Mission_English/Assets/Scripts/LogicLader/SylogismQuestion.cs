using UnityEngine;

[CreateAssetMenu(fileName = "New Syllogism Question Data", menuName = "Syllogism/QuestionData")]
public class SyllogismQuestionData : ScriptableObject
{
    public SyllogismQuestion[] questions;  // Array of syllogism questions

    [System.Serializable]
    public class SyllogismQuestion
    {
        [TextArea(3, 10)]
        public string premise1;  // First premise

        [TextArea(3, 10)]
        public string premise2;  // Second premise

        [TextArea(3, 10)]
        public string conclusion;  // Conclusion

        public bool isConclusionValid;  // true if conclusion is valid, false if invalid

        public string hint;  // Optional hint to assist player
        public int difficulty;  // Difficulty level, e.g., 1 for easy, 2 for medium, 3 for hard
    }
}
