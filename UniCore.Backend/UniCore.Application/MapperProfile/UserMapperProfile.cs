using Mapster;
using UniCore.Application.Feature.v1.Auth.Me;
using UniCore.Application.Feature.v1.Auth.Register;
using UniCore.Application.DTO.Entity;
using UniCore.Application.Entity;

namespace UniCore.Application.MapperProfile
{
    public class UserMapperProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserDTO>()
                .Map(dest => dest.Roles, src => src.UserRoles != null
                    ? src.UserRoles.Where(up => up.Role != null).Select(up => new RoleDTO
                    {
                        Id = up.Role.Id,
                        Code = up.Role.Code,
                        Name = up.Role.Name,
                        Description = up.Role.Description,
                    }).ToList()
                    : new List<RoleDTO>())
                .Map(dest => dest.Permissions, src => src.UserPermissions != null
                    ? src.UserPermissions.Where(up => up.Permission != null).Select(up => new PermissionDTO
                    {
                        Id = up.Permission.Id,
                        Code = up.Permission.Code,
                        Name = up.Permission.Name,
                        Description = up.Permission.Description,
                        Resource = up.Permission.Resource,
                    }).ToList()
                    : new List<PermissionDTO>())
                .Map(dest => dest.FirstName, src => src.UserProfile != null ? src.UserProfile.FirstName : null)
                .Map(dest => dest.LastName, src => src.UserProfile != null ? src.UserProfile.LastName : null)
                .Map(dest => dest.PhoneNumber, src => src.UserProfile != null ? src.UserProfile.PhoneNumber : null)
                .Map(dest => dest.AvatarUrl, src => src.UserProfile != null ? src.UserProfile.AvatarUrl : null)
                .Map(dest => dest.AvatarMediaFileId, src => src.UserProfile != null ? src.UserProfile.AvatarMediaFileId : null)
                .Map(dest => dest.Gender, src => src.UserProfile != null ? src.UserProfile.Gender : null)
                .Map(dest => dest.BirthDate, src => src.UserProfile != null ? src.UserProfile.BirthDate : null)
                .Map(dest => dest.Address, src => src.UserProfile != null ? src.UserProfile.Address : null);

            config.NewConfig<User, GetMeResponseDTO>()
                .Inherits<User, UserDTO>();

            config.NewConfig<RegisterRequestDTO, User>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.UserPermissions)
                .Ignore(dest => dest.UserTokens);

            config.NewConfig<User, RegisterResponseDTO>()
                .Map(dest => dest.Message, src => "Registration successful. Please verify your email with the OTP provided.");
        }
    }
}