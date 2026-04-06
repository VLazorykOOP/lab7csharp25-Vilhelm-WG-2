using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Lab7UI
{
    public partial class MainWindow : Window
    {
        private TextBox? _inputTextBox;
        private ComboBox? _myComboBox;
        private ObservableCollection<string> _myItems = new ObservableCollection<string>();

        public MainWindow()
        {
            // Викликаємо наш ручний метод замість стандартного
            CustomInitialize();

            _inputTextBox = this.FindControl<TextBox>("InputTextBox");
            _myComboBox = this.FindControl<ComboBox>("MyComboBox");
            
            var addButton = this.FindControl<Button>("AddButton");
            var removeButton = this.FindControl<Button>("RemoveButton");
            var goToBtn = this.FindControl<Button>("GoToImageTask");

            if (_myComboBox != null)
            {
                _myItems.Add("Елемент за замовчуванням");
                _myComboBox.ItemsSource = _myItems;
            }

            if (addButton != null) addButton.Click += OnAddClick;
            if (removeButton != null) removeButton.Click += OnRemoveClick;
            
            if (goToBtn != null)
            {
                goToBtn.Click += (s, e) => {
                    var imageWin = new Lab7UI.ImageWindow();
                    imageWin.Show(); 
                };
            }
            
            var goDrawingBtn = this.FindControl<Button>("GoToDrawingTask");
            if (goDrawingBtn != null)
            {
                goDrawingBtn.Click += (s, e) => {
                    var drawingWin = new Lab7UI.DrawingWindow();
                    drawingWin.Show(); 
                };
            }
        }

        // РУЧНИЙ МЕТОД ЗАВАНТАЖЕННЯ XAML
        private void CustomInitialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnAddClick(object? sender, RoutedEventArgs e)
        {
            string text = _inputTextBox?.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                _myItems.Add(text);
                if (_inputTextBox != null) _inputTextBox.Text = string.Empty;
            }
        }

        private void OnRemoveClick(object? sender, RoutedEventArgs e)
        {
            string text = _inputTextBox?.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                var itemToRemove = _myItems.FirstOrDefault(i => i == text);
                if (itemToRemove != null) _myItems.Remove(itemToRemove);
            }
            else if (_myComboBox?.SelectedItem is string selectedItem)
            {
                _myItems.Remove(selectedItem);
            }
        }
    }
}