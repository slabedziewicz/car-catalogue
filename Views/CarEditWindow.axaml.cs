using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Car_Catalogue_Desktop.ViewModels;

namespace Car_Catalogue_Desktop.Views;

public partial class CarEditWindow : Window {
    public CarEditWindow() {
        InitializeComponent();
    }

    private async void SelectImage_Click(object sender, RoutedEventArgs e) {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
            Title = "Wybierz Zdjęcie",
            AllowMultiple = false,
            FileTypeFilter = new[] {
                new FilePickerFileType("Obrazy") { 
                    Patterns = new[] { "*.png", "*.jpg", "*.jpeg" },
                    AppleUniformTypeIdentifiers = new[] { "public.image" },
                    MimeTypes = new[] { "image/*" }
                }
            }
        });

        if (files is not null && files.Count > 0 && DataContext is CarEditViewModel vm) {
            vm.SelectImage(files[0].Path.LocalPath);
        }
    }
}
