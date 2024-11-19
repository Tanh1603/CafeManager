using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public class ImportMapper
    {
        public static ImportDTO ToDTO(Import import)
        {
            if (import == null) throw new ArgumentNullException(nameof(import));

            return new ImportDTO
            {
                Importid = import.Importid,
                ImportSupplier = import.Supplier,
                Deliveryperson = import.Deliveryperson,
                Phone = import.Phone,
                Shippingcompany = import.Shippingcompany,
                Receiveddate = import.Receiveddate,
                Isdeleted = import.Isdeleted,
                ListImportDetailDTO = [.. import.Importdetails?.Select(h => ImportDetailMapper.ToDTO(h)).ToList()]
            };
        }

        public static Import ToEntity(ImportDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Import
            {
                Importid = dto.Importid,
                Supplierid = dto.ImportSupplier.Supplierid,
                Deliveryperson = dto.Deliveryperson,
                Phone = dto.Phone,
                Shippingcompany = dto.Shippingcompany,
                Receiveddate = dto.Receiveddate,
                Staffid = dto.ReceivedStaff.Staffid,
                Isdeleted = dto.Isdeleted,
                Importdetails = dto.ListImportDetailDTO?.Select(h => ImportDetailMapper.ToEntity(h)).ToList()
            };
        }
    }
}
