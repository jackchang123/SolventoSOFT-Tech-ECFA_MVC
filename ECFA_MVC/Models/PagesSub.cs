using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class PagesSub
{
    public int NtId { get; set; }

    public int? NtParentId { get; set; }

    public int? NtPageId { get; set; }

    public int? NtSort { get; set; }

    public string? NtContent { get; set; }
}
