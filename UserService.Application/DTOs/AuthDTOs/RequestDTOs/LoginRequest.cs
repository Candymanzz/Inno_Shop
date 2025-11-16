using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.DTOs.AuthDTOs.RequestDTOs
{
    internal record LoginRequest
    (
        string Email,
        string Password
    );
}
