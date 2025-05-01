using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using QCMaster.Core.Models.Types;

namespace QCMaster.ViewModels.Questions;

public partial class TrueFalseQuestionViewModel : ObservableObject, IQuestionTypeViewModel
{
    public BaseQuestionViewModel BaseViewModel { get; }

    [ObservableProperty] private bool _isTrue;
    [ObservableProperty] private bool _isFalse;
    private bool _correctAnswer;
    
    [ObservableProperty] private bool _doShowResults;
    [ObservableProperty] private NotificationType _trueNotificationType;
    [ObservableProperty] private NotificationType _falseNotificationType;
    [ObservableProperty] private bool _trueWasSelected;
    [ObservableProperty] private bool _falseWasSelected;
    
    public TrueFalseQuestionViewModel(BaseQuestionViewModel baseViewModel)
    {
        BaseViewModel = baseViewModel;
        var question = (baseViewModel.Question as TrueFalseQuestionType)!;
        _correctAnswer = question.CorrectAnswer;
        
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(IsTrue))
            {
                if (IsTrue)
                {
                    IsFalse = false;
                    BaseViewModel.Answer = true;
                }
            }
            else if (args.PropertyName == nameof(IsFalse))
            {
                if (IsFalse)
                {
                    IsTrue = false;
                    BaseViewModel.Answer = false;
                }
            }
        };
    }

    public void ShowResults()
    {
        DoShowResults = true;
        
        if (BaseViewModel.Answer is bool userAnswer)
        {
            TrueWasSelected = userAnswer;
            FalseWasSelected = !userAnswer;
            
            TrueNotificationType = _correctAnswer ? NotificationType.Success : NotificationType.Error;
            FalseNotificationType = !_correctAnswer ? NotificationType.Success : NotificationType.Error;
        }
    }
}