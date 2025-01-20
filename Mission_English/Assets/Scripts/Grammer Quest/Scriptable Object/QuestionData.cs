using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question1
{
    public string[] subQuestions; // Array for 4 sub-questions.
    public string[] correctAnswers; // Array for correct answers.
}

[CreateAssetMenu(fileName = "QuestionData", menuName = "GrammarQuest/QuestionData")]
public class QuestionData : ScriptableObject
{
    public List<Question1> questions = new List<Question1>();
}
