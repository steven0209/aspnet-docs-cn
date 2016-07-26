.. _dotnet-watch:

使用dotnet watch开发ASP.NET Core应用
=======================================================

由 `Victor Hurdugaci`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

简介
------------

``dotnet watch`` 是开发时工具保证在源代码文件变更时运行 ``dotnet`` 命令行. 它可以用来编译, 运行测试, 或者当代码变更时发布.

此教程中我们会使用既存WebApi应用计算两个数的总和和乘积演示 ``dotnet watch`` 用例. 实例应用包含一个需要修复的有意的缺陷.

入门
---------------

从下载 `the sample application <https://github.com/aspnet/Docs/tree/master/aspnet/tutorials/dotnet-watch/sample>`__ 开始. 它包含两个工程, ``WebApp`` (web应用)和 ``WebAppTests`` (Web应用单元测试)

在控制台, 打开你下载示例应用的文件夹并运行:

1.  ``dotnet restore``
2.  ``cd WebApp``
3.  ``dotnet run``

控制台输出将显示近似于如下的消息, 代表应用正在运行并等待请求:

.. code-block:: bash

  $ dotnet run
  Project WebApp (.NETCoreApp,Version=v1.0) will be compiled because inputs were modified
  Compiling WebApp for .NETCoreApp,Version=v1.0

  Compilation succeeded.
    0 Warning(s)
    0 Error(s)

  Time elapsed 00:00:02.6049991

  Hosting environment: Production
  Content root path: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp
  Now listening on: http://localhost:5000
  Application started. Press Ctrl+C to shut down.

在浏览器里, 导航到 ``http://localhost:5000/api/math/sum?a=4&b=5`` 你会看到结果 ``9``.

如果你导航到 ``http://localhost:5000/api/math/product?a=4&b=5``, 你期望结果 ``20``. 但你却又得到 ``9``.

我们会修复它.

向工程添加 ``dotnet watch``
------------------------------------

1. 将 ``Microsoft.DotNet.Watcher.Tools`` 添加到 *WebApp/project.json* 文件的 ``tools`` 节，如下:

.. literalinclude:: dotnet-watch/sample/WebAppTests/project.json
   :language: javascript
   :lines: 21-23
   :emphasize-lines: 2
   :dedent: 2

2. 运行 ``dotnet restore``.

控制台输出将显示近似如下的消息:

.. code-block:: bash

  log  : Restoring packages for /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp/project.json...
  log  : Restoring packages for tool 'Microsoft.DotNet.Watcher.Tools' in /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp/project.json...
  log  : Installing Microsoft.DotNet.Watcher.Core 1.0.0-preview2-final.
  log  : Installing Microsoft.DotNet.Watcher.Tools 1.0.0-preview2-final.

使用 ``dotnet watch`` 运行 ``dotnet``
--------------------------------------------------

任何 ``dotnet`` 命令都可以与 ``dotnet watch`` 一起运行:  例如:

========================================= ======================================
命令                                      带监视的命令
========================================= ======================================
``dotnet run``                            ``dotnet watch run``
``dotnet run -f net451``                  ``dotnet watch run -f net451``
``dotnet run -f net451 -- --arg1``        ``dotnet watch run -f net451 -- --arg1``
``dotnet test``                           ``dotnet watch test``
========================================= ======================================

使用监视器运行 ``WebApp``, 在 ``WebApp`` 文件夹运行 ``dotnet watch run``. 控制台输出将显示近似如下消息, 表明 ``dotnet watch`` 正在监视代码文件:

