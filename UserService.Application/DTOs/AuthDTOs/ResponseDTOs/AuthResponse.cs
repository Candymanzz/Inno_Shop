using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.DTOs.AuthDTOs.ResponseDTOs
{
    internal record AuthResponse
    (
        string AccessToken,
        string RefreshToken
    );
}
