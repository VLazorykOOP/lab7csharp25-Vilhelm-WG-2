using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Lab7UI
{
    public partial class MainWindow : Window
    {
        // ВИДАЛИЛИ ручні оголошення TextBox, ComboBox тощо. 
        // Avalonia створила їх сама завдяки x:Name у XAML!

        private ObservableCollection<string> _myItems = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent(); // Цього методу достатньо!

            // Прив'язуємо колекцію
            _myItems.Add("Елемент за замовчуванням");
            MyComboBox.ItemsSource = _myItems; 
            
            // Підключаємо події (змінні AddButton вже існують автоматично)
            AddButton.Click += OnAddClick;
            RemoveButton.Click += OnRemoveClick;
        }

        private void OnAddClick(object? sender, RoutedEventArgs e)
        {
            // Використовуємо InputTextBox прямо, він уже існує!
            string text = InputTextBox.Text?.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                _myItems.Add(text);
                InputTextBox.Text = string.Empty;
            }
        }

        private void OnRemoveClick(object? sender, RoutedEventArgs e)
        {
            string text = InputTextBox.Text?.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                var itemToRemove = _myItems.FirstOrDefault(i => i == text);
                if (itemToRemove != null)
                {
                    _myItems.Remove(itemToRemove);
                    InputTextBox.Text = string.Empty;
                }
            }
            else if (MyComboBox.SelectedItem is string selectedItem)
            {
                _myItems.Remove(selectedItem);
            }
        }
    }
}