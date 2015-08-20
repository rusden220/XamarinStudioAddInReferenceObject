// ReferenceObjectsPad.cs

using Gtk;

using System;


using MonoDevelop.Ide.Gui;
using Mono.Debugging.Client;


using Stock = MonoDevelop.Ide.Gui.Stock;
using MonoDevelop.Core;
using MonoDevelop.Components;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide;

using MonoDevelop.Debugger;


namespace XamarinStudioAddIn
{
	public class ReferenceObjectsPad : Gtk.ScrolledWindow, IPadContent
	{
		private TreeViewState treeViewState;
		private PadTreeView tree;
		private TreeStore store;
		private bool needsUpdate;
		private IPadWindow window;

		enum Columns
		{
			Icon,
			Name,
			Value,
			IntPtr,
			Object,
			Weight
		}

		public ReferenceObjectsPad ()
		{
			this.ShadowType = ShadowType.None;

			store = new TreeStore (typeof(string), typeof (string), typeof(string), typeof(string), typeof(object), typeof(int), typeof(string));

			tree = new PadTreeView (store);
			tree.RulesHint = true;
			tree.HeadersVisible = true;
			treeViewState = new TreeViewState (tree, (int)Columns.Object);

			TreeViewColumn col = new TreeViewColumn ();
			CellRenderer crp = new CellRendererImage ();
			col.PackStart (crp, false);
			col.AddAttribute (crp, "stock_id", (int) Columns.Icon);
			tree.AppendColumn (col);

			TreeViewColumn FrameCol = new TreeViewColumn ();
			FrameCol.Title = GettextCatalog.GetString ("Name");
			FrameCol.PackStart (tree.TextRenderer, true);
			FrameCol.AddAttribute (tree.TextRenderer, "text", (int) Columns.Name);
			FrameCol.AddAttribute (tree.TextRenderer, "weight", (int) Columns.Weight);
			FrameCol.Resizable = true;
			FrameCol.Alignment = 0.0f;
			tree.AppendColumn (FrameCol);

			col = new TreeViewColumn ();
			col.Title = GettextCatalog.GetString ("Value");
			col.Resizable = true;
			col.PackStart (tree.TextRenderer, false);
			col.AddAttribute (tree.TextRenderer, "text", (int) Columns.Value);
			col.AddAttribute (tree.TextRenderer, "weight", (int) Columns.Weight);
			tree.AppendColumn (col);

			col = new TreeViewColumn ();
			col.Title = GettextCatalog.GetString ("IntPtr");
			col.Resizable = true;
			col.PackStart (tree.TextRenderer, false);
			col.AddAttribute (tree.TextRenderer, "text", (int) Columns.IntPtr);
			col.AddAttribute (tree.TextRenderer, "weight", (int) Columns.Weight);
			tree.AppendColumn (col);

			Add (tree);
			ShowAll ();

			UpdateDisplay ();

			tree.RowActivated += OnRowActivated;
			DebuggingService.CallStackChanged += OnStackChanged;
			DebuggingService.PausedEvent += OnDebuggerPaused;
			DebuggingService.ResumedEvent += OnDebuggerResumed;
			DebuggingService.StoppedEvent += OnDebuggerStopped;
		}

		public override void Dispose ()
		{
			base.Dispose ();
			DebuggingService.CallStackChanged -= OnStackChanged;
			DebuggingService.PausedEvent -= OnDebuggerPaused;
			DebuggingService.ResumedEvent -= OnDebuggerResumed;
			DebuggingService.StoppedEvent -= OnDebuggerStopped;
		}

		void OnStackChanged (object s, EventArgs a)
		{
			UpdateDisplay ();
		}

		void IPadContent.Initialize (IPadWindow window)
		{
			this.window = window;
			window.PadContentShown += delegate {
				if (needsUpdate)
					Update ();
			};
		}

		public void UpdateDisplay ()
		{
			if (window != null && window.ContentVisible)
				Update ();
			else
				needsUpdate = true;
		}

