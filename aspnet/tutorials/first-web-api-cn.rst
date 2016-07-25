通过ASP.NET Core和Visual Studio创建你的第一个Web API
====================================================================

由 `Mike Wasson`_ and `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

HTTP并不只是为Web页面服务. 也是强大的平台来创建曝露服务和数据的API. HTTP简单、灵活、应用广泛. 几乎所有你能想到的平台都有HTTP类库, 所以HTTP服务有广泛的用户群, 包括浏览器, 移动设备, 和传统桌面应用.

在此教程中, 你会构建简单的Web API来管理"待办"列表项目. 此教程不包含任何UI构建.

ASP.NET Core对MVC构建Web API提供内建支持. 统一了两个框架让创建包含UI(HTML)和APIs更容易, 因为它们共享相同的代码基线和管道.

.. note:: 如果你打算引入既存的Web API应用到ASP.NET Core, 参照 :doc:`/migration/webapi`

.. contents:: Sections:
  :local:
  :depth: 1

概述
--------
以下是你将会创建的API:

=====================  ========================  ============  =============
API                    描述                      请求体         响应体
=====================  ========================  ============  =============
GET /api/todo          Get all to-do items       None          Array of to-do items
GET /api/todo/{id}     Get an item by ID         None          To-do item
POST /api/todo         Add a new item            To-do item    To-do item
PUT /api/todo/{id}     Update an existing item   To-do item    None
DELETE /api/todo/{id}  Delete an item.           None          None
=====================  ========================  ============  =============

如下图表展示了应用基础设计.

.. image:: first-web-api/_static/architecture.png

- 无论什么客户端都消费Web API(浏览器, 移动应用, 等等). 此教程我们不编写客户端.
- *model* 是应用中展示数据的对象. 此例中, 唯一的模型是待办项目. 模型使用简单的C#类(POCO).
- *controller* 是处理HTTP请求和创建HTTP响应的对象. 此应用将有一个简单的控制器.
- 为了保持教程简单应用不使用数据库. 只在内存中保存待办项目. 但是我们也会包含(不太要的)数据访问层, 来说明Web API和数据层的分离. 使用数据库的教程, 参照 :doc:`first-mvc-app/index`.

安装Fiddler
---------------

我们不构建客户端, 我们会使用 `Fiddler <http://www.fiddler2.com/fiddler2/>`__ 来测试API. Fiddler是Web调试工具让你创建HTTP请求并且查看未加工的HTTP响应.

创建工程
------------------

启动Visual Studio. 从 **File** 菜单, 选择 **New** > **Project**.

选择 **ASP.NET Core Web Application** 工程模板. 命名工程为 ``TodoApi`` 后点击 **OK**.

.. image:: first-web-api/_static/new-project.png

在 **New ASP.NET Core Web Application (.NET Core) - TodoApi** 对话框中, 选择 **Web API** 模板. 点击 **OK**.

.. image:: first-web-api/_static/web-api-project.png

添加模型类
-----------------

模型是应用中表示数据的对象. 此例中, 唯一的模型是待办事项.

添加一个文件夹命名为"Models". 在Solution Explorer中, 右键单击工程. 选择 **Add** > **New Folder**. 命名文件夹 *Models*.

.. image:: first-web-api/_static/add-folder.png

.. note:: 你能将模型类放于工程的任何地方, 但使用 *Models* 文件夹是约定俗成的.

接下来, 添加 ``TodoItem`` 类. 右键单击 *Models* 文件夹并选择 **Add** > **New Item**.

在 **Add New Item** 对话框中, 选择 **Class** 模板. 命名类为 ``TodoItem`` 后点击 **OK**.

.. image:: first-web-api/_static/add-class.png

替换生成的代码:

.. literalinclude:: first-web-api/sample/src/TodoApi/Models/TodoItem.cs
  :language: c#

添加仓库类
----------------------

*repository* 是封装数据层的对象, 并且包含查询数据和映射数据到实体模型的逻辑. 尽管示例应用不适用数据库, 了解如何将仓库注入控制器对你会非常有帮助. 在 *Models* 中创建仓库类.

由定义名为 ``ITodoRepository`` 的仓库接口开始. 使用类模板 (**Add New Item**  > **Class**).

.. literalinclude:: first-web-api/sample/src/TodoApi/Models/ITodoRepository.cs
  :language: c#

