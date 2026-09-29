var itemsCount = 20;
var itemsOnPage = 5;

$(function () {
    var QueryString = function () {
        // This function is anonymous, is executed immediately and 
        // the return value is assigned to QueryString!
        var query_string = {};
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            // If first entry with this name
            if (typeof query_string[pair[0]] === "undefined") {
                query_string[pair[0]] = pair[1];
                // If second entry with this name
            } else if (typeof query_string[pair[0]] === "string") {
                var arr = [query_string[pair[0]], pair[1]];
                query_string[pair[0]] = arr;
                // If third or later entry with this name
            } else {
                query_string[pair[0]].push(pair[1]);
            }
        }
        return query_string;
    }();
    if (QueryString.keyword == undefined)
        QueryString.keyword = "";

    $('#keyword').val(decodeURI(QueryString.keyword));

    $("#btnSearch").click(function () {
        var s = $('#keyword').val();

        if (!isValidate()) return;
        //搜尋完後回到年
        $("input[name='statDate']")[0].checked = true;
        $("#start").val(1);
        $('#datea').val("");
        $('#dateb').val("");
        $("#rangeContent").val("");
        $("#firstTitleTyperangeContent").val("");
        const $container = $("#pagination-container");
        $container.empty();
        doSearch(1);
    });

    if ($("#keyword").val() != "") {
        doSearch(1);
    }

    $("#treeBox :radio").change(function () {
        radioChange($(this).val());
    });
});

function isValidate() {
    var sStartDT = $("#calendarFrom").val();
    var sEndDT = $("#calendarTo").val();

    if (sStartDT != "") {
        if ((sStartDT.indexOf("/") < 0) && (sStartDT.indexOf("-") < 0)) {
            $(".resultStatsBox_text").html("開始時間格式不正確！").fadeOut("fast").fadeIn("fast").fadeOut("fast").fadeIn("fast");
            return false;
        }

        if (isNaN(Date.parse(sStartDT))) {
            $(".resultStatsBox_text").html("開始時間格式不正確！").fadeOut("fast").fadeIn("fast").fadeOut("fast").fadeIn("fast");
            return false;
        }
    }

    if (sEndDT != "") {
        if ((sEndDT.indexOf("/") < 0) && (sEndDT.indexOf("-") < 0)) {
            $(".resultStatsBox_text").html("結束時間格式不正確！").fadeOut("fast").fadeIn("fast").fadeOut("fast").fadeIn("fast");
            return false;
        }

        if (isNaN(Date.parse(sEndDT))) {
            $(".resultStatsBox_text").html("結束時間格式不正確！").fadeOut("fast").fadeIn("fast").fadeOut("fast").fadeIn("fast");
            return false;
        }
    }

    if ((sStartDT != "") && (sEndDT != "")) {
        if (sEndDT < sStartDT) {
            $(".resultStatsBox_text").html("結束時間不可小於開始時間！").fadeOut("fast").fadeIn("fast").fadeOut("fast").fadeIn("fast");
            return false;
        }
    }

    if ($.trim($("#keyword").val()) == '') {
        $(".resultStatsBox_text").html("請輸入查詢語句！").fadeOut("fast").fadeIn("fast").fadeOut("fast").fadeIn("fast");
        $("#keyword").focus();
        return false;
    }
    return true;
}

