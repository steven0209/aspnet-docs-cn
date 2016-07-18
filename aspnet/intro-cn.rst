ASP.NET Core简介
============================

由 `Daniel Roth`_ , `Rick Anderson`_ 和 `Shaun Luttin <https://twitter.com/dicshaunary>`__ 编著

ASP.NET Core是ASP.NET具有意义的重新设计. 此系列文章介绍了ASP.NET Core中的新概念并解释了它们如何帮助你开发现代Web应用.

.. contents:: 目录:
  :local:
  :depth: 1

什么是ASP.NET Core?
---------------------

ASP.NET Core是崭新的开源和跨平台的框架，用来创建现代基于云的因特网互联应用程序，如Web应用、IoT应用和移动后端. ASP.NET Core应用可以运行在 `.NET Core <https://www.microsoft.com/net/core/platform>`__ 或者在完整的.NET Framework上.它的架构被设计为提供一个优化的应用开发框架，让应用可以基于云或本地部署运行. 它包括最小功能单位的模块化组件,所以你可以在构建你的解决方案时保持灵活.你可以跨平台开发并运行你的ASP.NET Core应用于Windows, Mac 和 Linux. ASP.NET Core 在 `GitHub <https://github.com/aspnet/home>`__ 网站上开源.

为什么构建ASP.NET Core?
-----------------------

作为.NET Framework的一部分,ASP.NET第一个预览版发布距今已经15年.  然后无数的开发者开始使用它创建和运行高品质的Web应用, 在此过程中我们也在不断添加和包含更多的功能到其中.

ASP.NET Core拥有一系列的架构变化使其成为更领先和模块化的框架.  ASP.NET Core不再基于 *System.Web.dll* . 它基于一系列粒度良好 `NuGet <http://www.nuget.org/>`__ 程序包. 这就允许你只包含你需要的程序包来优化你的应用. 更小的应用表面区域的好处就是严密的安全性、减少维护、提高性能、和降低成本，采用"支付你所用"模式.

通过ASP.NET Core你可获得如下基础性的提高:

- 一致的web UI和web API构建感受
- 集成的 :doc:`modern client-side frameworks </client-side/index>` 和开发工作流
- 基于为云准备的环境 :doc:`configuration system </fundamentals/configuration>`
- 内建 :doc:`dependency injection </fundamentals/dependency-injection>`
- 新的轻量和模块化HTTP请求管道
- IIS宿主或在你自己的集成中自宿主的能力
- 构建在 `.NET Core`_ 上, 其支持真正的端对端(side-by-side)应用版本控制
- 整个应用都通过 `NuGet`_  程序包传递
- 新的工具简化了现代Web开发
- 构建并运行跨平台ASP.NET应用于Windows, Mac和Linux
- 开源并面向社区

应用分解
-------------------

.. comment In RC1, The work of the WebHostBuilder was hidden in dnx.exe

ASP.NET Core应用被简化为在它的 ``Main`` 方法中创建web服务器的控制台应用:

.. literalinclude:: /getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#

``Main`` 使用 :dn:cls:`Microsoft.AspNetCore.Hosting.WebHostBuilder` 类, 它遵循创建者(Builder)模式,创建一个Web应用宿主. 创建者通过方法定义Web服务器(例如 ``UseKestrel`` ) 和启动类 ``UseStartup``. 在上面的示例中, 使用Kestrel Web服务器, 当然也可以指定其它Web服务器. 我们将在下一个章节中展示更多关于 ``UseStartup`` 的使用方法. ``WebHostBuilder`` 提供很多可选的方法包括 ``UseIISIntegration`` 用来宿主于IIS和IIS Express, ``UseContentRoot`` 用来指定内容根目录. ``Build`` 和 ``Run`` 方法创建 ``IWebHost`` 将宿主应用并开始监听到来的HTTP请求.


启动
---------------------------
``WebHostBuilder`` 上的 ``UseStartup`` 方法指定了你应用的 ``Startup`` 类.

.. literalinclude:: /getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#
    :lines: 6-17
    :dedent: 4
    :emphasize-lines: 7

``Startup`` 类是你定义请求处理管道并且配置应用所需服务的地方. ``Startup`` 类必须是公开并且包含如下方法:

.. code-block:: c#

  public class Startup
  {
      public void ConfigureServices(IServiceCollection services)
      {
      }

      public void Configure(IApplicationBuilder app)
      {
      }
  }

- ``ConfigureServices`` 定义你的应用(如ASP.NET MVC Core framework, Entity Framework Core, Identity, 等等.) 所使用的服务 (参考下面的'服务'一节) 
- ``Configure`` 定义请求管道中的 :doc:`middleware </fundamentals/middleware>`
- 更多详细请参照 :doc:`/fundamentals/startup`

服务(Services)
--------

