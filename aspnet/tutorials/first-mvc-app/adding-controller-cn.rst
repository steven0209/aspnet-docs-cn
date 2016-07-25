添加控制器
==================================================

由 `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

MVC(Model-View-Controller)架构模式将应用分离为三个主要组件: 模型(**M**\odel), 视图(**V**\iew),和控制器(**C**\ontroller). MVC模式帮助创建出比传统庞大的应用更容易测试和维护和更新的应用. 基于MVC的应用包括: 

- 模型(**M**\odels): 表示应用数据的类, 通过验证逻辑保证数据的业务规则. 一般模型对象通过数据库查询并储存模型状态. 在此教程中, ``Movie`` 模型从数据库查询影片数据, 给视图提供数据并更新数据. 更新的数据写回到SQL Server数据库.
- 视图(**V**\iews): 视图是显示应用用户界面(UI)的组件. 一般显示模型数据.
- 控制器(**C**\ontrollers): 这些类处理浏览器请求, 查询模型数据, 然后指定视图模板返回响应给浏览器. 在MVC应用中, 视图只显示信息; 控制其处理并响应用户输入和互动. 例如, 控制器处理路由数据和查询字符串的值, 然后将这些值传递给模型. 模型可能使用这些值查询数据库.

MVC模式帮助你将应用的不同方面(输入逻辑, 业务逻辑, 和用户界面(UI)逻辑)分离来创建应用, 提供这些元素之间的松耦合. 模式指定每种逻辑应该在应用的哪个部分. 用户界面(UI)逻辑属于视图. 输入逻辑属于控制器. 业务逻辑属于模型. 这种分离帮助你管理创建应用的复杂性, 因为它让你一次只关注实现的一个方面而不会影响其它代码. 例如, 你可以修改视图代码而不依赖于业务逻辑代码.

我们会在系列教程中覆盖所有概念并向你展示如何使用它们创建简单的电影应用. 如下图片展示了MVC工程中的模型(*Models*), 视图(*Views*) 和控制器(*Controllers*)文件夹.

.. image:: adding-controller/_static/mvc1.png

- 在 **Solution Explorer** 中, 右键点击 **Controllers > Add > New Item...**

.. image:: adding-controller/_static/add_controller.png

- 在 **Add New Item** 对话框中, 输入 **HelloWorldController**.

将 *Controllers/HelloWorldController.cs* 内容如下替换:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: c#
  :lines: 5-28

控制器中的每个 ``public`` 方法都是可调用的HTTP终结点(endpoint). 在上面的样例中, 每个方法都返回一个字符串. 注意每个方法的注释:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: c#
  :lines: 10-28
  :dedent: 4
  :emphasize-lines: 4,12

第一个注释说明这是一个 `HTTP GET <http://www.w3schools.com/tags/ref_httpmethods.asp>`__ 方法由基础URL上添加 "/HelloWorld/" 进行调用. 第二个注释指定了 `HTTP GET <http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html>`__ 方法被基础URL上添加 "/HelloWorld/Welcome/" 调用. 后面的教程我们将使用脚手架引擎(scaffolding engine)生成 ``HTTP POST`` 方法.

