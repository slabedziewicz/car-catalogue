using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Car_Catalogue_Desktop.Models;
using Car_Catalogue_Desktop.Services;
using System;

namespace Car_Catalogue_Desktop.ViewModels;

public partial class CarDetailsViewModel : ObservableObject {
    private readonly DatabaseService _dbService;

    [ObservableProperty]
    private CarViewModel _carVm;

    public event Action? OnRequestClose;
    public event Action<Car>? OnRequestEdit;

    public CarDetailsViewModel(CarViewModel carVm) {
        _dbService = new DatabaseService();
        _carVm = carVm;
    }

    [RelayCommand]
    private void Edit() {
        OnRequestEdit?.Invoke(CarVm.CarModel);
    }

    [RelayCommand]
    private void Delete() {
        _dbService.DeleteCar(CarVm.CarModel.Id);
        FileService.DeleteImage(CarVm.CarModel.Id);
        OnRequestClose?.Invoke();
    }

    [RelayCommand]
    private void Close() {
        OnRequestClose?.Invoke();
    }
}
