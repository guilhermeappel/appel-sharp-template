using Appel.SharpTemplate.Domain.Entities;
using Appel.SharpTemplate.Domain.Models.User;
using Riok.Mapperly.Abstractions;

namespace Appel.SharpTemplate.Application.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial UserAuthenticationModel Map(UserEntity userEntity);

    public partial UserEntity Map(UserRegisterModel userRegisterModel);
}
