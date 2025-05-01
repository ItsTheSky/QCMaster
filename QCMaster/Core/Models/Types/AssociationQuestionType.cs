using System.Collections.Generic;
using System.Text.Json.Nodes;
using Avalonia.Controls;
using QCMaster.ViewModels.Questions;
using QCMaster.Views.Questions.Types;

namespace QCMaster.Core.Models.Types;

public class AssociationQuestionType : BaseQuestionType
{
    public AssociationQuestionType(Dictionary<string, string> associations = null)
    {
        Associations = associations ?? new Dictionary<string, string>();
        Type = QuestionType.Association;
    }
    
    public Dictionary<string, string> Associations { get; }
    
    public override bool CheckAnswer(object answer) 
    {
        if (answer is string[] { Length: 2 } userAnswers)
        {
            var key = userAnswers[0];
            var value = userAnswers[1];
            
            return Associations.ContainsKey(key) && Associations[key] == value;
        }
        
        return false;
    }

    public override void Load(JsonNode data)
    {
        base.Load(data);
        
        if (data["associations"] != null)
        {
            var associationsObject = data["associations"]!.AsObject();
            foreach (var property in associationsObject)
            {
                Associations[property.Key] = property.Value!.ToString();
            }
        }
    }

    public override JsonNode Save()
    {
        var data = base.Save();
        var associationsObject = new JsonObject();
        
        foreach (var pair in Associations)
        {
            associationsObject[pair.Key] = pair.Value;
        }
        
        data["associations"] = associationsObject;
        return data;
    }

    public override Control CreateDisplay(BaseQuestionViewModel baseViewModel)
    {
        return new AssociationQuestionDisplay 
        { 
            DataContext = new AssociationQuestionViewModel(baseViewModel) 
        };
    }
}