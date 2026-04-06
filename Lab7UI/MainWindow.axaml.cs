using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Lab7UI // <-- БЕЗПЕЧНИЙ ПРОСТІР ІМЕН
{
    public partial class MainWindow : Window
    {
        private TextBox? InputTextBox;
        private ComboBox? MyComboBox;
        private Button? AddButton;
        private Button? RemoveButton;

        // Це наша динамічна колекція. Все, що сюди потрапляє, одразу з'являється у списку!
        private ObservableCollection<string> _myItems = new ObservableCollection<string>();

        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            
            InputTextBox = this.FindControl<TextBox>("InputTextBox");
            MyComboBox = this.FindControl<ComboBox>("MyComboBox");
            AddButton = this.FindControl<Button>("AddButton");
            RemoveButton = this.FindControl<Button>("RemoveButton");

            // Прив'язуємо нашу колекцію до випадаючого списку
            if (MyComboBox != null)
            {
                _myItems.Add("Елемент за замовчуванням");
                MyComboBox.ItemsSource = _myItems; 
            }
            
            if (AddButton != null)
                AddButton.Click += OnAddClick;
            if (RemoveButton != null)
                RemoveButton.Click += OnRemoveClick;
        }

        private void OnAddClick(object? sender, RoutedEventArgs e)
        {
            string text = InputTextBox?.Text?.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                _myItems.Add(text); // Додаємо прямо в колекцію

                if (InputTextBox != null)
                    InputTextBox.Text = string.Empty;
            }
        }

        private void OnRemoveClick(object? sender, RoutedEventArgs e)
        {
            string text = InputTextBox?.Text?.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                var itemToRemove = _myItems.FirstOrDefault(i => i == text);
                if (itemToRemove != null)
                {
                    _myItems.Remove(itemToRemove);
                    
                    if (InputTextBox != null)
                        InputTextBox.Text = string.Empty;
                }
            }
            else if (MyComboBox?.SelectedItem is string selectedItem)
            {
                // Якщо TextBox порожній, видаляємо вибраний елемент
                _myItems.Remove(selectedItem);
            }
        }
    }
}