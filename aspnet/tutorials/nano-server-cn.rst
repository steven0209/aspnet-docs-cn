.. _nano-server:

Nano服务器上的ASP.NET Core
===========================

由 `Sourabh Shirhatti`_ 编辑, `Cui, Richard Chikun <http://richardcuick.github.io/>`__ 翻译.

.. attention:: 
此教程使用Windows Server Technical Preview 5安装选项中的Nano服务器的预发布版本. 你可以使用虚拟磁盘镜像中的软件进行内部的演示和评估. 你不能在真是操作环境中使用这个软件. 请参照 https://go.microsoft.com/fwlink/?LinkId=624232 获取关于预览截止日期的相关信息.

此教程中, 你会修改既存的ASP.NET Core应用并且将其部署到运行IIS的Nano服务器实例.

.. contents:: Sections:
  :local:
  :depth: 1

简介
------------

Nano服务器是Windows Server 2016中的安装选项, 提供比Server Core或完整的服务器更精简, 更好的安全性和更好的服务. 详细请参考官方 `Nano Server documentation <https://technet.microsoft.com/en-us/library/mt126167.aspx>`__. 有三种方法让你尝试Nano服务器:

1.	你可以下载Windows Server 2016技术预览5的ISO文件, 并构建Nano服务器镜像
2.	下载Nano服务器开发者VHD
3.	在Azure Gallery中选择Nano服务器镜像创建VM. 如果没有Azure账户, 你可以获得免费30天试用

此教程中, 我们会从Windows Server技术预览5使用构建好的 `Nano Server Developer VHD <https://msdn.microsoft.com/en-us/virtualization/windowscontainers/nano_eula>`_.

在此之前, 你将需要 :doc:`published </publishing/index>` 既存ASP.NET Core应用输出. 保证应用在 **64-bit** 进行下构建运行.

设置Nano服务器实例
-----------------------------------

`Create a new Virtual Machine using Hyper-V <https://technet.microsoft.com/en-us/library/hh846766.aspx>`_ 在开发机器上使用前面下载的VHD. 机器会要求你登录前设置管理员密码. 在VM控制台, 按F11在第一次登陆前设置密码.

设置本地密码后, 你可以通过PowerShell远程进行管理.

使用PowerShell远程连接到Nano服务器实例
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

打开提升权限的PowerShell窗口添加远程Nano服务器实例到 ``TrustedHosts`` 列表.

.. code:: ps1

  $nanoServerIpAddress = "10.83.181.14"
  Set-Item WSMan:\localhost\Client\TrustedHosts "$nanoServerIpAddress" -Concatenate -Force

``NOTE:`` 用正确的IP地址替换变量 ``$nanoServerIpAddress``.

当添加到Nano服务器实例到 ``TrustedHosts``, 你可以使用PowerShell远程连接它

.. code:: ps1

  $nanoServerSession = New-PSSession -ComputerName $nanoServerIpAddress -Credential ~\Administrator
  Enter-PSSession $nanoServerSession

成功连接后提示行显示格式如: ``[10.83.181.14]: PS C:\Users\Administrator\Documents>``


创建文件共享
---------------------
在Nano服务器创建文件共享就可以通过拷贝向其发布应用. 在远程绘画中运行如下命令:

.. code:: ps1

  mkdir C:\PublishedApps\AspNetCoreSampleForNano
  netsh advfirewall firewall set rule group="File and Printer Sharing" new enable=yes
  net share AspNetCoreSampleForNano=c:\PublishedApps\AspNetCoreSampleForNano /GRANT:EVERYONE`,FULL

运行上面命令后你应该能通过 ``\\<nanoserver-ip-address>\AspNetCoreSampleForNano`` 在宿主机器的Windows资源管理器访问共享.

给Firewall防火墙打开端口
--------------------------
在远程会话中运行如下命令打开防火墙端口监听TCP流量.

.. code:: ps1

  New-NetFirewallRule -Name "AspNet5 IIS" -DisplayName "Allow HTTP on TCP/8000" -Protocol TCP -LocalPort 8000 -Action Allow -Enabled True

安装IIS
--------------

从PowerShell Gallery添加 ``NanoServerPackage`` 提供者. 当提供者安装导入, 你就能安装Windows程序包.

在PowerShell绘画中运行早些时候创建的命令:

.. code:: ps1

  Install-PackageProvider NanoServerPackage
  Import-PackageProvider NanoServerPackage
  Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package

为了快速地验证是否IIS正确安装, 可以访问URL ``http://<nanoserver-ip-address>/`` 并应该看到欢迎页面. 当IIS安装后, 创建的默认站点称作 ``Default Web Site`` 监听80端口.

安装ASP.NET Core模块(ANCM)
-----------------------------------------

ASP.NET Core模块是IIS 7.5+以上模块负责ASP.NET Core HTTP监听器的进程管理并代理请求到进程. 此时, 为IIS安装ASP.NET Core模块的过程还是手动的.
 你会需要在常规机器(不是Nano)安装 `.NET Core Windows Server Hosting bundle <https://dot.net/>`__.
  当捆绑软件包在常规机器上安装后, 你将需要拷贝如下文件到我们早些时候创建的文件共享中去.

