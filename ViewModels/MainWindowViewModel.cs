using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Car_Catalogue_Desktop.Services;

namespace Car_Catalogue_Desktop.ViewModels;

public partial class MainWindowViewModel : ObservableObject {
    private readonly DatabaseService _dbService;

    [ObservableProperty]
    private ObservableCollection<CarViewModel> _cars = new();

    [ObservableProperty]
    private ObservableCollection<string> _availableBrands = new();

    [ObservableProperty]
    private ObservableCollection<string> _availableModels = new();

    [ObservableProperty]
    private ObservableCollection<int> _availableYears = new();

    [ObservableProperty]
    private string? _selectedBrand;

    [ObservableProperty]
    private string? _selectedModel;

    [ObservableProperty]
    private int? _selectedYear;

    private List<CarViewModel> _allCars = new();

    public event System.Action? OnAddCarRequested;
    public event System.Action<CarViewModel>? OnCarDetailsRequested;

    public MainWindowViewModel() {
        _dbService = new DatabaseService();
        LoadData();
    }

    public void LoadData() {
        var dbCars = _dbService.GetAllCars();
        _allCars = dbCars.Select(c => new CarViewModel(c)).ToList();
        
        UpdateFilters();
        ApplyFilters();
    }

    private void UpdateFilters() {
        var brands = _allCars.Select(x => x.CarModel.Brand).Distinct().OrderBy(x => x).ToList();
        AvailableBrands.Clear();
        foreach (var b in brands) AvailableBrands.Add(b);

        UpdateModelsFilter();

        var years = _allCars.Select(x => x.CarModel.Year).Distinct().OrderByDescending(x => x).ToList();
        AvailableYears.Clear();
        foreach (var y in years) AvailableYears.Add(y);
    }

    private void UpdateModelsFilter() {
        AvailableModels.Clear();
        if (string.IsNullOrEmpty(SelectedBrand)) {
            SelectedModel = null;
            return;
        }

        var models = _allCars
            .Where(x => x.CarModel.Brand == SelectedBrand)
            .Select(x => x.CarModel.Model)
            .Distinct()
            .ToList();

        foreach (var m in models) AvailableModels.Add(m);

        if (!string.IsNullOrWhiteSpace(SelectedModel) && !AvailableModels.Contains(SelectedModel))
            SelectedModel = null;
    }

    partial void OnSelectedBrandChanged(string? value) {
        UpdateModelsFilter();
        ApplyFilters();
    }

    partial void OnSelectedModelChanged(string? value) {
        ApplyFilters();
    }

    partial void OnSelectedYearChanged(int? value) {
        ApplyFilters();
    }

    [RelayCommand]
    private void ClearFilters() {
        SelectedBrand = null;
        SelectedModel = null;
        SelectedYear = null;
        ApplyFilters();
    }

    [RelayCommand]
    private void AddNewCar() {
        OnAddCarRequested?.Invoke();
    }

    [RelayCommand]
    private void OpenCarDetails(CarViewModel car) {
        if (car != null)
            OnCarDetailsRequested?.Invoke(car);
    }

    private void ApplyFilters() {
        var filtered = _allCars.AsEnumerable();

        if (!string.IsNullOrEmpty(SelectedBrand))
            filtered = filtered.Where(x => x.CarModel.Brand == SelectedBrand);

        if (!string.IsNullOrEmpty(SelectedModel))
            filtered = filtered.Where(x => x.CarModel.Model == SelectedModel);

        if (SelectedYear.HasValue)
            filtered = filtered.Where(x => x.CarModel.Year == SelectedYear.Value);

        Cars.Clear();
        foreach (var car in filtered) {
            Cars.Add(car);
        }
    }
}
