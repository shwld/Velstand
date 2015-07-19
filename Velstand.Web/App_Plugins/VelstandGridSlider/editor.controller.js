angular.module("umbraco").controller("VelstandGridSlider.Controller",
    function ($scope, $rootScope, dialogService, userService) {
        if (!$scope.control.value) {
            $scope.control.value = {
                images: [],
            };
        }

        $scope.setImages = function () {
            userService.getCurrentUser().then(function (userData) {
                dialogService.mediaPicker({
                    startNodeId: userData.startMediaId,
                    multiPicker: true,
                    //cropSize:  $scope.control.editor.config && $scope.control.editor.config.size ? $scope.control.editor.config.size : undefined,
                    callback: function (medias) {
                        $scope.control.value.images = [];
                        angular.forEach(medias, function (media, i) {
                            $scope.control.value.images.push({
                                focalPoint: media.focalPoint,
                                id: media.id,
                                url: media.image
                            });
                        });
                        $scope.setUrl();
                    }
                });
            });
        };

        $scope.setUrl = function(){
            if($scope.control.value.images.length > 0){
                var url = $scope.control.value.images[0].url;

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

        if ($scope.control.value.images) {
            $scope.setUrl();
        }
});
