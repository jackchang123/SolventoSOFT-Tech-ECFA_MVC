// ========== body 滾動控制 ==========
// 鎖定 body 滾動
function lockBodyScroll() {
    const hasScrollbar = window.innerWidth > document.documentElement.clientWidth;

    if (hasScrollbar) {
        // scrollbar 寬度
        const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
        $("body").addClass("scroll-lock");
        $("body").css({
            "--scrollbar-width": `${scrollbarWidth}px`
        });
    } else {
        $("body").addClass("scroll-lock");
    }
}
// 解鎖 body 滾動
function unlockBodyScroll() {
    $("body").removeClass("scroll-lock");

    // 等動畫結束後再移除變數
    setTimeout(() => {
        $("body").css("--scrollbar-width", "");
    }, 300);
}


$(document).ready(function () {

    // ==========用 JS 偵測 zoom 倍率==========
    function checkZoom() {
        const ratio = Math.round((window.outerWidth / window.innerWidth) * 100) / 100;

        if (ratio >= 2) {
            document.body.classList.add("zoom-200");
        } else {
            document.body.classList.remove("zoom-200");
        }
    }

    // 初次執行
    checkZoom();

    // 監聽縮放、視窗大小改變
    window.addEventListener("resize", checkZoom);


    // ========== 網站字體變更 font-size ==========
    const $fontSIzeToggle = $('.fontsize-toggle');
    const $fontSizeList = $('#fontSizeDropdownList');

    function toggleFontSizeDropdownList(button) {
        button.toggleClass('active');
        const isActive = button.hasClass('active');
        if (isActive) {
            button.attr('aria-expanded', "true");
            $fontSizeList.addClass('active');

        } else {
            button.attr('aria-expanded', "false");
            $fontSizeList.removeClass('active');
        }
    }

    function closeFontSizeDropdownList() {
        $fontSIzeToggle.removeClass('active')
            .attr('aria-expanded', "false");
        $fontSizeList.removeClass('active');
    }

    $fontSIzeToggle.on('click', function (e) {
        e.stopPropagation();
        closeSearchPanel();
        toggleFontSizeDropdownList($(this));
    });

    $fontSizeList.on('click', function (e) {
        e.stopPropagation();
    });

    // 焦點離開整個元件（toggle+dropdown）才關閉
    $('.fontsize-control').on('focusout', function (e) {
        const next = e.relatedTarget;

        if (!$(next).closest('.fontsize-control').length) {
            closeFontSizeDropdownList();
        }
    });

    $(".font-small").on("click", function (e) {
        fontSizeChange("small");
        closeFontSizeDropdownList();
        $fontSIzeToggle.focus();

    });
    $(".font-medium").on("click", function (e) {
        fontSizeChange("medium");
        closeFontSizeDropdownList();
        $fontSIzeToggle.focus();
    });
    $(".font-large").on("click", function (e) {
        fontSizeChange("large");
        closeFontSizeDropdownList();
        $fontSIzeToggle.focus();
    });

    function fontSizeChange(size) {
        $(".font-btn")
            .removeClass("active")
            .attr("aria-pressed", "false");
        $(".font-" + size)
            .addClass("active")
            .attr("aria-pressed", "true");
        switch (size) {
            case 'small':
                $("html").css("fontSize", "14px");
                break;
            case 'medium':
                $("html").css("fontSize", "16px");
                break;
            case 'large':
                $("html").css("fontSize", "18px");
                break;
        }
    }


    // ========== 導覽列全文搜索面板顯示控制 ==========
    const $allSearchBtn = $(".button-allsearch");
    const $allSearchPanel = $("#allSearchPanel");

    function openSearchPanel() {
        $allSearchPanel
            .stop(true, true)
            .removeAttr("hidden")
            .attr("aria-hidden", "false")
            .slideDown(200);

        $allSearchBtn.attr("aria-expanded", "true");
    }

    function closeSearchPanel() {
        $allSearchPanel
            .stop(true, true)
            .attr("aria-hidden", "true")
            .slideUp(200, function () {
                $(this).attr("hidden", "")
            });

        $allSearchBtn.attr("aria-expanded", "false");
    }

    function toggleSearchPanel() {
        const isOpen = $allSearchBtn.attr("aria-expanded") === "true";
        isOpen ? closeSearchPanel() : openSearchPanel();
    }

    $allSearchBtn.on('click', function (e) {
        e.stopPropagation();
        closeFontSizeDropdownList();
        toggleSearchPanel();
    });

    $allSearchPanel.on('click', function (e) {
        e.stopPropagation();
    });

    // 焦點離開整個元件（toggle+panel）才關閉
    $('.all-search').on('focusout', function (e) {
        const next = e.relatedTarget;

        if (!$(next).closest('.all-search').length) {
            closeSearchPanel();
        }
    });

    //alt+s 搜尋會自動打開
    window.addEventListener("keydown", function (event) {
        if (event.altKey && event.code === 'KeyS') {
            openSearchPanel();
            $allSearchPanel.find(".search-input").focus();
        }
    });

    // ESC 關閉搜尋面板
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape' && $allSearchBtn.attr('aria-expanded') === 'true') {
            closeSearchPanel();
            $allSearchBtn.focus();
        }
    });

    // 統一 document click 關閉全部
    $(document).on('click', function () {
        closeFontSizeDropdownList();
        closeSearchPanel();
    });


    // ========== 桌機nav選單套件設定 ==========
    if ($('#main-menu').length) {
        $("#main-menu").superfish({
            delay: 300,
            animation: { opacity: 'show', height: 'show' },
            animationOut: { opacity: 'hide', height: 'hide' },
            onBeforeShow: function () {
                $(this).prev('a').attr('aria-expanded', 'true');
            },
            onHide: function () {
                $(this).prev('a').attr('aria-expanded', 'false');
            }
        });

        $('#main-menu li:has(ul) > a').on('click', function (e) {
            e.preventDefault();
        });
    }


    // ========== 手機版nav控制 ==========
    const $navBtn = $(".mobile-nav-btn");
    const $overlay = $(".mobile-overlay");
    const $mobileMenu = $(".mobile-menu");
    const $menuBtn = $mobileMenu.find(".menu-button");
    const $menuLink = $mobileMenu.find(".menu-link");

    const menuFocusable = `
        a[href],
        button:not([disabled]),
        input:not([disabled]),
        [tabindex]:not([tabindex="-1"])
    `;

    $navBtn.on('click', function (e) {
        const isOpen = $(this).toggleClass('active').hasClass('active');

        $(this).attr({
            "aria-expanded": String(isOpen),
            "aria-label": isOpen ? "關閉手機版主選單" : "開啟手機版主選單"
        });

        $overlay.attr("aria-hidden", String(!isOpen)).toggleClass("active", isOpen);

        $mobileMenu.toggleClass("active", isOpen);

        isOpen ? lockBodyScroll() : unlockBodyScroll();
    });

    $menuLink.on("click", function () {
        const $link = $(this);

        // 移除所有 link active
        $menuLink.removeClass("active");

        // 加入目前點擊 active
        $link.addClass("active");
    });

    $menuBtn.on('click', function (e) {
        const $btn = $(this);
        const $currentItem = $btn.closest("li");
        const $submenu = $currentItem.children("ul");
        const isOpen = $btn.hasClass("active");

        const $siblings = $currentItem.siblings(".has-submenu");

        $siblings.each(function () {
            const $item = $(this);
            const $otherBtn = $item.children(".menu-button");
            const $otherSubmenu = $item.children(".menu-submenu");

            $otherBtn
                .removeClass("active")
                .attr("aria-expanded", "false");

            $otherSubmenu
                .stop(true, false)
                .slideUp(300)
                .attr("aria-hidden", "true");

            // 遞迴關閉更深層 submenu
            $item.find(".menu-button")
                .removeClass("active")
                .attr("aria-expanded", "false");

            $item.find(".menu-submenu")
                .not($otherSubmenu)
                .stop(true, false)
                .slideUp(300)
                .attr("aria-hidden", "true");
        });

        if (isOpen) {
            $btn.removeClass("active").attr({ "aria-expanded": "false" });
            $submenu.stop(true, false).slideUp(300).attr("aria-hidden", "true");
        } else {
            $btn.addClass("active").attr("aria-expanded", "true");
            $submenu.attr("aria-hidden", "false").stop(true, false).slideDown(300);
        }

    });

    //鍵盤控制tab離開手機nav選單最後一個按鈕後關閉選單
    function getLastFocusable() {
        return $('.mobile-menu').find(menuFocusable).filter(':visible').last();
    }
    $(document).on('keydown', '.mobile-menu', function (e) {
        const key = e.key || e.keyCode;

        if (key !== "Tab" && key !== 9) return;

        const $last = getLastFocusable();

        // 判斷目前 focus 是否在最後一個元素
        if (!e.shiftKey && $(document.activeElement).is($last)) {
            mobileNavClose();
        }
    });

    //手機nav關閉
    function mobileNavClose() {
        $navBtn
            .removeClass('active')
            .attr({
                "aria-expanded": "false",
                "aria-label": "開啟手機版主選單"
            });

        $menuBtn.each(function () {
            const $btn = $(this);
            const $submenu = $btn.closest("li").children("ul");

            $btn.removeClass("active").attr("aria-expanded", "false");
            $submenu.stop(true, true).slideUp(0).attr("aria-hidden", "true");
        });

        $overlay.removeClass("active").attr("aria-hidden", "true");

        $mobileMenu.removeClass('active');

        unlockBodyScroll();
    }

    //偵測畫面寬度，超過1199就解除body鎖定
    $(window).resize(function () {
        const isOpen = $navBtn.hasClass('active');

        if ($(window).width() > 1199) {
            if (isOpen) {
                $overlay.removeClass("active").attr("aria-hidden", "true");
                unlockBodyScroll();
            }
        } else {
            if (isOpen) {
                $overlay.attr("aria-hidden", "false").addClass("active");
                lockBodyScroll();
            }
        }
    });


    // ========== go to top button ==========
    // 按下GoTop按鈕時的事件
    $('.btn-gotop').click(function () {
        $("html,body").animate({ scrollTop: 0 }, 'slow');   // 返回到最頂上
        return false;
    });
    //  偵測捲軸滑動時，往下滑超過300px就讓GoTop按鈕出現
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $(".scroll-top-btn").addClass('show');
        } else {
            $(".scroll-top-btn").removeClass('show');
        }
    });


    // ========== 側邊欄選單控制 ==========
    const $sidebar = $(".sidebar-menu");

    $sidebar.on("click", ".menu-toggle", function (e) {
        e.preventDefault();

        const $btn = $(this);
        const $li = $btn.closest(".menu-item");

        const isOpen = $btn.hasClass("active");

        // 同層只開一個
        $li.siblings(".has-submenu").each(function () {
            closeMenu($(this));
        });

        // 切換自己
        if (isOpen) {
            closeMenu($li);
        } else {
            openMenu($li);
        }
    });

    initMenuState();

    function initMenuState() {
        $sidebar.find(".menu-item.has-submenu.active").each(function () {
            const $li = $(this);
            const $btn = $li.children(".menu-toggle");
            const $submenu = $li.children(".submenu");

            $submenu.stop(true, true).removeAttr("style");

            $btn.attr("aria-expanded", "true");
            $submenu.show().attr("aria-hidden", "false");
        })
    }

    // 開啟選單
    function openMenu($li) {
        const $btn = $li.children(".menu-toggle");
        const $submenu = $li.children(".submenu");

        $btn.addClass("active").attr("aria-expanded", "true");
        $li.addClass("active");

        $submenu
            .stop(true, true)
            .slideDown(200)
            .attr("aria-hidden", "false");
    }

    // 關閉選單（含子層）
    function closeMenu($li) {
        const $btn = $li.children(".menu-toggle");
        const $submenu = $li.children(".submenu");

        const $focused = $li.find(":focus");

        if ($focused.length) {
            // 移到目前 toggle（或上一個安全點）
            $btn.focus();
        }

        $btn.removeClass("active").attr("aria-expanded", "false");
        $li.removeClass("active");

        $submenu
            .stop(true, true)
            .slideUp(200)
            .attr("aria-hidden", "true");
    }


    // ========== 返回前一頁 ==========
    $('#backToPrev').click(function (e) {
        history.back();
    });


    // ========== 早收清單查詢 - 重設條件按鈕 ==========
    if ($("#resetBtn").length) {
        $("#resetBtn").on('click', function (e) {
            e.preventDefault();

            const $searchBox = $(this).closest(".search-box");

            // 重設 select
            $searchBox.find("select").each(function () {
                this.selectedIndex = 0;
            });

            // 清空 input
            $searchBox.find('input[type="text"]').val("");

            // 清除錯誤狀態（如有）
            $searchBox.find(".error-prompt").hide();
            $searchBox.find(".form-control").removeClass("error");
        })
    }


    // ========== 日期選擇 套件設定 ==========
    $(".date-range-group").each(function () {
        const $group = $(this);
        const $start = $group.find(".start-date");
        const $end = $group.find(".end-date");

        const startPicker = flatpickr($start[0], {
            dateFormat: "Y-m-d",
            locale: "zh_tw",
            disableMobile: true,
            allowInput: true,
            onChange: function (selectedDates) {
                if (selectedDates.length) {
                    endPicker.set("minDate", selectedDates[0]);
                } else {
                    endPicker.set("minDate", null);
                }
            }
        });

        const endPicker = flatpickr($end[0], {
            dateFormat: "Y-m-d",
            locale: "zh_tw",
            disableMobile: true,
            allowInput: true,
            onChange: function (selectedDates) {
                if (selectedDates.length) {
                    startPicker.set("maxDate", selectedDates[0]);
                } else {
                    startPicker.set("maxDate", null);
                }
            }
        });

        $group.data("startPicker", startPicker);
        $group.data("endPicker", endPicker);
    });


    // ========== Magnific Popup 套件設定 ==========
    if ($('.popup-gallery').length > 0) {
        $('.popup-gallery').magnificPopup({
            delegate: 'a',
            type: 'image',
            tLoading: 'Loading image #%curr%...',
            mainClass: 'mfp-img-mobile',
            gallery: {
                enabled: true,
                navigateByImgClick: true,
                preload: [0, 1]
            },
            image: {
                tError: 'The image #%curr% could not be loaded.',
                verticalFit: true,
                titleSrc: 'title'
            },
            callbacks: {
                open: function () {
                    lockBodyScroll();
                    $('html').css('overflow', 'visible');
                },
                close: function () {
                    unlockBodyScroll();
                    $('html').css('overflow', '');
                }
            }
        });
    }
    

    // ========== pagination頁碼控制 ==========
    function initPagination() {
        const $container = $("#pagination-container");

        const itemsCount = 300;     // 總筆數（切版假資料）
        const itemsOnPage = 10;     // 每頁筆數

        const maxPageVisibleItems = $(window).width() >= 992 ? 10 : 6;

        const pagination = new Pagination({
            container: $container,
            maxVisibleElements: maxPageVisibleItems,
            showInput: false,
            enhancedMode: false,

            // 每次點擊頁碼後觸發
            pageClickCallback: function (pageNumber) {
                addCustomButtons();
                updateJumpButtonsVisibility();
                updateCustomButtons(pageNumber);
                updateAriaCurrent(pageNumber);
                $container.find("li:not(.page-prev, .page-next, .jump-prev, .jump-next, .page-first, .page-last)").find("[data-page-number]").each(function () {
                    const page = $(this).data("page-number");
                    $(this).attr("aria-label", `第 ${page} 頁`);
                });
            },

            callPageClickCallbackOnInit: true
        });

        // 建立 pagination，根據「總筆數 / 每頁筆數」計算頁數並 render UI
        pagination.make(itemsCount, itemsOnPage);

        // 自訂 First / Last / jump-prev / jump-next / prev(class) / next(class)
        function addCustomButtons() {
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

        // active 頁面
        function updateAriaCurrent(pageNumber) {
            $container.find("li").removeAttr("aria-current");

            $container
                .find("li:not(.page-prev, .page-next, .jump-prev, .jump-next, .page-first, .page-last)")
                .find(`[data-page-number="${pageNumber}"]`)
                .attr("aria-current", "page");
        }

        // 控制 disabled 狀態
        function updateCustomButtons(currentPage) {
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

        // click 事件綁定
        $container.off("click", ".page-first a").on("click", ".page-first a", function (e) {
            e.preventDefault();
            pagination.goToPage(1);
        });

        $container.off("click", ".page-last a").on("click", ".page-last a", function (e) {
            e.preventDefault();
            const totalPages = Math.ceil(itemsCount / itemsOnPage);
            pagination.goToPage(totalPages);
        });

        $container.off("click", ".jump-prev a").on("click", ".jump-prev a", function (e) {
            e.preventDefault();
            const current = pagination.getCurrentPage();
            const target = Math.max(1, current - 10);
            pagination.goToPage(target);
        });

        $container.off("click", ".jump-next a").on("click", ".jump-next a", function (e) {
            e.preventDefault();

            const totalPages = Math.ceil(itemsCount / itemsOnPage);
            const current = pagination.getCurrentPage();
            const target = Math.min(totalPages, current + 10);

            pagination.goToPage(target);
        });
    }

    // 初始化
    if ($('#pagination-container').length && typeof Pagination !== 'undefined') {
        initPagination();
    }



    // ========== 全文檢索頁 - 進階搜尋面板顯示控制 ==========
    const $toggle = $(".advanced-search-toggle");
    const $panel = $("#advancedSearchPanel");

    $toggle.on('click', function () {
        const isExpanded = $(this).attr("aria-expanded") === "true";

        if (isExpanded) {
            // 收合
            $panel
                .slideUp(200, function () {
                    $(this)
                        .attr("hidden", true)
                        .attr("aria-hidden", "true");
                });

            $(this).attr("aria-expanded", "false");

        } else {
            // 展開
            $panel
                .removeAttr("hidden")
                .hide()
                .slideDown(200)
                .attr("aria-hidden", "false");

            $(this).attr("aria-expanded", "true");
        }
    });

    // ========== 全文檢索頁 - 清除按鈕 ==========
    // 預設狀態
    const defaultState = {
        querymode: 'exact',
        feature_options_1: true,
        feature_options_2: false,
        perPageData: '5'
    };

    $(".search-container .btn-clear").on('click', function () {
        const $container = $(this).closest('.search-container');

        $container.find('.keyword-input').val('');

        $container.find('input[name="querymode"]').prop('checked', false);
        $container
            .find(`input[name="querymode"][value="${defaultState.querymode}"]`)
            .prop('checked', true);

        $container.find('input[name="feature_options_1"]')
            .prop('checked', defaultState.feature_options_1);
        $container.find('input[name="feature_options_2"]')
            .prop('checked', defaultState.feature_options_2);

        $container.find('input[name^="file_format_"]')
            .prop('checked', false);

        $container.find('#perPageData')
            .val(defaultState.perPageData)
            .trigger('change');

        $container.find('.start-date, .end-date').each(function () {
            if (this._flatpickr) {
                this._flatpickr.clear();
            } else {
                $(this).val('');
            }
        });

        // 清除錯誤狀態（如有）
        $container.find('.error-prompt').hide();
        $container.find(".form-control.error").removeClass("error");

    })

    // ========== 全文檢索頁 - 側邊輔助區 accordion 手風琴顯示控制 ==========
    const $accordionBtn = $('.accordion-button');

    $accordionBtn.on('click', function () {
        const $btn = $(this);
        const $collapse = $btn.closest(".accordion-item").find(".accordion-collapse");

        const isOpen = $btn.hasClass("active");

        if (isOpen) {
            // 收合
            $collapse.stop(true, true).slideUp(200, function () {
                $collapse.attr('hidden', '').attr('aria-hidden', 'true');
            });
            $btn
                .attr("aria-expanded", "false")
                .removeClass("active");
        } else {
            // 展開
            $collapse
                .removeAttr('hidden')
                .attr('aria-hidden', 'false')
                .hide()
                .slideDown(200);

            $btn
                .attr('aria-expanded', 'true')
                .addClass("active");
        }
    });

    function mobileAccordionClose() {
        if ($(window).width() < 1200) {

            $('.accordion-button').each(function () {
                const $btn = $(this);
                const $collapse = $btn.closest(".accordion-item").find(".accordion-collapse");

                if ($btn.hasClass('active')) {

                    $collapse.stop(true, true).slideUp(200, function () {
                        $collapse.attr('hidden', '').attr('aria-hidden', 'true');
                    });

                    $btn.attr("aria-expanded", "false").removeClass('active');
                }
            });
        }
    }

    mobileAccordionClose();

    $(window).on('load reload resize', function () {
        mobileAccordionClose()
    });


    // ========== 認識ECFA-配套措施 - panel accordion 手風琴顯示控制 ==========
    // 點擊整條 .panel-accordion-toggle(button) 展開/收合 .panel-accordion-collapse，可同時展開多個
    $('.panel-accordion-toggle').on('click', function () {
        const $btn = $(this);
        const $collapse = $btn.closest(".panel-accordion-item").find(".panel-accordion-collapse");

        const isOpen = $btn.attr("aria-expanded") === "true";

        if (isOpen) {
            // 收合
            $collapse.stop(true, true).slideUp(200, function () {
                $collapse.attr('hidden', '').attr('aria-hidden', 'true');
            });
            $btn.attr("aria-expanded", "false").removeClass("active");
        } else {
            // 展開
            $collapse
                .removeAttr('hidden')
                .attr('aria-hidden', 'false')
                .hide()
                .slideDown(200);
            $btn.attr("aria-expanded", "true").addClass("active");
        }
    });
});
