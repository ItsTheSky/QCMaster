using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCMaster.Core.Models;
using QCMaster.Core.Models.Types;
using QCMaster.Views.Questions.Types;

namespace QCMaster.ViewModels.Questions;

public partial class BaseQuestionViewModel : ObservableObject
{

    public BaseQuestionViewModel(QuestionSet questionSet)
    {
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(Answer)) 
                HasInputAnswer = Answer != null;
        };
        
        QuestionSet = questionSet;
        Question = questionSet.Questions[questionSet.CurrentQuestionIndex];
        ShowQuestion();
    }
    
    [ObservableProperty] private QuestionSet _questionSet;
    
    // progress
    public string ProgressText => $"{QuestionSet.CurrentQuestionIndex + 1} / {QuestionSet.Questions.Length}";
    public float ProgressValue
    {
        get
        {
            return (float)(QuestionSet.CurrentQuestionIndex + 1) / QuestionSet.Questions.Length;
        }
    }

    [ObservableProperty] private BaseQuestionType _question;
    [ObservableProperty] private bool _hasInputAnswer;
    [ObservableProperty] private bool _showingResults;
    
    [ObservableProperty] private object? _answer;

    private Control _questionView;
    public Control QuestionView => _questionView == null! ? CreateView() : _questionView;

    public Control CreateView()
    {
        return _questionView = Question.CreateDisplay(this);
    }

    [RelayCommand]
    public void NextQuestion()
    {
        QuestionSet.CurrentQuestionIndex++;
        
        var question = QuestionSet.Questions[QuestionSet.CurrentQuestionIndex];
        Question = question;
        
        OnPropertyChanged(nameof(ProgressText));
        OnPropertyChanged(nameof(ProgressValue));
        
        ShowQuestion();
    }

    public void ShowQuestion()
    {
        Answer = null;
        HasInputAnswer = false;
        ShowingResults = false;
        
        _questionView = null!;
        OnPropertyChanged(nameof(QuestionView));
    }

    [RelayCommand]
    public void SubmitAnswer()
    {
        if (!HasInputAnswer || Answer == null)
            return;

        ShowingResults = true;
        
        var vm = QuestionView.DataContext as IQuestionTypeViewModel;
        vm?.ShowResults();
    }

}