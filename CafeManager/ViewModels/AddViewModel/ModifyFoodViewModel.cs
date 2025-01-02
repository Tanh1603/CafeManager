using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class ModifyFoodViewModel : ObservableValidator
    {
        private readonly IServiceProvider _provider;
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSubmit))]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        public FoodDTO _modifyFood = new();

        [ObservableProperty]
        private FoodCategoryDTO? _selectedFoodCategory = new();

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategory = [];

        public bool IsUpdating { get; set; } = false;
        public bool IsAdding { get; set; } = false;

        public event Action<FoodDTO>? ModifyFoodChanged;

        public ModifyFoodViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _fileDialogService = _provider.GetRequiredService<FileDialogService>();
        }

        public void ReceiveFood(FoodDTO foodDTO)
        {
            ModifyFood = foodDTO.Clone();
            SelectedFoodCategory = ListFoodCategory.FirstOrDefault(x => x.Foodcategoryid == foodDTO.Foodcategoryid);
        }

        public void ReceiveListFoodCategory(List<FoodCategoryDTO> foodcategories) => ListFoodCategory = [.. foodcategories];

        public void ClearValueOfForm()
        {
            ModifyFood = new();
            SelectedFoodCategory = null;
            IsAdding = false;
            IsUpdating = false;
        }

        public bool CanSubmit => !ModifyFood.GetErrors().Any();

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private void Submit()
        {
            var a = ModifyFood.GetErrors();
            if (SelectedFoodCategory != null)
            {
                ModifyFood.Foodcategoryid = SelectedFoodCategory.Foodcategoryid;
                ModifyFoodChanged?.Invoke(ModifyFood.Clone());
            }
            else
            {
                MyMessageBox.ShowDialog("Vui lòng chọn danh mục");
            }
        }

        [RelayCommand]
        private void OpenUploadImage()
        {
            string filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*";
            var filePath = _fileDialogService.OpenFileDialog(filter);

            if (!string.IsNullOrEmpty(filePath))
            {
                ModifyFood.Imagefood = new BitmapImage(new Uri(filePath));
            }
        }
    }
}