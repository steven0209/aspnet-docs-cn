控制器方法和视图
=================================================

由 `Rick Anderson`_ 编辑

影片应用是很好的开始, 但是表示层却不是很令人满意. 我不想看到时间 (12:00:00 AM in the image below) 和 **ReleaseDate** 差太多.

.. image:: working-with-sql/_static/m55.png

打开 *Models/Movie.cs* 文件添加如下高亮行:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieDate.cs
  :language: c#
  :lines: 8-18
  :emphasize-lines: 6-7
  :dedent: 4

- 邮件单击红色波浪线行 **> Quick Actions**.

 .. image:: controller-methods-views/_static/qa.png

- 点击 ``using System.ComponentModel.DataAnnotations;``

 .. image:: controller-methods-views/_static/da.png

在Visual studio中添加 ``using System.ComponentModel.DataAnnotations;``.

删除不再需要的 ``using`` 语句. 它们以灰色字体呈现. 邮件单击 *Movie.cs* 文件的任意部分 **> Organize Usings > Remove Unnecessary Usings**.

.. image:: controller-methods-views/_static/rm.png

更新的代码:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieDate.cs
  :language: c#
  :lines: 3-19

.. TO-DO 下一版本替换 DataAnnotations 的ASP.NET 5版本链接

下一教程中我们将覆盖 `DataAnnotations <http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx>`__. `Display <https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.displayattribute.aspx>`__ 特性 指示字段的显示名称(此例中用"Release Date"代替"ReleaseDate"). `DataType <https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.datatypeattribute.aspx>`__ 特性指示数据类型, 此例中是日期类型, 所以字段里的时间信息不表示.

浏览 ``Movies`` 控制器按住 **Edit** 链接上的指针查看目标URL.

.. image:: controller-methods-views/_static/edit7.png

**Edit**, **Details**, 和**Delete**链接通过MVC Core Anchor Tag Helper生成在 *Views/Movies/Index.cshtml* 文件里.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/IndexOriginal.cshtml
  :language: HTML
  :lines: 45-49
  :dedent: 12
  :emphasize-lines: 2-4

:doc:`Tag Helpers </mvc/views/tag-helpers/intro>` 让Razor文件中的服务器端代码参与创建和渲染HTML元素, :dn:class:`~Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper`
 从控制器行为方法和路由ID动态地生成HTML的 ``href`` 特性值. 使用浏览器的 **View Source** 或者 **F12** 工具检查生成的标记. **F12** 工具如下所示.

.. image:: controller-methods-views/_static/f12.png

重新调用 *Startup.cs* 文件中的路由集合格式.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Startup.cs
  :dedent: 12
  :emphasize-lines: 6
  :start-after: app.UseIdentity();
  :end-before: SeedData.Initialize(app.ApplicationServices);

ASP.NET Core 将 ``http://localhost:1234/Movies/Edit/4`` 翻译成请求执行  ``Movies`` 控制器的 ``Edit`` 行为方法并传递值为4的参数 ``ID``.(控制器方法也称为行为方法)

:doc:`/mvc/views/tag-helpers/index` 是ASP.NET Core众多新特性之一. 更多信息参照 `Additional resources`_.

打开 ``Movies`` 控制器检查两个 ``Edit`` 行为方法:

.. image:: controller-methods-views/_static/1.png

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // GET: Movies/Edit/5
  :end-before: // POST: Movies/Edit/5
  :dedent: 8

.. 此方法在我们添加评级后进行更新(添加到绑定)

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // Edit Post 
  :end-before:  // End of Edit Post
  :dedent: 8

``[Bind]`` 特性是保护不发生 `over-posting <http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost>`__ 的主要途径. 你应该只在你要更改的属性上添加 ``[Bind]`` 特性.
 更多信息参照 `Protect your controller from over-posting <http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost>`__. `ViewModels <http://rachelappel.com/use-viewmodels-to-manage-data-amp-organize-code-in-asp-net-mvc-applications/>`__ 提供同样的方法阻止过度提交.


注意第二个 ``Edit`` 行为方法被冠以 ``[HttpPost]`` 特性.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // Edit Post 
  :end-before:  // End of Edit Post
  :emphasize-lines: 1
  :dedent: 8

:dn:class:`~Microsoft.AspNetCore.Mvc.HttpPostAttribute` 特性指定 ``Edit`` 方法 *only* 可以被调用 ``POST`` 请求. 你可以添加 ``[HttpGet]`` 特性到第一个编辑方法, 但也不是必要的, 因为 ``[HttpGet]`` 是默认特性.

:dn:class:`~Microsoft.AspNetCore.Mvc.ValidateAntiForgeryTokenAttribute` 特性用来阻止请求伪造随着编辑视图文件生成了一个防伪造令牌 (*Views/Movies/Edit.cshtml*). 编辑视图文件生成通过 :doc:`Form Tag Helper </mvc/views/working-with-forms>` 生成防伪造令牌.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/Edit.cshtml
  :language: HTML
  :lines: 9

:doc:`Form Tag Helper </mvc/views/working-with-forms>` 生成隐藏的防伪造令牌必须匹配影片控制器 ``Edit`` 上 ``[ValidateAntiForgeryToken]`` 特性生成的放伪造令牌. 更多信息参照 :doc:`/security/anti-request-forgery`.

``HttpGet Edit`` 方法获取 ``ID`` 参数, 使用 Entity Framework ``SingleOrDefaultAsync`` 方法查询影片, 然后返回选择的影片给编辑视图. 如果影片找不到, 返回 ``NotFound`` (HTTP 404).

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // GET: Movies/Edit/5
  :end-before: // POST: Movies/Edit/5
  :dedent: 8

