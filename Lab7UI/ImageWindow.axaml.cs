using System;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Markup.Xaml;

namespace Lab7UI
{
    public partial class ImageWindow : Window
    {
        private WriteableBitmap? _currentBitmap;

        public ImageWindow()
        {
            AvaloniaXamlLoader.Load(this);
            
            // Підключаємо кнопки вручну, оскільки ми в новому вікні
            this.FindControl<Button>("OpenButton")!.Click += OnOpenClick;
            this.FindControl<Button>("SaveButton")!.Click += OnSaveClick;
            this.FindControl<Button>("ApplyButton")!.Click += OnApplyClick;
        }

        private async void OnOpenClick(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Оберіть BMP",
                FileTypeFilter = new[] { new FilePickerFileType("BMP") { Patterns = new[] { "*.bmp" } } }
            });

            if (files.Count > 0)
            {
                await using var stream = await files[0].OpenReadAsync();
                _currentBitmap = WriteableBitmap.Decode(stream);
                this.FindControl<Image>("MainImage")!.Source = _currentBitmap;
            }
        }

        private async void OnSaveClick(object? sender, RoutedEventArgs e)
        {
            if (_currentBitmap == null) return;
            var topLevel = TopLevel.GetTopLevel(this);
            var file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Зберегти BMP",
                DefaultExtension = "bmp"
            });

            if (file != null)
            {
                await using var stream = await file.OpenWriteAsync();
                _currentBitmap.Save(stream);
            }
        }

        private void OnApplyClick(object? sender, RoutedEventArgs e)
        {
            if (_currentBitmap == null) return;

            using var fb = _currentBitmap.Lock();
            int size = fb.RowBytes * fb.Size.Height;
            byte[] pixels = new byte[size];
            Marshal.Copy(fb.Address, pixels, 0, size);

            bool rInv = this.FindControl<RadioButton>("RbRed")!.IsChecked == true;
            bool gInv = this.FindControl<RadioButton>("RbGreen")!.IsChecked == true;
            bool bInv = this.FindControl<RadioButton>("RbBlue")!.IsChecked == true;
            bool full = this.FindControl<RadioButton>("RbFull")!.IsChecked == true;

            for (int i = 0; i < size; i += 4)
            {
                // i     => Blue
                // i + 1 => Green
                // i + 2 => Red

                if (full) // Повна інверсія (всі три канали)
                {
                    pixels[i] = (byte)(255 - pixels[i]);         // Синій
                    pixels[i + 1] = (byte)(255 - pixels[i + 1]); // Зелений
                    pixels[i + 2] = (byte)(255 - pixels[i + 2]); // Червоний
                }
                else if (rInv) // Тільки Червоний
                {
                    pixels[i] = (byte)(255 - pixels[i]); // Індекс i + 2
                }
                else if (gInv) // Тільки Зелений
                {
                    pixels[i + 1] = (byte)(255 - pixels[i + 1]); // Індекс i + 1
                }
                else if (bInv) // Тільки Синій
                {
                    pixels[i + 2] = (byte)(255 - pixels[i + 2]);         // Індекс i
                }
            }

            Marshal.Copy(pixels, 0, fb.Address, size);
            this.FindControl<Image>("MainImage")!.Source = null;
            this.FindControl<Image>("MainImage")!.Source = _currentBitmap;
        }
    }
}