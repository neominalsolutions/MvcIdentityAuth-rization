﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcLab4.Identity
{
    // kendi nesnemiz ile çalışarak geliştirme yapmak
    public class ApplicationUser: IdentityUser
    {
        public string? WebSite { get; set; }

    }
}
