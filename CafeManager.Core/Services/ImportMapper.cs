using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class ImportMapper
    {
        public static ImportDTO ToDTO(Import import)
        {
            if (import == null) return null;

            return new ImportDTO
            {
                Importid = import.Importid,
                Deliveryperson = import.Deliveryperson,
                Phone = import.Phone,
                Shippingcompany = import.Shippingcompany,
                Receiveddate = import.Receiveddate,
                Staffid = import.Staffid,
                Supplierid = import.Supplierid,
                Isdeleted = import.Isdeleted,

                //ListImportDetailDTO = [.. import.Importdetails?.Select(h => ImportDetailMapper.ToDTO(h))],
                //StaffDTO = StaffMapper.ToDTO(import.Staff),
                //SupplierDTO = Sup
            };
        }

        public static Import ToEntity(ImportDTO dto)
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