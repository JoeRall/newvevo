/// <reference path="../typings/angularjs/angular.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var newVevo = angular.module('newVevo', []);
var BaseController = (function () {
    function BaseController($scope) {
        var _this = this;
        // setup all non _ functions on the scope to be called so that the
        // context (this) is the instance of this class
        for (var d in _this) {
            if (typeof (_this[d]) == "function"
                && d.length
                && '_' !== d[0]
                && "constructor" !== d) {
                $scope[d] = (function (functionName) {
                    return function () {
                        _this[functionName].apply(_this, arguments);
                    };
                })(d);
            }
        }
    }
    return BaseController;
})();
var Video = (function () {
    function Video() {
    }
    return Video;
})();
var Stream = (function () {
    function Stream() {
    }
    return Stream;
})();
var MainCtrl = (function (_super) {
    __extends(MainCtrl, _super);
    function MainCtrl($scope, $http, $sce) {
        var _this = this;
        _super.call(this, $scope);
        this.$scope = $scope;
        this.$http = $http;
        this.$sce = $sce;
        if (location.pathname.indexOf("/u/") == 0) {
            $scope.userName = location.pathname.substring(3);
            $http.get("/api/newvevo/Next?userId=" + $scope.userName).then(function (result) {
                if (result.status != 200) {
                    alert('Error -> ' + result.statusText);
                    return;
                }
                _this.$scope.Videos = result.data;
                if (_this.$scope.Videos != null) {
                    _this.$scope.CurrentVideo = _this.$scope.Videos[0];
                    _this.$http.get("/api/newvevo/Streams?isrc=" + _this.$scope.CurrentVideo.Isrc)
                        .then(function (sResult) {
                        if (sResult.status != 200) {
                            alert('Error -> ' + sResult.statusText);
                            return;
                        }
                        _this.$scope.ActiveStreams = sResult.data;
                        for (var i = 0; i < _this.$scope.ActiveStreams.length; i++) {
                            _this.$scope.ActiveStreams[i].Url = _this.$sce.trustAsResourceUrl(_this.$scope.ActiveStreams[i].Url);
                        }
                        _this.PlayUrl(_this.$scope.ActiveStreams[0].Url);
                    });
                }
            });
            $scope.IsExistingUser = true;
        }
        else {
            $scope.IsNewUser = true;
        }
    }
    MainCtrl.prototype.PlayUrl = function (url) {
        setTimeout(function () {
            var player = $('#thePlayer')
                .attr('src', url)
                .get(0);
            player.play();
        }, 250);
    };
    MainCtrl.prototype.GenerateUrl = function (url) {
        return this.$sce.trustAsResourceUrl(url);
    };
    MainCtrl.prototype.Next = function () {
    };
    return MainCtrl;
})(BaseController);
newVevo.controller("mainCtrl", ["$scope", "$http", "$sce", function ($scope, $http, $sce) { return new MainCtrl($scope, $http, $sce); }]);
//# sourceMappingURL=NewVevo.js.map