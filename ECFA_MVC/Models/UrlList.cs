using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class UrlList
{
    public int NtId { get; set; }

    public string NtAuthor { get; set; } = null!;

    public string Url { get; set; } = null!;

    public int NtParentId { get; set; }

    public DateTime? NtPubDate { get; set; }

    public string? NtAttachment { get; set; }
}
