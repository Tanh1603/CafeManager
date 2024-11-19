using CafeManager.Core.Data;
using CafeManager.Core.DTOs;

namespace CafeManager.Core.Services
{
    public static class AppUserMapper
    {
        public static AppUserDTO ToDTO(Appuser entity)
        {
            if (entity == null) return null;

            return new AppUserDTO
            {
                Appuserid = entity.Appuserid,
                Username = entity.Username,
                Displayname = entity.Displayname,
                Email = entity.Email,
                Role = entity.Role == 1 ? "Admin" : "User",
                Avatar = ConvertImageServices.Base64ToBitmapImage(entity.Avatar),
                Isdeleted = entity.Isdeleted
            };
        }

        public static Appuser ToEntity(AppUserDTO dto)
        {
            if (dto == null) return null;

            return new Appuser
            {
                Appuserid = dto.Appuserid,
                Username = dto.Username,
                Displayname = dto.Displayname,
                Email = dto.Email,
                Role = dto.Role == "Admin" ? 1 : 0,
                Avatar = ConvertImageServices.BitmapImageToBase64(dto.Avatar),
                Isdeleted = dto.Isdeleted
            };
        }

        public static IEnumerable<AppUserDTO> ToDTOList(IEnumerable<Appuser> entities)
        {
            if (entities == null) return null;

            return entities.Select(entity => ToDTO(entity)).ToList();
        }

        public static IEnumerable<Appuser> ToEntityList(IEnumerable<AppUserDTO> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(dto => ToEntity(dto)).ToList();
        }
    }
}