using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class Service
{
    public int NtId { get; set; }

    public string NtTitle { get; set; } = null!;

    public string? NtContent { get; set; }

    public string? NtAuthor { get; set; }

    public DateTime? NtPubDate { get; set; }

    public string? NtAttachment { get; set; }

    public int NtParentId { get; set; }

    public string? NtAttr1 { get; set; }

    public string? NtAttr2 { get; set; }

    public string? NtAttr3 { get; set; }

    public DateTime? NtCrtDate { get; set; }

    public string? NtCrtUser { get; set; }

    public DateTime? NtModDate { get; set; }

    public string? NtModUser { get; set; }

    public byte[]? NtFile { get; set; }

    public string? NtFileName { get; set; }

    public string? NtFileSize { get; set; }
}
