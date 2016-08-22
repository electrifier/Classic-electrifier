using System;
using System.Xml;

namespace electrifier.Core
{
	/// <summary>
	/// Zusammenfassung für IPersistent.
	/// </summary>
	public interface IPersistent
	{
		XmlNode CreatePersistenceInfo(XmlDocument targetXmlDocument);
		void    ApplyPersistenceInfo(XmlNode persistenceInfo);
	}
}
