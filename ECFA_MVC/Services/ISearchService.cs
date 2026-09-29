namespace ECFA_MVC.Services
{
    using ECFA_MVC.Models.DTOs;
    using System.Collections.Generic;

    public interface ISearchService
    {
        SearchResultViewModel<SearchInfo> GetSearchInfos(HashSet<string> column, GufoQueryModel model, string years = "-30");
    }
}
