using CafeManager.Core.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

#nullable disable

public partial class FoodCategoryDTO : BaseDTO
{
    [ObservableProperty]
    private int _foodcategoryid;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Tên danh mục không được trống")]
    private string _foodcategoryname;

    [ObservableProperty]
    private ObservableCollection<FoodDTO> _foods;

    public FoodCategoryDTO Clone()
    {
        return new FoodCategoryDTO()
        {
            Id = Id,
            Foodcategoryid = Foodcategoryid,
            Foodcategoryname = Foodcategoryname,
            Isdeleted = Isdeleted,
            Foods = Foods,
        };
    }
}