入门指南
===============

1. 安装 `.NET Core`_

2. 创建一个新的.NET Core工程:

  .. code-block:: console
    
    mkdir aspnetcoreapp
    cd aspnetcoreapp
    dotnet new

3. 更新 *project.json* 文件,添加Kestrel HTTP服务器程序包作为依赖:

  .. literalinclude:: getting-started/sample/aspnetcoreapp/project.json
    :language: c#
    :emphasize-lines: 15

4. 恢复程序包:

  .. code-block:: console
    
    dotnet restore

5. 添加一个 *Startup.cs* 文件来定义请求处理逻辑:

  .. literalinclude:: getting-started/sample/aspnetcoreapp/Startup.cs
    :language: c#

6. 更新 *Program.cs* 中的代码设置并启动Web宿主:

  .. literalinclude:: getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#
    :emphasize-lines: 2,4,10-15

7. 运行应用  ( 如果应用过时,``dotnet run`` 指令将创建应用):

  .. code-block:: console
  
    dotnet run

8. 浏览 \http://localhost:5000:

  .. image:: getting-started/_static/running-output.png

下一步
----------

- :doc:`/tutorials/first-mvc-app/index`
- :doc:`/tutorials/your-first-mac-aspnet`
- :doc:`/tutorials/first-web-api`
- :doc:`/fundamentals/index`
