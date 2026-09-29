using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static GufonetUtility.GufonetConstants.DefaultValue;

namespace ECFA_MVC.Models.DTOs
{
    class JsonFormat
    {
        public bool Error { get; set; }
        public object? Data { get; set; } = "";
        public string Message { get; set; } = "";
    }

    public class ReadSetting
    {
        public int TotalCount { get; set; } = 0;

        public int AtPage { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 10;

        public string SearchKeyword { get; set; } = "";
    }
    public class GufoQueryModel
    {
        public string? __RequestVerificationToken { get; set; }
        public string Keyword { get; set; } = "";
        public string Wggroup { get; set; } = "";
        public int Sitetype { get; set; }
        public int RefreshKeyword { get; set; }
        public string PageNo { get; set; } = "";
        public string Start { get; set; } = "";
        public string Sort { get; set; } = "";
        public string SearchMode { get; set; } = "";
        public string S1 { get; set; } = "";
        public string S2 { get; set; } = "";
        public string F_word { get; set; } = "";
        public string F_ppt { get; set; } = "";
        public string F_xls { get; set; } = "";
        public string F_pdf { get; set; } = "";
        public string F_htm { get; set; } = "";
        public string F_zip { get; set; } = "";
        [FromForm(Name = "Date1")]
        public string Date1 { get; set; } = "";
        public string Date2 { get; set; } = "";
        public string RangeContent { get; set; } = "";
        public string FirstTitleTyperangeContent { get; set; } = "";
        public string Datea { get; set; } = "";
        public string Dateb { get; set; } = "";
        public string Years { get; set; } = "";
    }

    class ReadResult<T>
    {
        public int TotalCount { get; set; }
        public T[] Items { get; set; } = new T[0];
    }

    public class TermItem
    {
        public string Text { get; set; } = "";

        public string Data { get; set; } = "";

        public int Df { get; set; } = 0;
    }
}