此接口定义了基本的CRUD操作.

接下来, 添加 ``TodoRepository`` 类来实现 ``ITodoRepository``:

.. literalinclude:: first-web-api/sample/src/TodoApi/Models/TodoRepository.cs
  :language: c#

构建应用验证没有编译错误.

注册仓库
-----------------------

通过定义仓库接口, 我们能从使用它的MVC控制器中将其解耦.
 取代在控制器中实例化 ``TodoRepository`` 我们会采用ASP.NET Core 内建支持的 :doc:`dependency injection </fundamentals/dependency-injection>` 注入 ``ITodoRepository`` .

此方法让它能更简单地对控制器进行单元测试. 单元测试应该注入 ``ITodoRepository`` 的Mock或者Stub版本. 那样, 测试就只关心控制器逻辑而不是数据访问层.

为了将仓库注入到控制器, 我们需要将其注册到DI容器. 打开 *Startup.cs* 文件. 使用指令添加如下代码:

.. code-block:: c#

  using TodoApi.Models;

在 ``ConfigureServices`` 方法中, 添加高亮代码:

.. literalinclude:: first-web-api/sample/src/TodoApi/Startup.cs
  :language: c#
  :lines: 13-23
  :emphasize-lines: 9-10
  :dedent: 8

添加控制器
----------------

在Solution Explorer中, 右键单击 *Controllers* folder. Select **Add** > **New Item**. 在 **Add New Item** 对话框,
 选择 **Web  API Controller Class** 模板. 命名类为 ``TodoController``.

如下替换生成代码:

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 1-14,67-68

此处定义一个空的控制器类. 在下节, 我们会添加方法实现API.

取得待办事项
-------------------

为了取得待办事项, 添加如下方法到 ``TodoController`` 类.

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 17-31
  :dedent: 8

这些方法实现了两个GET方法:

- ``GET /api/todo``
- ``GET /api/todo/{id}``

这有对 ``GetAll`` 方法的HTTP响应示例:

  HTTP/1.1 200 OK
  Content-Type: application/json; charset=utf-8
  Server: Microsoft-IIS/10.0
  Date: Thu, 18 Jun 2015 20:51:10 GMT
  Content-Length: 82

  [{"Key":"4f67d7c5-a2a9-4aae-b030-16003dd829ae","Name":"Item1","IsComplete":false}]

接下来的教程我会展示如何使用Fiddler工具查看HTTP响应.

路由和和URL路径
^^^^^^^^^^^^^^^^^^^^^

`[HttpGet] <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/HttpGetAttribute/index.html>`_ 
特性指定了这些是HTTP GET方法. 每个方法的URL路径如下构成:

- 在控制器路由特性中使用模板字符串,  ``[Route("api/[controller]")]``
- 将 "[Controller]" 替换为控制器名字, 控制器名称使用 "Controller" 作为结尾. 此例控制器 "todo" (大小写不敏感). 为这个示例, 控制器类名是 **Todo**\Controller 并且根名字是"todo". ASP.NET MVC Core不是大小写敏感的.
- 如果 ``[HttpGet]`` 特性也有模板字符穿, 追加到路径. 此示例没有使用模板字符串.

对于 ``GetById`` 方法,  "{id}"是占位符变量. 在实际的HTTP请求中, 客户端将会使用 ``todo`` ID. 在运行时, 当MVC调用 ``GetById``, 它分配URL中 "{id}" 的值到方法的 ``id`` 参数.

返回值
^^^^^^^^^^^^^

``GetAll`` 方法返回CLR对象. MVC自动地序列化对象为 `JSON <http://www.json.org/>`__ 并将JSON写入到响应消息的主体.
 此方法的响应代码是200, 表示没有未处理异常. (未处理异常会显示5xx错误.)

相比而言, ``GetById`` 方法返回更多的是 ``IActionResult`` 类型, 代表通用返回类型. 这是因为 ``GetById`` 有两个不同的返回类型:

- 如果没有项目符合请求的ID, 方法返回404错误.  这时返回 ``NotFound``.
- 否则, 方法返回200和JSON响应体. 这时返回 `ObjectResult <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ObjectResult/index.html>`_.

使用Fiddler调用API
---------------------------

