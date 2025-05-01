using System.Text.Json;
using System.Text.Json.Nodes;
using Avalonia.Controls;
using QCMaster.ViewModels.Questions;
using QCMaster.Views.Questions;

namespace QCMaster.Core.Models.Types;

public abstract class BaseQuestionType
{
    
    public string Question { get; set; }
    public string? Hint { get; set; }
    public QuestionType Type { get; protected set; }
    
    public abstract bool CheckAnswer(object answer);
    
    public virtual void Load(JsonNode data)
    {
        Question = data["question"]?.ToString() ?? string.Empty;
        Hint = data["hint"]?.ToString();
    }
    
    public virtual JsonNode Save()
    {
        return new JsonObject
        {
            ["question"] = Question,
            ["hint"] = Hint,
            ["type"] = Type.ToString()
        };
    }

    public abstract Control CreateDisplay(BaseQuestionViewModel baseViewModel);
}