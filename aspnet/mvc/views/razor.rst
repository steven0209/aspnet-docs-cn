Razor Syntax Reference    
===========================
`Taylor Mullen`_, and `Rick Anderson`_

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
  :width: 400px

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

HTML attributes containing email addresses don’t treat the ``@`` symbol as a transition character.

 ``<a href="mailto:Support@contoso.com">Support@contoso.com</a>``

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
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

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

Explicit Line Transition
^^^^^^^^^^^^^^^^^^^^^^^^

To render an entire line inside of a code block, utilize the @: characters:

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
 
``@switch ``
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
  :width: 350px