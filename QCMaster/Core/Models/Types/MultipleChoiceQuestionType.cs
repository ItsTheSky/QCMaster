using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Avalonia.Controls;
using QCMaster.ViewModels.Questions;
using QCMaster.Views.Questions.Types;

namespace QCMaster.Core.Models.Types;

public class MultipleChoiceQuestionType : BaseQuestionType
{
    public MultipleChoiceQuestionType()
    {
        Type = QuestionType.MultipleChoice;
    }

    public override void Load(JsonNode data)
    {
        base.Load(data);
        
        if (data["choices"] is JsonArray choices)
            Choices = choices.Select(c => c!.ToString()).ToList();
    }

    public override JsonNode Save()
    {
        var json = base.Save();
        json["choices"] = new JsonArray();
        foreach (var choice in Choices)
            (json["choices"] as JsonArray)!.Add(choice);
        return json;
    }

    public List<string> Choices { get; set; }
    public List<string> CorrectChoices { get; set; }
    
    public override bool CheckAnswer(object answer)
    {
        if (answer is List<string> userAnswers)
        {
            return userAnswers.Count == CorrectChoices.Count && 
                   !userAnswers.Except(CorrectChoices).Any() && 
                   !CorrectChoices.Except(userAnswers).Any();
        }
        
        return false;
    }

    public override Control CreateDisplay(BaseQuestionViewModel baseViewModel)
    {
        return new MultipleChoicesQuestionDisplay()
            { DataContext = new MultipleChoicesQuestionViewModel(baseViewModel) };
    }
}