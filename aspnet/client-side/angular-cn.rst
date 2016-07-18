:version: 1.0.0-rc1

使用Angular进行SPA(Single Page Application)开发
=================================================

由 `Venkata Koppaka`_ and `Scott Addie`_ 编辑

在这篇文章中, 你将会学习如何使用AngularJS构建SPA风格ASP.NET应用.

.. contents:: Sections:
  :local:
  :depth: 1
  
`查看或下载样例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/client-side/angular/sample>`__

什么是AngularJS?
------------------

`AngularJS <http://angularjs.org/>`_ 是Google常用来实现单页面应用的现代JavaScript框架. AngularJS基于MIT许可开源, AngularJS开发进度可通过 `its GitHub repository <https://github.com/angular/angular.js>`_ 追踪. Angular得名于HTML使用尖括号.

AngularJS不是像jQuery的DOM manipulation库, 但是它使用称为jQLite的jQuery子集. AngularJS主要基于declarative HTML 特性, 它可以添加到HTML标签. 你可以在浏览器里使用 `Code School website <http://campus.codeschool.com/courses/shaping-up-with-angular-js/intro>`_ 尝试AngularJS.

版本 1.5.x 是目前的稳定版本, Angular团队正在进行大量的重写实现开发中的2.0版. 此文关注Angular 1.X版本并添加一些Angular 2.0的注释.

入门
---------------

要开始在你的ASP.NET应用中使用AngularJS, 需要将它安装并作为工程的一部分, 或者从内容分发网络(CDN)引用它.

安装
^^^^^^^^^^^^

有很多方法可以添加AngularJS到你的应用. 如果你在Visual Studio中开始新的ASP.NET Core Web应用, 你可以使用内建 :ref:`Bower <bower-index>` 支持添加AngularJS. 简单打开 ``bower.json``, 添加实例到 ``dependencies`` 属性:

.. _angular-bower-json:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/bower.json
  :language: json
  :linenos:
  :emphasize-lines: 9
  
当保存 ``bower.json`` 文件后, Angular 将会被安装到你工程的 ``wwwroot/lib`` 文件夹. 另外, 它将会被列到 ``Dependencies/Bower`` 文件夹. 参照下面的屏幕截图.

.. image:: angular/_static/angular-solution-explorer.png
  :width: 283px

接下来, 添加 ``<script>`` 引用到你的HTML页面 ``<body>`` 节的底部或者 `_Layout.cshtml` 文件, 如下:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Shared/_Layout.cshtml
  :language: html
  :linenos:
  :lines: 48-52
  :emphasize-lines: 4

推荐生产应用使用CDN引用共通库如Angular. 可以从很多CDN之一引用Angular, 如下:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Shared/_Layout.cshtml
  :language: html
  :linenos:
  :lines: 53-67
  :emphasize-lines: 10

当你引用了angular.js脚本文件, 就可以开始在Web页面中使用Angular了.

关键组件
--------------

AngularJS包括很多主要组件, 如 *directives*, *templates*, *repeaters*, *modules*, *controllers*, 等等. 让我们检视这些组件如何工作并添加行为到你的Web页面.

指令(Directives)
^^^^^^^^^^

AngularJS 使用 `directives <https://docs.angularjs.org/guide/directive>`_ 通过自定义特性和元素扩展HTML.
 AngularJS指令通过 ``data-ng-*`` 或 ``ng-*`` 等前缀定义 (``ng`` 是angular的简写). 有两种类型的AngularJS指令:

  #. **Primitive Directives**: 这些是Angular团队预定义的指令并作为AngularJS框架的组成部分.
  #. **Custom Directives**: 这些是你自己定义的自定义指令.

一个所有应用AngularJS应用都会使用的primitive directives就是 ``ng-app`` 指令, 用来启动AngularJS应用. 这个指令可以被附加到 ``<body>`` 标签或其子元素上. 让我们看一个可执行的样例. 假设你在ASP.NET项目中, 你可以添加文件到 ``wwwroot`` 文件夹, 或者添加一个新的控制器行动(controller action)和关联的视图(View).
   在此, 我添加一个新的 ``Directives`` 行动(Action)方法到 ``HomeController.cs``. 关联视图如下:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Directives.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 5,7