在常规(非Nano)机器运行如下拷贝命令:

.. code:: ps1

  copy C:\windows\system32\inetsrv\aspnetcore.dll ``\\<nanoserver-ip-address>\AspNetCoreSampleForNano``
  copy C:\windows\system32\inetsrv\config\schema\aspnetcore_schema.xml ``\\<nanoserver-ip-address>\AspNetCoreSampleForNano``

在Nano服务器上, 你将会需要从早些时候创建的文件共享拷贝如下文件到可用区域.
So, run the following copy commands:

.. code:: ps1

  copy C:\PublishedApps\AspNetCoreSampleForNano\aspnetcore.dll C:\windows\system32\inetsrv\
  copy C:\PublishedApps\AspNetCoreSampleForNano\aspnetcore_schema.xml C:\windows\system32\inetsrv\config\schema\

在远程会话中运行如下脚本:

.. literalinclude:: nano-server/enable-ancm.ps1

``NOTE:`` 上步完成后从共享删除文件 ``aspnetcore.dll`` 和 ``aspnetcore_schema.xml``.

安装.NET Core框架
------------------------------
如果你发布便携应用, .NET Core肯定是安装在目标服务器上. 在PowerShell远程会话中执行如下Powershell脚本来安装.NET框架到Nano服务器.

.. literalinclude:: nano-server/Download-Dotnet.ps1
  :language: powershell

发布应用
--------------------------
将既存应用发布的输出拷贝覆盖文件共享. 

可能需要改变 *web.config* 指出存放 ``dotnet.exe`` 的地方. 换句话说, 你能添加 ``dotnet.exe`` 到你的路径.

当 ``dotnet.exe`` **not** 在路径中web.config示例也许如下:

.. code:: xml

  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="C:\dotnet\dotnet.exe" arguments=".\AspNetCoreSampleForNano.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" forwardWindowsAuthToken="true" />
    </system.webServer>
  </configuration>

在远程会话中运行如下命令为发布的应用在IIS中创建新站点. 为简化此脚本使用 ``DefaultAppPool``. 运行在应用程序池下的更多考虑, 参照 :ref:`apppool`.

.. code:: powershell

  Import-module IISAdministration
  New-IISSite -Name "AspNetCore" -PhysicalPath c:\PublishedApps\AspNetCoreSampleForNano -BindingInformation "*:8000:"

在Nano服务器运行使用.NET Core CLI以及如何工作
---------------------------------------------------------------
如果你使用Nano服务器技术预览5和.NET Core CLI, 你会需要从 ``c:\windows\system32\forwarders`` 拷贝所有文件到 ``c:\windows\system32``, 这是因为存在一个缺陷可能会在下个发布版本中解决.

如果你使用 ``dotnet publish``, 保证你从 ``c:\windows\system32\forwarders`` 拷贝了所有DLL文件到你的发布文件夹.

如果Nano服务器技术预览5构建升级或维护, 请保证重复此过程, 就是要所有DLL文件更新.

运行应用
-----------------------

发布的Web应用可以通过浏览器访问 ``http://<nanoserver-ip-address>:8000``.
如果设置了 :ref:`log-redirection` 中描述的日志, 你可以在 *C:\\PublishedApps\\AspNetCoreSampleForNano\\logs* 查看日志内容.


