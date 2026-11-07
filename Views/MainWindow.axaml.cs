using Avalonia.Controls;
using Car_Catalogue_Desktop.ViewModels;

namespace Car_Catalogue_Desktop.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        if (DataContext is MainWindowViewModel vm) {
            SetupEvents(vm);
        }
    }

    protected override void OnDataContextChanged(System.EventArgs e) {
        base.OnDataContextChanged(e);
        if (DataContext is MainWindowViewModel vm) {
            SetupEvents(vm);
        }
    }

    private void SetupEvents(MainWindowViewModel vm) {
        vm.OnAddCarRequested -= Vm_OnAddCarRequested;
        vm.OnAddCarRequested += Vm_OnAddCarRequested;
        
        vm.OnCarDetailsRequested -= Vm_OnCarDetailsRequested;
        vm.OnCarDetailsRequested += Vm_OnCarDetailsRequested;
    }

    private async void Vm_OnAddCarRequested() {
        var editVm = new CarEditViewModel();
        var window = new CarEditWindow { DataContext = editVm };
        
        editVm.OnRequestClose += () => window.Close();
        
        await window.ShowDialog(this);
        
        ((MainWindowViewModel)DataContext!).LoadData();
    }

    private async void Vm_OnCarDetailsRequested(CarViewModel carVm) {
        var detailsVm = new CarDetailsViewModel(carVm);
        var window = new CarDetailsWindow { DataContext = detailsVm };
        
        detailsVm.OnRequestClose += () => window.Close();
        detailsVm.OnRequestEdit += async (carModel) => {
            window.Close();
            var editVm = new CarEditViewModel(carModel);
            var editWindow = new CarEditWindow { DataContext = editVm };
            editVm.OnRequestClose += () => editWindow.Close();
            
            await editWindow.ShowDialog(this);
            ((MainWindowViewModel)DataContext!).LoadData();
        };

        await window.ShowDialog(this);
        ((MainWindowViewModel)DataContext!).LoadData();
    }
}