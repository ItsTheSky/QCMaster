using System.Text.Json.Nodes;
using Avalonia.Controls;
using QCMaster.ViewModels.Questions;
using QCMaster.Views.Questions.Types;

namespace QCMaster.Core.Models.Types;

public class TrueFalseQuestionType : BaseQuestionType
{
    public TrueFalseQuestionType(bool correctAnswer = false)
    {
        CorrectAnswer = correctAnswer;
        Type = QuestionType.TrueFalse;
    }
    
    public bool CorrectAnswer { get; set; }
    
    public override bool CheckAnswer(object answer)
    {
        if (answer is bool userAnswer)
        {
            return userAnswer == CorrectAnswer;
        }
        return false;
    }

    public override void Load(JsonNode data)
    {
        base.Load(data);
        if (data["correct"] != null)
        {
            CorrectAnswer = data["correct"]!.GetValue<bool>();
        }
    }

    public override JsonNode Save()
    {
        var data = base.Save();
        data["correct"] = CorrectAnswer;
        return data;
    }

    public override Control CreateDisplay(BaseQuestionViewModel baseViewModel)
    {
        return new TrueFalseQuestionDisplay 
        { 
            DataContext = new TrueFalseQuestionViewModel(baseViewModel) 
        };
    }
}