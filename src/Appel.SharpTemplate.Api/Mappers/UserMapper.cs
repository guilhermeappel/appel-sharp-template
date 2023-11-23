using Appel.SharpTemplate.Api.Contracts.User;
using Appel.SharpTemplate.Domain.Models.User;
using Riok.Mapperly.Abstractions;

namespace Appel.SharpTemplate.Api.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial UserLoginModel Map(UserLoginContract userLoginContract);

    public partial UserRegisterModel Map(UserRegisterContract userRegisterContract);
}
