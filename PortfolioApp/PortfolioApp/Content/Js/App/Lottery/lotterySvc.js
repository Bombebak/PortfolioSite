StudentApp.factory('StudentService', ['$http', function ($http) {


    var StudentService = {};

    StudentService.getStudents = function () {

        return $http.get('/Home/GetPersons');

    };

    return StudentService;



}]);