.. code-block:: bash

  user$ dotnet watch run
  [DotNetWatcher] info: Running dotnet with the following arguments: run
  [DotNetWatcher] info: dotnet process id: 39746
  Project WebApp (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
  Hosting environment: Production
  Content root path: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp
  Now listening on: http://localhost:5000
  Application started. Press Ctrl+C to shut down.

改变 ``dotnet watch``
------------------------------------

保证 ``dotnet watch`` 正在运行.

让我们修复乘积计算错误的缺陷.

打开 *WebApp/Controllers/MathController.cs*.

我们在代码中有意地产生一个缺陷.

.. literalinclude:: dotnet-watch/sample/WebApp/Controllers/MathController.cs
   :language: c#
   :lines: 12-17
   :emphasize-lines: 5
   :dedent: 4

通过将 ``a + b`` 更改为 ``a * b`` 修复缺陷.

保存文件. 控制台输出将会显示近似如下信息, 表明 ``dotnet watch`` 检测到文件变更并且重启应用.


.. code-block:: bash

  [DotNetWatcher] info: File changed: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp/Controllers/MathController.cs
  [DotNetWatcher] info: Running dotnet with the following arguments: run
  [DotNetWatcher] info: dotnet process id: 39940
  Project WebApp (.NETCoreApp,Version=v1.0) will be compiled because inputs were modified
  Compiling WebApp for .NETCoreApp,Version=v1.0
  Compilation succeeded.
    0 Warning(s)
    0 Error(s)
  Time elapsed 00:00:03.3312829

  Hosting environment: Production
  Content root path: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp
  Now listening on: http://localhost:5000
  Application started. Press Ctrl+C to shut down.

验证 ``http://localhost:5000/api/math/product?a=4&b=5`` 返回正确结果.

使用 ``dotnet watch`` 运行测试
------------------------------------

文件监视器能运行其它 ``dotnet`` 命令如 ``test`` 或 ``publish``.

1. 打开已经在 *project.json* 中设置 ``dotnet watch`` 进行监视的 ``WebAppTests`` 文件夹.
2. 运行 ``dotnet watch test``.

如果你在 ``MathController`` 中修复了缺陷你会看到近似如下的输出, 否则你会看到测试失败:

.. code-block:: bash

  WebAppTests user$ dotnet watch test
  [DotNetWatcher] info: Running dotnet with the following arguments: test
  [DotNetWatcher] info: dotnet process id: 40193
  Project WebApp (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
  Project WebAppTests (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
  xUnit.net .NET CLI test runner (64-bit .NET Core osx.10.11-x64)
    Discovering: WebAppTests
    Discovered:  WebAppTests
    Starting:    WebAppTests
    Finished:    WebAppTests
  === TEST EXECUTION SUMMARY ===
     WebAppTests  Total: 2, Errors: 0, Failed: 0, Skipped: 0, Time: 0.259s
  SUMMARY: Total: 1 targets, Passed: 1, Failed: 0.
  [DotNetWatcher] info: dotnet exit code: 0
  [DotNetWatcher] info: Waiting for a file to change before restarting dotnet...

当所有测试运行, 监视器将表明它在等待文件变更以启动下一次 ``dotnet test``.

3. 打开控制器文件 *WebApp/Controllers/MathController.cs* 后改变一些代码. 如果你没有修复乘积缺陷, 现在做. 然后保存文件.

``dotnet watch`` will detect the file change and rerun the tests. The console output will show messages similar to the one below:

.. code-block:: bash

  [DotNetWatcher] info: File changed: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp/Controllers/MathController.cs
  [DotNetWatcher] info: Running dotnet with the following arguments: test
  [DotNetWatcher] info: dotnet process id: 40233
  Project WebApp (.NETCoreApp,Version=v1.0) will be compiled because inputs were modified
  Compiling WebApp for .NETCoreApp,Version=v1.0
  Compilation succeeded.
    0 Warning(s)
    0 Error(s)
  Time elapsed 00:00:03.2127590
  Project WebAppTests (.NETCoreApp,Version=v1.0) will be compiled because dependencies changed
  Compiling WebAppTests for .NETCoreApp,Version=v1.0
  Compilation succeeded.
    0 Warning(s)
    0 Error(s)
  Time elapsed 00:00:02.1204052

  xUnit.net .NET CLI test runner (64-bit .NET Core osx.10.11-x64)
    Discovering: WebAppTests
    Discovered:  WebAppTests
    Starting:    WebAppTests
    Finished:    WebAppTests
  === TEST EXECUTION SUMMARY ===
     WebAppTests  Total: 2, Errors: 0, Failed: 0, Skipped: 0, Time: 0.260s
  SUMMARY: Total: 1 targets, Passed: 1, Failed: 0.
  [DotNetWatcher] info: dotnet exit code: 0

  [DotNetWatcher] info: Waiting for a file to change before restarting dotnet...
