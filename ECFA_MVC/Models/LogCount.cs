using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class LogCount
{
    public int LcNodeId { get; set; }

    public int LcParentId { get; set; }

    public string LcTarget { get; set; } = null!;

    public int? LcId { get; set; }

    public int LcCount { get; set; }

    public DateTime? LcModifyDate { get; set; }
}
