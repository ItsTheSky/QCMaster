using System.Collections.Generic;
using System.Text.Json;
using Avalonia.Controls;
using QCMaster.ViewModels.Questions;

namespace QCMaster.Core.Models.Types;

public class AssociationQuestionType : BaseQuestionType
{
    public AssociationQuestionType(Dictionary<string, string> associations)
    {
        Associations = associations;
        Type = QuestionType.Association;
    }
    
    public Dictionary<string, string> Associations { get; }
    
    public override bool CheckAnswer(object answer) {
        if (answer is string[] { Length: 2 } userAnswers)
        {
            var key = userAnswers[0];
            var value = userAnswers[1];
            
            return Associations.ContainsKey(key) && Associations[key] == value;
        }
        
        return false;
    }

    public override Control CreateDisplay(BaseQuestionViewModel baseViewModel)
    {
        throw new System.NotImplementedException();
    }
}