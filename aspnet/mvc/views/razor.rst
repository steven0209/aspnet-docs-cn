Razor Syntax Reference    
===========================
`Taylor Mullen <https://twitter.com/ntaylormullen>`__, and `Rick Anderson`_

.. contents:: Sections
  :local:
  :depth: 1
  
What is Razor?
--------------
Razor is a markup syntax for embedding server based code into web pages. The Razor syntax consists of Razor markup, C# and HTML. Files containing Razor generally have a *.cshtml* file extension.

.. contents:: Sections:
  :local:
  :depth: 1
  
Rendering HTML
--------------

The default Razor language is HTML. Rendering HTML from Razor is no different than in an HTML file. A Razor file with the following markup:

.. code-block:: html

  <p>Hello World</p> 

Is rendered ``<p>Hello World</p>`` by the server.

Rendering server code
----------------------

Razor supports C# and uses the ``@`` symbol to transistion from HTML to C#. 
Razor can transition from HTML into C# or into Razor specific markup. When an ``@`` symbol is followed by a Razor [reserved keyword](TODO, LINK DOWN) it transitions into Razor specific markup, otherwise it transitions into plain C# . 

Razor expressions
---------------------

Implicit Razor expressions start with ``@`` followed by C# code. Explicit Razor expressions starts with ``@`` and are in a block enclosed by ``()`` or ``{}``. For example:

.. literalinclude:: razor/sample/Views/Home/Contact.cshtml
  :language: html
  :start-after: }
  :end-before: @* End of greeting *@ 

Renders this HTML markup:

.. code-block:: html

  <!-- Single statement blocks, explicit.  -->
  
  <!-- Inline expressions, implicit. -->
  <p>The value of your account is: 7 </p>
  <p>The value of myMessage is: Hello World</p>
  
  <!-- Multi-statement block, explicit.  -->
  <p>The greeting is :<br /> Welcome! Today is: Monday -Leap year: True</p>

Which is rendered by a browser as:

.. image:: razor/_static/r1.png
  :scale: 100

Explict expressesion generally cannot contain spaces. For example:

.. literalinclude:: razor/sample/Views/Home/Contact.cshtml
  :language: html
  :start-after: @* End of greeting *@ 
  :end-before: @*Add () to get correct time.*@

Will render the following HTML:

.. code-block:: html
 
  <p>
    Last week: 7/7/2016 4:39:52 PM - TimeSpan.FromDays(7)
  </p>

Adding parenthesis fixes the problem:

.. literalinclude:: razor/sample/Views/Home/Contact.cshtml
  :language: html
  :start-after: @*Add () to get correct time.*@
  :end-before: @*End of correct time*@

Which renders the following HTML:  

.. code-block:: html

  <p>
    Last week: 6/30/2016 4:39:52 PM
 </p>

.. review comment: I removed "unless dictated by the calling of a method". How is that dictated? Need to explain that if we want to add it back.

With the exception of the C# ``await`` keyword implicit expressions must not contain spaces. For example you can intermingle spaces as long as the C# statement has a clear ending:

<p>@await DoSomething("hello", "world")</p>

HTML containing ``@`` symbols may need to be escaped with a second ``@`` symbol. For example:
  
.. code-block:: html

 <p>@@Username</p> 
 
would render the following HTML:

.. code-block:: html
 
 <p>@Username</p> 

.. review: Doesn't Razor treat anything that looks like an email alias as email? Not just in HTML attributes?

.. _razor-email-ref:

``@`` used in an email alias
-----------------------------

HTML attributes containing email addresses don’t treat the ``@`` symbol as a transition character.

 ``<a href="mailto:Support@contoso.com">Support@contoso.com</a>``

Consider the following Razor markup:

.. code-block:: html

  @{
      var joe = new Person("Joe", 33);
   }
  
  <p>Age @joe.Age</p>

