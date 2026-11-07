using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Car_Catalogue_Desktop.Models;
using Car_Catalogue_Desktop.Services;
using System.IO;
using System;

namespace Car_Catalogue_Desktop.ViewModels;

public partial class CarViewModel : ObservableObject {
    [ObservableProperty]
    private Car _carModel;

    [ObservableProperty]
    private Bitmap? _carImage;

    public CarViewModel(Car car) {
        _carModel = car;
        LoadImage();
    }

    public void LoadImage() {
        var path = FileService.GetImagePath(CarModel.Id);
        if (File.Exists(path)) {
            try {
                using var stream = File.OpenRead(path);
                CarImage = new Bitmap(stream);
            } catch (Exception) {
                CarImage = null;
            }
        } else {
            CarImage = null;
        }
    }
}
