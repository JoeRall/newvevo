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
var MainCtrl = (function (_super) {
    __extends(MainCtrl, _super);
    function MainCtrl($scope, $http) {
        _super.call(this, $scope);
        this.$scope = $scope;
        this.$http = $http;
        if (location.pathname.indexOf("/u/") == 0) {
            $scope.userName = location.pathname.substring(3);
            $scope.IsExistingUser = true;
        }
        else {
            $scope.IsNewUser = true;
        }
    }
    return MainCtrl;
})(BaseController);
newVevo.controller("mainCtrl", ["$scope", "$http", function ($scope, $http) { return new MainCtrl($scope, $http); }]);
//# sourceMappingURL=NewVevo.js.map