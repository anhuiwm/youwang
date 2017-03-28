# Checkout-Mark-DotNet-mini-browser-TLS1.2
The ExpressCheckout ASP.NET C# Web Application contains the PayPal ExpressCheckout API Samples for In-Context Checkout mini browser flow using PayPalMerchantSDK.

The sample code was tested with Microsoft Visual Studio Express 2013 for Web.

For download information, visit https://www.visualstudio.com/downloads/download-visual-studio-vs and look for the following:

* Visual Studio 2013
  * Express 2013 for Web 

Prerequisites
-------------
*	Microsoft Visual Studio Express 2013 for Web (equivalent or better)
  *	Framework 4.5 (supports TLS 1.2)
  *	NuGet Package Manager (for downloading packages)

Run application from Microsoft Visual Studio Express 2013 for Web
-----------------------------------------------------------------

* Open Project: Open the src folder and you will find the ExpressCheckout.sln file. Open the solution into Visual Studio through File-> Open/Project...

* Build Solution: Right click on the solution in Solution Explorer and Build the Solution

* Run Solution: Press F5 / CTRL+F5 to run the Solution

You will be able to see the application running on your browser.

Project Dependencies
---------------------

* PayPalMerchantSDK (version 2.16)
* PayPalCoreSDK (version 1.7)
* Newtonsoft.Json
* log4net

NuGet Package Manager
-------------------------------------------------------

NuGet is the package manager integrated into Visual Studio.  Running the application should initiate
a download of the four project dependency packages.
 
You can also go to Tools --> NuGet Package Manager, and select Manage NuGet Packages for Solution…
On Manage NuGet Packages, search for the particular package to get the details.

Port
---------------------
Microsoft Visual Studio Express 2013 for Web uses an IIS development server with a dynamically assigned port.  To change the port, follow these instructions:


* In Solution Explorer, right-click the name of the application and then select Properties. Click the Web tab.

* In the Servers section, under Use Local IIS Web server, in the Project URL box change the port number.

* To the right of the Project URL box, click Create Virtual Directory, and then click OK.

* In the File menu, click Save Selected Items.

* To verify the change, press CTRL+F5 to run the project. The new port number appears in the address bar of the browser.



