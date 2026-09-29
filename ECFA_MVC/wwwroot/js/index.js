$(document).ready(function () {

    // ==========輪播slick套件設定==========
    $('.banner-items').on('init afterChange', function (event, slick) {

        setTimeout(function () {

            $(slick.$slides).removeAttr('tabindex');

        }, 0);

        $('.slick-slide[aria-hidden="true"] a')
            .attr('tabindex', '-1');

        $('.slick-slide[aria-hidden="false"] a')
            .removeAttr('tabindex');

    });


    $('.banner-items').slick({
        infinite: true,
        accessibility: true,
        speed: 3000,
        autoplay: true,
        autoplaySpeed: 5000,
        dots: false,
        prevArrow: '<button type="button" class="slick-prev">上一則</button>',
        nextArrow: '<button type="button" class="slick-next">下一則</button>',
        vertical: false,
    });

    function formatNumber(num) {
        return num;
    }

    const totalSlides = $('.banner-items').slick('getSlick').slideCount;

    $('.banner-pagination .total').text(formatNumber(totalSlides));

    $('.banner-pagination .current').text(formatNumber(1));

    $('.banner-items').on('afterChange', function (event, slick, currentSlide) {
        $('.banner-pagination .current').text(formatNumber(currentSlide + 1));
    });

    // Banner Slider - 播放/暫停按鈕
    $('.banner-playpause').on('click', function (e) {
        const button = $(this);
        bannerToggleBtn(button);
    });
    $('.banner-playpause').on('keydown', function (e) {
        const button = $(this);
        const key = e.key || e.keyCode;

        switch (key) {
            case " ":
            case 32:
                e.preventDefault();
                break;

            case "Enter":
            case 13:
                e.preventDefault();
                bannerToggleBtn(button);
                break;

            case "Tab":
            case 9:
                $(".banner-items").slick('slickGoTo', 0);
                break;
        }
    });
    $('.banner-playpause').on('keyup', function (e) {
        const button = $(this);
        const key = e.key || e.keyCode;

        if (key === " " || key === 32) {
            e.preventDefault();
            bannerToggleBtn(button);
        }
    })

    function bannerToggleBtn(button) {
        const isPaused = button.toggleClass('active').hasClass('active');

        if (isPaused) {
            button.attr({
                'aria-label': '開啟自動輪播',
                'aria-pressed': 'true'
            })
            $('.banner-items').slick('slickPause');
        } else {
            button.attr({
                'aria-label': '暫停自動輪播',
                'aria-pressed': 'false'
            })
            $('.banner-items').slick('slickPlay');
        }
    }

});

