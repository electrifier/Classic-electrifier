# electrifier
Desktop enhancement suite

electrifier is an open-source project. It aims to be a replacement for standard Windows Explorer.
electrifier features a ribbon as a replacement for its menu-bar (which can be used alternatively), full internationalization (currently English and German are supported), full drag and drop-support, allowing you to drag and drop shell objects from and to electrifier windows as also to windows explorer or your desktop, and a full-featured multi-tab experience.

# Development

electrifier is developed using Visual Studio Community 2013. It uses .NET-framework 4.0. Originally, the project was started back in 2004, using .NET-framework 1.0 and Windows XP. However, due to a system crash, I lost all my data, including the source-code, which was stored on a software RAID-5. Meanwhile I managed to restore those lost artefacts, converted them to Visual Studio 2013 and .NET 4.0 and it still works somehow, even using Windows 7 64-bit.

This is why I started this project on GitHub. Originally, the project was under version-control of Subversion, the last days I converted it to GIT and this is what you can see here.

# Contributing

Currently, the project is in an alpha-stadium. However, if you feel interested in helping me out, you are welcome. Please contact me.

# Pre-Requisites

To run electrifier, you supposedly need nothing except a running Windows Operating System, Vista SP1 or above. The .NET-framework should be installed due normal Windows-Update cycle.

To compile the sources:

* Install Visual Studio 2013
* Download the sources, you'll need them
* Open `electrifier/src/Electrifier - Main/Electrifier - Main.sln`
* In sub-project `Core`, right-click `Core/Forms/MainWindowForm.RibbonMarkup.xml` and select `Run Custom Tool`
* You most likely will get a missing reference, `Ribbon`. Select sub-project `Core`, right-click, Properties, Reference Paths, change `D:\src\trunk\src\Electrifier - Main\3rd Party\` to whatever fits your personal development environment. Ribbon.dll is located in folder `Electrifier - Main / 3rd Party`
* Set "Electrifier" as the startup project (Right-click on Solution `Electrifier - Main`, select `Set StartUp Projects...`, `Single startup project`, select `Electrifier` from dropdown
* electrifier will start with a blank UI, click `New Panel` from the ribbon or menu to get a fresh electrifier window
