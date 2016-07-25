
检查详细和删除方法
=====================================================

由 `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

打开影片控制器检查 ``Details`` 方法:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after:  // GET: Movies/Details/5
  :end-before: // GET: Movies/Create
  :dedent: 8

MVC脚手架引擎创建了这个方法并添加注释说明HTTP请求调用方法. 此例中GET请求包含三个URL片段, ``Movies`` 控制器, ``Details`` 方法和 ``id`` 值. 回忆一下在Startup中定义的这些片段.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Startup.cs
  :dedent: 12
  :emphasize-lines: 6
  :start-after: app.UseIdentity();
  :end-before: SeedData.Initialize(app.ApplicationServices);

代码优先使用 ``SingleOrDefaultAsync`` 方法让查询数据变得简单. 方法中内置了重要安全特性通过代码检查搜索方法找到没有发生改变的影片. 例如, 黑客可以将修改链接 *http://localhost:xxxx/Movies/Details/1* 为 *http://localhost:xxxx/Movies/Details/12345* 让应用出错(或者其它不代表存在影片的值). 如果不检查影片是否为Null, 应用汇抛出异常.

检查 ``Delete`` 和 ``DeleteConfirmed`` 方法.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after:  // GET: Movies/Delete/5
  :end-before: private bool MovieExists(int id)
  :dedent: 8

注意 ``HTTP GET Delete`` 方法没有删除指定的影片, 它在你提交(HttpPost)删除时返回影片视图. 通过响应GET请求执行删除操作(或者说, 执行编辑操作, 新建操作或者其它操作会改变数据) 打开了一个安全漏洞.

名为 ``DeleteConfirmed`` 的 ``[HttpPost]`` 方法向HTTP POST方法传递唯一签名或名称删除数据. 两个方法签名如下:

.. code-block:: c#

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)  
  
        // POST: Movies/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)

公共语言运行时(CLR)要求重载的方法要有唯一的参数签名(相同方法名称不同的参数列表). 尽管如此, 这里你需要两个 ``Delete`` 方法 -- 一个是GET一个是POST -- 都有相同的方法签名. (他们都需要接收单个整型数值作为参数.)

有两个方法可解决问题, 一个是使用不同的方法名. 这就是脚手架机制在前面的示例中做的. 尽管如此, 这带来一个小问题: ASP.NET通过名字映射URL片段到行为方法, 如果你重命名方法, 路由就无法正常找到方法. 示例中你看到的解决方案, 添加了 ``ActionName("Delete")`` 特性给 ``DeleteConfirmed`` 方法. 那个特性展示了接收到映射给路由系统URL包含/Delete/的POST请求将会找到 ``DeleteConfirmed`` 方法.

或者让方法拥有一致的名字和签名而人为改变POST方法签名包含额外(未使用)参数. 这就是我们在前一个添加了 ``notUsed`` 参数的提交所做的. 你可以为 ``[HttpPost] Delete`` 方法做相同的事情:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after:  // POST: Movies/Delete/6
  :end-before: // End of DeleteConfirmed
  :dedent: 8


.. ToDo - Next steps, but it really needs to start with Tom's EF/MVC Core