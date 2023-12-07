using System;
using AndreasBlog.Shared;

namespace AndreasBlog.Server.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}

