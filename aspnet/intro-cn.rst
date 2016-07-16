ASP.NET Core介绍
============================

由 `Daniel Roth`_, `Rick Anderson`_ 和 `Shaun Luttin <https://twitter.com/dicshaunary>`__编著

ASP.NET Core是ASP.NET具有意义的重新设计. 此系列文章介绍了ASP.NET Core中的新概念并解释了它们如何帮助你开发现代Web应用.

.. contents:: Sections:
  :local:
  :depth: 1

什么是ASP.NET Core?
---------------------

ASP.NET Core是崭新的开源和跨平台的框架，用来创建现代基于云的因特网互联应用程序，如Web应用、IoT应用和移动后端. ASP.NET Core应用可以运行在`.NET Core <https://www.microsoft.com/net/core/platform>`__ 或者在完整的.NET Framework上.它的架构被设计为提供一个优化的应用开发框架，让应用可以基于云或本地部署运行. 它包括最小功能单位的模块化组件,所以你可以在构建你的解决方案时保持灵活.你可以跨平台开发并运行你的ASP.NET Core apps于Windows, Mac 和 Linux. ASP.NET Core 在 `GitHub <https://github.com/aspnet/home>`_网站上开源.

为什么构建ASP.NET Core?
-----------------------

作为.NET Framework的一部分,ASP.NET第一个预览版发布距今已经15年.  然后无数的开发者开始使用它创建和运行高品质的Web应用, 在此过程中我们也在不断添加和包含更多的功能到其中.

ASP.NET Core拥有一系列的架构变化使其成为更领先和模块化的框架.  ASP.NET Core不再基于*System.Web.dll*. 它基于一系列粒度良好`NuGet <http://www.nuget.org/>`__ 程序包. 这就允许你只包含你需要的程序包来优化你的应用. 更小的应用表面区域的好处就是严密的安全性、减少维护、提高性能、和降低成本，采用"支付你所用"模式.

通过ASP.NET Core你尅获得如下基础性的提高:

- 一致的web UI和web API构建感受
- 集成的 :doc:`modern client-side frameworks </client-side/index>` 和开发工作流
- 基于为云准备的环境 :doc:`configuration system </fundamentals/configuration>`
- 内建 :doc:`dependency injection </fundamentals/dependency-injection>`
- 新的轻量和模块化HTTP请求管道
- IIS宿主或在你自己的集成中自宿主的能力
- 构建在 `.NET Core`_上, 其支持真正的端对端(side-by-side)应用版本控制
- 整个应用都通过 `NuGet`_  程序包传递
- 新的工具简化了现代web开发
- 构建并运行跨平台ASP.NET应用于Windows, Mac和Linux
- 开源并面向社区

应用分解(anatomy)
-------------------

.. comment In RC1, The work of the WebHostBuilder was hidden in dnx.exe

ASP.NET Core应用被简化为在它的``Main``方法中创建web服务器的控制台应用:

.. literalinclude:: /getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#

``Main`` 使用 :dn:cls:`~Microsoft.AspNetCore.Hosting.WebHostBuilder`类, 它遵循创建者(Builder)模式,创建一个Web应用宿主. 创建者通过方法定义Web服务器(例如``UseKestrel``) 和启动(startup)类 (``UseStartup``). 在上面的示例中, 使用Kestrel Web服务器, 当然也可以指定其它Web服务器. 我们将在下一个章节中展示更多关于``UseStartup``的使用方法. ``WebHostBuilder``提供很多可选的方法包括``UseIISIntegration``用来宿主于IIS和IIS Express，``UseContentRoot``用来指定内容根目录. ``Build`` 和 ``Run`` 方法创建``IWebHost``将宿主应用并开始监听到来的HTTP请求.


启动(Startup)
---------------------------
``WebHostBuilder``上的``UseStartup``方法指定了你应用的``Startup``类.

.. literalinclude:: /getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#
    :lines: 6-17
    :dedent: 4
    :emphasize-lines: 7

``Startup``类是你定义请求处理管道并且配置应用所需服务的地方. ``Startup``类必须是公开并且包含如下方法:

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

- ``ConfigureServices`` 定义你的应用(如ASP.NET MVC Core framework, Entity Framework Core, Identity, 等等.) 所使用的服务 (参考下面的Services_) 
- ``Configure`` 定义请求管道中的 :doc:`middleware </fundamentals/middleware>`
- 更多详细请参照 :doc:`/fundamentals/startup`