服务是应用中经常被调用的组件. 服务通过依赖注入生成. ASP.NET Core包含简单内建控制反转(IoC)容器默认支持构造注入, 也可以用你选择的IoC容器将其替换. 另外它具有松散耦合的优点, DI让服务可用于应用的各个地方. 例如, :doc:`Logging </fundamentals/logging>` 对于你的整个应用都是可用的. 更多详细参照 :doc:`/fundamentals/dependency-injection` .

中间件(Middleware)
----------

在ASP.NET Core中你将使用 :doc:`/fundamentals/middleware` 构造你的请求管道. ASP.NET Core中间件在 ``HttpContext`` 执行异步逻辑然后直接按顺序调用下一个中间件或者直接终止请求. 
通常通过在 ``Configure`` 方法里调用对应的 ``IApplicationBuilder`` 接口上的 ``UseXYZ`` 扩展方法"使用"中间件.

ASP.NET Core包含一系列丰富的预建中间件:

- :doc:`Static files </fundamentals/static-files>`
- :doc:`/fundamentals/routing`
- :doc:`/security/authentication/index`

也可以编写你自己的 :doc:`custom middleware </fundamentals/middleware>`.

可以在ASP.NET Core中使用任何基于中间件的 `OWIN <http://owin.org>`_. 详细参照 :doc:`/fundamentals/owin`. 

服务器(Servers)
-------

ASP.NET Core宿主模型并不直接监听请求; 实际上它依赖于HTTP :doc:`server </fundamentals/servers>` 实现将请求转发给应用. 转发的请求被一系列的特性借口包装起来然后由应用构成 ``HttpContext``. ASP.NET Core包含一个托管的跨平台Web服务器称作 :ref:`Kestrel <kestrel>`, 你也可以像以往一样运行在生产Web服务器如 `IIS <https://iis.net>`__ 或 `nginx <http://nginx.org>`__.

.. _content-root-lbl:

内容根目录(Content Root)
------------

内容根目录是应用使用的任何内容的基础路径, 这些内容包括它的试图和Web内容. 默认内容根目录与可执行宿主应用的应用基础路径相同; 它是一个全局位置可以通过 ``WebHostBuilder`` 指定.

.. _web-root-lbl:

Web根目录
--------

应用的Web根目录是存放项目的公开、静态资源如CSS、JavaScript脚本和图片文件的存储目录. 静态文件中间件默认只提供来自于Web根目录(和其子目录)的文件资源. 
Web根路径默认为`<content root>/wwwroot`, 你也可以使用 ``WebHostBuilder`` 指定一个不同的位置.

配置(Configuration)
-------------

ASP.NET Core使用崭新的配置模型来处理简单的键值对. 新的配置模型不再基于 ``System.Configuration`` 或 *web.config* ; 而是它获得一个有序的配置提供者集合. 内建配置提供者提供各种文件类型(XML, JSON, INI) 并通过环境变量进行基于环境的设置. 也可以编写你自己的自定义配置提供者. 更多信息参照 :doc:`/fundamentals/configuration`.

环境(Environments)
---------------------

环境, 如"开发" 和 "生产",是ASP.NET Core头等的观念并可以设置使用环境变量. 更多信息参照 :doc:`/fundamentals/environments`.

使用ASP.NET Core MVC构建Web UI和Web API
------------------------------------------------

- 你可以遵循Model-View-Controller (MVC)模式创建良好粒度并且可测试的Web应用. 参照 :doc:`/mvc/index` and :doc:`/testing/index`.
- 你可以创建HTTP服务支持多种格式和完整的内容协商(Content Negotiation)支持. 参照 :doc:`/mvc/models/formatting`
- `Razor <http://www.asp.net/web-pages/overview/getting-started/introducing-razor-syntax-c>`__ 提供创建 :doc:`Views </mvc/views/index>` 的生产性语言
- :doc:`标签帮手(Tag Helpers) </mvc/views/tag-helpers/intro>` 让服务器端代码参与Razor文件中HTML元素的创建和渲染
- 你可以使用自定义或内建格式化器(JSON, XML)通过对内容协商的完整支持创建HTTP服务
- :doc:`/mvc/models/model-binding` 自动地从HTTP请求映射数据到行动(Action)方法参数
- :doc:`/mvc/models/validation` 自动地提供客户端和服务器端验证

客户端开发
-----------------------

ASP.NET Core被设计成无缝集成各种客户端框架, 包括 :doc:`AngularJS </client-side/angular>`, :doc:`KnockoutJS </client-side/knockout>` 和 :doc:`Bootstrap </client-side/bootstrap>`. 更多详细参照 :doc:`/client-side/index` .

下一步
----------

- :doc:`/tutorials/first-mvc-app/index-cn`
- :doc:`/tutorials/your-first-mac-aspnet`
- :doc:`/tutorials/first-web-api`
- :doc:`/fundamentals/index`
