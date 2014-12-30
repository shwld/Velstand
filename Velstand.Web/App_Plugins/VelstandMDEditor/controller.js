angular.module("umbraco")
    .controller("Velstand.MDEditor.Controller",
    function ($scope, $log, assetsService, dialogService, imageHelper, contentResource) {
        $scope.model.config.editor_height = !!$scope.model.config.editor_height ? $scope.model.config.editor_height : 400;

        // markup
        var reMarker = new reMarked(markedOptions.getReMarkedOptions());

        $scope.velstandMd = new VelstandMarkdown(
                                            reMarker.render($scope.model.value),
                                            function () {
                                                $scope.model.value = marked($scope.velstandMd.getValue());
                                            });

        // media picker
        $scope.mediaPick = function () {
            dialogService.mediaPicker({
                callback: function (data) {
                    $scope.velstandMd.markdownFromMediaPicker(data, imageHelper, $scope);
                }
            });
        }

        // link picker
        $scope.linkPick = function () {
            dialogService.linkPicker({
                // currentTarget: textarea,
                callback: function (data) {
                    $scope.velstandMd.markdownFromLinkPicker(data, contentResource);
                }
            });
        }

        $scope.preview = function () {
            dialogService.open({
                template: "../App_Plugins/VelstandMdEditor/dialogs/view.html",
                callback: function (value) {
                },
                show: true,
                dialogData: $scope.model.value
            });
        }

        $scope.on_action = function (item) {
            $scope.velstandMd.updateTextArea(item);
            if ($scope.velstandMd.isUpdate) {
                $scope.model.value = marked($scope.velstandMd.getValue());
            }
        }
    }).directive('velstandHundler', function () {
        return {
            restrict: 'A',
            link: function (scope, element, $event) {
                /* undo
                element.bind("keyup", function ($event) {
                    if ($event.keyCode == 122 && $event.ctrlKey) {
                        scope.velstandMd.undo();
                        return;
                    }
                })*/
                element.bind("keyup", function () { scope.on_action(element.context); });
                element.bind("mouseup", function () { scope.on_action(element.context); });
            }
        }
    });