服务(Services)
--------

服务是应用中经常被调用的组件. 服务通过依赖注入生成. ASP.NET Core包含简单内建控制反转(IoC)容器默认支持构造注入, 也可以用你选择的IoC容器将其替换. 另外它具有松散耦合的优点, DI让服务可用于应用的各个地方. 例如, :doc:`Logging </fundamentals/logging>`对于你的整个应用都是可用的. 更多详细参照:doc:`/fundamentals/dependency-injection`.

中间件(Middleware)
----------

In ASP.NET Core you compose your request pipeline using :doc:`/fundamentals/middleware`. ASP.NET Core middleware performs asynchronous logic on an ``HttpContext`` and then either invokes the next middleware in the sequence or terminates the request directly. You generally "Use" middleware by invoking a corresponding ``UseXYZ`` extension method on the ``IApplicationBuilder`` in the ``Configure`` method.

ASP.NET Core comes with a rich set of prebuilt middleware:

- :doc:`Static files </fundamentals/static-files>`
- :doc:`/fundamentals/routing`
- :doc:`/security/authentication/index`

You can also author your own :doc:`custom middleware </fundamentals/middleware>`.

You can use any `OWIN <http://owin.org>`_-based middleware with ASP.NET Core. See :doc:`/fundamentals/owin` for details. 

Servers
-------

The ASP.NET Core hosting model does not directly listen for requests; rather it relies on an HTTP :doc:`server </fundamentals/servers>` implementation to forward the request to the application. The forwarded request is wrapped as a set of feature interfaces that the application then composes into an ``HttpContext``.  ASP.NET Core includes a managed cross-platform web server, called :ref:`Kestrel <kestrel>`, that you would typically run behind a production web server like `IIS <https://iis.net>`__ or `nginx <http://nginx.org>`__.

.. _content-root-lbl:

Content root
------------

The content root is the base path to any content used by the app, such as its views and web content. By default the content root is the same as application base path for the executable hosting the app; an alternative location can be specified with `WebHostBuilder`.

.. _web-root-lbl:

Web root
--------

The web root of your app is the directory in your project for public, static resources like css, js, and image files. The static files middleware will only serve files from the web root directory (and sub-directories) by default. The web root path defaults to `<content root>/wwwroot`, but you can specify a different location using the `WebHostBuilder`.

Configuration
-------------

ASP.NET Core uses a new configuration model for handling simple name-value pairs. The new configuration model is not based on ``System.Configuration`` or *web.config*; rather, it pulls from an ordered set of configuration providers. The built-in configuration providers support a variety of file formats (XML, JSON, INI) and environment variables to enable environment-based configuration. You can also write your own custom configuration providers.

See :doc:`/fundamentals/configuration` for more information.

Environments
---------------------

Environments, like "Development" and "Production", are a first-class notion in ASP.NET Core and can  be set using environment variables. See :doc:`/fundamentals/environments` for more information.

Build web UI and web APIs using ASP.NET Core MVC
------------------------------------------------

- You can create well-factored and testable web apps that follow the Model-View-Controller (MVC) pattern. See :doc:`/mvc/index` and :doc:`/testing/index`.
- You can build HTTP services that support multiple formats and have full support for content negotiation. See :doc:`/mvc/models/formatting`
- `Razor <http://www.asp.net/web-pages/overview/getting-started/introducing-razor-syntax-c>`__ provides a productive language to create :doc:`Views </mvc/views/index>`
- :doc:`Tag Helpers </mvc/views/tag-helpers/intro>` enable server-side code to participate in creating and rendering HTML elements in Razor files
- You can create HTTP services with full support for content negotiation using custom or built-in formatters (JSON, XML)
- :doc:`/mvc/models/model-binding` automatically maps data from HTTP requests to action method parameters
- :doc:`/mvc/models/validation` automatically performs client and server side validation

Client-side development
-----------------------

ASP.NET Core is designed to integrate seamlessly with a variety of client-side frameworks, including :doc:`AngularJS </client-side/angular>`, :doc:`KnockoutJS </client-side/knockout>` and :doc:`Bootstrap </client-side/bootstrap>`. See :doc:`/client-side/index` for more details.

Next steps
----------

- :doc:`/tutorials/first-mvc-app/index`
- :doc:`/tutorials/your-first-mac-aspnet`
- :doc:`/tutorials/first-web-api`
- :doc:`/fundamentals/index`