		void Update ()
		{
			if (tree.IsRealized)
				tree.ScrollToPoint(0, 0);
			treeViewState.Save();
			store.Clear();
			try
			{				
				foreach (var item in DebuggingService.CurrentFrame.GetLocalVariables())
				{
					store.AppendValues(GetIcon(item.Flags), item.Name, item.Value, "11", item );
				}
				//var processes = DebuggingService.DebuggerSession.GetProcesses();

				//if (processes.Length == 1)
				//{
				//	AppendThreads(TreeIter.Zero, processes[0]);
				//}
				//else
				//{
				//	foreach (var process in processes)
				//	{
				//		TreeIter iter = store.AppendValues(null, process.Id.ToString(), process.Name, process, (int)Pango.Weight.Normal, "");
				//		AppendThreads(iter, process);
				//	}
				//}
			}
			catch (Exception ex)
			{
				DebugProvider.Write("## Exception: " + ex.Message);
			}

			//tree.ExpandAll();
			treeViewState.Load();
		}

		//void AppendThreads (TreeIter iter, ProcessInfo process)
		//{
		//	var threads = process.GetThreads ();

		//	Array.Sort (threads, (ThreadInfo t1, ThreadInfo t2) => t1.Id.CompareTo (t2.Id));

		//	DebuggingService.DebuggerSession.FetchFrames (threads);

		//	foreach (var thread in threads) {
		//		ThreadInfo activeThread = DebuggingService.DebuggerSession.ActiveThread;
		//		var name = thread.Name == null && thread.Id == 1 ? "Main Thread" : thread.Name;
		//		var weight = thread == activeThread ? Pango.Weight.Bold : Pango.Weight.Normal;
		//		var icon = thread == activeThread ? Gtk.Stock.GoForward : null;

		//		if (iter.Equals (TreeIter.Zero))
		//			store.AppendValues (icon, thread.Id.ToString (), name, thread, (int) weight, thread.Location);
		//		else
		//			store.AppendValues (iter, icon, thread.Id.ToString (), name, thread, (int) weight, thread.Location);
		//	}
		//}

		//void UpdateThread (TreeIter iter, ThreadInfo thread, ThreadInfo activeThread)
		//{
		//	var weight = thread == activeThread ? Pango.Weight.Bold : Pango.Weight.Normal;
		//	var icon = thread == activeThread ? Gtk.Stock.GoForward : null;

		//	store.SetValue (iter, (int) Columns.Weight, (int) weight);
		//	store.SetValue (iter, (int) Columns.Icon, icon);
		//}		

		void OnRowActivated (object s, RowActivatedArgs args)
		{
			TreeIter selected;
			if (!tree.Selection.GetSelected(out selected))
				return;
			var objectValue = (ObjectValue)store.GetValue(selected, (int)Columns.Object);

			var objectValue1 = (string)store.GetValue(selected, (int)Columns.Name);

			if (objectValue != null)
			{
				DebuggingService.CallStackChanged -= OnStackChanged;
				try
				{	
					//foreach (var item in GetReferingObject())
					//{
					//	store.AppendValues(GetIcon(item.Flags), item.Name, item.Value, "11", item );
					//}

					store.AppendValues(selected, GetIcon(ObjectValueFlags.Error), objectValue.Name + "->child", "value", "intPtr", objectValue);// Note: setting the active thread causes CallStackChanged to be emitted, but we don't want to refresh our thread list.
					tree.ExpandToPath(args.Path);
				}
				catch (Exception ex)
				{
					DebugProvider.Write ("Exception: " + ex.Message);
				}
				finally
				{
					DebuggingService.CallStackChanged += OnStackChanged;
				}
			}
		}

		public Widget Control {
			get { return this; }
		}

		public string Id {
			get { return "XamarinStudioAddIn.ReferenceObjectsPad"; }
		}

		public string DefaultPlacement {
			get { return "Bottom"; }
		}

		public void RedrawContent ()
		{
			UpdateDisplay ();
		}

		void OnDebuggerPaused (object s, EventArgs a)
		{
			UpdateDisplay ();
		}

		void OnDebuggerResumed (object s, EventArgs a)
		{
			UpdateDisplay ();
		}

		void OnDebuggerStopped (object s, EventArgs a)
		{
			UpdateDisplay ();
		}

		private string GetIcon(ObjectValueFlags flags)
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
