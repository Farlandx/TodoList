var app = angular.module('todolistApp', []);

app.controller('todolistCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.todoText = '';
    $scope.todoItems = [];
    $scope.reverse = true;

    $scope.createTodo = function (text) {
        if (text) {
            var newItem = { complete: false, text: text, important: false };
            $http.post(
                '/api/TodoList',
                JSON.stringify(newItem),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            ).success(function (data) {
                $scope.todoItems.push(data);
                $scope.todoText = '';
            }).error(function (data, status, headers, config) {
                console.log('error:' + status);
            });
        }
    }

    $scope.updateTodo = function (item) {
        if (item.id) {
            $http.put('/api/TodoList/' + item.id, item).success(function (data) {
                
            }).error(function (data, status, headers, config) {
                console.log('error:' + status);
            });
        }
    }

    $scope.deleteTodo = function (item) {
        if (item.id) {
            $http.delete('/api/TodoList/' + item.id).success(function (data) {
                var index = $scope.todoItems.indexOf(item);
                $scope.todoItems.splice(index, 1);
            }).error(function (data, status, headers, config) {
                console.log('error:' + status);
            });
        }
    }

    $scope.test = function (sender) {
        console.log(sender.text);
        this.blur();
    }

    $http.get('/api/TodoList').success(function (data, status, headers, config) {
        $scope.todoItems = data;
    }).error(function (data, status, headers, config) {
        console.log('error:' + status);
    });
}]).directive('ngEnter', function () {
    return function (scope, elem, attrs) {
        elem.bind('keydown', function (event) {
            event = event ? event : window.event;
            if (event.keyCode == 13)
                scope.$apply(attrs.ngEnter);
        });
    };
});