为了保持这些样例独立, 我没有使用共享的布局文件. 你可以参照我们通过 ``ng-app`` 指令decorated的 Body标签来表明此页面是一个AngularJS应用.
 ``{{2+2}}`` 是需要你学习一段时间的Angular数据绑定表达式. 如下是你运行此应用的结果: 

.. image:: angular/_static/simple-directive.png

其它AngularJS中的primitive指令包括:

``ng-controller``
  决定哪个JavaScript控制器(controller)绑定到哪个视图(view).

``ng-model``
  决定绑定到HTML元素属性值的模型(model).

``ng-init``
  用于初始化当前范围(scope)表达式形式的应用数据.

``ng-if``
  移除或重建DOM中既定HTML元素 in the DOM based on the truthiness of the expression provided.

``ng-repeat``
  根据数据集重复给定HTML块.

``ng-show``
  基于提供的表达式展示或隐藏给定的HTML元素.

AngularJS中所有primitive指令完整的列表参考 `directive documentation section on the AngularJS documentation website <https://docs.angularjs.org/api/ng/directive>`_.

数据绑定
^^^^^^^^^^^^

AngularJS提供 `data binding <https://docs.angularjs.org/guide/databinding>`_ 支持 out-of-the-box using either the ``ng-bind`` directive or a data binding expression syntax such as ``{{expression}}``. AngularJS supports two-way data binding where data from a model is kept in synchronization with a view template at all times. Any changes to the view are automatically reflected in the model. Likewise, any changes in the model are reflected in the view.

创建HTML文件或者控制器动作 with 根据名为``Databinding``的视图(view) . 在视图中包括如下:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Databinding.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 8-10

注意你能使用指令或数据绑定显示模型值Notice that you can display model values using either directives or data binding (``ng-bind``). 结果页面如下:

.. image:: angular/_static/simple-databinding.png

模板
^^^^^^^^^

`Templates <https://docs.angularjs.org/guide/templates>`_ in AngularJS are just plain HTML pages decorated with AngularJS directives and artifacts. A template in AngularJS is a mixture of directives, expressions, filters, and controls that combine with HTML to form the view.

Add another view to demonstrate templates, and add the following to it:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Templates.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 8-10

The template has AngularJS directives like ``ng-app``, ``ng-init``, ``ng-model`` and data binding expression syntax to bind the ``personName`` property. Running in the browser, the view looks like the screenshot below: 

.. image:: angular/_static/simple-templates-1.png

If you change the name by typing in the input field, you will see the text next to the input field dynamically update, showing Angular two-way data binding in action.

.. image:: angular/_static/simple-templates-2.png

Expressions
^^^^^^^^^^^

`Expressions <https://docs.angularjs.org/guide/expression>`_ in AngularJS are JavaScript-like code snippets that are written inside the ``{{ expression }}`` syntax. The data from these expressions is bound to HTML the same way as ``ng-bind`` directives. The main difference between AngularJS expressions and regular JavaScript expressions is that AngularJS expressions are evaluated against the ``$scope`` object in AngularJS. 

The AngularJS expressions in the sample below bind ``personName`` and a simple JavaScript calculated expression:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Expressions.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 8-10

The example running in the browser displays the ``personName`` data and the results of the calculation:

.. image:: angular/_static/simple-expressions.png

Repeaters
^^^^^^^^^

Repeating in AngularJS is done via a primitive directive called ``ng-repeat``. The ``ng-repeat`` directive repeats a given HTML element in a view over the length of a repeating data array. Repeaters in AngularJS can repeat over an array of strings or objects. Here is a sample usage of repeating over an array of strings: 

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Repeaters.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 8,10-11

The `repeat directive <https://docs.angularjs.org/api/ng/directive/ngRepeat>`_ outputs a series of list items in an unordered list, as you can see in the developer tools shown in this screenshot:

.. image:: angular/_static/repeater.png

