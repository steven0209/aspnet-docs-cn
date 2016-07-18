ASP.NET Core MVC和Visual Studio入门
=======================================================

由 `Rick Anderson`_ 编辑

此教程将展示使用 `Visual Studio 2015 <https://www.visualstudio.com/en-us/visual-studio-homepage-vs.aspx>`__ 构建ASP.NET Core MVC Web应用的基础. 

安装Visual Studio和.NET Core
----------------------------------------

- 安装Visual Studio 2015社区版. 选择社区版下载并默认安装. 如果你已安装Visual Studio 2015请跳过此步.

  - `Visual Studio 2015 Home page installer  <https://www.visualstudio.com/en-us/visual-studio-homepage-vs.aspx>`__

- 安装 `.NET Core + Visual Studio tooling <http://go.microsoft.com/fwlink/?LinkID=798306>`__


创建Web应用
-----------------------------------

在Visual Studio **Start** 页面, 点击 **New Project**.

.. image:: start-mvc/_static/new_project.png

或者, 可以使用惨淡创建新的工程. 点选 **File > New > Project**.

.. image:: start-mvc/_static/alt_new_project.png

完成 **New Project** 对话框:

- 在左边面板, 点击 **Web**
- 中央面板点击 **ASP.NET Core Web Application (.NET Core)**
- 将项目命名为 "MvcMovie" (命名项目为 "MvcMovie" 非常重要, 这样当你拷贝代码时名称空间才会匹配. )
- 点击 **OK**

.. image:: start-mvc/_static/new_project2.png

完成 **New ASP.NET Core Web Application - MvcMovie** 对话框:

- 点击 **Web Application**
- 清空 **Host in the cloud**
- 点击 **OK**.

.. image:: start-mvc/_static/p3.png

Visual Studio会为你刚创建的MVC工程选用默认模板, 所以你要输入工程名字和一些选项才能让你的应用工作. 这是个简单的 "Hello World!" 工程, 也是很好入手的地方,

点击 **F5** 在调试模式下或 **Ctl-F5** 非调试模式下运行应用.

.. image:: start-mvc/_static/1.png

- Visual Studio会启动 `IIS Express <http://www.iis.net/learn/extensions/introduction-to-iis-express/iis-express-overview>`__ 并运行你的应用. 注意地址栏显示的应该是 ``localhost:port#`` 而不是像 ``example.com`` 一样. 因为 ``localhost`` 总是指向你自己本地的电脑, 它才是正在运行你刚创建的应用. 当Visual Studio创建一个Web工程, 随机端口被指定给Web服务器. 如上图, 端口号是1234. 当应用运行时, 将看到不同的端口.
- 通过 **Ctrl+F5** (非调试模式) 启动应用允许你改变代码、保存文件, 刷新浏览器查看代码变化. 很多开发人员倾向使用非调试模式快速地启动应用并查看变化.
- 可以选择启动应用时采用在 **Debug** 选择调试或非调试模式选项:

.. image:: start-mvc/_static/debug_menu.png

- 可以通过点击 **IIS Express** 按钮调试应用

.. image:: start-mvc/_static/iis_express.png

默认模板提供可工作的 **Home, Contact, About, Register** 和 **Log in** 链接. 浏览器图像中没有显示这些链接. 依赖于浏览器的尺寸, 你可能需要点击导航按钮来显示它们. 

.. image:: start-mvc/_static/2.png

在此教程的下一部分, 我们要学习更多MVC然后开始编写一些代码.
