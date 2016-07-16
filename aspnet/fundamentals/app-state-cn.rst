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
会话使用Cookie从不同的浏览器在请求间进行追踪和消除. By default this cookie is named ".AspNet.Session" and uses a path of "/". Further, by default this cookie does not specify a domain, and is not made available to client-side script on the page (because ``CookieHttpOnly`` defaults to ``true``).

These defaults, as well as the default ``IdleTimeout`` (used on the server independent from the cookie), can be overridden when configuring ``Session`` by using ``SessionOptions`` as shown here:

.. code-block:: c#

  services.AddSession(options =>
  {
    options.CookieName = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10);
  });

The ``IdleTimeout`` is used by the server to determine how long a session can be idle before its contents are abandoned. Each request made to the site that passes through the Session middleware (regardless of whether Session is read from or written to within that middleware) will reset the timeout. Note that this is independent of the cookie's expiration.

.. note:: ``Session`` is *non-locking*, so if two requests both attempt to modify the contents of session, the last one will win. Further, ``Session`` is implemented as a *coherent session*, which means that all of the contents are stored together. This means that if two requests are modifying different parts of the session (different keys), they may still impact each other.

ISession
^^^^^^^^^

Once session is installed and configured, you refer to it via HttpContext, which exposes a property called ``Session`` of type :dn:iface:`~Microsoft.AspNetCore.Http.ISession`. You can use this interface to get and set values in ``Session``, such as ``byte[]``.

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

Because ``Session`` is built on top of ``IDistributedCache``, you must always serialize the object instances being stored. Thus, the interface works with ``byte[]`` not simply ``object``. However, there are extension methods that make working with simple types such as ``String`` and ``Int32`` easier, as well as making it easier to get a byte[] value from session.

.. code-block:: c#

  // session extension usage examples
  context.Session.SetInt32("key1", 123);
  int? val = context.Session.GetInt32("key1");
  context.Session.SetString("key2", "value");
  string stringVal = context.Session.GetString("key2");
  byte[] result = context.Session.Get("key3");

If you're storing more complex objects, you will need to serialize the object to a ``byte[]`` in order to store them, and then deserialize them from ``byte[]`` when retrieving them.

使用会话的工作样例
------------------------------

The associated sample application demonstrates how to work with Session, including storing and retrieving simple types as well as custom objects. In order to see what happens when session expires, the sample has configured sessions to last just 10 seconds:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 15-23
  :dedent: 8
  :emphasize-lines: 2,6

When you first navigate to the web server, it displays a screen indicating that no session has yet been established:

.. image:: app-state/_static/no-session-established.png

This default behavior is produced by the following middleware in *Startup.cs*, which runs when requests are made that do not already have an established session (note the highlighted sections):

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 77-107
  :dedent: 12
  :emphasize-lines: 4,6,8-11,28-29

``GetOrCreateEntries`` is a helper method that will retrieve a ``RequestEntryCollection`` instance from ``Session`` if it exists; otherwise, it creates the empty collection and returns that. The collection holds ``RequestEntry`` instances, which keep track of the different requests the user has made during the current session, and how many requests they've made for each path.

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

Fetching the current instance of ``RequestEntryCollection`` is done via the ``GetOrCreateEntries`` helper method:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 109-124
  :dedent: 8
  :emphasize-lines: 4,8-9

When the entry for the object exists in ``Session``, it is retrieved as a ``byte[]`` type, and then deserialized using a ``MemoryStream`` and a ``BinaryFormatter``, as shown above. If the object isn't in ``Session``, the method returns a new instance of the ``RequestEntryCollection``.

In the browser, clicking the Establish session hyperlink makes a request to the path "/session", and returns this result:

.. image:: app-state/_static/session-established.png

Refreshing the page results in the count incrementing; returning to the root of the site (after making a few more requests) results in this display, summarizing all of the requests that were made during the current session:

.. image:: app-state/_static/session-established-with-request-counts.png

Establishing the session is done in the middleware that handles requests to "/session":

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: none
  :lines: 56-75
  :dedent: 12
  :emphasize-lines: 2,8-14

Requests to this path will get or create a ``RequestEntryCollection``, will add the current path to it, and then will store it in session using the helper method ``SaveEntries``, shown below:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 126-132
  :dedent: 8
  :emphasize-lines: 6

``SaveEntries`` demonstrates how to serialize a custom object into a ``byte[]`` for storage in ``Session`` using a ``MemoryStream`` and a ``BinaryFormatter``.

The sample includes one more piece of middleware worth mentioning, which is mapped to the "/untracked" path. You can see its configuration here:

.. literalinclude:: app-state/sample/src/AppState/Startup.cs
  :linenos:
  :language: c#
  :lines: 42-54
  :dedent: 12
  :emphasize-lines: 2,13

Note that this middleware is configured **before** the call to ``app.UseSession()`` is made (on line 13). Thus, the ``Session`` feature is not available to this middleware, and requests made to it do not reset the session ``IdleTimeout``. You can confirm this behavior in the sample application by refreshing the untracked path several times within 10 seconds, and then return to the application root. You will find that your session has expired, despite no more than 10 seconds having passed between your requests to the application.
