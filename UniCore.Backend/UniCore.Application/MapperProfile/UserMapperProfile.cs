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
                    : new List<PermissionDTO>());

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