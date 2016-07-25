添加验证
=================================================

由 `Rick Anderson`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

此节你将为 ``Movie`` 模型添加验证逻辑, 然后保证验证规则在用户视图创建或编辑影片时被应用.

保持DRY原则
---------------------

MVC设计信条之一就是 `DRY <http://en.wikipedia.org/wiki/Don't_repeat_yourself>`__ (不要重复你自己"Don't Repeat Yourself"). ASP.NET MVC鼓励你实现一个功能或行为, 然后反映到整个应用中. 这减少了你需要编写的代码量和出现错误的可能, 方便测试和维护.

MVC提供的验证支持和Entity Framework Core代码优先是DRY原则的实例. 你可以在一处声明指定验证规则(在模型类中)而此规则在整个应用中有效.

让我们看看如何在影片应用中发挥验证支持的作用.

给影片模型添加验证规则
-------------------------------------------------

打开 *Movie.cs* 文件. DataAnnotations 提供内建验证特性集合让你可以声明到任何类或方法上. (它也包含格式化的特性如 ``DataType`` 帮助格式化但并不提供任何验证.)

更新 ``Movie`` 类使用 ``Required``, ``StringLength``, ``RegularExpression``, 和 ``Range`` 验证特性.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieDateRatingDA.cs
  :language: none
  :start-after: // Movie with Validation attributes.
  :end-before: // End of Movie
  :dedent: 4
  :emphasize-lines: 5, 12-14, 17-18, 21,22

验证特性指定了你想应用在模型属性上的行为. ``Required`` 和 ``MinimumLength`` 特性指示属性必须有值; 但此验证阻止不了用户输入空格. ``RegularExpression`` 特性用来限制能输入的字符. 上面代码中, ``Genre`` 和 ``Rating`` 只能使用字母(空白, 数字和特殊字符都是不被接受的). ``Range`` 特性限制值在指定范围内. The ``StringLength`` 特性让你设置字符串属性的最大长度, 最小长度选填. 值类型 (如 ``decimal``, ``int``, ``float``, ``DateTime``) 总是必须要有值得不西药 ``[Required]`` 特性.

ASP.NET通过使用验证股则自动地帮助你的应用变得健壮. 也保证你不会忘记验证而粗心的让糟糕的数据进入数据库.

MVC验证错误界面
------------------------------------

运行应用导航到影片控制器.

点击 **Create New** 链接添加新影片. 用一些错误的值填写表单. 很快jQuery客户端验证就会检测到错误并显示错误消息.

.. image:: validation/_static/val.png

.. note:: 你不能在 ``Price`` 字段输入小数点或逗号. 为了支持 `jQuery validation <http://jqueryvalidation.org/>`__ 在非英语地区使用逗号(",")代表小数点, 还有非美国英语日期格式, 你必须全局化你的应用. 更多信息参照 `Additional resources`_. 现在, 只输入整数如10.

注意表单如何在包含非法输入值的时候自动为每个字段渲染合适的错误消息. 错误是由客户端 (使用JavaScript和jQuery) 和服务器端 (如果用户禁用JavaScript)提供出来的.

好处就是你需要更改代码 ``MoviesController`` 类或 *Create.cshtml* 视图中的代码来激活验证界面. 你在教程早些时候创建的控制器和视图会根据 ``Movie`` 模型类属性上的验证特性自动地挑选指定的验证规则. 使用 ``Edit`` 行为方法试着追加和测试相同的验证规则.

表单数据在没有客户端验证错误前不会发送给服务器.
 你可以在 ``HTTP Post`` 方法放置断点来检验, 还要使用到 `Fiddler tool <http://www.telerik.com/fiddler>`__ , 或者 `F12 Developer tools <https://dev.windows.com/en-us/microsoft-edge/platform/documentation/f12-devtools-guide/>`__.

创建视图和创建行为方法中如何发生验证
--------------------------------------------------------------------

你也许想知道为什么不修改任何控制器或视图代码就能生成验证界面. 下一个列表展示了两个 ``Create`` 方法.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :start-after: // GET: Movies/Create
  :end-before: // GET: Movies/Edit/5
  :dedent: 8

第一个 (HTTP GET) ``Create`` 行为方法显示初始化创建表单. 第二个 (``[HttpPost]``) 版本处理表单提交. 第二个 ``Create`` 方法 (``[HttpPost]`` 版本)调用 ``ModelState.IsValid`` 检查镜片是否有任何验证错误. 调用此方法评估所有应用到对象上的验证属性. 如果对象有验证错误, ``Create`` 方法重新显示表单.如果没有错误,
 方法保存新的影片数据到数据库. 在我们的影片示例中, 当客户端检测到验证错误后表单并没有发送给服务器; 因客户端验证错误第二个 ``Create`` 方法不会被调用. 如果你禁用浏览器里的JavaScript, 客户端验证禁用你可以测试 HTTP POST ``Create`` 方法 ``ModelState.IsValid`` 检测到所有验证错误.

你可以在 ``[HttpPost] Create`` 方法中设置断点检验方法没有被调用, 客户端验证在发现错误时不会提交表单数据. 如果你禁用了浏览器的JavaScript, 然后提交带有错误的表单, 断点命中. 你没有JavaScript的帮助还是得到了完整版本. 如下图片展示了如何警用IE中的JavaScript.

.. image:: validation/_static/p8_IE9_disableJavaScript.png

如下图片展示了如何在FireFox中禁用JavaScript.

