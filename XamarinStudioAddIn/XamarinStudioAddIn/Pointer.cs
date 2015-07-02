using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace XamarinStudioAddIn
{
	[StructLayout(LayoutKind.Explicit)]
	public class Pointer
	{

		//public const int GCHANDLESIZE = 4;//var size = sizeof(System.Runtime.InteropServices.GCHandle);

		/// <summary>
		/// wrapper for IntPtr because IntPtr is a structure
		/// </summary>
		class IntPtrWrapper
		{
			public IntPtr _intPtr;
		}
		/// <summary>
		/// wrapper for object because IntPtr use a wrapper
		/// </summary>
		class ObjectWrapper
		{
			public object _object;
		}

		[FieldOffset(0)]
		private ObjectWrapper _objectWrapper;//object for which will get pinter

		[FieldOffset(0)]
		private IntPtrWrapper _intPtrWrapper;//pointer for object

		//[FieldOffset(0)]
		//GCHandle gcHandle;//pinned object 

		/// <summary>
		/// return the object
		/// </summary>
		public object Object
		{
			get { return _objectWrapper._object; }
			set { _objectWrapper._object = value; }
		}
		/// <summary>
		/// pointer for object
		/// </summary>
		internal IntPtr IntPtr
		{
			get { return _intPtrWrapper._intPtr; }
		}
		public Pointer()
		{
			_intPtrWrapper = new IntPtrWrapper();
			_objectWrapper = new ObjectWrapper();
		}
		//~Pointer()
		//{
		//	gcHandle.Free();
		//}
		public Pointer(object obj)
			: this()
		{
			this._objectWrapper._object = obj;
		}
		/// <summary>
		/// return pointer for object
		/// </summary>
		/// <param name="obj">object for which will get pinter</param>
		/// <returns></returns>
		public IntPtr GetPointer()
		{
			if (_objectWrapper._object != null) return GetPointer(_objectWrapper._object);
			else throw new NullReferenceException("you should specify the object");
		}
		/// <summary>
		/// return pointer for object
		/// </summary>
		/// <param name="obj">object for which will get pinter</param>
		/// <returns></returns>
		public IntPtr GetPointer(object obj)
		{
			_objectWrapper._object = obj;
			//gcHandle = GCHandle.Alloc(_objectWrapper._object, GCHandleType.Pinned);
			return _intPtrWrapper._intPtr;
		}

	}
}

