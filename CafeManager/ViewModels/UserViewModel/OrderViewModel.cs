using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.Views.UserView;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System.Buffers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace CafeManager.WPF.ViewModels.UserViewModel
{
    public partial class OrderViewModel : ObservableObject, IDisposable, IDataViewModel
    {
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FoodServices _foodServices;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly InvoiceServices _invoiceServices;
        private readonly StaffServices _staffServices;
        private readonly IMapper _mapper;
        private List<FoodDTO> _allFood;

        [ObservableProperty]
        private ObservableCollection<CoffeetableDTO> _listCoffeeTableDTO = [];
  

        [ObservableProperty]
        private ObservableCollection<FoodCategoryDTO> _listFoodCategoryDTO = [];

        private FoodCategoryDTO _selectedFoodCategory = new();

        public FoodCategoryDTO SelectedFoodCategory
        {
            get => _selectedFoodCategory;
            set
            {
                if (_selectedFoodCategory != value)
                {
                    _selectedFoodCategory = value;
                    ListFoodDTO = value == null ? [.. _allFood] : [.. value.Foods.Where(x => x.Isdeleted == false) ?? []];
                }
            }
        }

        private ObservableCollection<FoodDTO> _listFoodDTO = [];

        public ObservableCollection<FoodDTO> ListFoodDTO
        {
            get => _listFoodDTO; set
            {
                if (_listFoodDTO != value)
                {
                    _listFoodDTO = value;
                    OnPropertyChanged(nameof(ListFoodDTO));
                }
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ListCustomerInvoiceDTO))]
        private ObservableCollection<InvoiceDTO> _listInvoiceDTO = [];

        [ObservableProperty]
        private InvoiceDTO _selectedInvoiceDTO = new();

        [ObservableProperty]
        private ObservableCollection<StaffDTO> _listStaffDTO = [];

        [ObservableProperty]
        private StaffDTO _selectedStaffDTO = new();

        public decimal TotalPrice => SelectedInvoiceDTO.TotalPrice;

        [ObservableProperty]
        private string _selectedPaymentMethod = string.Empty;

        public ObservableCollection<InvoiceDTO> ListCustomerInvoiceDTO => [.. ListInvoiceDTO.Where(x => x.IsCustomer == true)];

        [ObservableProperty]
        private CoffeetableDTO? _selectedSwapTable;

        public OrderViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            _staffServices = provider.GetRequiredService<StaffServices>();
            _mapper = provider.GetRequiredService<IMapper>();
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var dbListCoffeeTable = (await _coffeTableServices.GetListCoffeTable(token)).Where(x => x.Isdeleted == false);
                foreach (var table in dbListCoffeeTable)
                {
                    table.Statustable = "Trống";  // Trạng thái ban đầu của bàn là "Trống"
                }
                var dbListFoodCategory = (await _foodCategoryServices.GetAllListFoodCategory(token)).Where(x => x.Isdeleted == false);
                var dbStaff = (await _staffServices.GetListStaff(token)).Where(x => x.Isdeleted == false);
                var dbAllFood = (await _foodServices.GetAllFood(token)).Where(x => x.Isdeleted == false);

                ListCoffeeTableDTO = [.. _mapper.Map<List<CoffeetableDTO>>(dbListCoffeeTable)];
                ListFoodCategoryDTO = [.. _mapper.Map<List<FoodCategoryDTO>>(dbListFoodCategory)];
                ListStaffDTO = [.. _mapper.Map<List<StaffDTO>>(dbStaff)];
                ListFoodDTO = [.. _mapper.Map<List<FoodDTO>>(dbAllFood)];
                _allFood = [.. _mapper.Map<List<FoodDTO>>(dbAllFood)];
            }
            catch (OperationCanceledException oe)
            {
                Debug.WriteLine(oe.Message);
                throw;
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
                    tableDTO.Statustable = "Đang sử dụng";

                   
                    SelectedInvoiceDTO = new InvoiceDTO()
                    {
                        Coffeetableid = tableDTO.Coffeetableid,
                        Coffeetable = tableDTO,
                        IsCoffeeTable = true,
                    };
                    ListInvoiceDTO.Add(SelectedInvoiceDTO);
                }
                else return;
            }

            SelectedInvoiceDTO = ListInvoiceDTO.FirstOrDefault(x => x.Coffeetableid == tableDTO.Coffeetableid) ?? new();
            OnPropertyChanged(nameof(SelectedInvoiceDTO.Coffeetable.TableName));
        }


        [RelayCommand]
        private void ChooseFood(FoodDTO foodDTO)
        {
            if (SelectedInvoiceDTO.IsCoffeeTable == true || SelectedInvoiceDTO.IsCustomer == true)
            {
                var exisitingFood = SelectedInvoiceDTO.Invoicedetails.FirstOrDefault(x => x.Foodid == foodDTO.Foodid);
                if (exisitingFood != null)
                {
                    exisitingFood.Quantity += 1;
                }
                else
                {
                    var newDetail = new InvoiceDetailDTO()
                    {
                        Quantity = 1,
                        Food = foodDTO,
                        Foodid = foodDTO.Foodid,
                    };
                    newDetail.QuantityChanged += NewDetail_QuantityChanged;
                    SelectedInvoiceDTO.Invoicedetails.Add(newDetail);
                }
                OnPropertyChanged(nameof(SelectedInvoiceDTO));
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
            SelectedInvoiceDTO.Invoicedetails.Remove(invoiceDetailDTO);
            OnPropertyChanged(nameof(SelectedInvoiceDTO));
        }

        private async Task<Invoice?> AddInvoiceToDataBase(string statusInvoice)
        {
            try
            {
                SelectedInvoiceDTO.Staffid = SelectedStaffDTO.Staffid;
                SelectedInvoiceDTO.Paymentenddate = DateTime.Now;
                SelectedInvoiceDTO.Paymentmethod = SelectedPaymentMethod;
                SelectedInvoiceDTO.Paymentstatus = statusInvoice;
                Invoice addInvoice = _mapper.Map<Invoice>(SelectedInvoiceDTO);
                var res = await _invoiceServices.CreateInvoice(addInvoice);
                return res;
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
                string messageBox = string.Empty;
                if (SelectedInvoiceDTO.IsCustomer)
                {
                    messageBox = MyMessageBox.ShowDialog($"Bạn có muốn thanh toán hóa đơn {SelectedInvoiceDTO.InvoiceCustomerId}", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Information);
                }
                else if (SelectedInvoiceDTO.IsCoffeeTable)
                {
                    messageBox = MyMessageBox.ShowDialog($"Bạn có muốn thanh toán hóa đơn {SelectedInvoiceDTO.Coffeetable.TableName}", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                }

                if (messageBox.Equals("1"))
                {
                    var addInvoice = await AddInvoiceToDataBase("Hóa đơn đã thanh toán");
                    if (addInvoice != null)
                    {
                        if (SelectedInvoiceDTO.IsCoffeeTable)
                        {
                            var tableDTO = SelectedInvoiceDTO.Coffeetable;
                            tableDTO.Statustable = "Trống"; // Cập nhật trạng thái bàn về "Trống"

                           
                        }
                        ListInvoiceDTO.Remove(SelectedInvoiceDTO);
                        MyMessageBox.ShowDialog("Thanh toán hóa đơn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.None);
                        SelectedInvoiceDTO = new();
                        var exportInvocie = _mapper.Map<InvoiceDTO>(addInvoice);
                        ShowInvoice(_mapper.Map<InvoiceDTO>(addInvoice));
                        OnPropertyChanged(nameof(ListCustomerInvoiceDTO));
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message);
            }
        }

        private void ShowInvoice(InvoiceDTO invoiceDTO)
        {
            InvoiceExportViewModel invoiceExportViewModel = new()
            {
                InvoiceExport = invoiceDTO
            };
            InvoiceExport invoiceExport = new()
            {
                DataContext = invoiceExportViewModel,
            };
            invoiceExport.Show();
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

                string messageBox = string.Empty;
                if (SelectedInvoiceDTO.IsCustomer)
                {
                    messageBox = MyMessageBox.ShowDialog($"Bạn có muốn hủy hóa đơn {SelectedInvoiceDTO.InvoiceCustomerId}", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                }
                else if (SelectedInvoiceDTO.IsCoffeeTable)
                {
                    messageBox = MyMessageBox.ShowDialog($"Bạn có muốn hủy hóa hóa đơn {SelectedInvoiceDTO.Coffeetable.TableName}", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                }

                if (messageBox.Equals("1"))
                {
                    var cancelInvoice = await AddInvoiceToDataBase("Hóa đơn đã bị hủy");
                    if (cancelInvoice != null)
                    {
                        ListInvoiceDTO.Remove(SelectedInvoiceDTO);
                        MyMessageBox.Show("Hủy hóa đơn thành công");
                        SelectedInvoiceDTO = new();
                        OnPropertyChanged(nameof(ListCustomerInvoiceDTO));
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
            if (SelectedSwapTable == null || SelectedInvoiceDTO.Coffeetableid == 0)
            {
                MyMessageBox.ShowDialog("Vui lòng chọn bàn để chuyển và tạo hóa đơn");
                return;
            }
            if (SelectedInvoiceDTO.Coffeetableid == null)
            {
                MyMessageBox.ShowDialog("Đang chọn hóa đơn mang về vui lòng chọn bàn có hóa đơn");
                return;
            }
            if (SelectedInvoiceDTO.Coffeetableid == SelectedSwapTable.Coffeetableid)
            {
                MyMessageBox.ShowDialog("Vui lòng không chọn trùng bàn");
                return;
            }
            else
            {
                string res =
                    MyMessageBox.ShowDialog($"Bạn muốn chuyển hóa đơn {SelectedInvoiceDTO.Coffeetable.TableName} sang {SelectedSwapTable.TableName}", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (res == "1")
                {
                    var exsiting = ListInvoiceDTO.FirstOrDefault(x => x.Coffeetableid == SelectedSwapTable.Coffeetableid);
                    if (exsiting != null)
                    {
                        var tmp = SelectedSwapTable;

                        exsiting.Coffeetableid = SelectedInvoiceDTO.Coffeetableid;
                        exsiting.Coffeetable = SelectedInvoiceDTO.Coffeetable;

                        SelectedInvoiceDTO.Coffeetableid = tmp.Coffeetableid;
                        SelectedInvoiceDTO.Coffeetable = tmp;
                    }
                    else
                    {
                        SelectedInvoiceDTO.Coffeetableid = SelectedSwapTable.Coffeetableid;
                        SelectedInvoiceDTO.Coffeetable = SelectedSwapTable;
                    }
                    OnPropertyChanged(nameof(SelectedInvoiceDTO));
                }
            }
        }

        [RelayCommand]
        private void CreateTakeAwayInvoice()
        {
            string messageBox
                = MyMessageBox.ShowDialog($"Bạn muốn tạo hóa đơn mới mang về không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
            if (messageBox.Equals("1"))
            {
                SelectedInvoiceDTO = new()
                {
                    IsCustomer = true,
                };
                ListInvoiceDTO.Add(SelectedInvoiceDTO);
                OnPropertyChanged(nameof(ListCustomerInvoiceDTO));
            }
        }

        [RelayCommand]
        private void ShowSelectedInvoiceCustomer(InvoiceDTO invoiceDTO)
        {
            if (invoiceDTO.IsCustomer && !string.IsNullOrEmpty(invoiceDTO.InvoiceCustomerId))
            {
                var tmp = ListInvoiceDTO.FirstOrDefault(x => x.InvoiceCustomerId == invoiceDTO.InvoiceCustomerId);
                if (tmp != null)
                {
                    SelectedInvoiceDTO = ListInvoiceDTO.FirstOrDefault(x => x.InvoiceCustomerId == invoiceDTO.InvoiceCustomerId) ?? new();
                }
            }
        }

        public void Dispose()
        {
            foreach (var item in SelectedInvoiceDTO.Invoicedetails)
            {
                item.QuantityChanged -= NewDetail_QuantityChanged;
            }
        }
    }
}