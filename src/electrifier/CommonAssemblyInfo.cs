using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyProduct("electrifier, Codename Machu Picchu")]
[assembly: AssemblyCompany("electrifier.org")]
[assembly: AssemblyCopyright("(c) 2024 electrifier.org, tajbender")]

// Version information for an assembly consists of the following four values:
//
// - Major Version
// - Minor Version 
// - Build Number
// - Revision
//
// You can specify all the values, or you can default
// the Build and Revision Numbers by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.248.1")]
[assembly: AssemblyFileVersion("0.248.1")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
