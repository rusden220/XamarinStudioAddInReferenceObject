using System;
using Mono.Debugging.Client;
using System.Reflection;



namespace XamarinStudioAddIn
{
	public static class ReferringObjectsWrapper
	{
		public static void 	GetReferringObjects(object obj, out object args)
		{
			//the part of code taked from 
			//https://github.com/alexanderkyte/mono/blob/bd2cf7c8e9053c9d9df593f76400d38d68dbc284/mcs/class/corlib/Test/System/GCQuery.cs
			Assembly assembly = typeof (GC).Assembly;		
			Type GCQuery = assembly.GetType ("System.GCQuery");
			var getReferringObjects = GCQuery.GetMethod ("GetReferringObjects", BindingFlags.Static | BindingFlags.NonPublic);
			Type referringObject = assembly.GetType ("System.ReferringObject");
			var ptr_offset_field = referringObject.GetField ("ptr_offset", BindingFlags.NonPublic | BindingFlags.Instance);
			var referring_object_field = referringObject.GetField ("referring_object", BindingFlags.NonPublic | BindingFlags.Instance);
			object [] arrayArg = new object [2];
			// target
			arrayArg[0] = obj;
			// out parameter
			arrayArg[1] = null;
			getReferringObjects.Invoke(null, arrayArg);
			args = arrayArg[1];

			
		}
	}
}

