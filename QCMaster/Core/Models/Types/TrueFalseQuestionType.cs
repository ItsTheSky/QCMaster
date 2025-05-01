using System.Text.Json;
using Avalonia.Controls;
using QCMaster.ViewModels.Questions;

namespace QCMaster.Core.Models.Types;

public class TrueFalseQuestionType : BaseQuestionType
{
    public TrueFalseQuestionType(bool correctAnswer)
    {
        CorrectAnswer = correctAnswer;
        Type = QuestionType.TrueFalse;
    }
    
    public bool CorrectAnswer { get; init; }
    
    public override bool CheckAnswer(object answer)
    {
        if (answer is bool userAnswer)
        {
            return userAnswer == CorrectAnswer;
        }
        return false;
    }

    public override Control CreateDisplay(BaseQuestionViewModel baseViewModel)
    {
        throw new System.NotImplementedException();
    }
}