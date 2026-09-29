using ECFA_MVC.Models;
using ECFA_MVC.Services;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace ECFA_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var rawConnectionString = builder.Configuration.GetConnectionString("SecureConnection");

            if (!string.IsNullOrEmpty(rawConnectionString))
            {
                var connectionBuilder = new SqlConnectionStringBuilder(rawConnectionString);

                if (!string.IsNullOrEmpty(connectionBuilder.Password))
                {
                    byte[] passwordBytes = Convert.FromBase64String(connectionBuilder.Password);
                    connectionBuilder.Password = System.Text.Encoding.UTF8.GetString(passwordBytes).Trim();
                }
                var finalConnectionString = connectionBuilder.ConnectionString;

                builder.Services.AddDbContextPool<EcfaContext>(options =>
                    options.UseSqlServer(finalConnectionString)
                           .LogTo(Console.WriteLine, LogLevel.Information));
            }
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ISearchService, SearchService>();
            builder.Services.AddScoped<IMenuUrlService, MenuUrlService>();
            builder.Services.AddScoped<IExcelImportService, ExcelImportService>();
            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                // 1. COOP (Cross-Origin-Opener-Policy)
                // 確保您的文件與跨來源視窗隔離，防範 XS-Leaks
                context.Response.Headers.Append("Cross-Origin-Opener-Policy", "same-origin");

                // 2. COEP (Cross-Origin-Embedder-Policy)
                // 除非明確允許，否則阻止載入任何未啟用 CORS 或 CORP 的跨來源資源
                context.Response.Headers.Append("Cross-Origin-Embedder-Policy", "require-corp");

                // 3. CORP (Cross-Origin-Resource-Policy)
                // 宣告哪些網站可以載入當前網站的資源（視您的資源是否需要被外部嵌入而定）
                context.Response.Headers.Append("Cross-Origin-Resource-Policy", "same-origin");

                await next();
            });

            var options = new RewriteOptions()
                // 檢查網址是否為 ShowNews.aspx (不分大小寫)，並將後續的 QueryString ($1) 帶過去
                .AddRedirect("(?i)ShowDetail.aspx(.*)", "page/ShowDetail$1")
                .AddRedirect("(?i)Event.aspx(.*)", "list/Event$1")
                .AddRedirect("(?i)ATSFAQList.aspx(.*)", "list/ATSFAQ$1")
                .AddRedirect("(?i)FAQs.aspx(.*)", "list/FAQ$1")
                .AddRedirect("(?i)Video.aspx(.*)", "list/Video$1")
                .AddRedirect("(?i)DMAdList.aspx(.*)", "list/DMAdList$1")
                .AddRedirect("(?i)DownloadDoc.aspx(.*)", "list/DownloadDoc$1")
                .AddRedirect("(?i)NewsList.aspx(.*)", "list/News$1")
                .AddRedirect("(?i)EcfaCertiDoc.aspx(.*)", "list/Service$1")
                .AddRedirect("(?i)RelatedDoc.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)investAgreement1.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)investDocEn.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)EcfaCertiDoc.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)ShowNoticeNew.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)ShowTotalProfit.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)SerciveTradeIntro.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)SerciveTradeAgreement1.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)ServiceCertiDoc.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)investDoc2.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)OrganiNew.aspx(.*)", "page/relatedDoc$1")
                .AddRedirect("(?i)ShowFAQ.aspx(.*)", "faq/ShowFAQ$1")
                .AddRedirect("(?i)ShowATSFAQ.aspx(.*)", "faq/ShowATSFAQ$1");
            app.UseRewriter(options);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
