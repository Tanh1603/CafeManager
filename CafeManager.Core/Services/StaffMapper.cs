using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Linq;

namespace CafeManager.Core.Services
{
    public static class StaffMapper
    {
        public static StaffDTO ToDTO(Staff staff)
        {
            if (staff == null) throw new ArgumentNullException(nameof(staff));

            return new StaffDTO
            {
                Staffid = staff.Staffid,
                Staffname = staff.Staffname,
                Phone = staff.Phone,
                Sex = staff.Sex,
                Birthday = staff.Birthday,
                Address = staff.Address,
                Startworkingdate = staff.Startworkingdate,
                Endworkingdate = staff.Endworkingdate,
                Role = staff.Role,
                Isdeleted = staff.Isdeleted,
                Staffsalaryhistories = [.. staff.Staffsalaryhistories?.Select(h => StaffSalaryHistoryMapper.ToDTO(h)).ToList()]
            };
        }

        public static Staff ToEntity(StaffDTO staffDTO)
        {
            if (staffDTO == null) throw new ArgumentNullException(nameof(staffDTO));

            return new Staff
            {
                Staffid = staffDTO.Staffid,
                Staffname = staffDTO.Staffname,
                Phone = staffDTO.Phone,
                Sex = staffDTO.Sex,
                Birthday = staffDTO.Birthday,
                Address = staffDTO.Address,
                Startworkingdate = staffDTO.Startworkingdate,
                Endworkingdate = staffDTO.Endworkingdate,
                Role = staffDTO.Role,
                Isdeleted = staffDTO.Isdeleted,
            };
        }
    }
}