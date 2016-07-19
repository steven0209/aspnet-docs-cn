添加视图
================================================

由 `Rick Anderson`_ 编辑

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

Examine the *Views/_ViewStart.cshtml* file:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/_ViewStart.cshtml
  :language: HTML

The *Views/_ViewStart.cshtml* file brings in the *Views/Shared/_Layout.cshtml* file to each view. You can use the ``Layout`` property to set a different layout view, or set it to ``null`` so no layout file will be used.

Now, let's change the title of the ``Index`` view.

Open *Views/HelloWorld/Index.cshtml*. There are two places to make a change:

 - The text that appears in the title of the browser
 - The secondary header (``<h2>`` element).

You'll make them slightly different so you can see which bit of code changes which part of the app.

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/HelloWorld/Index2.cshtml
  :language: HTML
  :emphasize-lines: 2, 5

``ViewData["Title"] = "Movie List";`` in the code above sets the ``Title`` property of the :dn:class:`~Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary` to "Movie List". The ``Title`` property is used in the ``<title>`` HTML element in the layout page:

.. code-block:: HTML

  <title>@ViewData["Title"] - Movie App</title>

Save your change and refresh the page. Notice that the browser title, the primary heading, and the secondary headings have changed. (If you don't see changes in the browser, you might be viewing cached content. Press Ctrl+F5 in your browser to force the response from the server to be loaded.) The browser title is created with ``ViewData["Title"]`` we set in the *Index.cshtml* view template and the additional "- Movie App" added in the layout file.

Also notice how the content in the *Index.cshtml* view template was merged with the *Views/Shared/_Layout.cshtml* view template and a single HTML response was sent to the browser. Layout templates make it really easy to make changes that apply across all of the pages in your application. To learn more see :doc:`/mvc/views/layout`.

.. image:: adding-view/_static/hell3.png

Our little bit of "data" (in this case the "Hello from our View Template!" message) is hard-coded, though. The MVC application has a "V" (view) and you've got a "C" (controller), but no "M" (model) yet. Shortly, we'll walk through how to create a database and retrieve model data from it.

Passing Data from the Controller to the View
----------------------------------------------

Before we go to a database and talk about models, though, let's first talk about passing information from the controller to a view. Controller actions are invoked in response to an incoming URL request. A controller class is where you write the code that handles the incoming browser requests, retrieves data from a database, and ultimately decides what type of response to send back to the browser. View templates can then be used from a controller to generate and format an HTML response to the browser.

Controllers are responsible for providing whatever data or objects are required in order for a view template to render a response to the browser. A best practice: A view template should never perform business logic or interact with a database directly. Instead, a view template should work only with the data that's provided to it by the controller. Maintaining this "separation of concerns" helps keep your code clean, testable and more maintainable.

Currently, the ``Welcome`` method in the ``HelloWorldController`` class takes a ``name`` and a ``ID`` parameter and then outputs the values directly to the browser. Rather than have the controller render this response as a string, let’s change the controller to use a view template instead. The view template will generate a dynamic response, which means that you need to pass appropriate bits of data from the controller to the view in order to generate the response. You can do this by having the controller put the dynamic data (parameters) that the view template needs in a ``ViewData`` dictionary that the view template can then access.

Return to the *HelloWorldController.cs* file and change the ``Welcome`` method to add a ``Message`` and ``NumTimes`` value to the ``ViewData`` dictionary. The ``ViewData`` dictionary is a dynamic object, which means you can put whatever you want in to it; the ``ViewData`` object has no defined properties until you put something inside it. The :doc:`MVC model binding system  </mvc/models/model-binding>` automatically maps the named parameters (``name`` and ``numTimes``) from the query string in the address bar to parameters in your method. The complete *HelloWorldController.cs* file looks like this:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: c#
  :lines: 152-172

The ``ViewData`` dictionary object contains data that will be passed to the view. Next, you need a Welcome view template.

- Right click on the *Views/HelloWorld* folder, and then **Add > New Item**.
- In the **Add New Item - MvcMovie** dialog

  - In the search box in the upper-right, enter *view*
  - Tap **MVC View Page**
  - In the **Name** box, enter *Welcome.cshtml*
  - Tap **Add**

You'll create a loop in the *Welcome.cshtml* view template that displays "Hello" ``NumTimes``. Replace the contents of *Views/HelloWorld/Welcome.cshtml* with the following:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/HelloWorld/Welcome.cshtml
  :language: none

Save your changes and browse to the following URL:

\http://localhost:xxxx/HelloWorld/Welcome?name=Rick&numtimes=4

Data is taken from the URL and passed to the controller using the :doc:`MVC model binder </mvc/models/model-binding>` . The controller packages the data into a ``ViewData`` dictionary and passes that object to the view. The view then renders the data as HTML to the browser.

.. image:: adding-view/_static/rick.png

In the sample above, we used the ``ViewData`` dictionary to pass data from the controller to a view. Later in the tutorial, we will use a view model to pass data from a controller to a view. The view model approach to passing data is generally much preferred over the ``ViewData`` dictionary approach.

Well, that was a kind of an "M" for model, but not the database kind. Let's take what we've learned and create a database of movies. 
