using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;
        public partial class MainWindow : Window
        {
            public MainWindow()
            {
                InitializeComponent();
            }

            // Логіка додавання
            private void OnAddClick(object sender, RoutedEventArgs e)
            {
                string text = InputTextBox.Text?.Trim();

                if (!string.IsNullOrEmpty(text))
                {
                    // Додаємо новий рядок у список
                    MyComboBox.Items.Add(text);

                    // Очищуємо поле після додавання
                    InputTextBox.Text = string.Empty;
                }
            }

            // Логіка видалення
            private void OnRemoveClick(object sender, RoutedEventArgs e)
            {
                string text = InputTextBox.Text?.Trim();

                if (!string.IsNullOrEmpty(text))
                {
                    // Шукаємо елемент у списку, який збігається з текстом у TextBox
                    var itemToRemove = MyComboBox.Items
                        .Cast<object>()
                        .FirstOrDefault(i => i.ToString() == text);

                    if (itemToRemove != null)
                    {
                        MyComboBox.Items.Remove(itemToRemove);
                        InputTextBox.Text = string.Empty;
                    }
                }
                else if (MyComboBox.SelectedItem != null)
                {
                    // Якщо TextBox порожній, видаляємо виділений елемент у ComboBox
                    MyComboBox.Items.Remove(MyComboBox.SelectedItem);
                }
            }
        }