function doSearch(targetPage) {
    var pageindex = 0;
    document.cookie = 'cip=test;path=/';
    window.scroll(0, 0);
    var postURL = $("#btnSearch").data("search-url");
    var token = $("input[name='__RequestVerificationToken']").val();
    var s1 = (typeof ($("input[name='s1']:checked").val()) == "undefined") ? "0" : "1";
    var s2 = (typeof ($("input[name='s2']:checked").val()) == "undefined") ? "0" : "1";

    var currentStartPage = $("#start").val();

    var requestData = {
        __RequestVerificationToken: token,
        keyword: $("#keyword").val(),
        wggroup: $("#selectGroup").val(),
        sitetype: 1,
        refreshKeyword: 0,
        pageNo: $("#perPageData").val(),
        start: currentStartPage,
        sort: $("#selectSort").val(),
        searchMode: $("input[name='searchmode']:checked").val(),
        s1: (typeof ($("input[name='s1']:checked").val()) == "undefined") ? "0" : "1",
        s2: (typeof ($("input[name='s2']:checked").val()) == "undefined") ? "0" : "1",
        date1: $("#calendarFrom").val(), date2: $("#calendarTo").val(), rangeContent: $("#rangeContent").val(), firstTitleTyperangeContent: $("#firstTitleTyperangeContent").val(),
        datea: $("#datea").val(), dateb: $("#dateb").val(), years: $('input[name=statDate]:checked').val()
    };

    $.ajax({
        url: postURL,
        dataType: "json",
        async: false, // 設定為同步
        beforeSend: function () {
            //$("#SearchResult").html("<img src='images/ajax-loading.gif' alt='查詢中'>&nbsp;&nbsp;查詢中，請稍候...");
            $("#SearchResult, #pager, #DetailResult, #RelatedResult, #Back").fadeOut("fast");
        },
        headers: {
            "RequestVerificationToken": token
        },
        type: "POST",    // 確保是全部大寫的 POST
        method: "POST",  // 同時補上 method 確保相容性
        data: requestData,
        cache: false,
        success: function (response) {
            handleResult(response, targetPage);
        },
        error: errorHandle,
        complete: function () {
            if (targetPage ==1) {
                $("#old_datea").val($("#datea").val());
                $("#old_dateb").val($("#dateb").val());
                $("#datea").val("");
                $("#dateb").val("");
                $("#years").val("");
            }
        }
    });
}

function handleResult(data, isPaginationClick) {
    const errorDiv = document.getElementById("search-error-message");

    if (data.Error) {
        if (errorDiv) {
            errorDiv.style.display = "block"; // 1. 先顯示區塊（讓螢幕閱讀器準備好監聽）
            
            // 2. 延遲極短時間（或直接寫入），觸發 aria-live 的動態朗讀
            setTimeout(() => {
                errorDiv.textContent = "搜尋發生錯誤：" + data.Message;
            }, 50); 
            
            // 3. 將焦點（Focus）移至錯誤訊息上，讓鍵盤操作者也能立刻知道位置
            errorDiv.setAttribute("tabindex", "-1");
            errorDiv.focus();
        }
        return;
    }
    
    // 如果成功，記得清空並隱藏錯誤區塊
    if (errorDiv) {
        errorDiv.textContent = "";
        errorDiv.style.display = "none";
    }

    // 2. 防呆：檢查是否有搜尋到資料
    if (!data.Data || !data.Data.Items || data.Data.Items.length === 0) {
        $("#SearchResult").html("<li class='no-result'>查無相關結果。</li>").fadeIn("fast");
        var infoText = "共有 <span class=\"text-red\">" + data.Data.TotalPages + "筆</span>搜尋結果";
        $(".resultStatsBox_text").html(infoText);
        itemsCount = data.Data.TotalPages;
        $("#pager").hide();
        return;
    }
    var currentPage = parseInt($("#start").val()) || 1;
    var pageSize = parseInt($("#perPageData").val()) || 5;
    var startOffset = (currentPage - 1) * pageSize;

    // 3. 💡【核心替換】：使用原生 JS 的 map 迴圈組裝 HTML 樣板
    var htmlRows = data.Data.Items.map(function (info, index) {

        // 處理流水號 (從 1 開始)
        var itemNum = startOffset + index + 1;

        // 處理來源連結防呆
        var nodeUrl = (info.NodeLink && info.NodeLink.PageUrl) ? info.NodeLink.PageUrl : '#';

        // 處理日期格式化 (將 2026-02-20T... 轉為 2026/02/20)
        var dateText = "";
        if (info.Time && info.Time !== '0001-01-01T00:00:00') {
            dateText = " (" + info.Time.substring(0, 10).replace(/-/g, '/') + ")";
        }

        // 返回原生的 HTML 樣板字串
        return `
            <li class="search-result-item">
                <div>
                    <span class="item-num">${itemNum}</span>
                    <div class="result-info">
                        
                        <!-- 標題欄位 -->
                        <div class="row">
                            <span class="label">標題：</span>
                            <span class="value">
                                <a href="${info.PageUrl || ''}" target="_blank" title="${info.Title || ''}（另開新視窗）">${info.Title || ''}</a>
                                <span class="sr-only">（另開新視窗）</span>
                            </span>
                        </div>

                        <!-- 內容與日期欄位 (預設會完整保留後端加亮的 html 標籤) -->
                        <div class="row">
                            <span class="label">內容：</span>
                            <span class="value">
                                ${info.Content || ''}${dateText}
                            </span>
                        </div>

                    </div>
                </div>
            </li>
        `;
    });

    // 4. 將陣列透過 .join("") 結合成一條大字串，並塞入畫面的容器中
    $("#SearchResult").html(htmlRows.join("")).fadeIn("fast");

    // search information
    var infoText = "共有 <span class=\"text-red\">" + data.Data.TotalPages + "筆</span>搜尋結果";
    $(".resultStatsBox_text").html(infoText);
    itemsCount = data.Data.TotalPages;

    // 💡【核心邏輯判斷】：
    if (isPaginationClick==1) {
        // 全新查詢：重新初始化建立分頁器
        initPaginationWithRealCount(itemsCount);
        $("#pagination-container, #pager").attr("style", "display: block !important;"); 
    } else {
        // 換頁觸發：不重新建立分頁器，只需確保分頁區塊顯示出來即可
        $("#pagination-container, #pager").attr("style", "display: block !important;"); 
    }

    KeywordDisplay(data);

    var $dates = data.Data.Dates;
    var dataDates = [];
    var counts = [];

    var size = $dates.length;
    if ($dates.length > 5)
        size = 5;
    var datecount = 0;
    $.each($dates, function (index, term) {
        dataDates[datecount] = term.Text;
        counts[datecount] = parseInt(term.Df);
        datecount++;
    });
    showChart(counts, dataDates);
}

