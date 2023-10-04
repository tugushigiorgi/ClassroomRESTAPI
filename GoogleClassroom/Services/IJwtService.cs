using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database.Models;

namespace GoogleClassroom.Services
{
    public interface IJwtService
    {

        AuthenticationResponseDto CreateJwtToken(user user);

       


    }
}
