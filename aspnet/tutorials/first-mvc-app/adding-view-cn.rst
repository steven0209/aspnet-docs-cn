添加视图
================================================

由 `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

本节你会使用Razor视图模板文件修改 ``HelloWorldController`` 类干净地封装生成HTML响应给客户端的过程.

你将使用Razor创建视图模板文件. 基于Razor的视图模板以 *.cshtml* 为文件后缀, 并提供优雅的使用C#创建HTML输出的方法. Razor无缝结合C#和HTML, 最小化编写视图模板的字符数量, 提供快速流畅的编码工作流.

当前 ``Index`` 方法返回硬编码在控制器类中的消息字符串. 改变 ``Index`` 方法返回视图(View)对象, 如下代码所示:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: c#
  :lines: 102-105
  :dedent: 8

 ``Index`` 方法使用视图模板生成HTML响应给浏览器. 控制器方法 (行为方法) 如 ``Index`` 方法, 一般返回 ``IActionResult`` (或者是继承 ``ActionResult`` 的类), 而并非如String的原始类型.

- 右键单击 *Views* 文件夹, 然后选择 **Add > New Folder** 并命名文件夹为 *HelloWorld*.
- 右键点击 *Views/HelloWorld* 文件夹, 然后选择 **Add > New Item**.
- 在 **Add New Item - MvcMovie** 对话框中

  - 在右上的查询框中输入 *view*
  - 单击 **MVC View Page**
  - 在 **Name** 框中, 保持默认值 *Index.cshtml*
  - 单击 **Add**

.. image:: adding-view/_static/add_view.png

将 *Views/HelloWorld/Index.cshtml* Razor视图文件替换为如下内容:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/HelloWorld/Index.cshtml
  :language: HTML

导航至 ``http://localhost:xxxx/HelloWorld``. ``HelloWorldController`` 中的 ``Index`` 方法没有什么用处; 只是运行语句 ``return View();``, 指定方法应该返回视图模板文件渲染到浏览器的响应. 因为没有显式指定视图模板文件名, MVC 默认使用 */Views/HelloWorld* 文件夹中的 *Index.cshtml* 视图文件. 下图展示视图中硬编码了字符串 "Hello from our View Template!".

.. image:: adding-view/_static/hell_template.png

如果浏览器窗口很小 (例如在移动设备上), 你可能需要单击右上方的 `Bootstrap navigation button <http://getbootstrap.com/components/#navbar>`__ 查看 **Home**, **About**, 和 **Contact** 链接.

.. image:: adding-view/_static/1.png

改变视图和布局文件
--------------------------------------

单击菜单链接 (**MvcMovie**, **Home**, **About**). 每个页面展示了相同的菜单布局. 菜单布局是在 *Views/Shared/_Layout.cshtml* 文件中实现的. 打开 *Views/Shared/_Layout.cshtml* 文件.

:doc:`Layout </mvc/views/layout>` 模板允许在一个地方指定站点的HTML容器布局然后将它应用到站点的多个页面.
 找到 ``@RenderBody()`` 行. ``RenderBody`` 是占位符, 所有你创建的视图指定页面, 都"包装"在布局页面. 例如, 如果你选择 **About** 链接, **Views/Home/About.cshtml** 视图将在 ``RenderBody`` 方法中渲染.

.. _change-title-link-reference-label:

改变布局文件的标题和菜单链接
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

改变标题元素内容. 改变布局模板中的锚点文本为 "MVC Movie" 并且将控制器 ``Home`` 修改为 ``Movies`` 如下高亮所示:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/Shared/_Layout.cshtml
  :language: HTML
  :emphasize-lines: 29,6
  :lines: 1-46

.. warning:: 我们没有实现 ``Movies`` 控制器, 所以如果你单击链接, 你将会得到404(找不到)错误.

保存修改后单击 **About** 链接. 注意每个页面如何表示 **Mvc Movie** 链接. 我们可能在布局模板中改变一次然后站点所有的页面都会反映到新的链接文本和新的标题.

检查 *Views/_ViewStart.cshtml* 文件:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/_ViewStart.cshtml
  :language: HTML

*Views/_ViewStart.cshtml* 文件设置每个视图中都引用 *Views/Shared/_Layout.cshtml* 文件. 你可以使用 ``Layout`` 属性设置不同的布局视图, 或设置为 ``null`` 指定不适用布局文件.

现在让我们修改 ``Index`` 视图的标题.

打开 *Views/HelloWorld/Index.cshtml*. 有两个地方需要修改:

 - 浏览器标题上显示的文本
 - 次要头部标题 (``<h2>`` 元素).

你可以让他们稍有不同这样你可以看到代码修改的是应用的哪个部分.

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/HelloWorld/Index2.cshtml
  :language: HTML
  :emphasize-lines: 2, 5