Here is an example that repeats over an array of objects. The ``ng-init`` directive establishes a ``names`` array, where each element is an object containing first and last names. The ``ng-repeat`` assignment, ``name in names``, outputs a list item for every array element.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Repeaters2.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 8-11,13-14

The output in this case is the same as in the previous example.

Angular provides some additional directives that can help provide behavior based on where the loop is in its execution.

``$index``
  Use ``$index`` in the ``ng-repeat`` loop to determine which index position your loop currently is on.

``$even`` and ``$odd``
  Use ``$even`` in the ``ng-repeat`` loop to determine whether the current index in your loop is an even indexed row. Similarly, use ``$odd`` to determine if the current index is an odd indexed row.

``$first`` and ``$last``
  Use ``$first`` in the ``ng-repeat`` loop to determine whether the current index in your loop is the first row. Similarly, use ``$last`` to determine if the current index is the last row.

Below is a sample that shows ``$index``, ``$even``, ``$odd``, ``$first``, and ``$last`` in action: 

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Repeaters3.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 14-18


Here is the resulting output:

.. image:: angular/_static/repeaters2.png

$scope
^^^^^^

``$scope`` is a JavaScript object that acts as glue between the view (template) and the controller (explained below). A view template in AngularJS only knows about the values attached to the ``$scope`` object in the controller. 

.. note:: In the MVVM world, the ``$scope`` object in AngularJS is often defined as the ViewModel. The AngularJS team refers to the ``$scope`` object as the Data-Model. `Learn more about Scopes in AngularJS <https://docs.angularjs.org/guide/scope>`_.

Below is a simple example showing how to set properties on ``$scope`` within a separate JavaScript file, ``scope.js``:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/scope.js
  :language: html
  :linenos:
  :emphasize-lines: 2-3

Observe the ``$scope`` parameter passed to the controller on line 2. This object is what the view knows about. On line 3, we are setting a property called "name" to "Mary Jane". 

What happens when a particular property is not found by the view? The view defined below refers to "name" and "age" properties: 

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Scope.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 9-10,14

Notice on line 9 that we are asking Angular to show the "name" property using expression syntax. Line 10 then refers to "age", a property that does not exist. The running example shows the name set to "Mary Jane" and nothing for age. Missing properties are ignored.

.. image:: angular/_static/scope.png

Modules
^^^^^^^

A `module <https://docs.angularjs.org/guide/module>`_ in AngularJS is a collection of controllers, services, directives, etc. The ``angular.module()`` function call is used to create, register, and retrieve modules in AngularJS. All modules, including those shipped by the AngularJS team and third party libraries, should be registered using the ``angular.module()`` function. 

Below is a snippet of code that shows how to create a new module in AngularJS. The first parameter is the name of the module. The second parameter defines dependencies on other modules. Later in this article, we will be showing how to pass these dependencies to an ``angular.module()`` method call.

.. code-block:: javascript

  var personApp = angular.module('personApp', []);

Use the ``ng-app`` directive to represent an AngularJS module on the page. To use a module, assign the name of the module, ``personApp`` in this example, to the ``ng-app`` directive in our template.

.. code-block:: html

  <body ng-app="personApp">

Controllers
^^^^^^^^^^^

`Controllers <https://docs.angularjs.org/guide/controller>`_ in AngularJS are the first point of entry for your code. The ``<module name>.controller()`` function call is used to create and register controllers in AngularJS. The ``ng-controller`` directive is used to represent an AngularJS controller on the HTML page. The role of the controller in Angular is to set state and behavior of the data model (``$scope``). Controllers should not be used to manipulate the DOM directly.

Below is a snippet of code that registers a new controller. The ``personApp`` variable in the snippet references an Angular module, which is defined on line 2.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/controllers.js
  :language: javascript
  :linenos:
  :emphasize-lines: 2,5

The view using the ``ng-controller`` directive assigns the controller name: 

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/Home/Controllers.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 8,14

The page shows "Mary" and "Jane" that correspond to the ``firstName`` and ``lastName`` properties attached to the ``$scope`` object:

.. image:: angular/_static/controllers.png


Services
^^^^^^^^

