using MonoDevelop.Components.Commands;
using MonoDevelop.Components.Docking;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;   
using Mono.TextEditor;
using System;  

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
			var icon = Stock.OutputIcon;
			var title = "ReferenceObjcetPad";
			var id = title;
			var rop = new ReferenceObjectsPad ();
			string basePadId = "OutputPad-" + id + "-0";
			var pad = IdeApp.Workbench.AddPad (rop, id, title, basePadId + "/Center Bottom", DockItemStatus.AutoHide, icon);

			pad = IdeApp.Workbench.ShowPad (rop, id, title, basePadId + "/Center Bottom", DockItemStatus.AutoHide, icon);

			Document doc = IdeApp.Workbench.ActiveDocument;
			var textEditorData = doc.GetContent<ITextEditorDataProvider> ().GetTextEditorData ();  
			string date = DateTime.Now.ToString ();  
			textEditorData.InsertAtCaret (date); 
		}

		protected override void Update (CommandInfo info)
		{
			Document doc = IdeApp.Workbench.ActiveDocument;  
			info.Enabled = doc != null && doc.GetContent<ITextEditorDataProvider> () != null;  

		}   
	}
}
	