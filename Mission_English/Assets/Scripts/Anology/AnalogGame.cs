using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalogGame : MonoBehaviour
{
    public Text questionText;
    public Button[] optionButtons;
    public Text feedbackText;
    public Text scoreText;

    private List <Question> questions = new List<Question> ();
    private int currentQuestionIndex = 0;
    private int score = 0;

    private void Start()
    {
        feedbackText.text = "";
        score = 0;
        LoadQuestions();
        DisplayQuestions();
    }

    private void LoadQuestions()
    {
        questions.Add(new Question
        {
            question = "Ramanujan : Mathematician :: Einstein : ?",
            options = new string[] { "Physicist", "Biologist", "Chemist", "Geologist" },
            correctAnswerIndex = 0

        });

        questions.Add(new Question
        {
            question = "Cambridge : University :: Trinity : ?",
            options = new string[] { "College", "School", "Institute", "Academy" },
            correctAnswerIndex = 0
        });
    }

    private void DisplayQuestions()
    {
       if (currentQuestionIndex < questions.Count)
        {
            Question curent = questions[currentQuestionIndex];
            questionText.text = curent.question;

            for (int i = 0; i < curent.options.Length; i++) 
            {
                optionButtons[i].GetComponentInChildren<Text>().text = curent.options[i];
                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => CheckAnswer(index));

        }
    }
}

    private void CheckAnswer(int selected)
    {
        if (selected == questions[currentQuestionIndex].correctAnswerIndex)
        {
            feedbackText.text = "Correct";
            score += 10;

        }
        else 
        {
            feedbackText.text = "Try Again";   
        }

        UpdateScore();
        Invoke("NextQuestion", 1.5f);
    }


    private void NextQuestion()
    {
        feedbackText.text = "";
        currentQuestionIndex++;
        DisplayQuestions();
    }
    private void UpdateScore()
    {
        scoreText.text =$"Score : {score}";
    }
   private void EndGame()
    {
        questionText.text = "Congratulations! You've completed the game.";
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}

