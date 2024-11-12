using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using CafeManager.WPF.MessageBox;

namespace CafeManager.WPF.ViewModels
{
    public partial class MainUserViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly FoodServices _foodServices;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FileDialogService _fileDialogService;
        private readonly InvoiceServices _invoiceServices;

        [ObservableProperty]
        private ObservableCollection<Coffeetable> _listTable = [];

        [ObservableProperty]
        private Coffeetable? _selectedTable;

        [ObservableProperty]
        private ObservableCollection<Foodcategory> _listFoodCategory = [];

        [ObservableProperty]
        private ObservableCollection<FoodDTO> _listFoodByFoodCategoryId = [];

        private Foodcategory _selectedFoodCategory = new();

        public Foodcategory SelectedFoodCategory
        {
            get => _selectedFoodCategory;
            set
            {
                _selectedFoodCategory = value;
                if (value != null)
                {
                    if (value.Foodcategoryname.Equals("Tất cả"))
                    {
                        Task.Run(() => LoadAllFoodData()).Wait();
                    }
                    else
                    {
                        Task.Run(() => LoadFoodData(value.Foodcategoryid)).Wait();
                    }
                }
            }
        }

        private ObservableCollection<InvoiceDTO> _currentListInvoice = [];

        //[ObservableProperty]
        private InvoiceDTO _currentInvoice = new();

        public InvoiceDTO CurrentInvoice
        {
            get => _currentInvoice;
            set
            {
                _currentInvoice = value;
                OnPropertyChanged(nameof(CurrentInvoice));
            }
        }

        [ObservableProperty]
        private decimal _totalPrice = 0;

        public MainUserViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();

            InvoiceDetailDTO.UpdateTotalPriceAction = UpdateTotalPriceWhenQuantityChanged;