`Services <https://docs.angularjs.org/guide/services>`_ in AngularJS are commonly used for shared code that is abstracted away into a file which can be used throughout the lifetime of an Angular application. Services are lazily instantiated, meaning that there will not be an instance of a service unless a component that depends on the service gets used. Factories are an example of a service used in AngularJS applications. Factories are created using the ``myApp.factory()`` function call, where ``myApp`` is the module. 

Below is an example that shows how to use factories in AngularJS: 

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/simpleFactory.js
  :language: javascript
  :linenos:
  :emphasize-lines: 1

To call this factory from the controller, pass ``personFactory`` as a parameter to the ``controller`` function: 

.. code-block:: javascript

  personApp.controller('personController', function($scope,personFactory) {
    $scope.name = personFactory.getName();
  });

Using services to talk to a REST endpoint
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Below is an end-to-end example using services in AngularJS to interact with an ASP.NET Core Web API endpoint. The example gets data from the Web API and displays the data in a view template. Let's start with the view first: 

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/People/Index.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 5,8,10,17-19

In this view, we have an Angular module called ``PersonsApp`` and a controller called ``personController``. We are using ``ng-repeat`` to iterate over the list of persons. We are referencing three custom JavaScript files on lines 17-19.

The ``personApp.js`` file is used to register the ``PersonsApp`` module; and, the syntax is similar to previous examples. We are using the ``angular.module`` function to create a new instance of the module that we will be working with.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/personApp.js
  :language: javascript
  :linenos:
  :emphasize-lines: 3

Let's take a look at ``personFactory.js``, below. We are calling the module’s ``factory`` method to create a factory. Line 12 shows the built-in Angular ``$http`` service retrieving people information from a web service.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/personFactory.js
  :language: javascript
  :linenos:
  :emphasize-lines: 6-7,12

In ``personController.js``, we are calling the module’s ``controller`` method to create the controller. The ``$scope`` object's ``people`` property is assigned the data returned from the personFactory (line 13).

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/personController.js
  :language: javascript
  :linenos:
  :emphasize-lines: 6-7,13

Let's take a quick look at the Web API and the model behind it. The ``Person`` model is a POCO (Plain Old CLR Object) with ``Id``, ``FirstName``, and ``LastName`` properties:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Models/Person.cs
  :language: csharp
  :linenos:

The ``Person`` controller returns a JSON-formatted list of ``Person`` objects:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Controllers/Api/PersonController.cs
  :language: csharp
  :linenos:
  :emphasize-lines: 9-10,19

Let's see the application in action: 

.. image:: angular/_static/rest-bound.png

You can `view the application's structure on GitHub <https://github.com/aspnet/Docs/tree/master/aspnet/client-side/angular/sample>`_.

.. note:: For more on structuring AngularJS applications, see `John Papa's Angular Style Guide <https://github.com/johnpapa/angular-styleguide>`_

.. note:: To create AngularJS module, controller, factory, directive and view files easily, be sure to check out Sayed Hashimi's `SideWaffle template pack for Visual Studio <http://sidewaffle.com/>`_. Sayed Hashimi is a Senior Program Manager on the Visual Studio Web Team at Microsoft and SideWaffle templates are considered the gold standard. At the time of this writing, SideWaffle is available for Visual Studio 2012, 2013, and 2015.

Routing and Multiple Views
^^^^^^^^^^^^^^^^^^^^^^^^^^

AngularJS has a built-in route provider to handle SPA (Single Page Application) based navigation. To work with routing in AngularJS, you must add the ``angular-route`` library using Bower. You can see in the :ref:`bower.json <angular-bower-json>` file referenced at the start of this article that we are already referencing it in our project.

After you install the package, add the script reference (``angular-route.js``) to your view.

Now let's take the Person App we have been building and add navigation to it. First, we will make a copy of the app by creating a new ``PeopleController`` action called ``Spa`` and a corresponding ``Spa.cshtml`` view by copying the Index.cshtml view in the ``People`` folder. Add a script reference to ``angular-route`` (see line 11). Also add a ``div`` marked with the ``ng-view`` directive (see line 6) as a placeholder to place views in. We are going to be using several additional ``.js`` files which are referenced on lines 13-16.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/People/Spa.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 6,11-16
  
