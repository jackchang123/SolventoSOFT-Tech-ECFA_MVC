using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class SlideNews
{
    public int NtId { get; set; }

    public string NtContent { get; set; } = null!;

    public DateTime? NtPubDate { get; set; }

    public int NtParentId { get; set; }

    public string? NtAuthor { get; set; }
}