            _ = InitializeAsync();
        }

        private void UpdateTotalPriceWhenQuantityChanged()
        {
            TotalPrice = CurrentInvoice.CaculateTotalPrice() ?? 0;
            OnPropertyChanged(nameof(TotalPrice));
        }

        private async Task InitializeAsync()
        {
            await LoadTableData();
            await LoadFoodCategoryData();
        }

        private async Task LoadTableData()
        {
            List<Coffeetable> list = new(await _coffeTableServices.GetListCoffeTable());
            for (int i = 0; i < list.Count; i++)
            {
                ListTable.Add(new Coffeetable()
                {
                    Tablename = $"Bàn {i + 1}",
                    Statustable = list[i].Statustable,
                    Notes = list[i].Notes,
                    Coffeetableid = list[i].Coffeetableid,
                    Invoices = list[i].Invoices,
                });
            }
        }

        private async Task LoadFoodData(int id)
        {
            var list = await _foodCategoryServices.GetListFoodByFoodCatgoryId(id);
            ListFoodByFoodCategoryId = new(list.Select(x => new FoodDTO()
            {
                Id = x.Foodid,
                Foodname = x.Foodname,
                Price = x.Price,
                Imagefood = _fileDialogService.Base64ToBitmapImage(x.Imagefood),
                Discountfood = x.Discountfood,
                Foodcategory = x.Foodcategory,
            }));
        }

        private async Task LoadAllFoodData()
        {
            var list = await _foodServices.GetAllListFood();
            ListFoodByFoodCategoryId = new(list.Select(x => new FoodDTO()
            {
                Id = x.Foodid,
                Foodname = x.Foodname,
                Price = x.Price,
                Imagefood = _fileDialogService.Base64ToBitmapImage(x.Imagefood),
                Discountfood = x.Discountfood,
                Foodcategory = x.Foodcategory,
            }));
        }

        private async Task LoadFoodCategoryData()
        {
            var list = await _foodCategoryServices.GetListFoodCategory();
            ListFoodCategory.Add(new Foodcategory()
            {
                Foodcategoryname = "Tất cả"
            });
            foreach (var item in list)
            {
                ListFoodCategory.Add(item);
            }
            SelectedFoodCategory = ListFoodCategory[0];
        }

        [RelayCommand]
        private void SignOut()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
        }

        [RelayCommand]
        private void ChooseTable(Coffeetable table)
        {
            try
            {
                if (SelectedTable != null && SelectedTable.Coffeetableid == table.Coffeetableid)
                    return;
                var existingInvoice = _currentListInvoice.FirstOrDefault(x => x.Coffeetableid == table.Coffeetableid);

                if (existingInvoice != null)
                {
                    CurrentInvoice = existingInvoice;
                }
                else
                {
                    string messageBox = MyMessageBox.ShowDialog($"Bạn muốn tạo hóa đơn cho {table.Tablename} ?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                    if (messageBox == "1")
                    {
                        CurrentInvoice = new InvoiceDTO
                        {
                            Coffeetableid = table.Coffeetableid,
                            Paymentstartdate = DateTime.Now,
                            ListInvoiceDTO = new ObservableCollection<InvoiceDetailDTO>()
                        };
                        _currentListInvoice.Add(CurrentInvoice);
                    }
                    else
                    {
                        return;
                    }
                }
                TotalPrice = CurrentInvoice.CaculateTotalPrice() ?? 0;
                SelectedTable = table;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ChooseFood(FoodDTO food)
        {
            if (SelectedTable != null)
            {
                var existingInvoiceDetail = CurrentInvoice.ListInvoiceDTO.FirstOrDefault(x => x.FoodId == food.Id);

                if (existingInvoiceDetail != null)
                {
                    existingInvoiceDetail.Quantity += 1;
                }
                else
                {
                    CurrentInvoice.ListInvoiceDTO.Add(new InvoiceDetailDTO()
                    {
                        FoodId = food.Id,
                        Quantity = 1,
                        FoodName = food.Foodname,
                        Price = food.PriceDiscount,
                    });
                }
                var tmpListInvoiceDTO = CurrentInvoice.ListInvoiceDTO.Select(x => new Invoicedetail()
                {
                });
                TotalPrice = CurrentInvoice.CaculateTotalPrice() ?? 0;
                OnPropertyChanged(nameof(CurrentInvoice));
                UpdateValueCurrentListInvoice();
            }
            else
            {
                MyMessageBox.Show("Vui lòng chọn bàn hoặc tạo hóa đơn mới");
            }
        }

        private void UpdateValueCurrentListInvoice()
        {
            if (CurrentInvoice != null)
            {
                var existingIndex = _currentListInvoice
                    .ToList()
                    .FindIndex(invoice => invoice.Coffeetableid == CurrentInvoice.Coffeetableid);

                if (existingIndex >= 0)
                {
                    _currentListInvoice[existingIndex].ListInvoiceDTO = CurrentInvoice.ListInvoiceDTO;
                }
                else
                {
                    _currentListInvoice.Add(CurrentInvoice);
                }
            }
        }

        [RelayCommand]
        private void DeleteFood(InvoiceDetailDTO invoiceDetailDTO)
        {
            CurrentInvoice.ListInvoiceDTO.Remove(invoiceDetailDTO);
            var existingIndex = _currentListInvoice
                .ToList()
                .FindIndex(invoice => invoice.Coffeetableid == CurrentInvoice.Coffeetableid);

            if (existingIndex >= 0)
            {
                _currentListInvoice[existingIndex].ListInvoiceDTO = new ObservableCollection<InvoiceDetailDTO>(CurrentInvoice.ListInvoiceDTO);
                OnPropertyChanged(nameof(_currentListInvoice));
            }
            TotalPrice = CurrentInvoice.CaculateTotalPrice() ?? 0;
            OnPropertyChanged(nameof(CurrentInvoice));
        }

        private async Task AddInvoiceToDataBase(string statusInvoice, string message)
        {
            try
            {
                Invoice newCancelInvoice = new()
                {
                    Paymentstartdate = CurrentInvoice.Paymentstartdate,
                    Paymentenddate = DateTime.Now,
                    Paymentstatus = $"{statusInvoice}",
                    Paymentmethod = CurrentInvoice.Paymentmethod,
                    Discountinvoice = CurrentInvoice.Discountinvoice,
                    Coffeetableid = CurrentInvoice.Coffeetableid,
                };
                var resAddInvoice = await _invoiceServices.AddInvoice(newCancelInvoice);
                if (resAddInvoice != null)
                {
                    List<Invoicedetail> newCancelListInvoiceDetail = [..CurrentInvoice.ListInvoiceDTO.Select(x => new Invoicedetail()
                    {
                        Invoiceid = resAddInvoice.Invoiceid,
                        Foodid = x.FoodId,
                        Quantity = x.Quantity,
                    })];
                    var resAddArangeInoviceDetail = await _invoiceServices.AddArangeListInvoice(newCancelListInvoiceDetail);
                    if (resAddArangeInoviceDetail != null)
                    {
                        MyMessageBox.Show($"{message}");
                        _currentListInvoice.Remove(CurrentInvoice);
                        CurrentInvoice = new();
                        SelectedTable = null;
                        TotalPrice = 0;
                        return;
                    }
                }
                MyMessageBox.Show("Lỗi");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [RelayCommand]
        private async Task CompleteInvoicePayment()
        {
            await AddInvoiceToDataBase("Hóa đơn đã thanh toán", "Thanh toán hóa đơn thành công!");
        }

        [RelayCommand]
        private async Task CancelInvoicePayment()
        {
            await AddInvoiceToDataBase("Hóa đơn bị hủy", "Hủy bỏ hóa đơn thành công!");
        }

        [RelayCommand]
        private void SwapTable()
        {
        }

        [RelayCommand]
        private void CreateTakeAwayInvoice()
        {
        }

        public void Dispose()
        {
            InvoiceDetailDTO.UpdateTotalPriceAction = null;
        }
    }
}