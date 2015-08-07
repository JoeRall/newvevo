/// <reference path="../typings/angularjs/angular.d.ts" />

var newVevo = angular.module('newVevo', []);

class BaseController {
    constructor($scope: ng.IScope) {
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
}

interface IMainCtrlScope extends ng.IScope {
    Item: any;
    userName: string;
    IsExistingUser: boolean;
    IsNewUser: boolean;
}

class MainCtrl extends BaseController {

    constructor(private $scope: IMainCtrlScope, private $http: ng.IHttpService) {
        super($scope);

        if (location.pathname.indexOf("/u/") == 0) {
            $scope.userName = location.pathname.substring(3);
            $scope.IsExistingUser = true;
        }
        else {
            $scope.IsNewUser = true;
        }
        
    }
}

newVevo.controller("mainCtrl",
    ["$scope", "$http", ($scope, $http) => new MainCtrl($scope, $http)]);