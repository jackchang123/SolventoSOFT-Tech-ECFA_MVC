using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class DownloadCount
{
    public string LcSource { get; set; } = null!;

    public int LcId { get; set; }

    public string LcTitle { get; set; } = null!;

    public int LcCount { get; set; }

    public DateTime LcModifyDate { get; set; }
}
