:version: 1.0.0-rc1

管理应用状态
==========================

`Steve Smith`_ 编辑

在ASP.NET Core中, 应用状态可以使用多种方法进行管理, 取决于状态应该何时和如何被访问.
 这篇文章提供了很多选项的概览, 并且聚焦于安装和配置ASP.NET Core应用支持的会话状态.

.. contents:: Sections
  :local:
  :depth: 1

`查看或下载样例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/app-state/sample>`__

应用状态选项
-------------------------

`应用状态` 代表着所有被用来表示当前应用状态的数据. 这包括全局和用户指定的数据. ASP.NET (甚至ASP)的前版本拥有全局的``Application`` 和 ``Session`` 状态存储的内建支持, 其它选项也是如此.

.. note:: ``Application`` 与 ASP.NET ``Cache``有着相同特征, 拥有少量的能力. 在ASP.NET Core中, ``Application`` 不再存在; 
前版本ASP.NET编写的应用迁移到ASP.NET Core 将 ``Application`` 替换成 a :doc:`/performance/caching/index` 实现. 

应用开发者基于一系列因素可能会自由使用不同的状态存储提供者:

- 数据需要持久化多长时间?
- 数据量有多大?
- 数据格式是什么?
- 可以序列化吗?
- 数据的敏感程度? 它可以被保存到客户端吗?

基于这些问题的答案, ASP.NET Core应用中的应用状态可以通过不同的方法被存储或者管理.

HttpContext.Items
^^^^^^^^^^^^^^^^^

``Items``集合是存储处理已发生请求所需要数据的最佳位置. 它的内容会在每个请求后丢弃. 它最好被用来在组件或者中间件之间进行通信, 这些组件和中间件在请求过程的不同时间点执行, 并且它们之间只是传递参数或者返回值,并没有直接关系. 参照下面的 `Working with HttpContext.Items`_,.

查询字符串和POST方法
^^^^^^^^^^^^^^^^^^^^

请求状态可以通过添加值到新请求的查询字符串或通过POST数据提供给另一个请求. 这些技术不可用于敏感数据, 因为这些技术要求数据被发送到到客户端然后再回传给服务器.
 且最好用于少量数据. 查询字符串对在持久化方式下捕捉状态特别有用, 它允许连接到嵌入的被创建或通过邮件或者社交网络发送的状态, 可能会在将来的某个时间使用. 尽管如此, 不要假设用户创建了请求, 带有查询字符串的URL可以很容易地被共享, 并且必须要小心 `跨站点请求伪造(Cross-Site Request Forgery, CSRF) <https://www.owasp.org/index.php/Cross-Site_Request_Forgery_(CSRF)>`_ 攻击 (例如, 尽管假设只有认证的用户可以使用查询字符串进行操作, 攻击者可以骗取用户访问已认证的URL).

Cookie
^^^^^^^

非常小的状态相关数据可以存储到Cookie中. 它们与所有请求一起发送, 所以尺寸要求保持最新. 理论上, 只有标识信息应该存储到Cookie中, 而真实的数据存储在服务器上, 关联到作为键的标识.

会话(Session)
^^^^^^^

会话存储依赖于基于Cookie的标识来访问浏览器会话相关的数据 (一系列来自特别浏览器和机器的请求). 
没有必要假设会话严格绑定单独用户, 所以要小心你存储在会话中的信息类型.
 在特别的会话中可以很好的存储没有永久存储必要(或者可以从持久化存储重新获得)的应用状态 . 更多详细参照下方 `安装和配置会话`_.

缓存(Cache)
^^^^^

缓存通过开发人员定义的键提供了存储和有效查询随意的应用数据的平衡. 它提供了根据时间让存储项目过期和其它规则. 更多关于 :doc:`/performance/caching/index`.

配置(Configuration)
^^^^^^^^^^^^^

配置可以被想象为另一种形式的应用状态存储, 尽管它常是在应用运行过程中只读. 更多关于 :doc:`configuration`.

其它持久化
^^^^^^^^^^^^^^^^^

任何其它形式的持久化存储, 无论使用Entity Framework和数据库或者如Azure表存储(Table Storage), 都可以被用来存储应用状态, 但是这些都不在ASP.NET直接支持的范围.

与HttpContext.Items一起工作
------------------------------

