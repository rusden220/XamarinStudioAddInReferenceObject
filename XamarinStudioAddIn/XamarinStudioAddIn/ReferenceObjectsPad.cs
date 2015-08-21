
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
using System.Reflection;


namespace XamarinStudioAddIn
{
	public class ReferenceObjectsPad : Gtk.ScrolledWindow, IPadContent
	{
		private TreeViewState _treeViewState;
		private PadTreeView _tree;
		private TreeStore _store;
		private bool _needsUpdate;
		private IPadWindow _window;

		enum Columns
		{
			Icon,
			Name,
			Value,
			Object
		}

		public ReferenceObjectsPad ()
		{
			this.ShadowType = ShadowType.None;

			_store = new TreeStore (typeof(string), typeof (string), typeof(string), typeof(object));

			_tree = new PadTreeView (_store);
			_tree.RulesHint = true;
			_tree.HeadersVisible = true;
			_treeViewState = new TreeViewState (_tree, (int)Columns.Object);

			TreeViewColumn col = new TreeViewColumn ();
			CellRenderer crp = new CellRendererImage ();
			col.PackStart (crp, false);
			col.AddAttribute (crp, "stock_id", (int) Columns.Icon);
			_tree.AppendColumn (col);

			TreeViewColumn FrameCol = new TreeViewColumn ();
			FrameCol.Title = GettextCatalog.GetString ("Name");
			FrameCol.PackStart (_tree.TextRenderer, true);
			FrameCol.AddAttribute (_tree.TextRenderer, "text", (int) Columns.Name);
			//FrameCol.AddAttribute (tree.TextRenderer, "weight", (int) Columns.Weight);
			FrameCol.Resizable = true;
			FrameCol.Alignment = 0.0f;
			_tree.AppendColumn (FrameCol);

			col = new TreeViewColumn ();
			col.Title = GettextCatalog.GetString ("Value");
			col.Resizable = true;
			col.PackStart (_tree.TextRenderer, false);
			col.AddAttribute (_tree.TextRenderer, "text", (int) Columns.Value);
			//col.AddAttribute (tree.TextRenderer, "weight", (int) Columns.Weight);
			_tree.AppendColumn (col);

			Add (_tree);
			ShowAll ();

			UpdateDisplay ();

			_tree.RowActivated += OnRowActivated;
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

		void IPadContent.Initialize (IPadWindow window)
		{
			_window = window;
			window.PadContentShown += delegate {
				if (_needsUpdate)
					Update ();
			};
		}

		public void UpdateDisplay ()
		{
			if (_window != null && _window.ContentVisible)
				Update ();
			else
				_needsUpdate = true;
		}

		void Update ()
		{
			if (_tree.IsRealized)
				_tree.ScrollToPoint(0, 0);
			_treeViewState.Save();
			_store.Clear();			
			try
			{
	
				foreach (var item in DebuggingService.CurrentFrame.GetLocalVariables())
				{
					_store.AppendValues(XamarinIcon.GetIcon(item.Flags), item.Name, item.Value, "11", item );
				}			

			}
			catch (Exception ex)
			{
				DebugProvider.Write("## Exception: " + ex.Message);
			}

			//tree.ExpandAll();
			_treeViewState.Load();
		}

		private bool isExistVariable(object obj, out ObjectValue objectValue)
		{
			objectValue = null;
			foreach (var item in DebuggingService.CurrentFrame.GetLocalVariables())
				{
					if (obj.Equals(item.GetRawValue()))
					{
						objectValue = item;
						return true;
					}					
				}
			return false;
		}

		void OnRowActivated (object s, RowActivatedArgs args)
		{
			TreeIter selected;
			if (!_tree.Selection.GetSelected(out selected))
				return;
			var objectValue = (ObjectValue)_store.GetValue(selected, (int)Columns.Object);
		
			if (objectValue != null)
			{
				DebuggingService.CallStackChanged -= OnStackChanged;
				try
				{
					object refs;
					ReferringObjectsWrapper.GetReferringObjects(objectValue.GetRawValue(),out refs);
					foreach (var item in (Array)refs) {
						if(isExistVariable(item, out objectValue))
						{
							_store.AppendValues(selected, XamarinIcon.GetIcon(ObjectValueFlags.Error), objectValue.Name, objectValue.Value, "11", objectValue.GetRawValue());
						}
					}					
					_tree.ExpandToPath(args.Path);
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
		#region event
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
			void OnStackChanged (object s, EventArgs a)
			{
				UpdateDisplay ();
			}
		#endregion

		

	}
}
