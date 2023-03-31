﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcLab4.Identity
{
  public class ApplicationRole : IdentityRole
  {
    public string Description { get; set; }

    //Claim c = new Claim(type: "a", value: "b");

  }
}
