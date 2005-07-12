

namespace Electrifier.Win32API {
	/// <summary>
	/// Classes, interfaces, types and enums derived from include file wtypes.h
	/// </summary>
	public class WTypes {
		/// <summary>
		/// Values from the CLSCTX enumeration are used in activation calls to indicate the
		/// execution contexts in which an object is to be run. These values are also used in
		/// calls to CoRegisterClassObject to indicate the set of execution contexts in which
		/// a class object is to be made available for requests to construct instances.
		/// 
		/// ms-help://MS.VSCC.2003/MS.MSDNQTR.2003FEB.1031/com/htm/cme_a2d_152w.htm
		/// </summary>
		public enum CLSCTX { 
			INPROC_SERVER        = 0x00000001,
			INPROC_HANDLER       = 0x00000002,
			LOCAL_SERVER         = 0x00000004,
			INPROC_SERVER16      = 0x00000008,
			REMOTE_SERVER        = 0x00000010,
			INPROC_HANDLER16     = 0x00000020,
			RESERVED1            = 0x00000040,
			RESERVED2            = 0x00000080,
			RESERVED3            = 0x00000100,
			RESERVED4            = 0x00000200,
			NO_CODE_DOWNLOAD     = 0x00000400,
			RESERVED5            = 0x00000800,
			NO_CUSTOM_MARSHAL    = 0x00001000,
			ENABLE_CODE_DOWNLOAD = 0x00002000,
			NO_FAILURE_LOG       = 0x00004000,
			DISABLE_AAA          = 0x00008000,
			ENABLE_AAA           = 0x00010000,
			FROM_DEFAULT_CONTEXT = 0x00020000,

			INPROC               = ( INPROC_SERVER | INPROC_HANDLER ),
			SERVER               = ( INPROC_SERVER | LOCAL_SERVER | REMOTE_SERVER ),
			ALL                  = ( INPROC_SERVER | LOCAL_SERVER | REMOTE_SERVER | INPROC_HANDLER ),
		};

		private WTypes() {
			// No instantion allowed.
		}
	}
}
