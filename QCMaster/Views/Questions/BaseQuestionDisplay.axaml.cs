using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using QCMaster.Core.Models;
using QCMaster.Core.Models.Types;
using QCMaster.ViewModels.Questions;

namespace QCMaster.Views.Questions;

public partial class BaseQuestionDisplay : UserControl
{
    public BaseQuestionDisplay()
    {
        InitializeComponent();

        var questionSet = new QuestionSet
        {
            Name = "Example Question Set",
            Description = "This is an example question set.",
            CreatedAt = DateTime.Now,

            Questions =
            [
                new AssociationQuestionType(new Dictionary<string, string>
                {
                    { "France", "Paris" },
                    { "Germany", "Berlin" },
                    { "Italy", "Rome" },
                    { "Spain", "Madrid" },
                    { "United Kingdom", "London" }
                })
                {
                    Question = "Match each country with its capital city",
                },
                new TrueFalseQuestionType()
                {
                    Question = "Is the sky blue?",
                    CorrectAnswer = true,
                },
                new TrueFalseQuestionType()
                {
                    Question = "Are you fine? :(",
                    CorrectAnswer = false,
                },
                new MultipleChoiceQuestionType
                {
                    Question = "Which of the following are compiled languages?",
                    Choices = ["C", "Python", "Java", "JavaScript", "C++", "Scala", "Go"],
                    CorrectChoices = ["C", "Java", "C++"],
                },
                new OneChoiceQuestionType
                {
                    Choices = ["Choice 1", "Choice 2", "Choice 3"],
                    CorrectChoice = "Choice 2",
                    Question = "What is the capital of France?",
                },
                new OneChoiceQuestionType
                {
                    Choices = ["HTML", "CSS", "JavaScript"],
                    Question = "Which of the following is a programming language?",
                    CorrectChoice = "JavaScript",
                },
            ]
        };

        DataContext = new BaseQuestionViewModel(questionSet);
    }
}