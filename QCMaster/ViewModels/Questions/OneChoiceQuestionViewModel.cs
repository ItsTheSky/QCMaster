using System;
using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCMaster.Core.Models.Types;

namespace QCMaster.ViewModels.Questions;

public partial class OneChoiceQuestionViewModel : ObservableObject, IQuestionTypeViewModel
{
    public BaseQuestionViewModel BaseViewModel { get; }

    [ObservableProperty] private ObservableCollection<Choice> _choices;
    [ObservableProperty] private string _correctChoice;
        
    public OneChoiceQuestionViewModel(BaseQuestionViewModel baseViewModel)
    {
        BaseViewModel = baseViewModel;
        var question = (baseViewModel.Question as OneChoiceQuestionType)!;
        
        Choices = [];
        foreach (var choice in question.Choices)
        {
            Choices.Add(new Choice(this) { Text = choice });
        }
        
        CorrectChoice = question.CorrectChoice;
    }

    public void ShowResults()
    {
        var answer = BaseViewModel.Answer as string ?? string.Empty;
        foreach (var choice in Choices)
        { 
            choice.ShowResults = true;

            choice.NotificationType = CorrectChoice == choice.Text
                ? NotificationType.Success
                : NotificationType.Error;
            choice.WasSelected = answer == choice.Text;
        }
    }
}

public partial class Choice : ObservableObject
{
    public Choice(OneChoiceQuestionViewModel viewModel)
    {
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName != nameof(IsChecked)) 
                return;

            if (!IsChecked) 
                return;
            
            foreach (var choice in viewModel.Choices)
            {
                if (choice == this) 
                    continue;
                
                choice.IsChecked = false;
            }
            
            viewModel.BaseViewModel.Answer = Text;
        };
    }
        
    [ObservableProperty] private string _text;
    [ObservableProperty] private bool _isChecked;

    [ObservableProperty] private bool _showResults;
    [ObservableProperty] private NotificationType _notificationType; 
    [ObservableProperty] private bool _wasSelected;
        
}