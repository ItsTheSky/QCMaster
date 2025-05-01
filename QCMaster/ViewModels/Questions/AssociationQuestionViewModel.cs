using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QCMaster.Core.Models.Types;

namespace QCMaster.ViewModels.Questions;

public partial class AssociationQuestionViewModel : ObservableObject, IQuestionTypeViewModel
{
    private static readonly List<Color> Colors =
    [
        Color.FromRgb(0xFF, 0x00, 0x00),
        Color.FromRgb(0x00, 0xFF, 0x00),
        Color.FromRgb(0x00, 0x00, 0xFF),
        Color.FromRgb(0xFF, 0xFF, 0x00),
        Color.FromRgb(0xFF, 0x00, 0xFF),
        Color.FromRgb(0x00, 0xFF, 0xFF),
        Color.FromRgb(0xFF, 0x7F, 0x00),
        Color.FromRgb(0x7F, 0x00, 0xFF),
        Color.FromRgb(0x00, 0x7F, 0xFF),
        Color.FromRgb(0x7F, 0x7F, 0x00),
        Color.FromRgb(0x7F, 0x00, 0x7F),
        Color.FromRgb(0x00, 0x7F, 0x7F),
        Color.FromRgb(0xFF, 0x7F, 0x7F),
        Color.FromRgb(0x7F, 0xFF, 0x7F)
    ];
    
    public BaseQuestionViewModel BaseViewModel { get; }
    
    [ObservableProperty] private ObservableCollection<AssociationItem> _leftItems;
    [ObservableProperty] private ObservableCollection<AssociationItem> _rightItems;
    [ObservableProperty] private AssociationItem _selectedLeftItem;
    [ObservableProperty] private AssociationItem _selectedRightItem;
    
    [ObservableProperty] private bool _canReset = true;
    
    [ObservableProperty] private ObservableCollection<AssociationConnection> _connections;
    
    public Dictionary<string, string> Associations { get; }
    
    public AssociationQuestionViewModel(BaseQuestionViewModel baseViewModel)
    {
        BaseViewModel = baseViewModel;
        var question = (baseViewModel.Question as AssociationQuestionType)!;
        Associations = question.Associations;
        
        LeftItems = [];
        RightItems = [];
        Connections = [];
        
        // Add left items (keys)
        foreach (var key in Associations.Keys)
        {
            LeftItems.Add(new AssociationItem { Text = key, IsLeft = true, SelectCommand = SelectLeftCommand });
        }
        
        // Add right items (values) in random order
        var values = Associations.Values.ToList();
        var random = new Random();
        while (values.Count > 0)
        {
            var index = random.Next(values.Count);
            var value = values[index];
            values.RemoveAt(index);
            
            RightItems.Add(new AssociationItem { Text = value, IsLeft = false, SelectCommand = SelectRightCommand });
        }
        
        // Initialize the answer structure
        BaseViewModel.Answer = new Dictionary<string, string>();
    }
    
    // Add this to the AssociationQuestionViewModel class
    [RelayCommand]
    private void SelectLeft(AssociationItem item)
    {
        Console.WriteLine("selected left: " + item.Text);
        // Unselect all other left items
        foreach (var leftItem in LeftItems)
            leftItem.IsSelected = leftItem == item;
    
        item.IsSelected = true;
        SelectedLeftItem = item;

        Console.WriteLine("(pre) current selected right: " + SelectedRightItem);
        if (SelectedRightItem != null!)
        {
            Console.WriteLine("current selected right: " + SelectedRightItem.Text);
            // make the connection
            var existingConnection = Connections.FirstOrDefault(c => c.LeftItem == SelectedLeftItem);
            if (existingConnection != null)
            {
                Connections.Remove(existingConnection);
            }
            var connectionExists = Connections.Any(c => c.RightItem == SelectedRightItem);
            if (connectionExists)
            {
                var existingRightConnection = Connections.First(c => c.RightItem == SelectedRightItem);
                Connections.Remove(existingRightConnection);
            }
            Connections.Add(new AssociationConnection
            {
                LeftItem = SelectedLeftItem,
                RightItem = SelectedRightItem
            });
            
            Console.WriteLine("added connection: " + SelectedLeftItem.Text + " -> " + SelectedRightItem.Text);
            
            var color = Colors[Connections.Count % Colors.Count];
            
            SelectedLeftItem.IsSelected = false;
            SelectedLeftItem.IsEnabled = false;
            SelectedRightItem.IsSelected = false;
            SelectedRightItem.IsEnabled = false;
            SelectedLeftItem.ConnectionColor = new SolidColorBrush(color, 0.7);
            SelectedRightItem.ConnectionColor = new SolidColorBrush(color, 0.7);
            SelectedLeftItem = null!;
            SelectedRightItem = null!;
        }
    }