Predictibly, the server renders ``<p>Age 33</p>``. But suppose you needed to concatinate the output to get ``Age33`` with no space between "Age" and "33". The following markup:

.. code-block:: html

  @{
      var joe = new Person("Joe", 33);
   }
  
  <p>Age@joe.Age</p>

generates:

.. code-block:: html

  <p>Age@joe.Age</p>

Razor is treating ``Age@joe.Age`` as an email alias. In case like this, create an explicit expression with ``()``:

.. code-block:: html

 <p>Age@(joe.Age)</p>
 
Which renders

.. code-block:: html

  <p>Age33</p>
  
Explict Razor statement blocks surrounded by ``{}`` contain normal C# and therefore each C# statement must be terminated with the ``;`` character. Implict statements don't use ``;`` termination:

.. literalinclude:: razor/sample/Views/Home/Contact.cshtml
  :language: html
  :start-after: }
  :end-before: @* End of greeting *@ 
 
Expression encoding
-------------------

Non-:dn:iface:`~Microsoft.AspNetCore.Html.IHtmlContent` content is HTML encoded. For example, the following Razor markup:

.. code-block:: html

  @("<div>Hello World</div>") 

Renders this HTML:

.. code-block:: html

  &lt;div&gt;Hello World&lt;/div&gt;
  
Which the browser renders as:

``<div>Hello World</div>)`` 

:dn:cls:`~Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper` :dn:method:`~Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.Raw` wraps the HTML markup in an :dn:cls:`~Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper` instance so that it is not encoded but rendered as HTML markup. :dn:cls:`~Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper` implements :dn:iface:`~Microsoft.AspNetCore.Html.IHtmlContent`

.. warning:: Using ``HtmlHelper.Raw`` on unsanitzed user input is a security risk. User input might contain malicious JavaScript or other exploits. Sanitizing user input is difficult, avoid using ``HtmlHelper.Raw`` on user input.

The following Razor markup:

.. code-block:: html

  @Html.Raw("<div>Hello World</div>") 

Renders this HTML:

.. code-block:: html

  <div>Hello World</div> 
  
Which the browser renders as:
``Hello World`` 


Rendering markup in code blocks and implicit transition
-------------------------------------------------------

The default languge in a code block is c#, but you can transition back to HTML. HTML within a code block will transition back into rendering HTML:

.. code-block:: html

  @{
      var inCSharp = true;
      <p>Now in HTML, was in C# @inCSharp</p>
  }


Explicit delimited transition
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

To define a sub-section of a code block that should render HTML, surround the characters to be rendered with the ``<text>`` tag:

.. code-block:: html

  @{
  /* C# */<text>I'm HTML</text>/* C# */
  }

Which renders ``I'm HTML``. You generally use this approach when you want to render HTML that is not surrounded by an HTML tag.

.. _Explicit-Line-Transition-label:

Explicit Line Transition with ``@:``
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

To render an entire line inside of a code block, utilize the ``@:`` characters:

.. code-block:: html

  @{
  /* Still C# */@: <p>Hello World</p> /* This is not C#, it's HTML */
  }

Which renders the following HTML:

.. code-block:: html

  <p>Hello World</p> /* This is not C#, it's HTML */
  
And is rendered in a browser as:
 
.. code-block:: html

  Hello World

  /* This is not C#, it's HTML */ 
  
Consider the following Razor markup which renders a list of names:

.. code-block:: html

  @for (var i = 0; i < people.Length; i++)
  {
      var person = people[i];
      <p>Name: @person.Name</p>
  }

The HTML tag ``<p> </p>`` provides a boundry for Razor to transistion into C#. But suppose you wanted to render the names **without** HTML tags? The following code generates a Razor compilation error:

.. code-block:: html

  @for (var i = 0; i < people.Length; i++)
  {
      var person = people[i];
      Name: @person.Name
  }

Use the ``@:`` characters to specify that Razor should transition from C# to text:

