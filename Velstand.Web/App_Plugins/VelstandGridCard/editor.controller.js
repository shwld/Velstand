angular.module("umbraco").controller("VelstandGridCard.Controller",
    function ($scope, $rootScope, dialogService, userService) {
        if (!$scope.control.value) {
            $scope.control.value = {
                image: null
            };
        }

        $scope.setImage = function(){
            userService.getCurrentUser().then(function (userData) {
                dialogService.mediaPicker({
                    startNodeId: userData.startMediaId,
                    multiPicker: false,
                    //cropSize:  $scope.control.editor.config && $scope.control.editor.config.size ? $scope.control.editor.config.size : undefined,
                    callback: function (media) {
                        $scope.control.value.image = {
                            focalPoint: media.focalPoint,
                            id: media.id,
                            url: media.image
                        };
                        $scope.setUrl();
                    }
                });
            });
        };

        $scope.setUrl = function(){
            if($scope.control.value.image){
                var url = $scope.control.value.image.url;

                if($scope.control.editor.config && $scope.control.editor.config.size){
                    url += "?width=" + $scope.control.editor.config.size.width;
                    url += "&height=" + $scope.control.editor.config.size.height;

                    if($scope.control.value.focalPoint){
                        url += "&center=" + $scope.control.value.focalPoint.top +"," + $scope.control.value.focalPoint.left;
                        url += "&mode=crop";
                    }
                }

                $scope.url = url;
            }
        };

        if ($scope.control.value.image) {
            $scope.setUrl();
        }
});
