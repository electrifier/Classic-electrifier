using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Electrifier.Win32API;

namespace Electrifier.Core.Shell32 {
	public delegate DragDropEffects DropTargetEnterEventHandler(object source, DropTargetEventArgs e);
	public delegate DragDropEffects DropTargetOverEventHandler (object source, DropTargetEventArgs e);
	public delegate void DropTargetLeaveEventHandler(object source, EventArgs e);
	public delegate void DropTargetDropEventHandler (object source, DropTargetEventArgs e);

	/// <summary>
	/// ShellDragEventArgs
	/// </summary>
	public class DropTargetEventArgs : EventArgs {
		public DropTargetEventArgs(
			WinAPI.IDataObject    DataObject,
			Win32API.DragKeyState KeyState,
			ref WinAPI.POINT      MousePos,
			DragDropEffects       Effects) {
			this.DataObject = DataObject;
			this.KeyState   = KeyState;
			this.MousePos   = MousePos;
			this.Effects    = Effects;
		}

		public WinAPI.IDataObject    DataObject;
		public Win32API.DragKeyState KeyState;
		public WinAPI.POINT          MousePos;
		public DragDropEffects       Effects;
	}

	/// <summary>
	/// Zusammenfassung für ShellDragDropHelper.
	/// </summary>
	public class ShellDragDropHelper : WinAPI.IDropTarget {
		# region Drag and Drop event handler stuff

		public event DropTargetEnterEventHandler DropTargetEnter = null;
		public event DropTargetOverEventHandler  DropTargetOver  = null;
		public event DropTargetLeaveEventHandler DropTargetLeave = null;
		public event DropTargetDropEventHandler  DropTargetDrop  = null;

		#endregion Drag and Drop event handler stuff

		public    ShellAPI.IDropTargetHelper DropTargetHelper { get { return this.dropTargetHelper; } }
		protected ShellAPI.IDropTargetHelper dropTargetHelper = null;

		/// <summary>
		/// Valid only throughout an drag-operation lifecycle
		/// </summary>
		public    WinAPI.IDataObject DragDataObject { get { return this.dragDataObject; } }
		protected WinAPI.IDataObject dragDataObject = null;

		/// <summary>
		/// Valid only throughout an drag-operation lifecycle
		/// </summary>
		public    DragKeyState DragInitialKeyState { get { return this.dragInitialKeyState; } }
		protected DragKeyState dragInitialKeyState = MK.NONE;

		// TODO: Use HandleRef instead?!?
		public    IntPtr OwnerWindowHandle { get { return this.ownerWindowHandle; } }
		protected IntPtr ownerWindowHandle = IntPtr.Zero;


		public ShellDragDropHelper(IntPtr ownerWindowHandle) {
			this.ownerWindowHandle = ownerWindowHandle;

			object instance = null;
			int hResult = WinAPI.CoCreateInstance(ref ShellAPI.CLSID_DragDropHelper, null,
				Win32API.WTypes.CLSCTX.INPROC_SERVER, ref ShellAPI.IID_IDropTargetHelper, out instance);
			// TODO: Remove ThrowException and set preservesig to false for external function CoCreateInstance

			System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hResult);
			this.dropTargetHelper = instance as ShellAPI.IDropTargetHelper;
			int result = WinAPI.RegisterDragDrop(ownerWindowHandle, this);
			// TODO: Release this instance?!? RevokeDragDrop!
	
		}

		public void PrepareDragImage(WinAPI.IDataObject dataObject) {
			object instance = null;
			int hResult = WinAPI.CoCreateInstance(ref ShellAPI.CLSID_DragDropHelper, null,
				Win32API.WTypes.CLSCTX.INPROC_SERVER, ref ShellAPI.IID_IDragSourceHelper, out instance);

			// TODO: Remove ThrowException and set preservesig to false for external function CoCreateInstance
			System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hResult);
			ShellAPI.IDragSourceHelper dragSourceHelper = instance as ShellAPI.IDragSourceHelper;

			WinAPI.POINT pos = new WinAPI.POINT(0,0);
			hResult = dragSourceHelper.InitializeFromWindow(this.OwnerWindowHandle, ref pos, dataObject);

		}

		#region IDropTarget Member

		public int DragEnter(WinAPI.IDataObject pDataObj, DragKeyState KeyState, WinAPI.POINT pt, ref DragDropEffects pdwEffect) {
			this.dragInitialKeyState = KeyState;
			this.dragDataObject = pDataObj;

			if(this.DropTargetEnter != null)
				pdwEffect = this.DropTargetEnter(this, new DropTargetEventArgs(pDataObj, KeyState, ref pt, pdwEffect));

			this.DropTargetHelper.DragEnter(this.OwnerWindowHandle, pDataObj, ref pt, pdwEffect);

			return 0;
		}

		public int DragOver(DragKeyState KeyState, WinAPI.POINT pt, ref DragDropEffects pdwEffect) {
			if(!this.DragInitialKeyState.MouseRight) {
				// Get suggested drop effect by evaluating the keyboard modifier keys
				DragDropEffects suggestedEffect = DragDropEffects.Move;

				if((KeyState.AltControl) || (KeyState.Alt))
					suggestedEffect = DragDropEffects.Link;
				else if(KeyState.Control)
					suggestedEffect = DragDropEffects.Copy;

				if((pdwEffect & suggestedEffect) == suggestedEffect)
					pdwEffect = suggestedEffect;
				else
					pdwEffect = DragDropEffects.None;
			}

			if(this.DropTargetOver != null) {
				DragDropEffects effects = this.DropTargetOver(this, new DropTargetEventArgs(this.DragDataObject, KeyState, ref pt, pdwEffect));

				if(!this.DragInitialKeyState.MouseRight)
					pdwEffect = effects;
			}

			this.DropTargetHelper.DragOver(ref pt, pdwEffect);

			return 0;
		}

		public int DragLeave() {
			if(this.DropTargetLeave != null)
				this.DropTargetLeave(this, EventArgs.Empty);

			this.DropTargetHelper.DragLeave();

			this.dragDataObject = null;
			this.dragInitialKeyState = MK.NONE;

			return 0;
		}

		public int Drop(WinAPI.IDataObject pDataObj, DragKeyState KeyState, WinAPI.POINT pt, ref DragDropEffects pdwEffect) {
			if(!this.DragInitialKeyState.MouseRight) {
				// Get suggested drop effect by evaluating the keyboard modifier keys
				DragDropEffects suggestedEffect = DragDropEffects.Move;

				if((KeyState.AltControl) || (KeyState.Alt))
					suggestedEffect = DragDropEffects.Link;
				else if(KeyState.Control)
					suggestedEffect = DragDropEffects.Copy;

				if((pdwEffect & suggestedEffect) == suggestedEffect)
					pdwEffect = suggestedEffect;
				else
					pdwEffect = DragDropEffects.None;
			}

			KeyState = ((this.DragInitialKeyState.MouseRight) ? MK.RBUTTON : MK.NONE);

			
			if(this.DropTargetDrop != null)
				this.DropTargetDrop(this, new DropTargetEventArgs(pDataObj, KeyState, ref pt, pdwEffect));

			this.DropTargetHelper.DragLeave();

			this.dragDataObject = null;
			this.dragInitialKeyState = MK.NONE;

			return 0;
		}

		#endregion // IDropTarget Member

	}
}
