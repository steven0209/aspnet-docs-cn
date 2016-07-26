ASP.NET Core和Entity Framework 6入门
===========================================================

由 `Paweł Grudzień`_ and `Damien Pontifex`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

此文将展示如何使用在ASP.NET Core应用内部使用Entity Framework 6.

.. contents:: Sections:
  :local:
  :depth: 1
    
前期准备
-------------
    
开始前, make sure that you compile against full .NET Framework 在your project.json文件中 as Entity Framework 6 does not support .NET Core. 如果你需要跨平台特性将需要升级到 `Entity Framework Core`_.

在project.json文件中指定完整.NET框架唯一目标版本:

.. code-block:: none
    
    "frameworks": {
        "net46": {}
    }
    
设置连接字符串和依赖注入
-------------------------------------------------

最简单的改变就是从 ``DbContext`` 实例显式取得连接字符串并设置依赖注入. 

在 ``DbContext`` 子类中, 保证有接收查询字符串的构造函数:

.. code-block:: c#
    :linenos:
    
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
    }

在 ``ConfigureServices`` 的 ``Startup`` 类中添加通过查询字符串创建上下文的工厂方法. 上下文应该在范围内仅有一个实例以确保Entity Framework的性能和可靠性. 

.. code-block:: c#
    :linenos:
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped(() => new ApplicationDbContext(Configuration["Data:DefaultConnection:ConnectionString"]));
        
        // Configure remaining services
    }

从配置文件迁移配置到代码
-----------------------------------------

Entity Framework 6允许在XML文件或者代码中中指定配置(在web.config或者app.config中). 而在ASP.NET Core中, 所有配置都是基于代码的.

基于代码的配置是通过创建 ``System.Data.Entity.Config.DbConfiguration`` 的子类实现并添加 ``System.Data.Entity.DbConfigurationTypeAttribute`` 到 ``DbContext`` 子类.

配置文件一般如下所示:

.. code-block:: xml
    :linenos:
    
    <entityFramework>
        <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
            <parameters>
                <parameter value="mssqllocaldb" />
            </parameters>
        </defaultConnectionFactory>
        <providers>
            <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
        </providers>
    </entityFramework>

``defaultConnectionFactory`` 元素为链接设置工厂. 如果此特性没有设置则默认值为 ``SqlConnectionProvider``. 又或者, 提供了值, 给定类将会被用来创建带有 ``CreateConnection`` 方法的 ``DbConnection``. 如果给定工厂没有默认构造函数那你必须添加构造对象的参数.

.. code-block:: c#
    :linenos:

    [DbConfigurationType(typeof(CodeConfig))] // point to the class that inherit from DbConfiguration
    public class ApplicationDbContext : DbContext
    {
        [...]
    }
    
    public class CodeConfig : DbConfiguration
    {
        public CodeConfig()
        {
            SetProviderServices("System.Data.SqlClient",
                System.Data.Entity.SqlServer.SqlProviderServices.Instance);
        }
    }
    
SQL Server, SQL Server Express和LocalDB
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

默认不需要显式配置. 上面的 ``CodeConfig`` 类可用来显式设置提供者服务并且合适的连接字符串应该传递给 ``DbContext`` 构造函数如 `上 <#setup-connection-strings-and-dependency-injection>`_.

总结
-------
Entity Framework 6是对象关系映射(ORM)类库, 有能力通过很少的代价映射类到数据库实体. 迁移大量代码对很多项目不可行, 所以这些特性让它非常受欢迎. 本文展示如何进行迁移而没有讨论ASP.NET新特性的其它部分.

其它资源
--------------------

- `Entity Framework - Code-Based Configuration <https://msdn.microsoft.com/en-us/data/jj680699.aspx>`_

