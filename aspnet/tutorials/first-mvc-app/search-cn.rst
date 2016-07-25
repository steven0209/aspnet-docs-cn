添加搜索
==========================================

由 `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

此节你会添加搜索能力到 ``Index`` 行为方法, 让你能通过 *genre* 或者 *name* 搜索影片.

更新 ``Index`` 行为方法激活搜索:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // First Search
  :end-before: // End first Search
  :dedent: 8

 ``Index`` 行为方法的第一行创建了 `LINQ <http://msdn.microsoft.com/en-us/library/bb397926.aspx>`__ 查询选择影片:

.. code-block:: c#

    var movies = from m in _context.Movie
                 select m;

查询 *only* 在此处定义, 它 **has not** 在数据库上运行.

如果 ``searchString`` 参数包含字符串, 影片查询可以根据查询字符串的值进行过滤, 使用如下代码:

.. code-block:: c#
  :emphasize-lines: 3
  
     if (!String.IsNullOrEmpty(searchString))
     {
         movies = movies.Where(s => s.Title.Contains(searchString));
     }

上面的 ``s => s.Title.Contains()`` 代码是 `Lambda Expression <http://msdn.microsoft.com/en-us/library/bb397687.aspx>`__.
 Lambdas 是基于方法的 `LINQ <http://msdn.microsoft.com/en-us/library/bb397926.aspx>`__ 查询将参数传给标准查询操作符方法如 `Where <http://msdn.microsoft.com/en-us/library/system.linq.enumerable.where.aspx>`__ 方法或者上面代码中使用的 ``Contains`` 方法. LINQ定义时或调用如``Where``, ``Contains`` 或 ``OrderBy`` 方法时还不会执行. 取而代之, 查询执行是延迟的, 意味着表达式延迟计算直到预期结果通过迭代结束或者 ``ToListAsync`` 这样的方法被调用. 更多信息参考延迟查询执行, 参照 `Query Execution <http://msdn.microsoft.com/en-us/library/bb738633.aspx>`__.

.. Note:: `Contains <http://msdn.microsoft.com/en-us/library/bb155125.aspx>`__ 方法在数据库上运行, 并不是指上面的C#代码. 在数据库中, `Contains <http://msdn.microsoft.com/en-us/library/bb155125.aspx>`__ 映射到 `SQL LIKE <http://msdn.microsoft.com/en-us/library/ms179859.aspx>`__,
 它是大小写不敏感的.


导航到 ``/Movies/Index``. 追加查询字符串如 ``?searchString=ghost`` 到URL. 过滤后的影片将被显示出来.

.. image:: search/_static/ghost.png

如果更改了 ``Index`` 方法签名添加参数 ``id``, 参数 ``id`` 会自动匹配设置在 *Startup.cs* 中默认路由的 ``{id}`` 可选占位符.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Startup.cs
  :dedent: 12
  :emphasize-lines: 6
  :start-after: app.UseIdentity();
  :end-before: SeedData.Initialize(app.ApplicationServices);

你可以通过 **rename** 命令很快地将参数 ``searchString`` 重命名为 ``id``. 右键单击 ``searchString`` **> Rename**.

.. image:: search/_static/rename.png

重命名目标高亮.

.. image:: search/_static/rename2.png

修改为参数 ``id`` 并且所有出现 ``searchString`` 的地方都修改为 ``id``.

.. image:: search/_static/rename3.png

以前的 ``Index`` 方法:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // First Search
  :end-before: // End first Search
  :dedent: 8
  :emphasize-lines: 1,8

更新后的 ``Index`` 方法:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // Search ID
  :end-before: // End search ID
  :dedent: 8
  :emphasize-lines: 1,8

你现在可以用路由数据传递查询标题(URL片段)而不是查询字符串值.

.. image:: search/_static/g2.png

尽管如此, 你不能期待用户每次想查询影片都通过修改URL. 所以现在你要添加UI帮助他们过滤影片. 如果你改变了 ``Index`` 方法的签名测试如何传递路由绑定 ``ID`` 参数,
 改回来这样参数名字恢复成 ``searchString``:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // First Search
  :end-before: // End first Search
  :dedent: 8
  :emphasize-lines: 1

.. Index.cshtml不可以在 .rst 文件中引用并且只能用于测试代码.
  拷贝相关的 IndexXXX.cshtml 文件到 Index.cshtml 并测试.
  
打开 *Views/Movies/Index.cshtml* 文件, 然后添加如下高亮的 ``<form>`` 标记:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/IndexForm1.cshtml
  :language: HTML
  :start-after: @model IEnumerable<MvcMovie.Models.Movie>
  :end-before:  <thead>
  :emphasize-lines: 11-16