上面代码中的 ``ViewData["Title"] = "Movie List";`` 设置 :dn:class:`~Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary` 的 ``Title`` 属性为 "Movie List". ``Title`` 属性用于布局页中的 ``<title>`` HTML元素:

.. code-block:: HTML

  <title>@ViewData["Title"] - Movie App</title>

保存你的修改刷新页面. 注意浏览器标题, 主要标题, 和次要标题都已被修改. (如果没看到浏览器中的修改, 你可能正在查看缓存的内容. 在浏览器中按 Ctrl+F5 重新加载服务器响应内容.) 浏览器标题通过 ``ViewData["Title"]`` 创建我们在 *Index.cshtml* 视图模板中设置并添加 "- Movie App" 到布局文件.

还要注意 *Index.cshtml* 视图模板的内容与 *Views/Shared/_Layout.cshtml* 视图模板合并并且给浏览器返回单独的HTML响应. 布局模板使得跨应用所有页面进行修改变得容易. 更多参照 :doc:`/mvc/views/layout`.

.. image:: adding-view/_static/hell3.png

尽管我们的少量数据 "data" (此例中的 "Hello from our View Template!" 消息) 是硬编码的. MVC应用有"V"(视图) 而你有"C" (控制器), 但是还没有 "M" (模型).
 稍后, 我们会讨论如何创建数据库和从中查询模型数据.

从控制器传递数据给视图
----------------------------------------------

在我们考虑数据库前我们先聊聊模型, 让我们先考虑从控制器传递信息给视图. 调用控制器行为响应进入的URL请求. 控制器类是编码处理进入的浏览器请求, 从数据库查询数据,
 然后决定将什么类型的响应发送回浏览器. 视图模板可以被用来从控制器生成和组织发送给浏览器的HTML响应.

控制器的职责是提供任何需要的数据或对象给视图模板来渲染给浏览器的响应. 最佳实践是: 视图模板应该不会展示业务逻辑或直接与数据库交互. 取而代之的是, 视图模板应该只操作控制器提供的数据.
 维护这个 "关注点分离" 帮助保持你的代码整洁、可测试以及有更好的可维护性.

当前,  ``HelloWorldController`` 类里的 ``Welcome`` 方法获取 ``name`` 和 ``ID`` 参数然后直接输出值到浏览器. 相比让控制器用字符串渲染响应, 让我们更改控制器使用视图模板取而代之. 视图模板将会生成动态的响应, 意味着你需要从控制器传递适合的数据给视图以生成响应. 你可以用控制器将视图模板需要的动态数据(参数)放到 ``ViewData`` 词典中让视图模板访问.

回到 *HelloWorldController.cs* 文件并修改 ``Welcome`` 方法添加 ``Message`` 和 ``NumTimes`` 值到 ``ViewData`` 词典. ``ViewData`` 词典是动态对象, 
你可以方任何你想要保存的内容进去; ``ViewData`` 对象没有定义的属性直到你放值进去. :doc:`MVC model binding system  </mvc/models/model-binding>`
自动从地址栏里的查询字符串映射命名的属性 (``name`` 和 ``numTimes``) 到你方法的参数. 完整的 *HelloWorldController.cs* 文件如此:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: c#
  :lines: 152-172

``ViewData`` 词典对象包含将要传给视图的数据. 接下来, 你需要一个欢迎视图模板.

- 右键点击 *Views/HelloWorld* 文件夹, 然后选择 **Add > New Item**.
- 在 **Add New Item - MvcMovie** 对话框

  - 在右上角的搜索框中, 输入 *view*
  - 单击 **MVC View Page**
  - 在 **Name** 输入框中, 输入 *Welcome.cshtml*
  - 单击 **Add**

你将会在 *Welcome.cshtml* 视图模板中创建循环显示 "Hello" ``NumTimes``. 将 *Views/HelloWorld/Welcome.cshtml* 内容做如下替换:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/HelloWorld/Welcome.cshtml
  :language: none

保存更改浏览如下URL:

\http://localhost:xxxx/HelloWorld/Welcome?name=Rick&numtimes=4

数据从URL中提取并使用 :doc:`MVC model binder </mvc/models/model-binding>` 传递给控制器.
 控制器包装数据到 ``ViewData`` 词典并传递此对象到视图. 然后视图将数据渲染为HTML传递给浏览器.

.. image:: adding-view/_static/rick.png

在上面的样例中, 我们使用 ``ViewData`` 词典从控制器传递数据给视图. 此教程后面, 我们会使用视图模型从控制器传递数据给视图. 视图模型传递数据跟 ``ViewData`` 词典方法接近但更受欢迎.

"M"代表模型, 但不是数据库那种. 让我们实践我们所学并创建一个影片的数据库. 