function errorHandle(XMLHttpRequest, textStatus, errorThrown) {
    $(".resultStatsBox_text,#CategorysContent,#RelatedKeyword,#statChart").html("很抱歉，系統發生錯誤，查無符合的資料");
}

function handleEnter(event) {
    var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
    if (keyCode == 13 && $.trim($("#query").val()) != '') {
        $("#btnSearch").trigger("click");
    }
    return true;
}

function getIconPath(extension) {
    var IconPDF = "/images/page_white_acrobat.png";
    var IconWORD = "/images/icon_doc.gif";
    var IconPPT = "/images/page_white_powerpoint.png";
    var IconExcel = "/images/page_excel.png";
    var IconJPG = "/images/icon_jpg.gif";
    var IconText = "/images/page_white_text.gif";
    var IconZIP = "/images/icon_rar.png";
    var IconRAR = "/images/icon_rar.png";
    var IconUnKnow = "/images/icon_attachment.gif";
    var path = "";
    switch (extension.toLowerCase().trim()) {
        case "pdf":
            path = IconPDF;
            break;
        case "doc":
        case "docx":
        case "odt":
            path = IconWORD;
            break;
        case "ppt":
        case "pptx":
        case "odp":
            path = IconPPT;
            break;
        case "xls":
        case "xlsx":
        case "ods":
            path = IconExcel;
            break;
        case "jpg":
            path = IconJPG;
            break;
        case "txt":
            path = IconText;
            break;
        case "rar":
            path = IconRAR;
            break;
        case "zip":
            path = IconZIP;
            break;
        default:
            path = IconUnKnow;
            break;
    }
    return path;
}

function trimToLength(str, count) {
    if (str.length < count) {
        return str;
    }
    str = str.substring(0, count) + "...";
    return str;
}

function QueryString(name) {
    var AllVars = window.location.search.substring(1);
    var Vars = AllVars.split("&");
    for (i = 0; i < Vars.length; i++) {
        var Var = Vars[i].split("=");
        if (Var[0] == name) return Var[1];
    }
    return "";
}

function openResult(obj, site, lightid) {
    $("#SearchResult,#pager").hide();
    $(".resultStatsBox_text").html("&nbsp;");
    getDetailPage(lightid);
    getRelatedPage(lightid);
}

function getRelatedWord() {
    var postURL = 'ashx/getWiseRelatedWord.ashx';
    $.ajax({
        method: "POST",
        url: postURL,
        dataType: "xml",
        //dataType: "html",
        async: true,
        //data: postData,
        cache: false,
        success: handleWiseRelatedword,
        error: errorHandle
    });
}

