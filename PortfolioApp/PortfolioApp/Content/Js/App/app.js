angular.module('myFormApp', []).
    controller('lotteryCtrl', function ($scope, $http, $location, $window) {
    $scope.custModel = {};
    $scope.message = '';
    $scope.result = "color-default";
    $scope.isViewLoading = false;
    $scope.ListCustomer = null;
    getallData();
    //******=========Get All Customer=========******  
    function getallData() {
        //debugger;  
        $http.get('/Home/GetAllData').success(function (data, status, headers, config) {
            $scope.ListCustomer = data;
        }).error(function (data, status, headers, config) {
            $scope.errors = [];
            $scope.message = 'Unexpected Error while saving data!!';
            console.log($scope.message);
        });
    };
    //******=========Get Single Customer=========******  
    $scope.getCustomer = function (custModel) {
        $http.get('/Home/GetbyID/' + custModel.Id).success(function (data, status, headers, config) {
            //debugger;  
            $scope.custModel = data;
            getallData();
            console.log(data);
        }).error(function (data, status, headers, config) {
            $scope.errors = [];
            $scope.message = 'Unexpected Error while saving data!!';
            console.log($scope.message);
        });
    };
    //******=========Save Customer=========******  
    $scope.saveCustomer = function () {
        $scope.isViewLoading = true;
        $http(
        {
            method: 'POST',
            url: '/Home/Insert',
            data: $scope.custModel
        }).success(function (data, status, headers, config) {
            $scope.errors = [];
            if (data.success === true) {
                $scope.message = 'Form data Saved!';
                $scope.result = "color-green";
                getallData();
                $scope.custModel = {};
                console.log(data);
            }
            else {
                $scope.errors = data.errors;
            }
        }).error(function (data, status, headers, config) {
            $scope.errors = [];
            $scope.message = 'Unexpected Error while saving data!!';
            console.log($scope.message);
        });
        getallData();
        $scope.isViewLoading = false;
    };
    //******=========Edit Customer=========******  
    $scope.updateCustomer = function () {
        //debugger;  
        $scope.isViewLoading = true;
        $http(
        {
            method: 'POST',
            url: '/Home/Update',
            data: $scope.custModel
        }).success(function (data, status, headers, config) {
            $scope.errors = [];
            if (data.success === true) {
                $scope.custModel = null;
                $scope.message = 'Form data Updated!';
                $scope.result = "color-green";
                getallData();
                console.log(data);
            }
            else {
                $scope.errors = data.errors;
            }
        }).error(function (data, status, headers, config) {
            $scope.errors = [];
            $scope.message = 'Unexpected Error while saving data!!';
            console.log($scope.message);
        });
        $scope.isViewLoading = false;
    };
    //******=========Delete Customer=========******  
    $scope.deleteCustomer = function (custModel) {
        //debugger;  
        varIsConf = confirm('You are about to delete ' + custModel.CustName + '. Are you sure?');
        if (IsConf) {
            $http.delete('/Home/Delete/' + custModel.Id).success(function (data, status, headers, config) {
                $scope.errors = [];
                if (data.success === true) {
                    $scope.message = custModel.CustName + ' deleted from record!!';
                    $scope.result = "color-red";
                    getallData();
                    console.log(data);
                }
                else {
                    $scope.errors = data.errors;
                }
            }).error(function (data, status, headers, config) {
                $scope.errors = [];
                $scope.message = 'Unexpected Error while saving data!!';
                console.log($scope.message);
            });
        }
    };
    }).config(function ($routeProvider, $locationProvider) {
        $routeProvider
             .when('/', {
                 templateUrl: "templates/index.html",
                 controller: 'MainCtrl',
             })
              .when('/contacts', {
                  templateUrl: "templates/contacts.html",
                  controller: 'ContactsCtrl',
              })
              .otherwise({
                  template: 'does not exists'
              });
    });