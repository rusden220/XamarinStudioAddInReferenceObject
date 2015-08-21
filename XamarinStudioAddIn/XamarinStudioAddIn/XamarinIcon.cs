using System;
using Mono.Debugging.Client;

namespace XamarinStudioAddIn
{
	public static class XamarinIcon
	{		
		public static string GetIcon(ObjectValueFlags flags)
		{
			if ((flags & ObjectValueFlags.Field) != 0 && (flags & ObjectValueFlags.ReadOnly) != 0)
				return "md-literal";

			string global = (flags & ObjectValueFlags.Global) != 0 ? "static-" : string.Empty;
			string source;

			switch (flags & ObjectValueFlags.OriginMask)
			{
				case ObjectValueFlags.Property: source = "property"; break;
				case ObjectValueFlags.Type: source = "class"; global = string.Empty; break;
				case ObjectValueFlags.Method: source = "method"; break;
				case ObjectValueFlags.Literal: return "md-literal";
				case ObjectValueFlags.Namespace: return "md-name-space";
				case ObjectValueFlags.Group: return "md-open-resource-folder";
				case ObjectValueFlags.Field: source = "field"; break;
				case ObjectValueFlags.Variable: return "md-variable";
				default: return "md-empty";
			}

			string access;
			switch (flags & ObjectValueFlags.AccessMask)
			{
				case ObjectValueFlags.Private: access = "private-"; break;
				case ObjectValueFlags.Internal: access = "internal-"; break;
				case ObjectValueFlags.InternalProtected:
				case ObjectValueFlags.Protected: access = "protected-"; break;
				default: access = string.Empty; break;
			}
			return "md-" + access + global + source;
		}
	}
}

