Working with SQL Server LocalDB
=============================================

由 `Rick Anderson`_ 编辑, `Zhang, Jipeng`_ 翻译.

 ``ApplicationDbContext`` 类负责处理连接数据库及 ``Movie`` 实体与数据库记录之间匹配等任务. 数据库的相关环境变量由 *Startup.cs* 文件中 ``ConfigureServices`` 方法提供的 :doc:`依赖注入  </fundamentals/dependency-injection>` 的方式注册 :

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Startup.cs
  :language: c#
  :start-after: // This method gets called by the runtime. Use this method to add services to the container.
  :end-before: services.AddIdentity<ApplicationUser, IdentityRole>()
  :dedent: 8

ASP.NET Core通过 :doc:`配置 </fundamentals/configuration>` 系统读取 ``连接字符串``。 对于本地开发， 配置系统从 *appsettings.json* 文件中读取连接字符串:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/appsettings.json
  :language: javascript
  :lines: 1-6
  :emphasize-lines: 3

当你把程序部署到测试或生产环境时，你可以使用环境变量或其他方式设定连接字符串，使其连接到一个真正的SQL Server上。参考 :doc:`配置 </fundamentals/configuration>` .

SQL Server Express LocalDB
--------------------------------

LocalDB 是一个着眼于程序开发的轻量级SQL Server Express数据库引擎 LocalDB可以在有需要的时候以用户模式启动，所以不涉及复杂的配置。 LocalDB的数据库默认为创建在 *C:/Users/<user>* 目录下的 "\*.mdf" 文件。

- 在 **视图** 菜单下, 打开 **SQL Server Object Explorer** (SSOX).

.. image:: working-with-sql/_static/ssox.png

- 右键点击 ``Movie`` 表 **> 查看设计器**

.. image:: working-with-sql/_static/design.png

.. image:: working-with-sql/_static/dv.png

注意 ``ID`` 旁的钥匙图标。EF默认会创建一个名为 ``ID`` 的属性作为主键.

.. comment: add this when we have it for MVC 6: For more information on EF and MVC, see Tom Dykstra's excellent tutorial on MVC and EF.

- 右键单击 ``Movie`` 表 **> 查看数据**

.. image:: working-with-sql/_static/ssox2.png

.. image:: working-with-sql/_static/vd22.png

 构建数据库
--------------------------

在 *Models* 文件夹中创建一个名为 ``SeedData`` 的新类，用下面代码替换掉生成的代码：

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/SeedData.cs
  :language: c#
  :start-after: // Seed without Rating
  :end-before: #endif

注意如果数据库中存在任何电影记录，初始化构建将会返回。

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/SeedData.cs
  :language: c#
  :start-after: // Look for any movies.
  :end-before: context.Movie.AddRange(
  :dedent: 16

在 *Startup.cs* 文件中，把构建初始化器加到 ``Configure`` 方法的末尾：

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Startup.cs
  :dedent: 8
  :emphasize-lines: 9
  :start-after: app.UseIdentity();
  :end-before: // End of Configure.

测试程序

- 删除数据库中所有记录，你既可以在浏览器中删除所有链接，也可以从SSOX中删除记录。
- 强制程序初始化 (调用 ``Startup`` 类中的方法) 使得构建方法运行。 强制初始化必须要IIS Express停止并重启。 你可以通过以下任意方法完成：

  - 在通知区域右键单击IIS Express系统托盘图标并点击 **退出** 或 **停止网站**

|

.. image:: working-with-sql/_static/iisExIcon.png
  :height: 100px
  :width: 200 px

|

.. image:: working-with-sql/_static/stopIIS.png

|

  - 如果VS运行在非调试模式，按F5进入调试模式
  - 如果VS运行在调试模式，停止调试器并按 ^F5

.. Note:: 如果数据库没有初始化，在 ``if (context.Movie.Any())`` 这一行创建断点并调试。

.. image:: working-with-sql/_static/dbg.png

程序显示构建后的数据

.. image:: working-with-sql/_static/m55.png