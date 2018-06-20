JsTree = {
    init: function () {
        var trees = $('.js-tree-view');
        for (var i = 0; i < trees.length; i++) {
            $(trees[i]).on('click.jstree', '.jstree-anchor', function (e) {
                if ($(this).closest('.widget-box.readonly').length) {
                    e.stopImmediatePropagation();
                }
            });

            var tree = $(trees[i]).jstree({
                'core': {
                    'themes': {
                        'icons': false
                    }
                },
                'plugins': [
                    'checkbox'
                ],
                'checkbox': {
                    'keep_selected_style': false
                }
            });

            tree.on('ready.jstree', function (e, data) {
                var selected = $(this).prev('.js-tree-view-ids').children();
                for (var j = 0; j < selected.length; j++) {
                    data.instance.select_node(selected[j].value, false, true);
                }

                data.instance.open_node($.makeArray(data.instance.element.find('> ul > li')), null, null);
                data.instance.element.show();
            });
        }

        $(document).on('submit', 'form', function () {
            var trees = $(this).find('.js-tree-view');
            for (var i = 0; i < trees.length; i++) {
                var tree = $(trees[i]).jstree();
                var ids = tree.element.prev('.js-tree-view-ids');

                ids.empty();
                var selected = tree.get_selected();
                for (var j = 0; j < selected.length; j++) {
                    var node = tree.get_node(selected[j]);
                    if (node.li_attr.id) {
                        ids.append('<input type="hidden" value="' + node.li_attr.id + '" name="' + tree.element.attr('for') + '" />');
                    }
                }
            }
        });
    }
};