Let's take a look at ``personModule.js`` file to see how we are instantiating the module with routing. We are passing ``ngRoute`` as a library into the module. This module handles routing in our application.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/personModule.js
  :language: javascript
  :linenos:

The ``personRoutes.js`` file, below, defines routes based on the route provider. Lines 4-7 define navigation by effectively saying, when a URL with ``/persons`` is requested, use a template called ``partials/personlist`` by working through ``personListController``. Lines 8-11 indicate a detail page with a route parameter of ``personId``. If the URL doesn't match one of the patterns, Angular defaults to the ``/persons`` view.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/personRoutes.js
  :language: javascript
  :linenos:
  :emphasize-lines: 4-7, 8-11, 13

The ``personlist.html`` file is a partial view containing only the HTML needed to display person list. 

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/partials/personlist.html
  :language: html
  :linenos:
  :emphasize-lines: 3

The controller is defined by using the module's ``controller`` function in ``personListController.js``.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/personListController.js
  :language: javascript
  :linenos:
  :emphasize-lines: 1

If we run this application and navigate to the ``people/spa#/persons`` URL, we will see:

.. image:: angular/_static/spa-persons.png

If we navigate to a detail page, for example ``people/spa#/persons/2``, we will see the detail partial view: 

.. image:: angular/_static/spa-persons-2.png

You can view the full source and any files not shown in this article on `GitHub <https://github.com/aspnet/Docs/tree/master/aspnet/client-side/angular/sample>`_.

Event Handlers
^^^^^^^^^^^^^^

There are a number of directives in AngularJS that add event-handling capabilities to the input elements in your HTML DOM. Below is a list of the events that are built into AngularJS.

  - ``ng-click``
  - ``ng-dbl-click``
  - ``ng-mousedown``
  - ``ng-mouseup``
  - ``ng-mouseenter``
  - ``ng-mouseleave``
  - ``ng-mousemove``
  - ``ng-keydown``
  - ``ng-keyup``
  - ``ng-keypress``
  - ``ng-change``

.. note:: You can add your own event handlers using the `custom directives feature in AngularJS <https://docs.angularjs.org/guide/directive>`_.

Let's look at how the ``ng-click`` event is wired up. Create a new JavaScript file named ``eventHandlerController.js``, and add the following to it:

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/wwwroot/app/eventHandlerController.js
  :language: javascript
  :linenos:
  :emphasize-lines: 5-7

Notice the new ``sayName`` function in ``eventHandlerController`` on line 5 above. All the method is doing for now is showing a JavaScript alert to the user with a welcome message.

The view below binds a controller function to an AngularJS event. Line 9 has a button on which the ``ng-click`` Angular directive has been applied. It calls our ``sayName`` function, which is attached to the ``$scope`` object passed to this view.

.. literalinclude:: angular/sample/AngularSample/src/AngularSample/Views/People/Events.cshtml
  :language: html
  :linenos:
  :emphasize-lines: 9

The running example demonstrates that the controller's ``sayName`` function is called automatically when the button is clicked.

.. image:: angular/_static/events.png

For more detail on AngularJS built-in event handler directives, be sure to head to the `documentation website <https://docs.angularjs.org/api/ng/directive/ngClick>`_ of AngularJS.

Angular 2.0
-----------

Angular 2.0 is the next version of AngularJS, which is completely reimagined with ES6 and mobile in mind. It's built using Microsoft's TypeScript language. Angular 2.0 is currently a RC product and is expected to be released in early 2016. Several breaking changes will be introduced in the Angular 2.0 release, so the Angular team is working hard to provide guidance to developers. A migration path will become more clear as the release date approaches. If you wish to play with Angular 2.0 now, the Angular team has created `Angular.io <http://angular.io>`_ to show their progress, to provide early documentation, and to gather feedback. 

Summary
-------

This article provides an overview of AngularJS for ASP.NET developers. It aims to help developers who are new to this SPA framework get up-to-speed quickly.

Related Resources
-----------------

- `Angular Docs <https://docs.angularjs.org>`_
- `Angular 2 Info <http://angular.io>`_

