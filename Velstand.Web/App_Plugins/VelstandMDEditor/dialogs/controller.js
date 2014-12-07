angular.module("umbraco")
    .controller("Velstand.MDEditor.PreviewController",
    function ($scope, $log, dialogService) {
        $scope.close = function() {
            dialogService.close();
        };
    });
