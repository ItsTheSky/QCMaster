using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCMaster.Core.Models.Types;

namespace QCMaster.ViewModels.Questions;

public partial class MultipleChoicesQuestionViewModel : ObservableObject, IQuestionTypeViewModel
{
    public BaseQuestionViewModel BaseViewModel { get; }

    [ObservableProperty] private ObservableCollection<MultipleChoice> _choices;
    [ObservableProperty] private List<string> _correctChoices;
        
    public MultipleChoicesQuestionViewModel(BaseQuestionViewModel baseViewModel)
    {
        BaseViewModel = baseViewModel;
        var question = (baseViewModel.Question as MultipleChoiceQuestionType)!;
        
        Choices = [];
        foreach (var choice in question.Choices)
            Choices.Add(new MultipleChoice(this) { Text = choice });
        
        CorrectChoices = question.CorrectChoices;
    }

    public void ShowResults()
    {
        if (BaseViewModel.Answer is not List<string> answer) 
            return;
        
        foreach (var choice in Choices)
        { 
            choice.ShowResults = true;
            
            choice.NotificationType = CorrectChoices.Contains(choice.Text)
                ? NotificationType.Success
                : NotificationType.Error;
            choice.WasSelected = answer.Contains(choice.Text);
        }
    }
}

public partial class MultipleChoice : ObservableObject
{
    public MultipleChoice(MultipleChoicesQuestionViewModel viewModel)
    {
        viewModel.BaseViewModel.Answer = new List<string>();
        
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName != nameof(IsChecked)) 
                return;

            if (viewModel.BaseViewModel.Answer is not List<string> answer) 
                return;
            
            if (IsChecked) 
                answer.Add(Text);
            else
                answer.Remove(Text);
        };
    }
        
    [ObservableProperty] private string _text;
    [ObservableProperty] private bool _isChecked;

    [ObservableProperty] private bool _showResults;
    [ObservableProperty] private NotificationType _notificationType; 
    [ObservableProperty] private bool _wasSelected;
        
}