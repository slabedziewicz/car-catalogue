using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Car_Catalogue_Desktop.Models;
using Car_Catalogue_Desktop.Services;
using System;
using System.IO;

namespace Car_Catalogue_Desktop.ViewModels;

public partial class CarEditViewModel : ObservableObject {
    private readonly DatabaseService _dbService;

    [ObservableProperty]
    private Car _editingCar;

    [ObservableProperty]
    private Bitmap? _previewImage;

    [ObservableProperty]
    private string? _selectedImagePath;

    public event Action? OnRequestClose;

    public bool IsNew { get; }

    public CarEditViewModel() {
        _dbService = new DatabaseService();
        EditingCar = new Car { Year = DateTime.Now.Year };
        IsNew = true;
    }

    public CarEditViewModel(Car car) {
        _dbService = new DatabaseService();
        EditingCar = new Car {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            BodyType = car.BodyType,
            TopSpeed = car.TopSpeed,
            HorsePower = car.HorsePower,
            Torque = car.Torque,
            EngineCapacity = car.EngineCapacity,
            ZeroToHundred = car.ZeroToHundred,
            Weight = car.Weight
        };
        IsNew = false;
        LoadCurrentPreview();
    }

    private void LoadCurrentPreview() {
        var path = FileService.GetImagePath(EditingCar.Id);
        if (File.Exists(path)) {
            try {
                using var stream = File.OpenRead(path);
                PreviewImage = new Bitmap(stream);
            } catch { }
        }
    }

    [RelayCommand]
    private void Save() {
        if (IsNew) {
            _dbService.AddCar(EditingCar);
        } else {
            _dbService.UpdateCar(EditingCar);
        }

        if (!string.IsNullOrEmpty(SelectedImagePath)) {
            FileService.SaveImage(SelectedImagePath, EditingCar.Id);
        }

        OnRequestClose?.Invoke();
    }

    [RelayCommand]
    private void Cancel() {
        OnRequestClose?.Invoke();
    }

    public void SelectImage(string path) {
        SelectedImagePath = path;
        try {
            using var stream = File.OpenRead(path);
            PreviewImage = new Bitmap(stream);
        } catch { }
    }
}
