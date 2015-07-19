app.requires.push('ui.ace');
angular.module("umbraco")
    .controller("MdEdit.Controller",
    function ($scope, $log, assetsService, dialogService, imageHelper, contentResource) {
        $scope.model.config.editor_height = !!$scope.model.config.editor_height ? $scope.model.config.editor_height : 400;

        // markup
        var reMarker = new reMarked(markedOptions.getReMarkedOptions());

        $scope.aceLoaded = function(_editor){
            // Editor part
            var _session = _editor.getSession();
            var _renderer = _editor.renderer;

            // Options
            _session.setUndoManager(new ace.UndoManager());

            _editor.setValue(reMarker.render($scope.model.value));
            _session.on("change", function () {
                $scope.model.value = marked(_editor.getValue());
            });

            // media picker
            $scope.mediaPick = function () {
                dialogService.mediaPicker({
                    callback: function (image) {
                        var imagePropVal = imageHelper.getImagePropertyValue({ imageModel: image, scope: $scope });
                        _editor.insert("\n![" + image.name + "](" + imagePropVal + " \"" + image.name + "\")\n");
                    }
                });
            }

            // link picker
            $scope.linkPick = function () {
                dialogService.linkPicker({
                    // currentTarget: textarea,
                    callback: function (link) {
                        if (link) {
                            var href = link.url;

                            // content or media
                            if (link.id) {
                                var niceUrl = contentResource.getNiceUrl(link.id);
                                if (niceUrl) {
                                    insertLink(href, link.name);
                                    return true;
                                }
                            }

                            // unlink
                            if (!href) {
                                return true;
                            }

                            // is_mail
                            if (href.indexOf('@') > 0 && href.indexOf('//') == -1 && href.indexOf('mailto:') == -1) {
                                href = 'mailto:' + href;
                                insertLink(href, link.name);
                                return true;
                            }

                            if (/^\s*www\./i.test(href)) {
                                href = 'http://' + href;
                                insertLink(href, link.name);
                                return true;
                            }

                            insertLink(href, link.name);
                            return true;
                        }
                    }
                });
            }

            function insertLink(url, title) {
                if (!title) {
                    title = _editor.session.getTextRange(_editor.getSelectionRange());
                }
                _editor.insert("[" + title + "](" + url + " \"" + title + "\")\n");
            }

            function insertHead(text) {
                _cursor = _editor.selection.getCursor();
                _editor.gotoLine(_cursor.row + 1, 0);
                _editor.insert(text);
            }

            function sandwich(head, foot) {
            }
            function insertLines(text) {
                range = _editor.getSelectionRange();
                for(var i = range.start.row + 1; i <= range.end.row + 1; i++) {
                    _editor.gotoLine(i, 0);
                    _editor.insert(text);
                }
            }

            // preview
            $scope.preview = function () {
                dialogService.open({
                    template: "../App_Plugins/MdEdit/dialogs/view.html",
                    callback: function (value) {
                    },
                    show: true,
                    dialogData: $scope.model.value
                });
            }

            
            $scope.toH2 = function () {
                insertHead("## ");
            }

            $scope.toH3 = function () {
                insertHead("### ");
            }

            $scope.toH4 = function () {
                insertHead("#### ");
            }

            $scope.toNumberList = function () {
                insertLines("1. ");
            }

            $scope.toList = function () {
                insertLines("* ");
            }

            $scope.insertHr = function() {
                insertHead("\n- - -\n\n");
            }

            $scope.toBlockquotes = function() {
                insertLines("> ");
            }

            $scope.undo = function () {
                _editor.undo();
            }

            $scope.redo = function () {
                _editor.redo();
            }

        };
    });