.. code-block:: html

  @for (var i = 0; i < people.Length; i++)
  {
      var person = people[i];
      Name: @person.Name
  }

  
Control Structures
------------------

Control structures are an extension of code blocks. All aspects of code blocks (transitioning to markup, inline C#) also apply to the following structures.

Conditionals ``@if``, ``else if``, ``else`` and ``@switch``
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The ``@if`` family controls when code runs:

.. code-block:: html

  @if (value % 2 == 0)
  {
      <p>The value was even</p>
  }

``else`` and ``else if`` don't require the ``@`` symbol:

.. code-block:: html

 @if (value % 2 == 0)
 {
     <p>The value was even</p>
 }
 else if (value >= 1337)
 {
     <p>The value is large.</p>
 }
 else
 {
     <p>The value was not large and is odd.</p>
 }
 
``@switch``
^^^^^^^^^^^^^

.. code-block:: html

 @switch (value)
 {
     case 1:
         <p>The value is 1!</p>
         break;
     case 1337:
         <p>Your number is 1337!</p>
         break;
     default:
         <p>Your number was not 1 or 1337.</p>
         break;
 }
 
Looping ``@for``, ``@foreach``, ``@while``, and ``@do while``
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can render templated HTML with looping control statements. For example, to render a list of people:

.. code-block:: html

  @{
      var people = new Person[]
      {
            new Person("John", 33),
            new Person("Doe", 41),
      };
  }
  
``@for``
^^^^^^^^^

.. code-block:: html

  @for (var i = 0; i < people.Length; i++)
  {
      var person = people[i];
      <p>Name: @person.Name</p>
      <p>Age: @person.Age</p>
  }

``@foreach``
^^^^^^^^^^^^

.. code-block:: html

  @foreach (var person in people)
  {
      <p>Name: @person.Name</p>
      <p>Age: @person.Age</p>
  }

``@while``
^^^^^^^^^^^^

.. code-block:: html

  @{ var i = 0; }
  @while (i < people.Length)
  {
      var person = people[i];
      <p>Name: @person.Name</p>
      <p>Age: @person.Age</p>
  
      i++;
  }

``@do while``
^^^^^^^^^^^^^^^^

.. code-block:: html

  @{ var i = 0; }
  @do
  {
      var person = people[i];
      <p>Name: @person.Name</p>
      <p>Age: @person.Age</p>
  
      i++;
  } while (i < people.Length);

Compound ``@using``
^^^^^^^^^^^^^^^^^^^^

Compound using statements can be used to represent scoping. For instance, we can utilize :doc:`/mvc/views/html-helpers` to render a form tag with the ``@using`` statement:

.. code-block:: html

  @using (Html.BeginForm())
  {
      // Form content.
  }

You can also perform scope level actions like the above with :doc:`/mvc/views/tag-helpers/index`


``@try``, ``catch``, ``finally`` 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Exception handling is similar to  C#:

.. code-block:: html
  
  @try
  {
      throw new InvalidOperationException("You did something invalid.");
  }
  catch (Exception ex)
  {
      <p>The exception message: @ex.Message</p>
  }
  finally
  {
      // Do something
  }

``@lock``
^^^^^^^^^

Razor has the capability to protect critical sections with lock statements:

.. code-block:: html

  @lock (SomeLock)
  {
      // Do critical section work
  }

Comments
^^^^^^^^^^

Razor supports C# and HTML comments. The following markup:

.. code-block:: html

  @{
      /* C# comment. */
      // Another C# comment.
  }
  <!-- HTML comment -->

Is rendered by the server as:

.. code-block:: html

  <!-- HTML comment -->

Razor comments are removed by the server before the page is rendered. Razor uses ``@*  *@`` to delimit comments. The following code is commented out, so the server will not render any markup:

.. code-block:: html

    @*
    @{
        /* C# comment. */
        // Another C# comment.
    }
    <!-- HTML comment -->
   *@

.. _Razor-Directives-label:

Directives
-----------
Razor directives are represented by implicit expressions with reserved keywords following the ``@`` symbol. A directive will typically change the way a page is parsed or enable different functionality within your Razor page. A Razor page is just a generated C# file. A simple example of what a Razor page generates:

.. code-block:: html

  @{
      Layout = null;
  }
  
  @{
      var output = "Hello World";
  }
  
  <div>Output: @output</div>
 
The Razor markup above will generate a class similar to the following:

.. review: I copy/pasted from what was generated so my call differs from your version.

.. code-block:: c#

  public class _Views_Something_cshtml : RazorPage<dynamic>
  {
      public override async Task ExecuteAsync()
      {
          var output = "Hello World";
  
          WriteLiteral("\r\n<div>Output: ");
          Write(output);
          WriteLiteral("</div>");
      }
  }
   
:ref:`Razor-CustomCompilationService-label` explains how to view this class. Understanding how Razor generates code for a view will make it easier to follow how directives work.
   
``@using``
The ``@using`` directive will add the c# ``using`` directive to the generated razor page:

.. review: You had @using System.Collections.Generic - but that's included in the Razor page.

.. code-block:: html

  @using  System.IO
  @{ 
      var dir = Directory.GetCurrentDirectory();
  }
  <p>@dir</p>
   

``@model``
^^^^^^^^^^^^^

The ``@model`` directive allows you to specify the type of the model past to your Razor page. It uses the following syntax:

``@model TypeNameOfModel``

For example, if you create a new ASP.NET Core MVC app with individual user accounts, the *Views\Account\Login.cshtml* Razor view file contains the follow model declaration:

.. code-block:: c#

  @model LoginViewModel

In the class example in :ref:`Razor-Directives-label`, the class generated inherits from ``RazorPage<dynamic>``. By adding an ``@model`` you control what’s inherited. For example

.. code-block:: c#

  @model LoginViewModel
  
Generates the following class

.. code-block:: c#

 public class _Views_Account_Login_cshtml : RazorPage<LoginViewModel>
 
 
This allows you to access the strongly typed model in your Razor page through the ``Model`` property:

.. code-block:: html

  <div>The Login Email: @Model.Email</div>
  
Obviously you must pass the model from your controller to the view. See :ref:`Strongly-typed-models-keyword-label` for more information.
















   
Working with ``\`` and ``"``
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

To display a backslash character ``\`` or double quotation marks ``"``, use a verbatim string literal that's prefixed with the @ operator. In C#, the ``\`` character has special meaning unless you use a verbatim string literal. 

.. code-block:: html

  <!-- Embedding a backslash in a string -->
  @{ var myFilePath = @"C:\MyFolder\"; }
  <p>The path is: @myFilePath</p>

To embed double quotation marks, use a verbatim string literal and repeat the quotation marks:

.. code-block:: html

  <!-- Embedding double quotation marks in a string -->
  @{ var myQuote = @"The person said: ""Hello, today is Monday."""; }
  <p>@myQuote</p>

The browser rendering of the above Razor markup:

.. image:: razor/_static/r2.png
  :scale: 100
  
  
  
  
  
  
.. _Razor-CustomCompilationService-label:

Viewing the Razor C# class generated for a view
------------------------------------------------

Add the following class to your ASP.NET Core MVC project:

.. literalinclude:: razor\sample\Services\CustomCompilationService.cs

Override the :dn:iface:`~Microsoft.AspNetCore.Mvc.Razor.Compilation.ICompilationService` added by MVC with the above class;

.. literalinclude:: razor\sample\Startup.cs
  :start-after:  Use this method to add services to the container.
  :end-before:  // This method gets called by the runtime.
  :dedent: 8
  :emphasize-lines: 4

Set a break point on the ``Compile`` method of ``CustomCompilationService`` and view ``compilationContent``. 

.. image:: razor/_static/tvr.png
  :scale: 100
  :alt: Text Visualizer view of compilationContent