此步骤可选, 但是对于从Web API查看未处理的HTTP响应非常有帮助.
在Visual Studio里, 按 ^F5 启动应用. Visual Studio启动浏览器并导航到 ``http://localhost:port/api/todo``, 其中 *port* 是随机选定的端口号.
 如果你使用Chrome, Edge 或者 Firefox, *todo* 数据会显示出来. 如果你使用IE, IE会提示你打开或保存 *todo.json* 文件.

启动Fiddler. 从 **File** 菜单, 取消 **Capture Traffic** 选项. 这会关闭HTTP流量捕捉.

.. image:: first-web-api/_static/fiddler1.png

选择 **Composer** 页面. 在 **Parsed** 标签页, 输入 ``http://localhost:port/api/todo``, 这里 *port* 是端口号. 点击 **Execute** 发送请求.

.. image:: first-web-api/_static/fiddler2.png

结果显示在会话列表里. 响应代码应该是200. 使用 **Inspectors** 标签查看响应内容, 包括响应体.

.. image:: first-web-api/_static/fiddler3.png

实现其它CRUD操作
------------------------------------

最后一步是向控制器添加 ``Create``, ``Update``, 和 ``Delete`` 方法. 这些方法不尽相同, 所以我仅展示代码并高亮主要差别.

创建
^^^^^^

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 33-42
  :dedent: 8

这是HTTP POST方法, 由 `[HttpPost] <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/HttpPostAttribute/index.html>`_ 特性指明. `[FromBody] <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/FromBodyAttribute/index.html>`_ 特性告诉MVC从HTTP请求主体取得待办事项.

`CreatedAtRoute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Controller/index.html>`_ 方法返回201响应,
 是HTTP POST方法的标准响应创建新的资源给服务器. 
 ``CreateAtRoute`` 也向响应添加了位置标头. 位置标头指示了新建待办事项的URI. 参照 `10.2.2 201 Created <http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html>`_.

我们能使用Fiddler发送创建请求:

#.  在 **Composer** 页面, 从下来菜单选择POST.
#.  在请求标头文本框中, 添加 ``Content-Type: application/json``, ``Content-Type`` 标头带有值 ``application/json``. Fiddler自动地添加Content-Length标头.
#.  在请求主体文本框中, 输入如下内容: ``{"Name":"<your to-do item>"}``
#.  点击 **Execute**.

.. image:: first-web-api/_static/fiddler4.png


这是一个示例HTTP会话. 使用 **Raw** 标签页查看如下格式的会话数据.

请求::

  POST http://localhost:29359/api/todo HTTP/1.1
  User-Agent: Fiddler
  Host: localhost:29359
  Content-Type: application/json
  Content-Length: 33

  {"Name":"Alphabetize paperclips"}

响应::

  HTTP/1.1 201 Created
  Content-Type: application/json; charset=utf-8
  Location: http://localhost:29359/api/Todo/8fa2154d-f862-41f8-a5e5-a9a3faba0233
  Server: Microsoft-IIS/10.0
  Date: Thu, 18 Jun 2015 20:51:55 GMT
  Content-Length: 97

  {"Key":"8fa2154d-f862-41f8-a5e5-a9a3faba0233","Name":"Alphabetize paperclips","IsComplete":false}


更新
^^^^^^

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 44-60
  :dedent: 8

``Update`` 与 ``Create`` 近似, 但是使用HTTP PUT. 响应是 `204 (No Content) <http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html>`_.
根据HTTP规范, PUT请求要求客户端发送完整的更新后实体, 而不是增量. 要支持部分更新, 使用HTTP PATCH.

.. image:: first-web-api/_static/put.png

删除
^^^^^^

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 62-68
  :dedent: 8

void返回类型返回204(无内容)响应. 这意味着客户端会收到204尽管项目已经被删除, 又或者根本不存在. 有两种情况请求删除不存在的资源:

- "Delete"意味着"delete an existing item", 然而项目不存在, 所以返回404.
- "Delete"意味着"ensure the item is not in the collection." 项目已经不在集合中, 所以返回 204.

尽管此方法可行. 如果你返回404, 客户端需要相应处理.

.. image:: first-web-api/_static/delete.png

下一步
----------

- To learn about creating a backend for a native mobile app, see :doc:`/mobile/native-mobile-backend`.
- For information about deploying your API, see :doc:`Publishing and Deployment </publishing/index>`.
- `View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/tutorials/first-web-api/sample>`__