    [RelayCommand]
    private void SelectRight(AssociationItem item)
    {
        Console.WriteLine("selected right: " + item.Text);
        // Unselect all other right items
        foreach (var rightItem in RightItems)
            rightItem.IsSelected = rightItem == item;
    
        item.IsSelected = true;
        SelectedRightItem = item;
        
        Console.WriteLine("(pre) current selected left: " + SelectedLeftItem);
        if (SelectedLeftItem != null!)
        {
            Console.WriteLine("current selected left: " + SelectedLeftItem.Text);
            // make the connection
            var existingConnection = Connections.FirstOrDefault(c => c.RightItem == SelectedRightItem);
            if (existingConnection != null)
            {
                Connections.Remove(existingConnection);
            }
            var connectionExists = Connections.Any(c => c.LeftItem == SelectedLeftItem);
            if (connectionExists)
            {
                var existingLeftConnection = Connections.First(c => c.LeftItem == SelectedLeftItem);
                Connections.Remove(existingLeftConnection);
            }
            Connections.Add(new AssociationConnection
            {
                LeftItem = SelectedLeftItem,
                RightItem = SelectedRightItem
            });
            
            Console.WriteLine("added connection: " + SelectedLeftItem.Text + " -> " + SelectedRightItem.Text);
            
            var color = Colors[Connections.Count % Colors.Count];
            
            SelectedLeftItem.IsSelected = false;
            SelectedLeftItem.IsEnabled = false;
            SelectedRightItem.IsSelected = false;
            SelectedRightItem.IsEnabled = false;
            SelectedLeftItem.ConnectionColor = new SolidColorBrush(color, 0.7);
            SelectedRightItem.ConnectionColor = new SolidColorBrush(color, 0.7);
            SelectedLeftItem = null!;
            SelectedRightItem = null!;
        }
    }
    
    public void ShowResults()
    {
        CanReset = false;
        
        foreach (var connection in Connections)
        {
            var leftItem = connection.LeftItem;
            var rightItem = connection.RightItem;
            
            rightItem.IsEnabled = true;
            leftItem.IsEnabled = true;
            
            if (Associations.TryGetValue(leftItem.Text, out var value) && value == rightItem.Text)
            {
                leftItem.IsCorrect = true;
                rightItem.IsCorrect = true;
            }
            else
            {
                leftItem.IsWrong = true;
                rightItem.IsWrong = true;
            }
        }
    }
    
    [RelayCommand]
    public void Reset()
    {
        // Reset the connections
        Connections.Clear();
        
        // Reset the left items
        foreach (var leftItem in LeftItems)
        {
            leftItem.IsSelected = false;
            leftItem.IsEnabled = true;
            leftItem.ConnectionColor = Brushes.Transparent;
        }
        
        // Reset the right items
        foreach (var rightItem in RightItems)
        {
            rightItem.IsSelected = false;
            rightItem.IsEnabled = true;
            rightItem.ConnectionColor = Brushes.Transparent;
        }
        
        SelectedLeftItem = null!;
        SelectedRightItem = null!;
    }
}

public partial class AssociationItem : ObservableObject
{
    public AssociationItem()
    {
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(IsSelected))
                OnPropertyChanged(nameof(Theme));
        };
    }
    
    [ObservableProperty] private string _text;
    [ObservableProperty] private bool _isLeft;
    [ObservableProperty] private bool _isSelected;
    [ObservableProperty] private bool _isEnabled = true;
    
    [ObservableProperty] private bool _isCorrect;
    [ObservableProperty] private bool _isWrong;
    
    [ObservableProperty] private IBrush _connectionColor = Brushes.Transparent;

    public ControlTheme? Theme => IsSelected
        ? (ControlTheme) Application.Current!.FindResource("SolidButton")!
        : null;
    
    [ObservableProperty] private IRelayCommand<AssociationItem> _selectCommand;
}

public partial class AssociationConnection : ObservableObject
{
    [ObservableProperty] private AssociationItem _leftItem;
    [ObservableProperty] private AssociationItem _rightItem;
}