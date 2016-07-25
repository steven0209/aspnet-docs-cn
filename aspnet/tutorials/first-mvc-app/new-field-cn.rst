添加新字段
===============================================

由 `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

此节中你将使用 `Entity Framework <http://docs.efproject.net/en/latest/platforms/aspnetcore/new-db.html>`__ 代码优先迁移(Code First Migrations)通过添加新字段到模型更新数据库.

当你使用EF代码优先自动创建数据库, 代码优先添加数据库表并跟踪数据库架构与生成它的模型类同步. 如果两者不同步, EF抛出异常. 这让我们在开发时很容易找出问题否则就只能在运行时才会发现(通过隐蔽的错误).

添加评级属性到影片模型
---------------------------------------------

打开 *Models/Movie.cs* 文件添加 ``Rating`` 属性:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieDateRating.cs
  :language: c#
  :lines: 7-18
  :dedent: 4
  :emphasize-lines: 11

构建应用 (Ctrl+Shift+B).

因为你添加了新字段到 ``Movie`` 类, 你就需要更新绑定白名单包含新字段. 更新 ``Create`` 和 ``Edit`` 行为方法上的 ``[Bind]`` 特性包含 ``Rating`` 属性:

.. code-block:: c#

 [Bind("ID,Title,ReleaseDate,Genre,Price,Rating")]

你还需要更新视图模板以更新浏览器视图中的表示, 创建和编辑新的 ``Rating`` 属性.

编辑文件 */Views/Movies/Index.cshtml* 添加 ``Rating`` 字段:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/IndexGenreRating.cshtml
  :language: HTML
  :emphasize-lines: 16,37
  :lines: 24-61

在 */Views/Movies/Create.cshtml* 中更新 ``Rating`` 字段. 你可以拷贝/粘贴以前的 "表单组" 并让智能提示帮助你更新字段. 通过 :doc:`Tag Helpers </mvc/views/tag-helpers/intro>` 使用智能提示.

.. image:: new-field/_static/cr.png

只有更新数据库包含新字段后应用才能工作. 如果现在运行, 会得到数据库 ``SqlException`` 异常:

.. image:: new-field/_static/se.png

出现错误的原因是更新的影片模型类与数据库中现存的影片表架构不同. (数据库表中没有评级列.)

有一部分方法可以解决错误:

#. 让Entity Framework自动地删除并基于新的模型类架构重建数据库. 此方法在开发周期的前期使用测试数据库时还很方便; 它允许你快速同步模型和数据库架构. 尽管如此缺点是你会失去数据库中的既存数据 —
 所以你不能再生产数据库上使用此方法! 使用初始化器自动地初始化数据库并导入测试数据对应用开发则更为有效.

#. 显式地修改既存数据库架构让它匹配模型类. 此方法有点是会保留你的数据. 你可以手动更新或通过创建数据库变更脚本.

#.使用代码优先迁移特性更新数据库架构.

此教程我们将使用代码优先迁移.

更新 ``SeedData`` 类让它提供给新列的值. 变更示例如下, 但你希望此变更是对所有 ``新影片`` 的.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/SeedDataRating.cs
  :language: c#
  :start-after: context.Movie.AddRange(
  :end-before: // Add another movie.
  :dedent: 16
  :emphasize-lines: 6

.. warning:: 必须在运行 ``dotnet ef`` 命令前停止 IIS Express. 参照 :ref:`stop-IIS-Express-reference-label`

构建解决方案然后打开命令行提示. 输入如下命令:

.. code-block:: console

  dotnet ef migrations add Rating
  dotnet ef database update

``migrations add`` 命令告诉迁移框架检查现在的 ``Movie`` 模型与当前的 ``Movie`` 数据库架构然后创建必要的代码来迁移数据库匹配新的模型. 名字"Rating"是随意起的, 同样也用来命名迁移文件. 使用有意义的名字对迁移来说会非常有帮助.

如果你删除了数据库中所有记录, 初始化器将会初始化数据库并添加 ``Rating`` 字段. 你可以通过浏览器中的删除链接或从SSOX执行操作.

运行应用检验创建/编辑/显示影片的功能是否带有 ``Rating`` 字段. 你还应该添加 ``Rating`` 字段到 ``Edit``, ``Details``, 和 ``Delete`` 视图模板.