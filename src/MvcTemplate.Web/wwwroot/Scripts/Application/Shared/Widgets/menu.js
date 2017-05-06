Menu = {
    init: function () {
        $('.menu-search > input').on('keyup', function () {
            var search = this.value.toLowerCase();
            var menus = $('.menu li');

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
        });

        $('.menu').on('mouseleave', '.menu > ul', function () {
            if ($('.menu').width() < 100) {
                var submenu = $('.menu li.open');
                submenu.children('ul').fadeOut();
                submenu.toggleClass('open');
            }
        });

        $('.menu').on('click', '.submenu > a', function (e) {
            e.preventDefault();
            var action = $(this);
            var submenu = action.parent();
            var openSiblings = submenu.siblings('.open');

            if ($('.menu').width() > 100) {
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

        $(window).on('resize', function () {
            if ($('.menu').width() < 100) {
                $('.menu .open').removeClass('open').children('ul').hide();
                $('.menu-search > input').val('').keyup();
            }
        });

        if ($('.menu').width() < 100) {
            $('.menu li.open').removeClass('open');
        }
    }
};
