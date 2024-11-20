using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class ImportMapper
    {
        public static ImportDTO ToDTO(this Import import, bool isLazyLoad = false, HashSet<object> visited = null)
        {
            if (import == null) return null;

            visited ??= new HashSet<object>();
            if (visited.Contains(import)) return null;
            visited.Add(import);

            var dto = new ImportDTO
            {
                Importid = import.Importid,
                Deliveryperson = import.Deliveryperson,
                Phone = import.Phone,
                Shippingcompany = import.Shippingcompany,
                Receiveddate = import.Receiveddate,
                Staffid = import.Staffid,
                Supplierid = import.Supplierid,
                Isdeleted = import.Isdeleted,
            };
            if (isLazyLoad)
            {
                //dto.StaffDTO = import.Staff.ToDTO(true, visited);
                dto.StaffDTO = new()
                {
                    Staffname = import.Staff.Staffname
                };
                dto.SupplierDTO = import.Supplier.ToDTO(true, visited);
                dto.ListImportDetailDTO = [.. import.Importdetails.Select(x => x.ToDTO(true, visited))];
            }
            return dto;
        }

        public static Import ToEntity(this ImportDTO dto)
        {
            if (dto == null) return null;

            return new Import
            {
                Importid = dto.Importid,
                Deliveryperson = dto.Deliveryperson,
                Phone = dto.Phone,
                Shippingcompany = dto.Shippingcompany,
                Receiveddate = dto.Receiveddate,
                Staffid = dto.Staffid,
                Supplierid = dto.Supplierid,
                Isdeleted = dto.Isdeleted,
            };
        }
    }
}