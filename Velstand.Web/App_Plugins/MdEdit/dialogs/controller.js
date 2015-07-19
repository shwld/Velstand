angular.module("umbraco")
    .controller("MdEdit.PreviewController",
    function ($scope, $log, dialogService) {
        $scope.close = function() {
            dialogService.close();
        };
    });
