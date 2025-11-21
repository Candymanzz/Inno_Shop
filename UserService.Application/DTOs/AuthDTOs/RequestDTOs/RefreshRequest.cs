using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.DTOs.AuthDTOs.RequestDTOs
{
    public record RefreshRequest
    (
        string RefreshToken
    );
}
