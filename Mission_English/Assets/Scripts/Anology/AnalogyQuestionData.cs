
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnalogyQuestionData", menuName = "ScriptableObjects/QuestionData", order = 1)]
public class AnalogyQuestionData : ScriptableObject
{
    [Serializable]
    public class Question3
    {
        public string question;
        public string[] options;
        public int correctAnswerIndex;
    }

    public Question3[] questions;
}
