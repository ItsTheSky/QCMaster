using System;
using System.Collections.Generic;
using QCMaster.Core.Models.Types;

namespace QCMaster.Core.Models;

public class QuestionSet
{
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public BaseQuestionType[] Questions { get; set; } = [];
    public int CurrentQuestionIndex { get; set; }
    
}