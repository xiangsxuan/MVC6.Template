Navigation = {
    init: function () {
        $('.menu-search > input').on('input', function () {
            filter(this);
        });

        $('.navigation').on('mouseleave', '.navigation > ul', function () {
            if ($('.navigation').width() < 100) {
                var submenu = $('.navigation li.open');
                submenu.children('ul').fadeOut();
                submenu.toggleClass('open');
            }
        });

        $('.navigation').on('click', '.submenu > a', function (e) {
            e.preventDefault();
            var action = $(this);
            var submenu = action.parent();
            var openSiblings = submenu.siblings('.open');

            if ($('.navigation').width() > 100) {
                openSiblings.toggleClass('changing');
                openSiblings.children('ul').slideUp(function () {
                    openSiblings.removeClass('changing open');
                });

                submenu.toggleClass('changing');
                action.next('ul').slideToggle(function () {
                    submenu.toggleClass('changing open');
                });
            } else {
                openSiblings.children('ul').fadeOut(function () {
                    openSiblings.removeClass('open');
                });

                action.next('ul').fadeToggle(function () {
                    submenu.toggleClass('open');
                });
            }
        });

        function filter(input) {
            var search = input.value.toLowerCase();
            var menus = $('.navigation li');

            for (var i = 0; i < menus.length; i++) {
                var menu = $(menus[i]);
                if (menu.text().toLowerCase().indexOf(search) >= 0) {
                    if (menu.hasClass('submenu')) {
                        if (menu.find('li:not(.submenu)').text().toLowerCase().indexOf(search) >= 0) {
                            menu.show(500);
                        } else {
                            menu.hide(500);
                        }
                    } else {
                        menu.show(500);
                    }
                } else {
                    menu.hide(500);
                }
            }
        }

        $(window).on('resize', function () {
            if ($('.navigation').width() < 100) {
                $('.navigation .open').removeClass('open').children('ul').hide();
                filter($('.menu-search > input').val('')[0]);
            }
        });

        if ($('.navigation').width() < 100) {
            $('.navigation li.open').removeClass('open');
        }
    }
};
