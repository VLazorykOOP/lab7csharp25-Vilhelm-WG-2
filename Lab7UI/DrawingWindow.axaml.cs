using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Lab7UI
{
    // Абстрактний базовий клас Фігура
    public abstract class Figure
    {
        // Координати фігури
        public double X { get; set; }
        public double Y { get; set; }
        
        // Колір фігури
        public Color Color { get; set; }
        
        // Розміри фігури
        public double Width { get; set; }
        public double Height { get; set; }
        
        // Конструктор
        protected Figure(double x, double y, Color color, double width, double height)
        {
            X = x;
            Y = y;
            Color = color;
            Width = width;
            Height = height;
        }
        
        // Абстрактний метод для малювання фігури
        public abstract void Draw(DrawingContext context);
        
        // Віртуальний метод для переміщення фігури
        public virtual void Move(double deltaX, double deltaY)
        {
            X += deltaX;
            Y += deltaY;
        }
    }
    
    // Похідний клас Квадрат
    public class Square : Figure
    {
        public Square(double x, double y, Color color, double size) 
            : base(x, y, color, size, size) { }
        
        public override void Draw(DrawingContext context)
        {
            var brush = new SolidColorBrush(Color);
            var rect = new Rect(X, Y, Width, Height);
            context.DrawRectangle(brush, null, rect);
        }
    }
    
    // Похідний клас Прямокутник
    public class Rectangle : Figure
    {
        public Rectangle(double x, double y, Color color, double width, double height) 
            : base(x, y, color, width, height) { }
        
        public override void Draw(DrawingContext context)
        {
            var brush = new SolidColorBrush(Color);
            var rect = new Rect(X, Y, Width, Height);
            context.DrawRectangle(brush, null, rect);
        }
    }
    
    // Похідний клас Коло
    public class Circle : Figure
    {
        public Circle(double x, double y, Color color, double size) 
            : base(x, y, color, size, size) { }
        
        public override void Draw(DrawingContext context)
        {
            var brush = new SolidColorBrush(Color);
            var center = new Point(X + Width / 2, Y + Height / 2);
            var radiusX = Width / 2;
            var radiusY = Height / 2;
            context.DrawEllipse(brush, null, center, radiusX, radiusY);
        }
    }
    
    // Похідний клас Трикутник
    public class Triangle : Figure
    {
        public Triangle(double x, double y, Color color, double size) 
            : base(x, y, color, size, size) { }
        
        public override void Draw(DrawingContext context)
        {
            var brush = new SolidColorBrush(Color);
            
            // Створюємо геометрію трикутника
            var geometry = new StreamGeometry();
            using (var geometryContext = geometry.Open())
            {
                // Вершина 1 (верхня)
                var point1 = new Point(X + Width / 2, Y);
                // Вершина 2 (нижня ліва)
                var point2 = new Point(X, Y + Height);
                // Вершина 3 (нижня права)
                var point3 = new Point(X + Width, Y + Height);
                
                geometryContext.BeginFigure(point1, true);
                geometryContext.LineTo(point2);
                geometryContext.LineTo(point3);
                geometryContext.EndFigure(true);
            }
            
            context.DrawGeometry(brush, null, geometry);
        }
    }
    
    // Клас для кастомного полотна малювання
    public class DrawingCanvas : Control
    {
        // Список фігур для малювання
        public List<Figure> Figures { get; set; } = new List<Figure>();
        
        // Перевизначений метод для рендерингу
        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);
            
            // Малюємо всі фігури зі списку
            foreach (var figure in Figures)
            {
                figure.Draw(drawingContext);
            }
        }
    }
    
    // Головний клас вікна
    public partial class DrawingWindow : Window
    {
        // Список всіх фігур
        private List<Figure> _figures = new List<Figure>();
        
        // Генератор випадкових чисел
        private Random _random = new Random();
        
        // Елементи керування
        private ComboBox? _figureTypeComboBox;
        private TextBox? _sizeTextBox;
        private ComboBox? _colorComboBox;
        private TextBlock? _figureCountTextBlock;
        private DrawingCanvas? _drawingCanvas;
        
        public DrawingWindow()
        {
            InitializeComponent();
            InitializeControls();
            SetupEventHandlers();
        }
        
        // Ініціалізація компонентів
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        // Ініціалізація елементів керування
        private void InitializeControls()
        {
            _figureTypeComboBox = this.FindControl<ComboBox>("FigureTypeComboBox");
            _sizeTextBox = this.FindControl<TextBox>("SizeTextBox");
            _colorComboBox = this.FindControl<ComboBox>("ColorComboBox");
            _figureCountTextBlock = this.FindControl<TextBlock>("FigureCountTextBlock");
            
            Console.WriteLine($"Ініціалізація елементів:");
            Console.WriteLine($"  FigureTypeComboBox: {_figureTypeComboBox != null}");
            Console.WriteLine($"  SizeTextBox: {_sizeTextBox != null}");
            Console.WriteLine($"  ColorComboBox: {_colorComboBox != null}");
            Console.WriteLine($"  FigureCountTextBlock: {_figureCountTextBlock != null}");
            
            // Створюємо кастомне полотно
            _drawingCanvas = new DrawingCanvas();
            Console.WriteLine($"  DrawingCanvas створено");
            
            // Знаходимо оригінальний Canvas і замінюємо його нашим кастомним
            var originalCanvas = this.FindControl<Canvas>("DrawingCanvas");
            Console.WriteLine($"  Оригінальний Canvas знайдено: {originalCanvas != null}");
            
            if (originalCanvas != null && originalCanvas.Parent is Border border)
            {
                Console.WriteLine("  Заміна Canvas на кастомний DrawingCanvas");
                // Фон буде білий за замовчуванням через Border
                
                // Замінюємо оригінальний Canvas на наш кастомний
                border.Child = _drawingCanvas;
            }
            else
            {
                Console.WriteLine("  Помилка: не вдалося знайти Canvas або його батьківський Border");
            }
        }
        
        // Налаштування обробників подій
        private void SetupEventHandlers()
        {
            var addButton = this.FindControl<Button>("AddFigureButton");
            var clearButton = this.FindControl<Button>("ClearCanvasButton");
            
            Console.WriteLine($"Налаштування обробників подій:");
            Console.WriteLine($"  AddFigureButton знайдено: {addButton != null}");
            Console.WriteLine($"  ClearCanvasButton знайдено: {clearButton != null}");
            
            if (addButton != null) 
            {
                addButton.Click += OnAddFigureClick;
                Console.WriteLine("  Обробник для AddFigureButton додано");
            }
            if (clearButton != null) 
            {
                clearButton.Click += OnClearCanvasClick;
                Console.WriteLine("  Обробник для ClearCanvasButton додано");
            }
        }
        
        // Обробник натискання кнопки "Додати фігуру"
        private void OnAddFigureClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Кнопка 'Додати фігуру' натиснута");
                
                // Отримуємо вибраний тип фігури
                string? figureType = null;
                if (_figureTypeComboBox?.SelectedItem is ComboBoxItem selectedItem)
                {
                    figureType = selectedItem.Content?.ToString();
                }
                else
                {
                    figureType = _figureTypeComboBox?.SelectedItem as string;
                }
                
                Console.WriteLine($"Тип фігури: {figureType}");
                
                if (string.IsNullOrEmpty(figureType))
                {
                    Console.WriteLine("Тип фігури не обрано");
                    return;
                }
                
                // Отримуємо розмір
                double size = 50; // значення за замовчуванням
                if (double.TryParse(_sizeTextBox?.Text, out double parsedSize))
                {
                    size = Math.Max(10, Math.Min(150, parsedSize)); // обмежуємо діапазон 10-150
                }
                
                // Отримуємо колір
                Color color = GetSelectedColor();
                
                // Генеруємо випадкові координати в межах полотна
                double canvasWidth = _drawingCanvas?.Bounds.Width ?? 500;
                double canvasHeight = _drawingCanvas?.Bounds.Height ?? 400;
                
                double x = _random.Next(0, Math.Max(0, (int)(canvasWidth - size)));
                double y = _random.Next(0, Math.Max(0, (int)(canvasHeight - size)));
                
                // Створюємо фігуру відповідного типу
                Console.WriteLine($"Створення фігури типу '{figureType}' з розміром {size} на позиції ({x}, {y})");
                
                Figure? newFigure = figureType switch
                {
                    "Квадрат" => new Square(x, y, color, size),
                    "Прямокутник" => new Rectangle(x, y, color, size * 1.5, size),
                    "Коло" => new Circle(x, y, color, size),
                    "Трикутник" => new Triangle(x, y, color, size),
                    _ => null
                };
                
                if (newFigure == null)
                {
                    Console.WriteLine("Не вдалося створити фігуру");
                    return;
                }
                
                Console.WriteLine($"Фігуру успішно створено: {newFigure.GetType().Name}");
                
                // Додаємо фігуру до списку
                _figures.Add(newFigure);
                Console.WriteLine($"Фігуру додано до списку. Загальна кількість: {_figures.Count}");
                
                // Оновлюємо полотно для перемальовування
                if (_drawingCanvas != null)
                {
                    _drawingCanvas.Figures = _figures;
                    _drawingCanvas.InvalidateVisual();
                    Console.WriteLine("Полотно оновлено для перемальовування");
                }
                else
                {
                    Console.WriteLine("Помилка: полотно не знайдено");
                }
                
                // Оновлюємо лічильник фігур
                UpdateFigureCount();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при додаванні фігури: {ex.Message}");
            }
        }
        
        // Обробник натискання кнопки "Очистити полотно"
        private void OnClearCanvasClick(object? sender, RoutedEventArgs e)
        {
            _figures.Clear();
            
            if (_drawingCanvas != null)
            {
                _drawingCanvas.Figures = _figures;
                _drawingCanvas.InvalidateVisual();
            }
            
            UpdateFigureCount();
        }
        
        // Отримання вибраного кольору
        private Color GetSelectedColor()
        {
            string? colorName = null;
            if (_colorComboBox?.SelectedItem is ComboBoxItem selectedColorItem)
            {
                colorName = selectedColorItem.Content?.ToString();
            }
            else
            {
                colorName = _colorComboBox?.SelectedItem as string;
            }
            
            return colorName switch
            {
                "Червоний" => Colors.Red,
                "Зелений" => Colors.Green,
                "Синій" => Colors.Blue,
                "Жовтий" => Colors.Yellow,
                "Оранжевий" => Colors.Orange,
                "Фіолетовий" => Colors.Purple,
                "Чорний" => Colors.Black,
                _ => Colors.Black
            };
        }
        
        // Оновлення лічильника фігур
        private void UpdateFigureCount()
        {
            if (_figureCountTextBlock != null)
            {
                _figureCountTextBlock.Text = _figures.Count.ToString();
            }
        }
    }
}
