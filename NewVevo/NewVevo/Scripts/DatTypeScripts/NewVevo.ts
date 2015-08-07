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


class Video {
    Isrc: string;
    Title: string;
    Artist: string;
    ArtistId: string;
    VideoYear: number;
    TotalViews: number;
    Genre: string;
}

class Stream {
    IsLive: boolean;
    Quality: string;
    Url: string;
    Version: number;
}

interface IMainCtrlScope extends ng.IScope {
    Item: any;
    userName: string;
    IsExistingUser: boolean;
    IsNewUser: boolean;
    Videos: Array<Video>;
    CurrentVideo: Video;
    ActiveStreams: Array<Stream>;
    DaVids: any;
}

class MainCtrl extends BaseController {

    constructor(private $scope: IMainCtrlScope, private $http: ng.IHttpService, private $sce: ng.ISCEService) {
        super($scope);

        if (location.pathname.indexOf("/u/") == 0) {
            $scope.userName = location.pathname.substring(3);

            $http.get<Array<Video>>("/api/newvevo/Next?userId=" + $scope.userName).then(result => {
                if (result.status != 200) {
                    alert('Error -> ' + result.statusText);
                    return;
                }
                this.$scope.Videos = result.data;

                if (this.$scope.Videos != null) {
                    this.$scope.CurrentVideo = this.$scope.Videos[0];
                    this.$http.get<Array<Stream>>("/api/newvevo/Streams?isrc=" + this.$scope.CurrentVideo.Isrc)
                        .then(sResult => {
                            if (sResult.status != 200) {
                                alert('Error -> ' + sResult.statusText);
                                return;
                            }

                            this.$scope.ActiveStreams = sResult.data;




                            for (var i = 0; i < this.$scope.ActiveStreams.length; i++) {
                                this.$scope.ActiveStreams[i].Url = this.$sce.trustAsResourceUrl(this.$scope.ActiveStreams[i].Url);
                            }


                            
                            this.PlayUrl(this.$scope.ActiveStreams[0].Url);


                        });
                }

            });

            $scope.IsExistingUser = true;
        }
        else {
            $scope.IsNewUser = true;
        }

    }

    PlayUrl(url: string) {
        setTimeout(() => {
            var player = <any>$('#thePlayer')
                .attr('src', url)
                .get(0);
            player.play();
        }, 250);
        
    }

    GenerateUrl(url: string) {
        return this.$sce.trustAsResourceUrl(url);
    }

    Next() {
        
    }
}

newVevo.controller("mainCtrl",
    ["$scope", "$http", "$sce", ($scope, $http, $sce) => new MainCtrl($scope, $http, $sce)]);