HTML ``<form>`` 标签使用 :doc:`Form Tag Helper </mvc/views/working-with-forms>`,
 所以当你提交表单时, 过滤字符串发送给影片控制器的 ``Index`` 行为. 保存更改然后测试过滤.

.. image:: search/_static/filter.png

这里没有你可能期待的 ``[HttpPost]`` 的重载 ``Index`` 方法, 因为方法没有改变应用状态, 只是过滤了数据.

你可以添加如下 ``[HttpPost] Index`` 方法.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :dedent: 8
  :emphasize-lines: 1
  :start-after: // Search Post
  :end-before:  // End SP

 ``notUsed`` 参数用来创建 ``Index`` 方法的重载. 我们将在本教程后面讨论.

如果你添加此方法, 行为调用者会匹配 ``[HttpPost] Index`` 方法, 且 ``[HttpPost] Index`` 方法运行结果如下图.

.. image:: search/_static/fo.png

尽管如此, 尽管你添加这个 ``[HttpPost]`` 到 ``Index`` 方法版本, 存在所有都实现的限制. 想想一下你想保存特殊查询到书签或者你像发送这个链接给朋友, 他们就可以通过点击看到影片的过滤. 
注意HTTP POST请求的URL与GET请求的URL相同 (localhost:xxxxx/Movies/Index) -- 在URL中并不存在查询信息. 查询字符串信息被以 `form field value <https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/Forms/Sending_and_retrieving_form_data>`__ 发送到服务器. 你可以通过 `F12 Developer tools <https://dev.windows.com/en-us/microsoft-edge/platform/documentation/f12-devtools-guide/>`__ 或者优秀的 `Fiddler tool <http://www.telerik.com/fiddler>`__ 检验. 开启 `F12 tool <https://dev.windows.com/en-us/microsoft-edge/platform/documentation/f12-devtools-guide/>`__:


点击 **http://localhost:xxx/Movies  HTTP POST 200** 行然后点击 **Body  > Request Body**.

.. image:: search/_static/f12_rb.png

你可以在请求主体中看见查询参数和 :doc:`XSRF </security/anti-request-forgery>` 令牌. 注意, 前面的教程提到过, :doc:`Form Tag Helper </mvc/views/working-with-forms>` 生成 :doc:`XSRF </security/anti-request-forgery>` 防伪造令牌. 我们没有修改数据, 所以我们没必要验证控制器方法中的令牌.

因为请求主体中的搜索参数并非来自URL, 你不能捕获查询信息保存书签或将它们分享给他人. 我们会通过指定请求为 ``HTTP GET`` 来实现这一目标. 请注意智能提示如何帮助我们更新标记.

.. image:: search/_static/int_m.png

.. image:: search/_static/int_get.png

注意 ``<form>`` 标签中的特殊字体标记. 此特殊字体表示标签由 :doc:`Tag Helpers </mvc/views/tag-helpers/intro>` 支持.

.. image:: search/_static/th_font.png

现在当你提交搜索, URL包含搜索查询字符串. 所有也会被转到 ``HttpGet Index`` 行为方法, 即使你也有 ``HttpPost Index`` 方法.

.. image:: search/_static/search_get.png

如下标记展示了 ``form`` 标签的变化:

.. code-block:: html

  <form asp-controller="Movies" asp-action="Index" method="get">

添加类型查询
------------------------

添加如下 ``MovieGenreViewModel`` 类到 *Models* 文件夹:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieGenreViewModel.cs
  :language: c#

影片种类视图模型将包含:

 - 影片列表
 - `SelectList <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Rendering/SelectList/index.html>`__ 包含种类列表. 这将允许用户从列表中选择种类.
 - ``movieGenre``, 其中包含选择的种类

将 ``Index`` 方法替换为如下代码:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :dedent: 8
  :start-after: // Search by genre.
  :end-before:  // End of genre search.

如下 ``LINQ`` 查询用来从数据库获取所有种类.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :dedent: 12
  :start-after: // Use LINQ to get list of genre's.
  :end-before: var movies = from m in _context.Movie
 
种类的 ``SelectList`` 保存不重复的种类 (我们不希望列表中有相同的种类).

.. code-block:: c#

   movieGenreVM.genres = new SelectList(await genreQuery.Distinct().ToListAsync())

添加按种类搜索到索引视图
--------------------------------------------

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/IndexFormGenre.cshtml
  :language: HTML
  :lines: 1-64
  :emphasize-lines: 1, 15-17,27,41

通过按种类搜索测试应用, 也可以通过影片标题或者两者一起进行搜索.