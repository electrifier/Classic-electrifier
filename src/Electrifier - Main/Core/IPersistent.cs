using System.Xml;

namespace electrifier.Core {
	/// <summary>
	/// Summary of IPersistent.
	/// </summary>
	public interface IPersistent {
		void CreatePersistenceInfo(XmlWriter xmlWriter);
		void ApplyPersistenceInfo(XmlNode persistenceInfo);
	}
}
