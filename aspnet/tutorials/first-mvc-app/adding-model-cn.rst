添加模型
==================================================
由 `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

此节中你将添加一些类来管理数据库中的影片. 这些类是 **M**\VC 中的 "**M**\odel" 部分.

你将会使用.NET Framework中名为 `Entity Framework Core <http://ef.readthedocs.org/>`__ 数据访问技术来定义并与这些数据模型类工作. Entity Framework Core (可以称为 **EF** Core) 引申出 *Code First* 的开发方式. 你先编写代码, 然后数据库表将会根据这些代码生成.
  Code First 允许你通过编写简单类创建数据模型对象 (也经常被称为POCO类, 全称"plain-old CLR objects.") 数据库根据你的类创建.
   如果你需要先创建数据库, 仍然可以根据此教程了解MVC和EF应用开发.

使用个人账户新建工程
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

ASP.NET Core MVC 当前版本的Visual Studio工具中, 模型脚手架只在个人账户新建工程中支持. 我们希望在下一版本的工具更新中修复这个问题. 直到这个问题修复前, 你需要用相同的名称新建项目. 因为只有项目同名我们才能在另一个目录中创建它.

从 Visual Studio **Start** 页面, 点击**New Project**.

.. image:: start-mvc/_static/new_project.png

同样, 你也可以使用菜单新建工程. 点击 **File > New > Project**.

.. image:: start-mvc/_static/alt_new_project.png

完成 **New Project** 对话框:

- 在左边的面板, 点击 **Web**
- 在中间面板, 点击 **ASP.NET Core Web Application (.NET Core)**
- 与之前的项目不同你需要修改位置到不同的路径, 否则会出错
- 将项目命名为 "MvcMovie" (将项目命名为 "MvcMovie" 才能保证命名空间匹配.)
- Tap **OK**

.. image:: start-mvc/_static/new_project2.png

.. Warning:: 在此版本发布中你必须要将 **Authentication** 设置为 **Individual User Accounts** 让脚手架引擎工作.

在 **New ASP.NET Core Web Application - MvcMovie** 对话框:

- 点击 **Web Application**
- 点击 **Change Authentication** 按钮然后修改认证模式到 **Individual User Accounts** 并点击 **OK**

.. image:: start-mvc/_static/p4.png

.. image:: adding-model/_static/indiv.png

遵照 :ref:`change-title-link-reference-label` 中的说明你可以点击 **MvcMovie** 链接调用Movie控制器. 我们将演示如何在影片控制器中应用脚手架.

添加数据模型类
--------------------------

在解决方案管理器中, 右键单击 *Models* folder > **Add** > **Class**. 命名类为 **Movie** 并添加如下属性:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieNoEF.cs
  :language: c#
  :lines: 4-16
  :dedent: 0
  :emphasize-lines: 7

给影片建模是你对属性的额外要求是 ``ID`` 是必须输入的数据库主键. 构建工程. 如果你不构建应用, 将会得到下面的错误.
 我们最后添加了一个模型(**M**\odel)到我们的 **M**\VC应用.

在控制器应用脚手架
--------------------------

在 **Solution Explorer**, 右键单击 *Controllers* folder **> Add > Controller**.

.. image:: adding-model/_static/add_controller.png

在 **Add Scaffold** 对话框, 点击 **MVC Controller with views, using Entity Framework > Add**.

.. image:: adding-model/_static/add_scaffold2.png

完成 **Add Controller** 对话框

- **Model class:** *Movie(MvcMovie.Models)*
- **Data context class:** *ApplicationDbContext(MvcMovie.Models)*
- **Views:**: 保持每个默认选项勾选
- **Controller name:** 保持默认的 *MoviesController*
- 点击 **Add**

.. image:: adding-model/_static/add_controller2.png

Visual Studio脚手架引擎创建如下:

- 影片控制器 (*Controllers/MoviesController.cs*)
- 创建, 删除, 详细, 编辑和索引(Index)Razor视图文件(*Views/Movies*)

Visual Studio自动创建了 `CRUD <https://en.wikipedia.org/wiki/Create,_read,_update_and_delete>`__ (创建, 读取, 更新, 和删除)行为方法和视图(CRUD行为方法和视图的自动创建就是我们熟知的 *scaffolding*). 你很快就拥有了完整功能的Web应用让你创建、罗列、编辑和删除影片实例.

