using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class PageAttachment
{
    public int NtId { get; set; }

    public string NtTitle { get; set; } = null!;

    public int? NtParentId { get; set; }

    public int? NtSort { get; set; }

    public byte[]? NtFile { get; set; }

    public string? NtFileName { get; set; }

    public string? NtFileSize { get; set; }

    public DateTime? NtPubDate { get; set; }

    public DateTime? NtCrtDate { get; set; }

    public string? NtCrtUser { get; set; }

    public DateTime? NtModDate { get; set; }

    public string? NtModUser { get; set; }
}
