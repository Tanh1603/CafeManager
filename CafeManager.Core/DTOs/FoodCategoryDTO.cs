using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

public class FoodCategoryDTO : INotifyPropertyChanged
{
    private int _foodcategoryid;
    private string _foodcategoryame;
    private bool? _isdeleted;
    private ObservableCollection<FoodDTO> _foods = new();

    public int Foodcategoryid
    {
        get => _foodcategoryid;
        set
        {
            if (_foodcategoryid != value)
            {
                _foodcategoryid = value;
                OnPropertyChanged();
            }
        }
    }

    public string Foodcategoryname
    {
        get => _foodcategoryame;
        set
        {
            if (_foodcategoryame != value)
            {
                _foodcategoryame = value;
                OnPropertyChanged();
            }
        }
    }

    public bool? Isdeleted
    {
        get => _isdeleted;
        set
        {
            if (_isdeleted != value)
            {
                _isdeleted = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<FoodDTO> Foods
    {
        get => _foods;
        set
        {
            if (_foods != value)
            {
                _foods = value;
                OnPropertyChanged();
            }
        }
    }

    public FoodCategoryDTO Clone()
    {
        return new FoodCategoryDTO()
        {
            Foodcategoryid = this.Foodcategoryid,
            Foodcategoryname = this.Foodcategoryname,
            Isdeleted = this.Isdeleted,
            Foods = [.. this.Foods.Select(f => f.Clone()).ToList()],
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}