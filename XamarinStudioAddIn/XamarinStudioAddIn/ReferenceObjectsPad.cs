using System;
using Gtk;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Debugger;
using Mono.Debugging.Client;
using System.Diagnostics;

namespace XamarinStudioAddIn
{
	public class ReferenceObjectsPad: IPadContent
	{
		protected Widget _control;
		private TreeView _treeView;
		private TreeStore _treeStore;
		private IPadWindow _iPadWindow;

		public ReferenceObjectsPad()
		{
			try {
				DebugProvider.Write("Start ReferenceObjectsPad");
				_treeView = new TreeView ();
				//_treeStore = new TreeStore (_treeView.Handle);
				_treeStore = new TreeStore (typeof(string));
				var expCol = new TreeViewColumn ();
				expCol.Title = "Name" ;
				expCol.Resizable = true;
				expCol.Sizing = TreeViewColumnSizing.Fixed;
				expCol.MinWidth = 15;
				_treeView.AppendColumn (expCol);
				_control = new ScrolledWindow ();
				((ScrolledWindow)_control).Add (_treeView);
				((ScrolledWindow)_control).ShowAll ();
			} catch (Exception ex) {
				DebugProvider.Write (ex.Message);
			}

		}
		public void Initialize (IPadWindow window)
		{
			DebugProvider.Write("#### public void Initialize (IPadWindow window) ####");
			_iPadWindow = window;
			_iPadWindow.PadContentShown += delegate {
				OnUpdateList();
			};

		}
		public void RedrawContent ()
		{
			DebugProvider.Write("#### public void RedrawContent () ####");
			OnUpdateList ();
		}

		public virtual void OnUpdateList()
		{
			DebugProvider.Write("#### OnUpdateList() ####");
			//_treeStore.Clear ();
			//var treeIter = _treeStore.AppendNode();
			var localVar = DebuggingService.CurrentFrame.GetLocalVariables();
			foreach (var item in localVar) {
				DebugProvider.Write ("#### Name: " + item.Name + "value: " + item.Value);
			}
			//_treeStore.AppendValues (treeIter, localVar);
		}

		public Widget Control {
			get {
				return _control;
			}
		}

		public void Dispose ()
		{
			
		}
	}
}
	