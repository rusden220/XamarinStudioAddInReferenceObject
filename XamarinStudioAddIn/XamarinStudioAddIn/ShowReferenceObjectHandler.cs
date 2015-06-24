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
		private string _padId = "XamarinStudioAddIn.ReferenceObjectsPad";
		private string _lable = "Reference object";
		private string _defaultPlacement = "Bottom";
		MonoDevelop.Core.IconId _iconId = null;

		protected override void Run ()
		{
			DebugProvider.Write ("Start");

			Pad pad = null;
			foreach (var item in IdeApp.Workbench.Pads) {
				if (item.Id == _padId) {
					pad = item;
				}
			}

			var content = new XamarinStudioAddIn.ReferenceObjectsPad();
			pad = IdeApp.Workbench.ShowPad(content, _padId, _lable, _defaultPlacement, _iconId);
			if (pad == null) {
				DebugProvider.Write("IdeApp.Workbench.ShowPad return null for: _padId" + _padId);
				return;
			}

//			var icon = Stock.OutputIcon;
//			var title = "ReferenceObjcetPad";
//			var id = title;
//			var rop = new ReferenceObjectsPad ();
//			string basePadId = "OutputPad-" + id + "-0";
//			var pad = IdeApp.Workbench.AddPad (rop, id, title, basePadId + "/Center Bottom", DockItemStatus.AutoHide, icon);
//
//			pad = IdeApp.Workbench.ShowPad (rop, id, title, basePadId + "/Center Bottom", DockItemStatus.AutoHide, icon);
//
//			Document doc = IdeApp.Workbench.ActiveDocument;
//			var textEditorData = doc.GetContent<ITextEditorDataProvider> ().GetTextEditorData ();  
//			string date = DateTime.Now.ToString ();  
//			textEditorData.InsertAtCaret (date); 
		}

		protected override void Update (CommandInfo info)
		{
			Document doc = IdeApp.Workbench.ActiveDocument;  
			info.Enabled = doc != null && doc.GetContent<ITextEditorDataProvider> () != null;  

		}   
	}
}
	