在非调试模式下运行应用(按Ctrl+F5)然后追加"HelloWorld"到地址栏中的路径. (在下图中, 使用http://localhost:1234/HelloWorld, 但是你需要将 *1234* 替换成你应用的端口号.) ``Index`` 方法返回一个字符换. 你告诉系统返回HTML, 让并且它照做了!

.. image:: adding-controller/_static/hell1.png

MVC根据接收的URL调用控制器类(还有它们中的行为(Action)方法). MVC使用默认 :doc:`URL routing logic </mvc/controllers/routing>` 格式决定调用哪部分代码:

``/[Controller]/[ActionName]/[Parameters]``

可以在 *Startup.cs* 文件中设置路由格式.

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :start-after: // Default routing.
  :emphasize-lines: 5
  :dedent: 8

当你运行应用但没有提供任何URL片段, 默认指向上面高亮表示的模板行中指定"Home"控制器的"Index"方法.

第一个URL片段决定运行的控制类. 所以 ``localhost:xxxx/HelloWorld`` 映射到 ``HelloWorldController`` 类. URL片段的第二部分决定了类中的行动(action)方法. 所以 ``localhost:xxxx/HelloWorld/Index`` 会运行 ``HelloWorldController`` 类的 ``Index`` 方法. 注意如果我们只浏览到 ``localhost:xxxx/HelloWorld`` 则会默认调用 ``Index`` 方法. 这是因为 ``Index`` 是没有显示指定控制器方法名时的默认方法. URL片段的第三部分(``id``)是路由数据. 在此教程的后面我们将看到路由数据相关的内容.

浏览 ``http://localhost:xxxx/HelloWorld/Welcome``. ``Welcome`` 运行并返回字符串 "This is the Welcome action method...". 这个URL中, 控制器为 ``HelloWorld`` , ``Welcome`` 是行为方法. 我们没有使用URL的 ``[Parameters]`` 部分.

.. image:: adding-controller/_static/welcome.png

稍稍修改样例就可以通过URL传递参数信息给控制器(例如, ``/HelloWorld/Welcome?name=Scott&numtimes=4``). 改变 ``Welcome`` 方法包含如下两个参数. 注意代码中使用了 C# 可选参数特性标识 ``numTimes`` 参数如果没有传递值则默认值为1.

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: none
  :lines: 51-54
  :dedent: 8

.. note:: 以上代码使用 ``HtmlEncoder.Default.Encode`` 来保护应用免受恶意输入(即JavaScript). 同时使用 `Interpolated Strings <https://msdn.microsoft.com/en-us/library/dn961160.aspx>`__.

.. note:: 在Visual Studio 2015中, 如果不使用调试(Ctl+F5)运行IIS Express, 修改代码后就不需要构建应用. 只需保存文件, 刷新浏览器你就可以看到改变.

运行你的应用并浏览:

  ``http://localhost:xxxx/HelloWorld/Welcome?name=Rick&numtimes=4``

(替换 xxxx 为你的端口号.) 你可以尝试在URL中为 ``name`` 和 ``numtimes`` 指定不同的值. MVC :doc:`model binding </mvc/models/model-binding>` 系统自动地从地址栏里的查询字符串映射为方法中的命名参数. 更多信息参照 :doc:`/mvc/models/model-binding`.

.. image:: adding-controller/_static/rick4.png

在上面样例中, 没有使用(``Parameters``)的URL片段, ``name`` 和 ``numTimes`` 参数通过 `query strings <http://en.wikipedia.org/wiki/Query_string>`__ 传递. 上面URL中 ``?`` (问号)是分隔符, 随后是查询字符串.  ``&`` 字符分离查询字符串.

将 ``Welcome`` 方法内容替换为如下代码:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: none
  :lines: 80-84
  :dedent: 8

运行应用并输入如下URL:  ``http://localhost:xxx/HelloWorld/Welcome/3?name=Rick``

.. image:: adding-controller/_static/rick_routedata.png

这次第三部分URL片段符合路由参数 ``id``. ``Welcome`` 方法包含一个参数  ``id`` 在 ``MapRoute`` 方法中匹配URL. 结尾 ``?``  (在 ``id?``) 标示 ``id`` 参数是可选的.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Startup.cs
  :language: c#
  :start-after: app.UseIdentity();
  :end-before:  SeedData.Initialize(app.ApplicationServices);
  :dedent: 12
  :emphasize-lines: 5
  
这些样例中控制器实现MVC的"VC"部分, 视图和控制器工作的很好. 控制器直接返回HTML. 一般是不希望控制器直接返回HTML, 这样会让代码变得非常笨重. 取而代之的是我们采用 Razor 视图模板文件帮助生成HTML响应. 我们会在下个教程中介绍.