function handleWiseRelatedword(xml, status) {
    var $result = $("root", xml);
    //Hot keyword display
    var $hotkeywordArea = $("#hotQueryContent");
    $hotkeywordArea.html("");
    var $hotkeywords = $result.find("hotkeywords").find("term");

    for (i = 0; i < $hotkeywords.length; i++) {
        $("<a href='#' class='hotkeywordItem'>" + $hotkeywords.eq(i).find("text").text() + "</a>").appendTo($hotkeywordArea);
    }

    $(".hotkeywordItem").css("color", "#15396E").css("padding-left", "2pt").css("padding-right", "3pt").css("cursor", "pointer").hover(
        function () {
            $(this).css("backgroundColor", "#CCDBB9");
        },
        function () {
            $(this).css("backgroundColor", "");
        }
    ).click(function () {
        var kword = $(this).html();
        $("#keyword").val(kword);
        $("#start").val(1);
        $("#rangeContent").val("");
        $("#firstTitleTyperangeContent").val("");
        doSearch(1);
    });
}

function getHotKeyword() {
    var postURL = 'ashx/getHotKeyword.ashx';
    $.ajax({
        method: "POST",
        url: postURL,
        dataType: "xml",
        //dataType: "html",
        async: true,
        //data: postData,
        cache: false,
        //success: handleTopResult,
        success: handleHotKeyword,
        error: errorHandle
    });
}

function handleHotKeyword(xml, status) {
    var $result = $("root", xml);

    //Hot keyword display
    var $hotkeywordArea = $("#hotQueryContent");
    $hotkeywordArea.html("");
    var $hotkeywords = $result.find("hotkeywords").find("term");
    for (i = 0; i < $hotkeywords.length; i++) {
        $("<a href='#' class='hotkeywordItem'>" + $hotkeywords.eq(i).find("text").text() + "</a>").appendTo($hotkeywordArea);
    }

    $(".hotkeywordItem").css("color", "#15396E").css("padding-left", "2pt").css("padding-right", "3pt").css("cursor", "pointer").hover(
        function () {
            $(this).css("backgroundColor", "#CCDBB9");
        },
        function () {
            $(this).css("backgroundColor", "");
        }
    ).click(function () {
        var kword = $(this).html();
        $("#keyword").val(kword);
        $("#start").val(1);
        $("#rangeContent").val("");
        $("#firstTitleTyperangeContent").val("");
        doSearch(1); //2014/06/27
    });
}

function trimToLength(str, count) {
    if (str.length < count) {
        return str;
    }
    str = str.substring(0, count) + "...";
    return str;
}

