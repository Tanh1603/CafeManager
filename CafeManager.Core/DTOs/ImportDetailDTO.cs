using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.DTOs
{
    public class ImportDetailDTO : INotifyPropertyChanged
    {
        private int _importdetailid;

        public int Importdetailid
        {
            get => _importdetailid;
            set
            {
                if (_importdetailid != value)
                {
                    _importdetailid = value;
                    OnPropertyChanged();
                }
            }
        }
        public int ImportId { get; set; }
        public int MaterialId { get; set; }

        private decimal? _quantity;

        public decimal? Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    UpdateTotalPriceAction?.Invoke();
                }
            }
        }

        private MaterialDetailDTO _modifyMaterialDetail;

        public MaterialDetailDTO ModifyMaterialDetail
        {
            get => _modifyMaterialDetail;
            set
            {
                if (_modifyMaterialDetail != value)
                {
                    _modifyMaterialDetail = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool? _isdeleted;
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

        public ImportDetailDTO Clone()
        {
            return new ImportDetailDTO
            {
                Importdetailid = this.Importdetailid,
                ImportId = this.ImportId,
                MaterialId = this.MaterialId,
                ModifyMaterialDetail = this.ModifyMaterialDetail,
                Quantity = this.Quantity
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static Action? UpdateTotalPriceAction { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