如果你运行应用并点击 **Mvc Movie** 链接, 你将得到如下错误:

.. image:: adding-model/_static/m1.png

.. image:: adding-model/_static/pending.png

我们将按照下面的说明为我们的影片应用准备数据库.

更新数据库
-------------------------------------------

.. warning:: 更新数据库前你必须停止IIS Express.

.. _stop-IIS-Express-reference-label:

停止IIS Express:
^^^^^^^^^^^^^^^^^^^^^

- 右键点击通知区域的IIS Express系统托盘图标

 .. image:: working-with-sql/_static/iisExIcon.png
  :height: 100px
  :width: 200 px

- 点击 **Exit** 或者 **Stop Site**

.. image:: working-with-sql/_static/stopIIS.png

- 同样, 你可以退出并重启Visual Studio

.. To-do替换了PMC命令行提示. 它是更好的方法,不需要离开VS

- 在工程目录打开命令行提示(MvcMovie/src/MvcMovie). 按照说明快速打开工程文件夹.

  - 打开工程根目录下的文件(此例中使用 *Startup.cs*.)
  - 右键单击 *Startup.cs*  **> Open Containing Folder**.

|

  .. image:: adding-model/_static/quick.png

|

  - Shift + 右键单击文件夹 > **Open command window here**

|

  .. image:: adding-model/_static/folder.png

|

  - 执行 ``cd ..`` 回到工程目录

- 在命令行提示中运行如下命令:

.. code-block:: console

  dotnet ef migrations add Initial
  dotnet ef database update

.. note:: 如果IIS-Express在运行, 你会得到 *CS2012: Cannot open 'MvcMovie/bin/Debug/netcoreapp1.0/MvcMovie.dll' for writing -- 'The process cannot access the file 'MvcMovie/bin/Debug/netcoreapp1.0/MvcMovie.dll' because it is being used by another process.'* 错误

dotnet ef 命令
^^^^^^^^^^^^^^^^^^^

- ``dotnet`` (.NET Core) 是.NET的跨平台实现. 你可以在 `here <http://go.microsoft.com/fwlink/?LinkId=798644>`__ 阅读关于它的信息
- ``dotnet ef migrations add Initial`` 运行Entity Framework .NET Core CLI 迁移命令并创建初始化迁移.
 参数 "Initial" 可选, 但是第一次数据库迁移习惯使用(*initial*). 此操作创建 *Data/Migrations/<date-time>_Initial.cs* 文件, 其中包含迁移命令来添加(或废除)数据库中的 `Movie` 表.
- ``dotnet ef database update``  使用刚创建的迁移更新数据库


测试应用
------------------

.. note:: 如果你的浏览器不能连接影片应用可能需要等待IIS Express加载应用. 有时候需要大概30秒创建应用并让它响应请求.

- 运行应用点击 **Mvc Movie** 链接
- 点击 **Create New** 链接创建影片

.. image:: adding-model/_static/movies.png

.. note:: 你不能在 ``Price`` 字段中输入浮点数或者逗号. 为了支持 `jQuery validation <http://jqueryvalidation.org/>`__ 在非英语地区使用逗号(",")代表小数点, 和非美国英语日期格式, 你必须按步骤全球化你的应用. 更多信息参照 `Additional resources`_. 现在, 只输入整数如10.

.. _DisplayFormatDateLocal:

.. note:: 在一些地区你需要指定日期格式. 参照下方高亮代码.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieDateFormat.cs
  :language: c#
  :start-after: // Start
  :end-before:  */
  :emphasize-lines: 10,2


点击 **Create** 表单会发送给服务器, 影片信息被存储于数据库. 然后被重定向到 `/Movies` URL, 你将看到新建影片出现在列表中.

.. image:: adding-model/_static/h.png

多创建些影片信息. 尝试使用 **Edit**, **Details**, 和 **Delete** 链接, 它们已包含完整功能.

检查生成代码
---------------------------------

打开 *Controllers/MoviesController.cs* 文件检查生成的 ``Index`` 方法. 部分影片控制器的 ``Index`` 方法内容如下所示:

.. code-block:: c#

    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movie.ToListAsync());
        }


.. can't use this because we commenent out the initial index method and update it later
.. comment literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // The Movies Controller
  :end-before: // GET: Movies/Details/5
  :dedent: 4

