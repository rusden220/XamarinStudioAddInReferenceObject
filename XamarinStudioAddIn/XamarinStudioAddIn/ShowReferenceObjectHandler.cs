using MonoDevelop.Components.Commands;
using MonoDevelop.Components.Docking;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;   
using Mono.TextEditor;
using System;  
using System.Diagnostics;

namespace XamarinStudioAddIn
{
	public enum ShowReferenceObjectCommands
	{
		ShowRefObj,
	}
	class ShowReferenceObjectHandler : CommandHandler
	{
		protected override void Run ()
		{
			DebugProvider.Write ("Run");
		}

		protected override void Update (CommandInfo info)
		{
			DebugProvider.Write ("Update");
		}   
	}
}