function KeywordDisplay(res) {
    var viewModel = res.Data;
    if (!viewModel) return;
    // --- 1. 智慧關聯提示詞區塊 ---
    var $relatedKeywordArea = $("#hotQueryContent");
    $relatedKeywordArea.html("");

    if (viewModel.RelatedKeywords && viewModel.RelatedKeywords.length > 0) {
        var relatedHtml = viewModel.RelatedKeywords.map(function (term) {
            // 使用原生的樣板字串拼接，效能比用 $.each 一筆筆 append 更好
            return `<a class="keywordItem" href="javascript:void(0);">${term.Text || ''}</a>`;
        }).join(" ");

        $relatedKeywordArea.html(relatedHtml);
    }
    else {
        $relatedKeywordArea.html("<span class='no-keyword'>查無智慧關聯提示詞</span>");
    }

    // --- 2. 熱門關鍵字區塊 ---
    var $keywordArea = $("#hotKeywords");
    $keywordArea.html("");

    if (viewModel.Keywords && viewModel.Keywords.length > 0) {
        var keywordsHtml = viewModel.Keywords.map(function (term) {
            return `<li><a class="keywordItem" href="javascript:void(0);">${term.Text || ''}</a></li>`;
        }).join("");

        $keywordArea.html(keywordsHtml);
    }
    else {
        // 必須精確塞入 HTML 的容器元素 $keywordArea 中
        $keywordArea.html("<span class='no-keyword'>查無熱門關鍵字</span>");
    }

    // --- 3. 點選關鍵字重新搜尋的事件綁定 ---
    $(".keywordItem").off("click").on("click", function () {
        var kword = $(this).text();
        $("#keyword").val(kword);
        $("#start").val(1);
        $("#rangeContent").val("");
        $("#firstTitleTyperangeContent").val("");
        $("#btnSearch").trigger("click");
    });
}
// 動態傳入實體總筆數，並初始化分頁
function initPaginationWithRealCount(realTotalCount) {

    const $container = $("#pagination-container");
    const itemsOnPage = $("#perPageData").val();
    const maxPageVisibleItems = $(window).width() >= 992 ? 10 : 6;

    // 清空舊的分頁 UI（避免重複初始化產生衝突）
    $container.empty();

    // 實例化套件
    const pagination = new Pagination({
        container: $container,
        maxVisibleElements: maxPageVisibleItems,
        showInput: false,
        enhancedMode: false,

        pageClickCallback: function (pageNumber) {
            $("#start").val(pageNumber);
            doSearch(0);
            updateJumpButtonsVisibility();
            updateCustomButtons(pageNumber);
            updateAriaCurrent(pageNumber);
            $container.find("li:not(.page-prev, .page-next, .jump-prev, .jump-next, .page-first, .page-last)").find("[data-page-number]").each(function () {
                const page = $(this).data("page-number");
                $(this).attr("aria-label", `第 ${page} 頁`);
            });
        },
    });

    // 完美注入後端傳來的實體總筆數！
    pagination.make(realTotalCount, itemsOnPage);
}
function addCustomButtons() {
    const $container = $("#pagination-container");
    const $ul = $container.find("ul");

    if (!$ul.find(".page-prev").length) {
        $ul.find("li").eq(0).addClass("page-prev").find("a").attr({
            "aria-label": "上一頁",
            "title": "上一頁"
        }).empty();
    }

    if (!$ul.find(".page-next").length) {
        $ul.find("li").eq(-1).addClass("page-next").find("a").attr({
            "aria-label": "下一頁",
            "title": "下一頁"
        }).empty();
    }

    if (!$ul.find(".page-first").length) {
        $ul.prepend(`
                    <li class="page-first">
                        <a href="#" data-page="first" aria-label="第一頁" title="第一頁"></a>
                    </li>
                `);
    }

    if (!$ul.find(".jump-prev").length) {
        $ul.find(".page-first").after(`
                    <li class="jump-prev">
                        <a href="#" aria-label="往前十頁" title="往前十頁"></a>
                    </li>
                `);
    }

    if (!$ul.find(".page-last").length) {
        $ul.append(`
                    <li class="page-last">
                        <a href="#" data-page="last" aria-label="最末頁" title="最末頁"></a>
                    </li>
                `);
    }

    if (!$ul.find(".jump-next").length) {
        $ul.find(".page-last").before(`
                    <li class="jump-next">
                        <a href="#" aria-label="往後十頁" title="往後十頁"></a>
                    </li>
                `);
    }
}
function updateAriaCurrent(pageNumber) {
    const $container = $("#pagination-container");
    $container.find("li").removeAttr("aria-current");

    $container
        .find("li:not(.page-prev, .page-next, .jump-prev, .jump-next, .page-first, .page-last)")
        .find(`[data-page-number="${pageNumber}"]`)
        .attr("aria-current", "page");
}

// 控制 disabled 狀態
function updateCustomButtons(currentPage) {
    const $container = $("#pagination-container");
    // 總頁數
    const totalPages = Math.ceil(itemsCount / itemsOnPage);

    const $first = $container.find(".page-first");
    const $last = $container.find(".page-last");
    const $prev = $container.find(".page-prev");
    const $next = $container.find(".page-next");
    const $prev10 = $container.find(".jump-prev");
    const $next10 = $container.find(".jump-next");


    if (currentPage === 1) {
        $first.addClass("disabled").find("a").attr({
            "aria-disabled": "true",
            "tabindex": "-1"
        });
        $prev.addClass("disabled").find("a").attr({
            "aria-disabled": "true",
            "tabindex": "-1"
        });
    } else {
        $first.removeClass("disabled")
            .find("a").attr("tabindex", "0").removeAttr("aria-disabled");
        $prev.removeClass("disabled")
            .find("a").attr("tabindex", "0").removeAttr("aria-disabled");
    }

    if (currentPage === totalPages) {
        $last.addClass("disabled").find("a").attr({
            "aria-disabled": "true",
            "tabindex": "-1"
        });
        $next.addClass("disabled").find("a").attr({
            "aria-disabled": "true",
            "tabindex": "-1"
        });
    } else {
        $last.removeClass("disabled")
            .find("a").attr("tabindex", "0").removeAttr("aria-disabled");
        $next.removeClass("disabled")
            .find("a").attr("tabindex", "0").removeAttr("aria-disabled");
    }

    if (currentPage <= 10) {
        $prev10.addClass("disabled").find("a").attr({
            "aria-disabled": "true",
            "tabindex": "-1"
        });
    } else {
        $prev10.removeClass("disabled")
            .find("a").attr("tabindex", "0").removeAttr("aria-disabled");
    }

    if (currentPage > totalPages - 10) {
        $next10.addClass("disabled").find("a").attr({
            "aria-disabled": "true",
            "tabindex": "-1"
        });
    } else {
        $next10.removeClass("disabled")
            .find("a").attr("tabindex", "0").removeAttr("aria-disabled");
    }
}

