第一个在Mac上使用Visual Studio Code的ASP.NET Core应用
=====================================================================

由By `Daniel Roth`_, `Steve Smith`_ and `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

此文展示如何编写在Mac上的第一个ASP.NET Core应用.

.. contents:: Sections:
  :local:
  :depth: 1

设置开发环境
---------------------------------------

为了设置你的开发机器下载并安装 `.NET Core`_ 和 `Visual Studio Code`_ 以及 `C# extension <https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp>`__.

使用Yeoman脚手架应用
-------------------------------------

按照 :doc:`/client-side/yeoman` 中的指示创建ASP.NET工程.

使用Visual Studio Code在Mac上开发ASP.NET Core应用
---------------------------------------------------------------------

- 启动 **Visual Studio Code**

.. image:: your-first-mac-aspnet/_static/vscode-welcome.png

- 点击 **File > Open** 并导航到你的空ASP.NET Core应用

.. image:: your-first-mac-aspnet/_static/file-open.png

从 Terminal / bash prompt, 运行 ``dotnet restore`` 来恢复项目依赖. 或者, 你可以在Visual Studio Code中输入 ``command shift p``然后如下面输入 ``dot`` :

.. image:: your-first-mac-aspnet/_static/dotnet-restore.png

你可以在Visual Studio Code中直接运行命令, 包括 ``dotnet restore`` 和任何在 *project.json* 文件中参考的工具, 如 *.vscode/tasks.json* 中定义的任务.

这个空的项目模板简单显示"Hello World!". 在Visual Studio Code打开 *Startup.cs* 看如何配置:

.. image:: your-first-mac-aspnet/_static/vscode-startupcs.png

如果这是你第一次使用 Visual Studio Code (或剪短称为 *Code*), 注意它提供了非常流畅、快速、干净的接口以快速操作文件, 还能提供工具高效地编写代码. 

在左侧导航栏, 有四个图标, 用来打开四个窗栏:

- Explore
- Search
- Git
- Debug

Explorer窗栏允许你快速在文件夹系统中导航, 也能很容易看到你在操作的文件. 它通过标记指示没有保存的文件更改, 还可以很容易地创建文件夹和文件(不需要打开其它的对话框). 你还可以轻松地通过菜单选项保存所有更改.

搜索窗栏允许你快速在文件夹结构内搜索, 搜索文件名或者内容.

如果在你的系统中安装了Git, *Code* 将会与其集成. 你可以轻松地从Git窗栏初始化新仓库, 创建提交, 和推送变更.

.. image:: your-first-mac-aspnet/_static/vscode-git.png

调试窗栏支持应用的交互式调试.

最后, Code的编辑器还有很多优秀功能. 你会注意到用下划线标注的没有使用的using语句可以通过出现的灯泡提示图标中的 ``command .`` 自动删除. 类和方法也会显示在工程中有多少引用到它们. 如果你曾使用过Visual Studio, Code包含许多相同的键盘快捷键, 如 ``command k c`` 注释代码块, 和 ``command k u`` 取消注释.

使用Kestrel本地运行
-----------------------------

实例配置使用 :ref:`Kestrel <kestrel>` 作为Web服务器. 你会看到 *project.json* 文件中的配置信息, 它是通过依赖的形式被指定.

.. code-block:: json
  :emphasize-lines: 10
 
  {
    "buildOptions": {
      "emitEntryPoint": true
    },
    "dependencies": {
      "Microsoft.NETCore.App": {
        "type": "platform",
        "version": "1.0.0"
      },
      "Microsoft.AspNetCore.Server.Kestrel": "1.0.0"
    },
    "frameworks": {
      "netcoreapp1.0": {}
    }
  }


- 运行 ``dotnet run`` 命令启动应用

- 导航到 ``localhost:5000``:

.. image:: your-first-mac-aspnet/_static/hello-world.png

- 为了停止Web服务器按下 ``Ctrl+C``.


推送到Azure
-------------------

当你开发了应用, 你可以轻松使用构建在Visual Studio Code中的Git集成推送变更到宿主在上的 `Microsoft Azure <http://azure.microsoft.com>`_ 生产环境. 

初始化Git
^^^^^^^^^^^^^^

在你工作的文件夹初始化Git. 点击Git窗栏然后单击 ``Initialize Git repository`` 按钮.

.. image:: your-first-mac-aspnet/_static/vscode-git-commit.png

添加注释消息然后点击回车或点选复选标记提交分段提交(Staged)的文件. 

.. image:: your-first-mac-aspnet/_static/init-commit.png

Git会跟踪变更, 所以如果你更新文件, Git 窗栏会显示从上次提交你变更的文件.

初始化Azure网站
^^^^^^^^^^^^^^^^^^^^^^^^

你可以直接使用Git将Web应用部署到Azure. 

- 在Azure中 `Create a new Web App <https://tryappservice.azure.com/>`__.
 如果你没有Azure账号, 你可以 `create a free trial <http://azure.microsoft.com/en-us/pricing/free-trial/>`__. 

- 在Azure中配置Web应用支持 `continuous deployment using Git <http://azure.microsoft.com/en-us/documentation/articles/web-sites-publish-source-control/>`__.

从Azure门户记录Web应用的Git URL:

.. image:: your-first-mac-aspnet/_static/azure-portal.png

- 在终端窗口, 通过你前面写下的Git URL添加远程仓库命名为 ``azure``.

  - ``git remote add azure https://ardalis-git@firstaspnetcoremac.scm.azurewebsites.net:443/firstaspnetcoremac.git``

- 推送到master.

  - 输入 ``git push azure master`` 部署. 

  .. image:: your-first-mac-aspnet/_static/git-push-azure-master.png

- 浏览最新部署的Web应用. 你应该看到 ``Hello world!``

.. .. image:: your-first-mac-aspnet/_static/azure.png 


额外的资源
--------------------

- `Visual Studio Code`_
- :doc:`/client-side/yeoman`
- :doc:`/fundamentals/index`
