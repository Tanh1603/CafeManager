using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CafeManager.WPF.ViewModels.UserViewModel
{
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FoodServices _foodServices;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly InvoiceServices _invoiceServices;

        [ObservableProperty]
        private ObservableCollection<CoffeetableDTO> _listCoffeeTableDTO = [];

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategoryDTO = [];

        [ObservableProperty]
        private FoodCategoryDTO _selectedFoodCategory = new();

        [ObservableProperty]
        private ObservableCollection<FoodDTO> _listFoodDTO = [];

        public OrderViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();

            Task.Run(LoadData);
        }

        private async Task LoadData()
        {
            var dbListCoffeeTable = await _coffeTableServices.GetListCoffeTable();
            ListCoffeeTableDTO = [.. dbListCoffeeTable.Select(x => CoffeeTableMapper.ToDTO(x))];

            var dbListFoodCategory = await _foodCategoryServices.GetListFoodCategory();
            ListFoodCategoryDTO = [.. dbListFoodCategory.Select(x => FoodCategoryMapper.ToDTO(x))];

            SelectedFoodCategory = ListFoodCategoryDTO[0];
            if (SelectedFoodCategory != null)
            {
                await SeletedFoodCategoryChangedCommand.ExecuteAsync(SelectedFoodCategory);
            }
        }

        [RelayCommand]
        private async Task SeletedFoodCategoryChanged(FoodCategoryDTO foodCategoryDTO)
        {
            if (foodCategoryDTO != null)
            {
                var dbListFood =
            await _foodCategoryServices.GetListFoodByFoodCatgoryId(foodCategoryDTO.Foodcategoryid);
                ListFoodDTO = [.. dbListFood.Select(x => FoodMapper.ToDTO(x))];
            }
        }

        [RelayCommand]
        private void ChooseTable(CoffeetableDTO tableDTO)
        {
            //try
            //{
            //    if (SelectedTable != null && SelectedTable?.Coffeetableid == table.Coffeetableid)
            //        return;
            //    var existingInvoice = _currentListInvoice.FirstOrDefault(x => x.Coffeetableid == table.Coffeetableid);

            //    if (existingInvoice != null)
            //    {
            //        CurrentInvoice = existingInvoice;
            //    }
            //    else
            //    {
            //        string messageBox = MyMessageBox.ShowDialog($"Bạn muốn tạo hóa đơn cho bàn {table.Tablenumber} ?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
            //        if (messageBox == "1")
            //        {
            //            CurrentInvoice = new InvoiceDTO
            //            {
            //                Coffeetableid = table.Coffeetableid,
            //                Paymentstartdate = DateTime.Now,
            //                ListInvoiceDTO = new ObservableCollection<InvoiceDetailDTO>()
            //            };
            //            _currentListInvoice.Add(CurrentInvoice);
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    //TotalPrice = CurrentInvoice.CaculateTotalPrice() ?? 0;

            //    SelectedTable = table;
            //}
            //catch (Exception ex)
            //{
            //    MyMessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            //}
        }

        [RelayCommand]
        private void ChooseFood(FoodDTO food)
        {
            //if (SelectedTable != null)
            //{
            //    var existingInvoiceDetail = CurrentInvoice.ListInvoiceDTO.FirstOrDefault(x => x.FoodId == food.Id);

            //    if (existingInvoiceDetail != null)
            //    {
            //        existingInvoiceDetail.Quantity += 1;
            //    }
            //    else
            //    {
            //        CurrentInvoice.ListInvoiceDTO.Add(new InvoiceDetailDTO()
            //        {
            //            //FoodId = food.Id,
            //            Quantity = 1,
            //            FoodName = food.Foodname,
            //            Price = food.PriceDiscount,
            //        });
            //    }
            //    var tmpListInvoiceDTO = CurrentInvoice.ListInvoiceDTO.Select(x => new Invoicedetail()
            //    {
            //    });
            //    TotalPrice = CurrentInvoice.CaculateTotalPrice() ?? 0;
            //    OnPropertyChanged(nameof(CurrentInvoice));
            //    UpdateValueCurrentListInvoice();
            //}
            //else
            //{
            //    MyMessageBox.Show("Vui lòng chọn bàn hoặc tạo hóa đơn mới");
            //}
        }

        private void UpdateValueCurrentListInvoice()
        {
            //if (CurrentInvoice != null)
            //{
            //    var existingIndex = _currentListInvoice
            //        .ToList()
            //        .FindIndex(invoice => invoice.Coffeetableid == CurrentInvoice.Coffeetableid);

            //    if (existingIndex >= 0)
            //    {
            //        _currentListInvoice[existingIndex].ListInvoiceDTO = CurrentInvoice.ListInvoiceDTO;
            //    }
            //    else
            //    {
            //        _currentListInvoice.Add(CurrentInvoice);
            //    }
            //}
        }

        [RelayCommand]
        private void DeleteFood(InvoiceDetailDTO invoiceDetailDTO)
        {
            //CurrentInvoice.ListInvoiceDTO.Remove(invoiceDetailDTO);
            //var existingIndex = _currentListInvoice
            //    .ToList()
            //    .FindIndex(invoice => invoice.Coffeetableid == CurrentInvoice.Coffeetableid);

            //if (existingIndex >= 0)
            //{
            //    _currentListInvoice[existingIndex].ListInvoiceDTO = new ObservableCollection<InvoiceDetailDTO>(CurrentInvoice.ListInvoiceDTO);
            //    OnPropertyChanged(nameof(_currentListInvoice));
            //}
            //TotalPrice = CurrentInvoice.CaculateTotalPrice() ?? 0;
            //OnPropertyChanged(nameof(CurrentInvoice));
        }

        private async Task AddInvoiceToDataBase(string statusInvoice, string message)
        {
            //try
            //{
            //    Invoice newCancelInvoice = new()
            //    {
            //        Paymentstartdate = CurrentInvoice.Paymentstartdate,
            //        Paymentenddate = DateTime.Now,
            //        Paymentstatus = $"{statusInvoice}",
            //        Paymentmethod = CurrentInvoice.Paymentmethod,
            //        Discountinvoice = CurrentInvoice.Discountinvoice,
            //        Coffeetableid = CurrentInvoice.Coffeetableid,
            //    };
            //    var resAddInvoice = await _invoiceServices.AddInvoice(newCancelInvoice);
            //    if (resAddInvoice != null)
            //    {
            //        List<Invoicedetail> newCancelListInvoiceDetail = [..CurrentInvoice.ListInvoiceDTO.Select(x => new Invoicedetail()
            //        {
            //            Invoiceid = resAddInvoice.Invoiceid,
            //            //Foodid = x.FoodId,
            //            Quantity = x.Quantity,
            //        })];
            //        var resAddArangeInoviceDetail = await _invoiceServices.AddArangeListInvoice(newCancelListInvoiceDetail);
            //        if (resAddArangeInoviceDetail != null)
            //        {
            //            MyMessageBox.Show($"{message}");
            //            _currentListInvoice.Remove(CurrentInvoice);
            //            CurrentInvoice = new();
            //            SelectedTable = null;
            //            TotalPrice = 0;
            //            return;
            //        }
            //    }
            //    MyMessageBox.Show("Lỗi");
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
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
    }
}