// 手機版 - 往前/往後十頁按鈕隱藏
function updateJumpButtonsVisibility() {
    const $container = $("#pagination-container");
    const isMobile = $(window).width() < 992;

    const $prev10 = $container.find(".jump-prev");
    const $next10 = $container.find(".jump-next");

    if (isMobile) {
        $prev10.attr("aria-hidden", "true");
        $next10.attr("aria-hidden", "true");

        $prev10.find("a").attr("tabindex", "-1");
        $next10.find("a").attr("tabindex", "-1");
    } else {
        $prev10.removeAttr("aria-hidden");
        $next10.removeAttr("aria-hidden");
    }
}

$(window).on("resize", function () {
    updateJumpButtonsVisibility();
});

function lowerStat(rawDate) {
    var length = parseInt(rawDate.length);

    switch (length) {
        case 4:
            //year
            $("input[name='statDate']")[1].checked = true;
            $("#start").val(1);
            //myPagination.goToPage(0);
            //doSearch(rawDate + "/01/01", rawDate + "/12/31");
            setDate(rawDate + "/01/01", rawDate + "/12/31");
            //doStatistics(rawDate + "/01/01", rawDate + "/12/31");
            break;
        case 7:
            //month
            var year = rawDate.split("/")[0];
            var month = rawDate.split("/")[1];
            var endDay;
            if (month == "01" || month == "03" || month == "05" || month == "07" || month == "08" || month == "10" || month == "12") {
                endDay = "31";
            }
            else if (month == "04" || month == "06" || month == "09" || month == "11") {
                endDay = "30";
            }
            else if (month == "02") {
                if (parseInt(year) % 4 == 0)
                    endDay = "29";
                else
                    endDay = "28";
            }

            //$("input[name='statDate']")[2].checked = true;
            $("#start").val(1);
            //myPagination.goToPage(0);
            setDate(year + "/" + month + "/01", year + "/" + month + "/" + endDay);
            break;
        case 10:
            //day
            $("#start").val(1);
            //myPagination.goToPage(0);
            //doSearch(rawDate, rawDate);
            setDate(rawDate, rawDate);
            //doStatistics(rawDate, rawDate);
            break;
    }
}
function setDate(datea, dateb) {
    $('#datea').val(datea);
    $('#dateb').val(dateb);
    $("#start").val(1);
    const $container = $("#pagination-container");
    $container.empty();
    doSearch(1);
    //initPagination();
}
function showChart(xData, yData) {
    const rowHeight = 28;
    $('#timelineChart').highcharts({
        chart: {
            type: 'bar',
            backgroundColor: 'transparent',
            height: xData.length * rowHeight + 80,
            marginTop: 0,
            spacingLeft: 0,
            spacingRight: 0,
        },
        title: {
            text: ''
        },
        subtitle: {
            text: ''
        },
        credits: { enabled: false },
        xAxis: {
            categories: yData,
            tickWidth: 1,
            tickColor: '#d5deec',
            lineWidth: 0,
            labels: {
                style: {
                    fontSize: '10px',
                    color: '#2a2a2a'
                }
            },
        },
        yAxis: {
            allowDecimals: false,
            min: 0,
            title: {
                text: '篇數',
                style: {
                    fontSize: '16px',
                    fontWeight: '400',
                    color: '#1b61c0'
                },
                align: 'high'
            },
            min: 0,
            max: 40,
            tickInterval: 10,
            labels: {
                style: {
                    fontSize: '14px',
                    color: '#2a2a2a'
                }
            },
            gridLineColor: '#d5deec',
        },
        legend: {
            enabled: false
        },
        series: [{
            name: '次數',
            data: xData,
            color: '#1b61c0'
        }],
        plotOptions: {
            bar: {
                dataLabels: { enabled: false },
                borderRadius: 0,
                borderWidth: 0,
                pointWidth: 14,
                cursor: 'pointer',
                point: {
                    events: {
                        click: function () {
                            lowerStat(this.category);
                        }
                    }
                }
            },
        },
        tooltip: {
            useHTML: true,
            formatter: function () {
                const year = this.series.xAxis.categories[this.point.x];
                return `
                        <div style="line-height:1.2;">
                            <div style="font-size:12px;">${year}</div>
                            <div style="font-size:14px;">
                                <span style="font-weight:400;color:#1b61c0;">次數：</span>
                                <span style="font-weight:700;">${this.y}</span>
                        </div
                    `;
            },
            shared: false,
            headerFormat: '',
            pointFormat: ''
        },
        accessibility: {
            enabled: false
        }
    });
}
// ==========pagination頁碼控制==========
const totalItems = $(".pagination").data("total");  // 總資料數
let currentPage = 1;
const visiblePages = 3;  // 中間最多顯示幾個頁碼

