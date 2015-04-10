using System;
using System.Xml;

namespace Electrifier.Core
{
	/// <summary>
	/// Zusammenfassung f�r IPersistent.
	/// </summary>
	public interface IPersistent
	{
		XmlNode CreatePersistenceInfo(XmlDocument targetXmlDocument);
		void    ApplyPersistenceInfo(XmlNode persistenceInfo);
	}
}
