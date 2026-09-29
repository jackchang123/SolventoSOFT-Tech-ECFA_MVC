using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class RelatedDoc
{
    public int NtId { get; set; }

    public int NtNodeId { get; set; }

    public string? NtTitle { get; set; }

    public string? NtContent { get; set; }

    public string? NtAuthor { get; set; }

    public DateTime? NtPubDate { get; set; }
}