function renderPagination(total, current, visible, backendTotalPages) {
    $("#totalItems").text(backendTotalPages);

    const pagination = $(".pagination ul");
    pagination.empty();

    // 上一頁
    if (current > 1) {
        pagination.append(`
                <li class="prev">
                    <a href="#" data-page="${current - 1}" aria-label="上一頁">
                        <img src="./images/icon/icon_direction_left.png" alt="">
                    </a>
                </li>
            `);
    } else {
        pagination.append(`
                <li class="prev disabled">
                    <a href="#" aria-label="上一頁不可用" aria-disabled="true" tabindex="-1">
                        <img src="./images/icon/icon_direction_left_gray.png" alt="">
                    </a>
                </li>
            `);
    }

    // 總是顯示第一頁
    if (current === 1) {
        pagination.append(`<li class="active"><a href="#" aria-label=第1頁" aria-current="page">1</a></li>`);
    } else {
        pagination.append(`<li><a href="#" data-page="1" aria-label="第1頁">1</a></li>`);
    }

    // 中間動態頁碼
    let start, end;

    if (total <= visible + 2) {
        // // 總頁數少時，頁碼全部顯示
        start = 2;
        end = total - 1;
    } else {
        start = current - Math.floor(visible / 2);
        end = current + Math.floor(visible / 2);

        if (start < 2) {
            start = 2;
            end = start + visible - 1;
        }

        if (end > total - 1) {
            end = total - 1;
            start = end - visible + 1;
            if (start < 2) start = 2;
        }
    }

    // 左邊省略號
    if (start > 2) {
        pagination.append(`
                <li class="ellipsis">
                    <img src="./images/icon/icon_more_black.png" alt="">
                </li>
            `);
    }

    // 中間頁碼
    for (let i = start; i <= end; i++) {
        if (i === current) {
            pagination.append(`<li class="active"><a href="#" aria-label="第${i}頁" aria-current="page">${i}</a></li>`);
        } else {
            pagination.append(`<li><a href="#" data-page="${i}" aria-label="第${i}頁">${i}</a></li>`);
        }
    }

    // 右邊省略號
    if (end < total - 1) {
        pagination.append(`
                <li class="ellipsis">
                    <img src="./images/icon/icon_more_black.png" alt="">
                </li>
            `);
    }

    // 總是顯示最後一頁
    if (total > 1) {
        if (current === total) {
            pagination.append(`<li class="active"><a href="#" aria-label="第${total}頁" aria-current="page">${total}</a></li>`);
        } else {
            pagination.append(`<li><a href="#" data-page="${total}" aria-label="第${total}頁">${total}</a></li>`);
        }
    }

    // 下一頁
    if (current < total) {
        pagination.append(`
                <li class="next">
                    <a href="#" data-page="${current + 1}" aria-label="下一頁">
                        <img src="./images/icon/icon_direction_right.png" alt="">
                    </a>
                </li>
            `);
    } else {
        pagination.append(`
                <li class="next disabled">
                    <a href="#" aria-label="下一頁不可用" aria-disabled="true" tabindex="-1">
                        <img src="./images/icon/icon_direction_right_gray.png" alt="">
                    </a>
                </li>
            `);
    }
}
//$.views.helpers({
//    getSerialNumber: function (index, currentPage) {
//        // 動態取得畫面上的每頁幾筆，若拿不到則預設為 10
//        const perPage = parseInt($("#perPageData").val(), 10) || 10;
//        const page = parseInt(currentPage, 10) || 1;

//        // 數學公式：(目前頁碼 - 1) * 每頁筆數 + 目前索引 + 1
//        return ((page - 1) * perPage) + index + 1;
//    }
//});

