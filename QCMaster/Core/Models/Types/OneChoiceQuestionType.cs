using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using QCMaster.ViewModels.Questions;
using QCMaster.Views.Questions.Types;

namespace QCMaster.Core.Models.Types;

public class OneChoiceQuestionType : BaseQuestionType
{
    public OneChoiceQuestionType()
    {
        Type = QuestionType.OneChoice;
    }

    public List<string> Choices { get; set; }
    public string CorrectChoice { get; set; }

    public override void Load(JsonNode data)
    {
        base.Load(data);
        
        Choices = [];
        foreach (var choice in data["choices"]?.AsArray() ?? new JsonArray())
            Choices.Add(choice!.ToString());
    }

    public override JsonNode Save()
    {
        var data = base.Save();
        data["choices"] = new JsonArray();
        foreach (var choice in Choices)
            (data["choices"] as JsonArray)!.Add(choice);
        data["correct"] = CorrectChoice;
        return data;
    }

    public override bool CheckAnswer(object answer)
    {
        if (answer is string userAnswer)
            return Choices.Contains(userAnswer) && userAnswer == CorrectChoice;

        return false;
    }

    public override Control CreateDisplay(BaseQuestionViewModel baseViewModel)
    {
        return new OneChoiceQuestionDisplay { DataContext = new OneChoiceQuestionViewModel(baseViewModel) };
    }
}