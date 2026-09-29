using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class User
{
    public int Sequence { get; set; }

    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string AuthCode { get; set; } = null!;

    public string UserGroup { get; set; } = null!;

    public DateTime? SetDate { get; set; }

    public string? SetUser { get; set; }
}
