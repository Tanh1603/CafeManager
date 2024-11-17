using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public static class CoffeeTableMapper
    {
        public static CoffeetableDTO ToDTO(Coffeetable coffeetable)
        {
            return new CoffeetableDTO()
            {
                Coffeetableid = coffeetable.Coffeetableid,
                Tablenumber = coffeetable.Tablenumber,
                Seatingcapacity = coffeetable.Seatingcapacity,
                Statustable = coffeetable.Statustable,
                Notes = coffeetable.Notes,
                Isdeleted = coffeetable.Isdeleted,
            };
        }

        public static Coffeetable ToEntity(CoffeetableDTO coffeetable)
        {
            return new Coffeetable()
            {
                Coffeetableid = coffeetable.Coffeetableid,
                Tablenumber = coffeetable.Tablenumber,
                Seatingcapacity = coffeetable.Seatingcapacity,
                Statustable = coffeetable.Statustable,
                Notes = coffeetable.Notes,
                Isdeleted = coffeetable.Isdeleted,
            };
        }
    }
}