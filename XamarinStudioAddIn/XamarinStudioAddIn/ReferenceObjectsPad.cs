using System;
using Gtk;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Debugger;
using Mono.Debugging.Client;
using System.Diagnostics;

namespace XamarinStudioAddIn
{
	public class ReferenceObjectsPad:IPadContent
	{
		protected Widget _control;
		private IPadWindow _iPadWindow;
		private TreeView _treeView;
		private TreeStore _treeStore;
		private Button _button;

		public ReferenceObjectsPad ()
		{
			
		}

		public void Initialize (IPadWindow window)
		{
			DebugProvider.Write ("Initialize");

//			DebuggingService.CurrentFrameChanged += OnCurrentFrameChanged;
//			DebuggingService.PausedEvent += OnPausedEvent;
//			DebuggingService.ResumedEvent += OnResumedEvent;
//			DebuggingService.StoppedEvent += OnStoppedEvent;
//			DebuggingService.EvaluationOptionsChanged += OnEvaluationOptionsChanged;

			_iPadWindow = window;
			_treeView = new TreeView ();
			_treeView.ClientEvent += OnClientEvent ;

			var nameColumn = new TreeViewColumn (){ Title = "Name " };
			var nameCellRender = new Gtk.CellRendererText ();
			nameColumn.PackStart (nameCellRender, true);
			_treeView.AppendColumn (nameColumn);

			var valueColumn = new TreeViewColumn (){ Title = "Value " };
			var valueCellRender = new Gtk.CellRendererText ();
			valueColumn.PackStart (valueCellRender, true);
			_treeView.AppendColumn (valueColumn);

			var intPtrColumn = new TreeViewColumn (){ Title = "IntPtr " };
			var intPtrCellRender = new Gtk.CellRendererText ();
			intPtrColumn.PackStart (intPtrCellRender, true);
			_treeView.AppendColumn (intPtrColumn);

			nameColumn.AddAttribute (nameCellRender, "text", 0);
			valueColumn.AddAttribute (valueCellRender, "text", 1);
			intPtrColumn.AddAttribute (intPtrCellRender, "text", 2);

			_treeStore = new TreeStore (typeof(string),typeof(string),typeof(string));
			_treeView.Model = _treeStore;
			var tb = window.GetToolbar (PositionType.Top);
			_button = new Button ("Update");
			_button.Clicked += OnClicked;
			tb.Add (_button);
			tb.Add (_treeView);
			_button.Show ();
			_treeView.Show ();
			tb.ShowAll ();
			_iPadWindow.PadContentShown += delegate {
				OnUpdateList();
			};
		}
		public virtual void OnClicked(object sender, EventArgs e)
		{
			DebugProvider.Write ("OnClientEvent");
			OnUpdateList ();
		}
		public virtual void OnClientEvent(object o, ClientEventArgs args)
		{
			DebugProvider.Write ("OnClientEvent");
			OnUpdateList ();
		}

		public void RedrawContent ()
		{
			DebugProvider.Write ("RedrawContent");
		}

		public virtual void OnUpdateList()
		{
			_treeStore.Clear();
			DebugProvider.Write ("OnUpdateList");
			var localVar = DebuggingService.CurrentFrame.GetLocalVariables();

			foreach (var item in localVar) {
				var tt = new Pointer (item.GetRawValue ());
				_treeStore.AppendValues (item.Name, item.Value, tt.IntPtr.ToString());
				DebugProvider.Write ("OnUpdateList Name: " + item.Name + " value: " + item.Value + " raw value: " + item.GetRawValue().ToString());
//				foreach (var tem in item.GetRawValue().GetType().GetProperties()) {
//					DebugProvider.Write("OnUpdateList \t\t " + tem.Name);
//				}
			}
			_treeView.Model = _treeStore;
			_treeView.Show ();
		}

		public Widget Control {
			get {
				return _control;
			}
		}
		public virtual void OnCurrentFrameChanged (object sender, EventArgs e)
		{
			OnUpdateList();
		}
		public virtual void OnPausedEvent (object sender, EventArgs e)
		{
			OnUpdateList();
		}
		public virtual void OnResumedEvent (object sender, EventArgs e)
		{
			OnUpdateList();
		}
		public virtual void OnStoppedEvent (object sender, EventArgs e)
		{
			OnUpdateList();
		}
		public virtual void OnEvaluationOptionsChanged (object sender, EventArgs e)
		{
			OnUpdateList();
		}
		public void Dispose ()
		{
			DebugProvider.Write ("Dispose");
		}
	}
}

