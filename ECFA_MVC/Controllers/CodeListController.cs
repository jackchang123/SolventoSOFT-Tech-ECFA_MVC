using ECFA_MVC.Models;
using ECFA_MVC.Models.DTOs;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class CodeListController : Controller
    {
        private readonly EcfaContext _context;
        public CodeListController(EcfaContext context)
        {
            _context = context;
        }
        [Route("CategoryListResult")]
        public IActionResult CategoryListResult(int nid)
        {
            return View();
        }
        [Route("CodeList")]
        public IActionResult CodeList(int nid,int type)
        {
            if (!(type == 1 || type == 2))
            {
                return NotFound();
            }
            return View();
        }
        [HttpGet]
        public IActionResult CodeListResult(string code, int type)
        {
            if (!(type == 1 || type == 2))
            {
                return NotFound();
            }

            ViewBag.type = type;
            if (type == 1)
            {
                ViewBag.tabletitle = "ECFA早收清單大陸方面減讓稅號對照台灣方面稅號及PSR對應表";
                ViewBag.tag1 = "大陸";
                ViewBag.tag2 = "台灣";
            }
            else if (type == 2)
            {
                ViewBag.tabletitle = "ECFA早收清單台灣方面減讓稅號對照大陸方面稅號及PSR對應表";
                ViewBag.tag1 = "台灣";
                ViewBag.tag2 = "大陸";
            }


            List<CodeListDto> resultList = new List<CodeListDto>();

            if (type == 1)
            {
                var query = _context.CodeList539s.AsQueryable();

                if (!string.IsNullOrEmpty(code))
                    query = query.Where(x => x.TwCode != null && x.TwCode.Replace("EX", "").StartsWith(code)
                                || x.CnCode != null && x.CnCode.Replace("EX", "").StartsWith(code));

                resultList = query
                    .AsNoTracking()
                    .Select(x => new CodeListDto
                    {
                        // 將 Table A 的欄位一一對應進來
                        TW_CODE = x.TwCode ?? string.Empty,
                        TW_CNAME = x.TwCname ?? string.Empty,
                        TW_ENAME = x.TwEname ?? string.Empty,
                        TW_TAX = x.TwTax ?? string.Empty,
                        PSR = x.Psr ?? string.Empty,
                        EX = x.Ex ?? string.Empty,
                        CN_CODE = x.CnCode ?? string.Empty,
                        CN_CNAME = x.CnCname ?? string.Empty,
                        CN_ENAME = x.CnEname ?? string.Empty,
                        CN_TAX = x.CnTax ?? string.Empty,
                        MEMO = x.Memo ?? string.Empty
                    })
                    .ToList();

                var sortedResult = resultList
                    .OrderBy(x => x.CN_CODE?.Replace("EX", "") ?? "")
                    .ThenBy(x => x.TW_CODE ?? "")
                    .ToList();


                return View(sortedResult);
            }
            else if (type == 2)
            {
                var query = _context.CodeList267s.AsQueryable();

                if (!string.IsNullOrEmpty(code))
                    query = query.Where(x => x.TwCode != null && x.TwCode.Replace("EX", "").StartsWith(code)
                                || x.CnCode != null && x.CnCode.Replace("EX", "").StartsWith(code));

                resultList = query
                    .AsNoTracking()
                    .Select(x => new CodeListDto
                    {
                        // 將 Table A 的欄位一一對應進來
                        TW_CODE = x.TwCode ?? string.Empty,
                        TW_CNAME = x.TwCname ?? string.Empty,
                        TW_ENAME = x.TwEname ?? string.Empty,
                        TW_TAX = x.TwTax ?? string.Empty,
                        PSR = x.Psr ?? string.Empty,
                        EX = x.Ex ?? string.Empty,
                        CN_CODE = x.CnCode ?? string.Empty,
                        CN_CNAME = x.CnCname ?? string.Empty,
                        CN_ENAME = x.CnEname ?? string.Empty,
                        CN_TAX = x.CnTax ?? string.Empty,
                        MEMO = x.Memo ?? string.Empty
                    })
                    .ToList();

                var sortedResult = resultList
                    .OrderBy(x => x.TW_CODE?.Replace("EX", "") ?? "")
                    .ThenBy(x => x.CN_CODE ?? "")
                    .ToList();

                return View(sortedResult);
            }
            return View();
        }

        [HttpGet]
        public IActionResult SearchDataJson(string category, string code, string name)
        {
            //List<CodeListDto> resultList = new List<CodeListDto>();

            if (category == "2")
            {
                var query = _context.CodeList539s.AsQueryable();

                if (!string.IsNullOrEmpty(code))
                    query = query.Where(x => x.CnCode != null && x.CnCode.Contains(code));
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(x => x.CnCname != null && x.CnCname.Contains(name));
                var groupedQuery = query.GroupBy(x => new
                {
                    x.CnCode,
                    x.CnCname,
                    x.CnTax
                });
                var resultList = groupedQuery.Select(g => new
                {
                    categoryText = (category == "2") ? "台灣=>大陸" : "大陸=>台灣",

                    // 💡 這些是 Group By 的 Key 欄位（絕對不會重複）
                    code = g.Key.CnCode,
                    name = g.Key.CnCname,
                    tax = g.Key.CnTax
                })
                .ToList(); // 資料撈回伺服器記憶體
                return Json(resultList);
            }
            else if (category == "1")
            {
                var query = _context.CodeList267s.AsQueryable();

                if (!string.IsNullOrEmpty(code))
                    query = query.Where(x => x.TwCode != null && x.TwCode.Contains(code));
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(x => x.TwCname != null && x.TwCname.Contains(name));

                var groupedQuery = query.GroupBy(x => new
                {
                    x.TwCode,
                    x.TwCname,
                    x.TwTax
                });
                var resultList = groupedQuery.Select(g => new
                {
                    categoryText = (category == "2") ? "台灣=>大陸" : "大陸=>台灣",

                    // 💡 這些是 Group By 的 Key 欄位（絕對不會重複）
                    code = g.Key.TwCode,
                    name = g.Key.TwCname,
                    tax = g.Key.TwTax
                })
                .ToList(); // 資料撈回伺服器記憶體
                return Json(resultList);
            } 
            else
            {
                return NoContent();
            } 
        }
    }
}