``HttpContext`` 抽象提供了 ``IDictionary<object, object>`` 接口的简单词典集合, 称为``Items``. 此集合从`HttpRequest``实例的开始到其在请求结束时销毁之前都是可用的. 可以简单通过分配值到键的实例, 或者通过从给定键查询值.

例如, 在一个 :doc:`middleware` 中可以添加信息到 ``Items`` 集合:

.. code-block:: c#

  app.Use(async (context, next) =>
  {
      // perform some verification
      context.Items["isVerified"] = true;
      await next.Invoke();
  });

在后面的管道中, 另一个中间件可以访问它:

.. code-block:: c#

  app.Run(async (context) =>
  {
      await context.Response.WriteAsync("Verified request? " + context.Items["isVerified"]);
  });

.. note:: 鉴于 ``Items`` 里的键都是简单字符串, 如果你开发中间件需要跨应用工作, 你也许希望在你的键前面设置唯一标识的前缀以阻止键冲突 (例如, 使用 "MyComponent.isVerified" 代替 "isVerified").

.. _session: 

安装和配置会话
----------------------------------

ASP.NET Core 发布了会话程序包提供中间件对会话状态进行管理. 你可以在project.json文件中包含``Microsoft.AspNetCore.Session``的引用来安装它.

当程序包安装好, 会话就可以在你应用的 ``Startup`` 类中进行配置. 会话构建于 ``IDistributedCache`` 接口之上, 所以需要根据其特性进行配置, 否则会发生错误.

.. note:: 如果你没有配置任何 ``IDistributedCache`` 实现, 你将会得到 "Unable to resolve service for type 'Microsoft.Extensions.Caching.Distributed.IDistributedCache' while attempting to activate 'Microsoft.AspNetCore.Session.DistributedSessionStore'." 的异常

ASP.NET 发布了 ``IDistributedCache`` 的许多实现, 包括内存(in-memory)选项 (只在开发和测试使用).
 想要设置会话使用该内存(in-memory)选项添加 ``Microsoft.Extensions.Caching.Memory`` 程序包引用到project.json文件然后添加如下代码到 ``ConfigureServices``:

.. code-block:: c#

  services.AddDistributedMemoryCache();
  services.AddSession();

然后, 添加如下代码到 ``Configure`` 就可以在应用代码中使用会话:

.. code-block:: c#

  app.UseSession();

但会话安装和设置后,可以从 ``HttpContext`` 引用.

.. note:: 如果尝试在 ``UseSession`` 被调用前访问``会话`` , 将会得到 ``InvalidOperationException`` 异常, 其信息以 "Session has not been configured for this application or request." 开头.

.. warning:: 如果在已经开始编写``Response``流之后尝试创建新的 ``会话`` (换言之, 会话Cookie还没有被创建) , 将会得到 ``InvalidOperationException`` 异常, 以"The session cannot be established after the response has started" 开头. 此异常也许不会表示在浏览器里;
 可能需要通过查看Web服务器日志发现它, 内容如下:

.. image:: app-state/_static/session-after-response-error.png

实现细节
^^^^^^^^^^^^^^^^^^^^^^
会话使用Cookie从不同的浏览器在请求间进行追踪和消除. 默认Cookie以 ".AspNet.Session" 命名并使用路径 "/".
 还有, 默认此Cookie不明确指定域(domain), 并且在客户端脚本中不可用 (因为 ``CookieHttpOnly`` 默认设置为 ``true``).

这些默认设置当然也包括 ``IdleTimeout`` (用来将服务器从Cookie独立出来), 可以通过如下使用``SessionOptions``配置 ``Session`` 进行覆盖:

.. code-block:: c#

  services.AddSession(options =>
  {
    options.CookieName = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10);
  });

``IdleTimeout`` 被服务器用来决定会话空闲多长时间后被废弃. 每个通过会话中间件的请求 (不管会话是在中间件内进行读取或写入) 都将超时重置. 注意这是独立于Cookie过期的.

.. note:: ``Session`` 是 *non-locking*, 如果两个请求同时尝试修改会话内容, 最后的将会是最终结果. 还有, ``Session`` 被实现为 *coherent session*, 代表所有内容都是存储在一起的. 如果两个请求修改会话的不同部分 (不同键), 它们都将影响对方.

ISession
^^^^^^^^^

当会话安装配置后, 可以通过HttpContext引用到, 它的实例将会曝露称为类型为:dn:iface:`~Microsoft.AspNetCore.Http.ISession`的 ``Session`` 实例属性. 可以使用这个接口获取或设置``Session``值, 如 ``byte[]``.

.. code-block:: c#

  public interface ISession
  {
      bool IsAvailable { get; }
      string Id { get; }
      IEnumerable<string> Keys { get; }
      Task LoadAsync();
      Task CommitAsync();
      bool TryGetValue(string key, out byte[] value);
      void Set(string key, byte[] value);
      void Remove(string key);
      void Clear();
  }

因为 ``Session`` 构建于 ``IDistributedCache``之上, 必须序列化存储的对象化实例. 所以, 与 ``byte[]`` 工作的接口不会简化 ``object``. 尽管如此, 扩展方法让简单类型如 ``String`` 和 ``Int32`` 的工作变得简单, 也让从会话获取 byte[] 值变得容易.

.. code-block:: c#

  // session extension usage examples
  context.Session.SetInt32("key1", 123);
  int? val = context.Session.GetInt32("key1");
  context.Session.SetString("key2", "value");
  string stringVal = context.Session.GetString("key2");
  byte[] result = context.Session.Get("key3");

如果存储更复杂的对象需要序列化对象到 ``byte[]`` 以存储它们, 然后再查询的时候从 ``byte[]`` 反序列化.

使用会话的工作样例
------------------------------

相关的样例应用演示如何与会话工作, 包括存储和查询简单自定义对象类型. 为了展示会话过期会发生什么, 样例设置会话持续只有10秒钟:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 15-23
  :dedent: 8
  :emphasize-lines: 2,6

当你第一次导航到Web服务器, 屏幕会显示还没有会话发布:

.. image:: app-state/_static/no-session-established.png

此默认行为由如下在 *Startup.cs* 中的中间件完成, 当请求发生但还没有发布时运行 (注意高亮部分):

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 77-107
  :dedent: 12
  :emphasize-lines: 4,6,8-11,28-29

``GetOrCreateEntries`` 是帮助方法将会从 ``Session`` 查询存在的 ``RequestEntryCollection`` 实例; 否则, 它将创建并返回空集合. 集合存储 ``RequestEntry`` 实例, 其中保留当前会话中用户的不同请求, 和每个路径有多少请求.

.. literalinclude:: app-state/sample/src/AppState/Model/RequestEntry.cs
  :linenos:
  :language: c#
  :lines: 3-
  :dedent: 4

.. literalinclude:: app-state/sample/src/AppState/Model/RequestEntryCollection.cs
  :linenos:
  :language: c#
  :lines: 6-
  :dedent: 4

通过 ``GetOrCreateEntries`` 帮助方法查找 ``RequestEntryCollection`` 当前实例:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 109-124
  :dedent: 8
  :emphasize-lines: 4,8-9

当对象实例存在于 ``Session``, 它被以 ``byte[]`` 类型查询出来, 然后如下使用 ``MemoryStream`` 和 ``BinaryFormatter`` 反序列化. 如果对象不在 ``Session`` 中, 方法返回 ``RequestEntryCollection`` 的新实例.

在浏览器中, 点击发布会话链接创建一个到路径"/session"的请求, 会返回结果:

.. image:: app-state/_static/session-established.png

刷新页面结果计数会增加; 回到站点根路径 (在创建了少量请求后) 就会出现这样的结果, 总结了当前会话发生的所有请求:

.. image:: app-state/_static/session-established-with-request-counts.png

中间件处理到 "/session" 的请求并完成发布会话:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: none
  :lines: 56-75
  :dedent: 12
  :emphasize-lines: 2,8-14

到此路径的请求会得到或创建新的 ``RequestEntryCollection``, 将会添加最新的路径, 然后如下通过 ``SaveEntries``方法存储在会话中:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 126-132
  :dedent: 8
  :emphasize-lines: 6

``SaveEntries`` 展示如何序列化自定义对象到 ``byte[]`` 然后使用``MemoryStream`` 和 ``BinaryFormatter`` 存储到 ``Session`` 中.

样例包括更多可涉及的中间件, 如映射到 "/untracked" 路径的. 可以参照它的配置:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 42-54
  :dedent: 12
  :emphasize-lines: 2,13

注意此设置的中间件 **before** 创建到 ``app.UseSession()`` 的调用 (13行). 所以, ``Session`` 特性对此中间件不可用, and requests made to it do not reset the session ``IdleTimeout``. You can confirm this behavior in the sample application by refreshing the untracked path several times within 10 seconds, and then return to the application root. You will find that your session has expired, despite no more than 10 seconds having passed between your requests to the application.
