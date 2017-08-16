using System;
using System.Collections;

namespace electrifier.Core.Services {
    /// <summary>
    /// Summary for ServiceManager.
    /// </summary>
    [Obsolete]
    public class ServiceManager {
		protected        ArrayList      servicesList           = new ArrayList();
		protected        Hashtable      servicesHashtable      = new Hashtable();
		protected static ServiceManager serviceManagerInstance = new ServiceManager();

		public static ServiceManager Services {
			get {
				return serviceManagerInstance;
			}
		}

		public void AddService(IService service) {
			servicesList.Add(service);
		}

		public void AddServices(IService[] services) {
			foreach(IService service in services) {
				AddService(service);
			}
		}

		public void UnloadAllServices() {
			foreach(IService service in servicesList) {
				service.UnloadService();
				servicesList.Remove(service);
				// TODO: remove also from hashtable
			}
		}

		public IService GetService(Type serviceType) {
			IService service = (IService)servicesHashtable[serviceType];

			if(service == null) {
				foreach(IService serviceItem in servicesList) {
					if(serviceItem.GetType().Equals(serviceType)) {
						servicesHashtable[serviceType] = serviceItem;
						service = serviceItem;
						break;
					}
				}
			}

			return service;
		}

		private ServiceManager() {
		}
	}
}
