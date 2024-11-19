using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public static class StaffSalaryHistoryMapper
    {
        public static StaffsalaryhistoryDTO ToDTO(Staffsalaryhistory entity)
        {
            if (entity == null) return null;

            return new StaffsalaryhistoryDTO
            {
                Staffsalaryhistoryid = entity.Staffsalaryhistoryid,
                Staffid = entity.Staffid,
                Salary = entity.Salary,
                Effectivedate = entity.Effectivedate,
                Isdeleted = entity.Isdeleted,
            };
        }

        public static Staffsalaryhistory ToEntity(StaffsalaryhistoryDTO dto)
        {
            if (dto == null) return null;

            return new Staffsalaryhistory
            {
                Staffsalaryhistoryid = dto.Staffsalaryhistoryid,
                Staffid = dto.Staffid,
                Salary = dto.Salary,
                Effectivedate = dto.Effectivedate,
                Isdeleted = dto.Isdeleted
            };
        }
    }
}