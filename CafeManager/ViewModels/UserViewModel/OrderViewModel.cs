using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CafeManager.WPF.ViewModels.UserViewModel
{
    public partial class OrderViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FoodServices _foodServices;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly InvoiceServices _invoiceServices;
        private readonly StaffServices _staffServices;

        [ObservableProperty]
        private ObservableCollection<CoffeetableDTO> _listCoffeeTableDTO = [];

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategoryDTO = [];

        [ObservableProperty]
        private FoodCategoryDTO _selectedFoodCategory = new();

        [ObservableProperty]
        private ObservableCollection<FoodDTO> _listFoodDTO = [];

        [ObservableProperty]
        private ObservableCollection<InvoiceDTO> _listInvoiceDTO = [];

        [ObservableProperty]
        private InvoiceDTO _selectedInvoiceDTO = new();

        [ObservableProperty]
        private ObservableCollection<StaffDTO> _listStaffDTO = [];

        [ObservableProperty]
        private StaffDTO _selectedStaffDTO = new();

        public decimal TotalPrice => SelectedInvoiceDTO.TotalPrice;

        [ObservableProperty]
        private string _selectedPaymentMethod;

        public OrderViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            _staffServices = provider.GetRequiredService<StaffServices>();

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

            var dbStaff = await _staffServices.GetListStaff();
            ListStaffDTO = [.. dbStaff.Select(x => StaffMapper.ToDTO(x))];
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
            bool isExsiting = ListInvoiceDTO.FirstOrDefault(x => x.Coffeetableid == tableDTO.Coffeetableid) != null;
            if (!isExsiting)
            {
                string messageBox =
                    MyMessageBox.ShowDialog($"Bạn muốn tạo hóa đơn cho {tableDTO.TableName} ?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    SelectedInvoiceDTO = new InvoiceDTO()
                    {
                        Coffeetableid = tableDTO.Coffeetableid,
                        CoffeetableDTO = tableDTO,
                        IsCoffeeTable = true,
                    };
                    ListInvoiceDTO.Add(SelectedInvoiceDTO);
                }
            }

            SelectedInvoiceDTO = ListInvoiceDTO.FirstOrDefault(x => x.Coffeetableid == tableDTO.Coffeetableid) ?? new();
            OnPropertyChanged(nameof(SelectedInvoiceDTO.CoffeetableDTO.TableName));
        }

        [RelayCommand]
        private void ChooseFood(FoodDTO foodDTO)
        {
            if (SelectedInvoiceDTO.IsCoffeeTable)
            {
                var exisitingFood = SelectedInvoiceDTO.ListInvoiceDetailDTO.FirstOrDefault(x => x.Foodid == foodDTO.Foodid);
                if (exisitingFood != null)
                {
                    exisitingFood.Quantity += 1;
                }
                else
                {
                    var newDetail = new InvoiceDetailDTO()
                    {
                        Quantity = 1,
                        FoodDTO = foodDTO,
                        Foodid = foodDTO.Foodid,
                    };
                    newDetail.QuantityChanged += NewDetail_QuantityChanged;
                    SelectedInvoiceDTO.ListInvoiceDetailDTO.Add(newDetail);
                }
                OnPropertyChanged(nameof(SelectedInvoiceDTO));
            }
            else if (SelectedInvoiceDTO.IsCustomer)
            {
            }
            else
            {
                MyMessageBox.Show("Vui lòng chọn bàn hoặc tạo hóa đơn mang về");
            }
        }

        private void NewDetail_QuantityChanged()
        {
            OnPropertyChanged(nameof(SelectedInvoiceDTO));
        }

        [RelayCommand]
        private void DeleteFood(InvoiceDetailDTO invoiceDetailDTO)
        {
            invoiceDetailDTO.QuantityChanged -= NewDetail_QuantityChanged;
            SelectedInvoiceDTO.ListInvoiceDetailDTO.Remove(invoiceDetailDTO);
            OnPropertyChanged(nameof(SelectedInvoiceDTO));
        }

        private async Task<bool> AddInvoiceToDataBase(string statusInvoice)
        {
            try
            {
                SelectedInvoiceDTO.Staffid = SelectedStaffDTO.Staffid;
                SelectedInvoiceDTO.StaffDTO = SelectedStaffDTO;
                SelectedInvoiceDTO.Paymentenddate = DateTime.Now;
                SelectedInvoiceDTO.Paymentmethod = SelectedPaymentMethod;
                SelectedInvoiceDTO.Paymentstatus = statusInvoice;
                Invoice addInvoice = InvoiceMapper.ToEntity(SelectedInvoiceDTO);
                var res = await _invoiceServices.CreateInvoice(addInvoice);
                if (res != null)
                {
                    foreach (var item in SelectedInvoiceDTO.ListInvoiceDetailDTO)
                    {
                        item.Invoiceid = res.Invoiceid;
                    }
                    var listInvoiceDetail =
                        await _invoiceServices
                        .AddArangeListInvoiceDetail(SelectedInvoiceDTO.ListInvoiceDetailDTO.Select(x => InvoiceDetailMapper.ToEntity(x)));
                    SelectedInvoiceDTO.Invoiceid = res.Invoiceid;
                    return true;
                }
                return false;
            }
            catch (InvalidOperationException ioe)
            {
                throw new InvalidOperationException(ioe.Message);
            }
        }

        [RelayCommand]
        private async Task CompleteInvoicePayment()
        {
            try
            {
                if (SelectedStaffDTO.Staffid == 0)
                {
                    MyMessageBox.Show("Vui lòng chọn người thanh toán");
                    return;
                }
                string messageBox = MyMessageBox.ShowDialog($"Bạn có muốn thanh toán hóa đơn cho {SelectedInvoiceDTO.CoffeetableDTO.TableName}");
                if (messageBox.Equals("1"))
                {
                    bool isSuccess = await AddInvoiceToDataBase("Hóa đơn đã thanh toán");
                    if (isSuccess)
                    {
                        ListInvoiceDTO.Remove(SelectedInvoiceDTO);
                        MyMessageBox.Show("Thanh toán hóa đơn thành công");
                        SelectedInvoiceDTO = new();
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message);
            }
        }

        [RelayCommand]
        private async Task CancelInvoicePayment()
        {
            try
            {
                if (SelectedStaffDTO.Staffid == 0)
                {
                    MyMessageBox.Show("Vui lòng chọn người hủy hóa đơn");
                    return;
                }
                string messageBox = MyMessageBox.ShowDialog($"Bạn có muốn hủy hóa hóa đơn cho {SelectedInvoiceDTO.CoffeetableDTO.TableName}");
                if (messageBox.Equals("1"))
                {
                    bool isSuccess = await AddInvoiceToDataBase("Hóa đơn đã bị hủy");
                    if (isSuccess)
                    {
                        ListInvoiceDTO.Remove(SelectedInvoiceDTO);
                        MyMessageBox.Show("Hủy hóa đơn thành công");
                        SelectedInvoiceDTO = new();
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message);
            }
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
            foreach (var item in SelectedInvoiceDTO.ListInvoiceDetailDTO)
            {
                item.QuantityChanged -= NewDetail_QuantityChanged;
            }
        }
    }
}