当脚手架系统创建编辑视图后, 它会检查 ``Movie`` 类并创建代码渲染生成每个类属性对应的 ``<label>`` 和 ``<input>`` 元素. 下面的例子展示了由Visual Studio脚手架系统生成的编辑视图:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/EditCopy.cshtml
  :language: HTML
  :emphasize-lines: 1

注意视图模板在文件顶部声明 ``@model MvcMovie.Models.Movie`` — 这表示视图期望视图模板绑定的模型类型为 ``Movie``.

脚手架生成代码使用一些Tag Helper方法简化HTML标记. :doc:`Label Tag Helper </mvc/views/working-with-forms>` 显示字段名称 ("Title", "ReleaseDate", "Genre", or "Price"). :doc:`Input Tag Helper </mvc/views/working-with-forms>` 渲染HTML ``<input>`` 元素. :doc:`Validation Tag Helper </mvc/views/working-with-forms>` 显示所有与属性相关的验证消息.

运行应用导航到 ``/Movies`` URL. 点击 **Edit** 链接. 在浏览器中, 查看页面源文件. 为 ``<form>`` 元素生成的HTML如下所示.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Shared/edit_view_source.html
  :language: HTML
  :emphasize-lines: 1,6,10,17,24, 28

``<input>`` 元素在 ``HTML <form>`` 元素中由 ``action`` 特性标记用来发送数据给 ``/Movies/Edit/id`` URL.  ``Save`` 按钮点击后表单数据将会发送给服务器
. ``</form>`` 元素前面的最后一行展示了隐藏的由 :doc:`Form Tag Helper </mvc/views/working-with-forms>` 生成的 :doc:`XSRF </security/anti-request-forgery>` 令牌.

处理POST请求
--------------------------------------

如下列表展示了带有 ``[HttpPost]`` 特性的 ``Edit`` 行为方法.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // Edit Post 
  :end-before:  // End of Edit Post
  :dedent: 8
  :emphasize-lines: 1,2,10,14,15,28

``[ValidateAntiForgeryToken]`` 特性验证隐藏的在 :doc:`Form Tag Helper </mvc/views/working-with-forms>` 中由防伪造令牌生成器生成的 `XSRF <:doc:/security/anti-request-forgery>`__ 令牌 in the :doc:`Form Tag Helper </mvc/views/working-with-forms>`

 :doc:`model binding </mvc/models/model-binding>` 系统获得发送的表单值并创建一个 ``Movie`` 对象来传递 ``movie`` 参数. ``ModelState.IsValid`` 方法检验表单提交数据用来修改 (编辑或更新) ``Movie`` 对象. 如果保存的数据有效, 更新的 (编辑的) 影片数据 将通过调用数据库上下文的 ``SaveChangesAsync`` 方法保存到数据库.
  保存数据后, 代码重定向用户到 ``MoviesController`` 类的 ``Index`` 行为方法, 它展示了影片集合, 包括刚刚做的更改.

在表单提交到服务器前, 客户端验证检查字段上的所有验证规则. 如果存在验证错误, 显示错误消息取消表单提交. 如果JavaScript禁用, 不会有任何客户端验证但是服务器端会检查提交的数据是否有效, 然后表单值将与错误消息一起表示出来.
   后面的教程我们会讨论 :doc:`/mvc/models/validation` 验证的细节.在 *Views/Book/Edit.cshtml* 视图模板中的 :doc:`Validation Tag Helper </mvc/views/working-with-forms>` 负责显示对应的错误信息.

.. image:: controller-methods-views/_static/val.png

影片控制器中所有的 ``HttpGet`` 方法都遵照相似的模式. 获得影片数据 (或者对象列表, 例如从 ``Index`` 方法乎获得), 然后传递对象 (模型) 给视图. ``Create`` 方法传递空的影片对象给 ``Create`` 视图. 所有创建、编辑、删除或者修改数据的方法都是 ``[HttpPost]`` 的方法重载. 在HTTP GET方法中修改数据有安全风险, 如同在 `ASP.NET MVC Tip #46 – Don’t use Delete Links because they create Security Holes <http://stephenwalther.com/blog/archive/2009/01/21/asp.net-mvc-tip-46-ndash-donrsquot-use-delete-links-because.aspx>`__ 中提到的. 在 ``HTTP GET`` 方法中修改数据也违反了HTTP最佳实践和 `REST <http://rest.elkstein.org/>`__ 的架构模式, 后者定义了GET请求不应该改变应用的状态. 换句话说, 实施GET操作应该更小心而不能有负面影响或修改你的持久化数据.

额外的资源
-----------------------

.. To-Do put label heading in working with forms then link to the exact TH

- :doc:`/fundamentals/localization`
- :doc:`/mvc/views/tag-helpers/intro`
- :doc:`/mvc/views/tag-helpers/authoring`
- :doc:`/security/anti-request-forgery`
- Protect your controller from `over-posting <http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost>`__
- `ViewModels <http://rachelappel.com/use-viewmodels-to-manage-data-amp-organize-code-in-asp-net-mvc-applications/>`__
- :doc:`Form Tag Helper </mvc/views/working-with-forms>`
- :doc:`Input Tag Helper </mvc/views/working-with-forms>`
- :doc:`Label Tag Helper </mvc/views/working-with-forms>`
- :doc:`Select Tag Helper </mvc/views/working-with-forms>`
- :doc:`Validation Tag Helper </mvc/views/working-with-forms>`
