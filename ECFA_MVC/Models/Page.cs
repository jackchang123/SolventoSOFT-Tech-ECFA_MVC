using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class Page
{
    public int NtId { get; set; }

    public int NtNodeId { get; set; }

    public string NtTitle { get; set; } = null!;

    public string? NtContent { get; set; }

    public string? NtAuthor { get; set; }

    public DateTime? NtPubDate { get; set; }

    public string? NtAttachment { get; set; }

    public string? NtModHistory { get; set; }

    public int NtParentId { get; set; }
}