.. image:: validation/_static/ff.png

如下图片展示了如何在Chrome中禁用JavaScript.

.. image:: validation/_static/chrome.png


禁用JavaScript后, 发送非法数据然后步进调试.

.. image:: validation/_static/ms.png

下面是早些时候脚手架生成的 *Create.cshtml* 视图模板的一部分. 用来通过行为方法展示显示初始化表单和在错误发生时重新显示表单.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Views/Movies/CreateRatingBrevity.cshtml
  :language: HTML
  :emphasize-lines: 9,10,17,18,13
  :lines: 9-35

:doc:`Input Tag Helper </mvc/views/working-with-forms>` 消费 `DataAnnotations <http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx>`__ 特性并产生jQuery客户端验证需要的HTML特性. :doc:`Validation Tag Helper </mvc/views/working-with-forms>` 显示验证错误. 更多信息参照 :doc:`Validation </mvc/models/validation>`.

此方法的好处是控制器和 ``Create`` 视图模板都不需要了解任何实际验证规则和要表示的错误消息. 验证规则和错误字符串值定义在 ``Movie`` 类中. 这些相同的验证规则自动追加到你可能会创建或编辑你模型的 ``Edit`` 视图以及任何其它视图模板.

当你需要改变验证逻辑, 你只需要通过追加验证特性到模型修改一处(此例中的``Movie``类). 你不需要担心应用的其它部分会与规则不一致 — 所有验证逻辑定义于一处而应用到所有地方、. 这保持代码整洁, 易于维护和处理. 也意味着你遵守了DRY原则.

使用DataType特性
--------------------------

打开 *Movie.cs* 文件检查 ``Movie`` 类. ``System.ComponentModel.DataAnnotations`` 命名空间为内建的验证特性集合提供格式化特性. 我们已经应用了 ``DataType`` 枚举值给发布日期和价格字段. 如下代码展示了 ``ReleaseDate`` 和 ``Price`` 属性上都应用了对应的 ``DataType`` 特性.

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieDateRatingDA.cs
  :language: c#
  :lines: 16-19, 25-27
  :dedent: 8
  :emphasize-lines: 2,6

``DataType`` 特性只能为视图引擎提供建议来格式化数据(并提供如URL的 ``<a>`` 特性和邮件的 ``<a href="mailto:EmailAddress.com">``. 你能使用 ``RegularExpression`` 特性验证数据格式. ``DataType`` 特性用来指定特殊的数据类型而不是数据库的固有类型,
 它们不是验证特性. 此例中我只希望能保持日期的跟踪, 而不是时间. ``DataType`` 美剧提供很多数据类型, 如Date, Time, PhoneNumber, Currency, EmailAddress等等. ``DataType`` 特性也能让应用自动提供类型指定的特性. 例如, 为 ``DataType.EmailAddress``创建的 ``mailto:``, 在浏览器中为 ``DataType.Date`` 提供支持HTML5的日期选择器.
   ``DataType`` 特性生成HTML5浏览器可理解的HTML 5 ``data-`` (发音data dash) 特性. ``DataType`` 特性 **not** 提供任何验证.

``DataType.Date`` 不会指定显示的日期格式. 默认数据字段显示是根据服务器端 ``CultureInfo`` 的默认格式.

``DisplayFormat`` 特性用来显式指定日期格式:

.. code-block:: c#

  [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
  public DateTime ReleaseDate { get; set; }

``ApplyFormatInEditMode`` 设置指定当只显示在文本框中用于编辑时应用格式化. (也许不想每个字段都如此 — 例如货币值, 你可能不想货币符号出现在编辑用文本框中.)

你可以单独使用 ``DisplayFormat`` 特性, 但一般使用 ``DataType`` 特性是一个好主意. ``DataType`` 特性表达了数据的语义而不是如何在屏幕上渲染, 并且提供了如下使用DisplayFormat时无法获得的优点:

- 浏览器可以使用HTML5特性(例如展示日历控件, 本地对应货币符号, 邮件链接, 等等.)
- 默认, 浏览器将使用基于 `locale <http://msdn.microsoft.com/en-us/library/vstudio/wyzd2bce.aspx>`__ 的正确格式渲染数据
- ``DataType`` 特性能让MVC选择正确的字段模板渲染数据(``DisplayFormat``只能使用字符串模板).

.. note:: jQuery验证不支持 ``Range`` 特性和 ``DateTime``. 例如, 尽管日期在指定范围内如下代码还将一直表示客户端验证错误:

.. code-block:: c#

  [Range(typeof(DateTime), "1/1/1966", "1/1/2020")]

你可能需要禁用jQuery日期验证以在 ``DateTime`` 上使用 ``Range`` 特性. 在你的模型中硬编码日期并不好, 所以不鼓励使用 ``Range`` 特性和 ``DateTime``.

如下代码展示将特性合并为一行:

.. literalinclude:: start-mvc/sample2/src/MvcMovie/Models/MovieDateRatingDAmult.cs
  :language: none
  :lines: 7-25
  :dedent: 4
  :emphasize-lines: 5,8,11,14,17

系列的下一部分, 我们将评审应用并做一些改进如自动地生成``Details`` 和 ``Delete`` 方法.

额外资源
-----------------------

- :doc:`/mvc/views/working-with-forms`
- :doc:`/fundamentals/localization`
- :doc:`/mvc/views/tag-helpers/intro`
- :doc:`/mvc/views/tag-helpers/authoring`