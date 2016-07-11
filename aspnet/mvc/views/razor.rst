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

Rendering Server Code
----------------------

Razor supports C# and uses the ``@`` symbol to transistion from HTML to C#. 
Razor can transition from HTML into C# or into Razor specific markup. When an ``@`` symbol is followed by a Razor [reserved keyword](TODO, LINK DOWN) it transitions into Razor specific markup, otherwise it transitions into plain C# . 

Implicit Expressions
---------------------

Implicit Razor expressions starts with ``@`` followed by C# code. For example:

.. literalinclude:: razor/sample/Views/Home/Contact.cshtml
  :language: html
  :start-after: }
  :end-before: @* End of greeting *@ 

Renders this HTML markup:

.. code-block:: html

  <!-- Single statement blocks  -->
  <!-- Inline expressions -->
  <p>The value of your account is: 7 </p>
  <p>The value of myMessage is: Hello World</p>
  
  <!-- Multi-statement block -->
  <p>The greeting is :<br /> Welcome! Today is: Thursday -Leap year: True</p>

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
 
would render as 

.. code-block:: html
 
 <p>@Username</p> 

HTML attributes containing email addresses don’t treat the ``@`` symbol as a transition character.



Explicit Expressions
An explicit Razor expression consists of an @ symbol with balanced parenthesis. To render last weeks’ time with an explicit expression you could do:
<p>Last week this time: @(DateTime.Now - TimeSpan.FromDays(7))</p>
Any content within the yellow parenthesis is evaluated and rendered to the output.