构造方法使用 :doc:`Dependency Injection  </fundamentals/dependency-injection>` 将数据库上下文注入到控制器. 数据库上下文用于控制器中的所有 `CRUD <https://en.wikipedia.org/wiki/Create,_read,_update_and_delete>`__ 方法.

到影片控制器的请求返回所有 ``Movies`` 表的实体并且传递数据给 ``Index`` 视图.

.. _Strongly-typed-models-keyword-label:

强类型模型和@model关键字
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

前面教程, 你看到了控制器如何使用 ``ViewData`` 词典传递数据或对象到视图. ``ViewData`` 词典是提供了方便的延迟绑定方法将数据传递给视图的动态对象.

MVC也提供能力传递强类型对象到视图. 这个强类型方法提供更好的编译时检查和Visual Studio中更丰富的 `IntelliSense <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`__ 功能. VS中的脚手架机制在 ``MoviesController`` 类和视图创建方法或视图时、使用此方法(传递强类型模型).

Examine the generated ``Details`` method in the *Controllers/MoviesController.cs* file:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :start-after: // GET: Movies/Details/5
 :end-before: // GET: Movies/Create
 :dedent: 8

``id`` 参数一般作为路由数据传递, 例如 ``http://localhost:1234/movies/details/1`` 设置:

- 控制器指向 ``movies`` controller (第一URL片段)
- 行为指向 ``details`` (第二URL片段)
- id值1 (最后URL片段)

你也可以如下通过查询字符串传递 ``id``:

``http://localhost:1234/movies/details?id=1``

如果找到影片, ``Movie`` 模型的实例就传递给 ``Details`` 视图:

.. code-block:: C#

  return View(movie);

.. make a copy of details - later we add Ratings to it.

检查 *Views/Movies/Details.cshtml* 文件内容:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/DetailsOriginal.cshtml
 :language: HTML
 :emphasize-lines: 1

通过在视图文件顶部包含 ``@model`` 声明, 你可以指定视图期待的对象类型. 当你创建了影片控制器, Visual Studio自动地包含如下 ``@model`` 声明到 *Details.cshtml* 文件顶部:

.. code-block:: HTML

  @model MvcMovie.Models.Movie

这个 ``@model`` 指令允许你访问控制器使用 ``Model`` 强类型对象传递给视图的影片信息. 例如, 在 *Details.cshtml* 视图中, 代码通过 ``Model`` 强类型对象传递每个影片字段给 ``DisplayNameFor`` 和 ``DisplayFor`` HTML
 帮助类. ``Create`` 和 ``Edit`` 方法和视图也传递 ``Movie`` 模型对象.

检查 *Index.cshtml* 视图和影片控制器中的 ``Index`` 方法. 注意当代码调用视图方法时创建了 ``List``. 代码将此来自 ``Index`` 行为方法的 ``Movies`` 列表传递给视图:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :start-after:  // GET: Movies
 :end-before:  // End of first Index
 :dedent: 8

当你创建影片控制器时, Visual Studio自动地包含如下 ``@model`` 声明到 *Index.cshtml* 文件顶部:

.. Copy Index.cshtml to IndexOriginal.cshtml

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/IndexOriginal.cshtml
 :language: HTML
 :lines: 1

这个 ``@model`` 指令允许你访问控制器使用强类型 ``Model`` 对象传递给视图的影片列表. 例如, 在 *Index.cshtml* 视图中, 代码通过 ``foreach`` 语句循环遍历强类型 ``Model`` 对象组成的影片列表:

.. 将 Index.cshtml 拷贝成 IndexOriginal.cshtml

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/IndexOriginal.cshtml
  :language: HTML
  :emphasize-lines: 1,31, 34,37,40,43,46-48

因为强类型对象 ``Model`` (如 ``IEnumerable<Movie>`` 对象), 遍历的每个项目都是 ``Movie``. 这代表你可以获得代码的编译时检查和代码编辑器完整的 `IntelliSense <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`__ 支持:

.. image:: adding-model/_static/ints.png

你现在有了数据库和要显示的页面, 可以编辑、更新和删除数据. 在下一教程, 我们会讨论数据库.

附加资源
-----------------------

- :doc:`/mvc/views/tag-helpers/index`
- :doc:`/fundamentals/localization`
