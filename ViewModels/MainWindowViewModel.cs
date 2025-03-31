using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Queue.Models;

namespace Queue.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly CustomQueue<string> _queue = new();
    private string _newItem = string.Empty;
    private string _status = "Очередь пуста";

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<string> QueueItems { get; } = new();
    
    public ICommand EnqueueCommand { get; }
    public ICommand DequeueCommand { get; }
    public ICommand ClearCommand { get; }

    public string NewItem
    {
        get => _newItem;
        set
        {
            if (_newItem != value)
            {
                _newItem = value;
                OnPropertyChanged();
                (EnqueueCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public string Status
    {
        get => _status;
        private set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }

    public MainWindowViewModel()
    {
        EnqueueCommand = new RelayCommand(
            _ => Enqueue(),
            _ => !string.IsNullOrWhiteSpace(NewItem));
            
        DequeueCommand = new RelayCommand(
            _ => Dequeue(),
            _ => !_queue.IsEmpty);
            
        ClearCommand = new RelayCommand(
            _ => Clear(),
            _ => !_queue.IsEmpty);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void Enqueue()
    {
        _queue.Enqueue(NewItem);
        QueueItems.Add(NewItem);
        UpdateStatus();
        NewItem = string.Empty;
        (DequeueCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (ClearCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private void Dequeue()
    {
        var item = _queue.Dequeue();
        QueueItems.RemoveAt(0);
        Status = $"Извлечен элемент: {item}";
        UpdateStatus();
        (DequeueCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (ClearCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private void Clear()
    {
        _queue.Clear();
        QueueItems.Clear();
        Status = "Очередь очищена";
        (DequeueCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (ClearCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private void UpdateStatus()
    {
        Status = $"Элементов в очереди: {_queue.Count}. Текущий: {_queue.CurrentItem ?? "нет"}";
    }
}