Alerts = {
    init: function () {
        $('.alerts').on('click', '.close', function () {
            Alerts.close(this.parentNode);
        });
        $('.alerts .alert').each(function () {
            Alerts.bind(this);
        });
    },

    show: function (alerts) {
        alerts = [].concat(alerts);
        var container = $('.alerts');
        for (var i = 0; i < alerts.length; i++) {
            var alert = document.createElement('div');
            var close = document.createElement('span');
            var message = document.createElement('span');

            alert.setAttribute('data-timeout', alerts[i].timeout || '0');
            alert.className = 'alert alert-' + getType(alerts[i].type);
            message.innerText = alerts[i].message || '';
            close.innerHTML = '&#x00D7;';
            close.className = 'close';

            container.append(alert);
            alert.append(message);
            alert.append(close);

            Alerts.bind(alert);
        }

        function getType(id) {
            switch (id) {
                case 0:
                    return 'danger';
                case 1:
                    return 'warning';
                case 2:
                    return 'info';
                case 3:
                    return 'success';
                default:
                    return id;
            }
        }
    },
    bind: function (alert) {
        if ($(alert).data('timeout')) {
            setTimeout(function () {
                Alerts.close(alert);
            }, $(alert).data('timeout'));
        }
    },
    close: function (alert) {
        $(alert).fadeTo(300, 0).slideUp(300, function () {
            $(this).remove();
        });
    },
    closeAll: function () {
        $('.alerts .alert').fadeTo(300, 0).slideUp(300, function () {
            $(this).remove();
        });
    },

    clear: function () {
        $('.alerts .alert').remove();
    }
};
