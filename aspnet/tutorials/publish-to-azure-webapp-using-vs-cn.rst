使用Visual Studio部署ASP.NET Core Web应用到Azure
===========================================================

由 `Rick Anderson`_, `Cesar Blum Silveira`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.


.. contents:: Sections:
  :local:
  :depth: 1

设置开发环境
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

- 安装最新的 `Azure SDK for Visual Studio 2015 <http://go.microsoft.com/fwlink/?linkid=518003>`__. SDK安装Visual Studio 2015如果你还没安装过.

.. note:: 如果你的机器没有太多依赖安装SDK安装也要花费超过30分钟.

- 安装 `.NET Core + Visual Studio tooling <http://go.microsoft.com/fwlink/?LinkID=798306>`__

- 验证你的 `Azure account <https://portal.azure.com/>`__. 你可以 `open a free Azure account <https://azure.microsoft.com/pricing/free-trial/>`__ 或者 `Activate Visual Studio subscriber benefits <https://azure.microsoft.com/pricing/member-offers/msdn-benefits-details/>`__.

创建Web应用
^^^^^^^^^^^^^^^^

在Visual Studio开始页面, 点击 **New Project...**.

.. image:: publish-to-azure-webapp-using-vs/_static/new_project.png

或者, 你可以使用惨淡创建新工程. 点击 **File > New > Project...**.

.. image:: publish-to-azure-webapp-using-vs/_static/alt_new_project.png

完成 **New Project** 对话框:

- 在左面板, 点击 **Web**
- 在中央面板, 点击 **ASP.NET Core Web Application (.NET Core)**
- 点击 **OK**

.. image:: publish-to-azure-webapp-using-vs/_static/new_prj.png

在 **New ASP.NET Core Web Application (.NET Core)** 对话框中:

- 点击 **Web Application**
- 验证 **Authentication** 设置为 **Individual User Accounts**
- 验证 **Host in the cloud** **not** 勾选
- 点击 **OK**

.. image:: publish-to-azure-webapp-using-vs/_static/noath.png

本地测试应用
^^^^^^^^^^^^^^^^^^^^^

- 按 **Ctrl-F5** 在本地运行应用
- 点击 **About** 和 **Contact** 链接. 依赖于你设备的尺寸, 你也许需要点击导航图标显示链接

.. image:: publish-to-azure-webapp-using-vs/_static/show.png

- 点击 **Register** 并注册新用户. 你可以使用虚构邮件地址. 当你提交时, 会得到如下错误你可以:

.. image:: publish-to-azure-webapp-using-vs/_static/mig.png

你可以使用两个不同的方法修复错误:

- 点击 **Apply Migrations** 然后, 等页面更新, 刷新页面; 或者
- 在工程文件夹从命令行运行如下命令:

    dotnet ef database update

应用显示用来注册新用户的邮件和 **Log off** 链接.

.. image:: publish-to-azure-webapp-using-vs/_static/hello.png

部署应用到Azure
^^^^^^^^^^^^^^^^^^^^^^^

在Solution Explorer中右键单击工程并选择 **Publish...**.

.. image:: publish-to-azure-webapp-using-vs/_static/pub.png

在 **Publish** 对话框, 点击 **Microsoft Azure App Service**.

.. image:: publish-to-azure-webapp-using-vs/_static/maas1.png

点击 **New...** 创建新资源组. 创建新资源组让删除所有你在此教程中创建的Azure资源变得容易.

.. image:: publish-to-azure-webapp-using-vs/_static/newrg1.png

创建新的资源组和应用服务计划:

- 点击 **New...** 为新的资源组输入名字
- 点击 **New...** 为应用服务计划选择离你最近的地理区域. 你可以保持默认生成名称
- 点击 **Explore additional Azure services** 创建新数据库

.. image:: publish-to-azure-webapp-using-vs/_static/cas.png

- 点击绿色 **+** 图标创建新的SQL数据库

.. image:: publish-to-azure-webapp-using-vs/_static/sql.png

- 在 **Configure SQL Database** 对话框点击 **New...** 创建新的数据库服务器.

.. image:: publish-to-azure-webapp-using-vs/_static/conf.png

- 输入管理员用户名和密码, 然后点击 **OK**. 别忘了此步创建的用户名和密码. 你可以保留默认的 **Server Name**

.. image:: publish-to-azure-webapp-using-vs/_static/conf_servername.png

.. note:: "admin" 不允许作为管理员用户名.

- 在  **Configure SQL Database** 对话框点击 **OK**

.. image:: publish-to-azure-webapp-using-vs/_static/conf_final.png

- 在 **Create App Service** 对话框点击 **Create** 

.. image:: publish-to-azure-webapp-using-vs/_static/create_as.png

- 在 **Publish** 对话框点击 **Next**

.. image:: publish-to-azure-webapp-using-vs/_static/pubc.png

- **Publish** 对话框的 **Settings** 步骤:

  - 展开 **Databases** 然后勾选 **Use this connection string at runtime**
  - 展开 **Entity Framework Migrations** 然后勾选 **Apply this migration on publish**

- 点击 **Publish** 然后等待直到Visual Studio完成应用发布

.. image:: publish-to-azure-webapp-using-vs/_static/pubs.png

Visual Studio会发布应用到Azure然后在浏览器启动云上的Web应用.

在Azure中测试应用
-----------------------

- 测试 **About** 和 **Contact** 链接
- 注册新用户

.. image:: publish-to-azure-webapp-using-vs/_static/final.png

更新应用
--------------------

- 编辑 ``Views/Home/About.cshtml`` Razor视图文件后修改内容. 例如:

.. code-block:: html
  :emphasize-lines: 7

  @{
      ViewData["Title"] = "About";
  }
  <h2>@ViewData["Title"].</h2>
  <h3>@ViewData["Message"]</h3>

  <p>My updated about page.</p>

- 右键单击工程后再次点击 **Publish...**

.. image:: publish-to-azure-webapp-using-vs/_static/pub.png

- 应用发布后, 检验改变在Azure可用

清理
--------

当你完成应用测试, 到 `Azure portal <https://portal.azure.com/>`__ 删除应用.

- 选择 **Resource groups**, 然后单击你创建的资源组

.. image:: publish-to-azure-webapp-using-vs/_static/portalrg.png

- 在 **Resource group** 处, 点击 **Delete**

.. image:: publish-to-azure-webapp-using-vs/_static/rgd.png

- 输入资源组的名字后点击 **Delete**. 应用和所有在此教程中创建的其它资源都从Azure删除了.

下一步
----------

- :doc:`/tutorials/first-mvc-app/start-mvc`
- :doc:`/intro`
- :doc:`/fundamentals/index`