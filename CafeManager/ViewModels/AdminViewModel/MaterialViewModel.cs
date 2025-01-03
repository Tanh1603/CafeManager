﻿using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class MaterialViewModel : ObservableObject, IDisposable
    {
        private MaterialSupplierServices _materialSupplierServices;
        private IMapper _mapper;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isOpenModifyMaterial = false;

        [ObservableProperty]
        private ObservableCollection<MaterialDTO> _listMaterial = [];

        [ObservableProperty]
        private ObservableCollection<MaterialDTO> _listDeletedMaterial = [];

        [ObservableProperty]
        private AddMaterialViewModel _modifyMaterialVM;

        public MaterialViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _mapper = provider.GetRequiredService<IMapper>();

            ModifyMaterialVM = provider.GetRequiredService<AddMaterialViewModel>();
            ModifyMaterialVM.ModifyMaterialChanged += ModifyMaterialVM_ModifyMaterialChanged;
            ModifyMaterialVM.Close += ModifyMaterialVM_Close;
        }

        private void ModifyMaterialVM_Close()
        {
            IsOpenModifyMaterial = false;
            ModifyMaterialVM.ClearValueOfFrom();
        }

        [RelayCommand]
        private void OpenAddMaterial()
        {
            IsOpenModifyMaterial = true;
            ModifyMaterialVM.IsAdding = true;
            ModifyMaterialVM.ModifyMaterial = new();
        }

        [RelayCommand]
        private void OpenUpdateMaterial(MaterialDTO material)
        {
            IsOpenModifyMaterial = true;
            ModifyMaterialVM.IsUpdating = true;
            ModifyMaterialVM.ReceiveMaterialDTO(material);
        }

        private async void ModifyMaterialVM_ModifyMaterialChanged(MaterialDTO obj)
        {
            try
            {
                IsLoading = true;
                if (ModifyMaterialVM.IsAdding)
                {
                    var addMaterial = await _materialSupplierServices.AddMaterial(_mapper.Map<Material>(obj));
                    if (addMaterial != null)
                    {
                        ListMaterial.Add(_mapper.Map<MaterialDTO>(addMaterial));
                        IsOpenModifyMaterial = false;
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Thêm vật liệu cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Thêm vật liệu cấp thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
                if (ModifyMaterialVM.IsUpdating)
                {
                    var res = await _materialSupplierServices.UpdateMaterial(_mapper.Map<Material>(obj));
                    if (res != null)
                    {
                        var updateSupplierDTO = ListMaterial.FirstOrDefault(x => x.Materialid == res.Materialid);
                        if (updateSupplierDTO != null)
                        {
                            _mapper.Map(res, updateSupplierDTO);
                            IsOpenModifyMaterial = false;
                            IsLoading = false;
                            MyMessageBox.ShowDialog("Sửa vật liệu cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                        }
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Sửa vật liệu thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
                ModifyMaterialVM.ClearValueOfFrom();
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task DeleteMaterial(MaterialDTO material)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn ẩn vật liệu không không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    IsLoading = true;
                    bool isDeleted = await _materialSupplierServices.DeleteMaterial(material.Materialid);
                    if (isDeleted)
                    {
                        ListDeletedMaterial.Add(material);
                        ListMaterial.Remove(material);
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Ẩn vật liệu thanh công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Ẩn vật liệu thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task RestoreMaterial(MaterialDTO material)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn hiển thị vật liệu không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    IsLoading = true;
                    material.Isdeleted = false;
                    var res = await _materialSupplierServices.UpdateMaterial(_mapper.Map<Material>(material));
                    if (res != null)
                    {
                        ListMaterial.Add(_mapper.Map<MaterialDTO>(res));
                        ListDeletedMaterial.Remove(material);
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Hiển thị vật liệu thanh công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Hiển thị vật liệu thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void Dispose()
        {
            ModifyMaterialVM.ModifyMaterialChanged -= ModifyMaterialVM_ModifyMaterialChanged;
            ModifyMaterialVM.Close -= ModifyMaterialVM_Close;
            GC.SuppressFinalize(this);
        }
    }
}