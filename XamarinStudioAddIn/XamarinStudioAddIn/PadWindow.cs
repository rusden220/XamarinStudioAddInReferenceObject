/*using System;
using System.Drawing;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Codons;
using MonoDevelop.Core;
using MonoDevelop.Components.Docking;
using MonoDevelop.Components.Commands;

namespace XamarinStudioAddIn
{
	public class PadWindow: IPadWindow
	{
		string title;
		IconId icon;
		bool isWorking;
		bool hasErrors;
		bool hasNewData;
		IPadContent content;
		PadCodon codon;
		DefaultWorkbench workbench;

		internal DockItem Item { get; set; }

		internal PadWindow (DefaultWorkbench workbench, PadCodon codon)
		{
			this.workbench = workbench;
			this.codon = codon;
			this.title = GettextCatalog.GetString (codon.Label);
			this.icon = codon.Icon;
		}

		public IPadContent Content {
			get {
				CreateContent ();
				return content; 
			}
		}

		public string Title {
			get { return title; }
			set {
				if (title != value) {
					title = value;
					if (StatusChanged != null)
						StatusChanged (this, EventArgs.Empty);
				}
			}
		}

		public IconId Icon  {
			get { return icon; }
			set { 
				if (icon != value) {
					icon = value;
					if (StatusChanged != null)
						StatusChanged (this, EventArgs.Empty);
				}
			}
		}

		public bool IsWorking {
			get { return isWorking; }
			set {
				isWorking = value;
				if (value) {
					hasErrors = false;
					hasNewData = false;
				}
				if (StatusChanged != null)
					StatusChanged (this, EventArgs.Empty);
			}
		}

		public bool HasErrors {
			get { return hasErrors; }
			set {
				hasErrors = value;
				if (value)
					isWorking = false;
				if (StatusChanged != null)
					StatusChanged (this, EventArgs.Empty);
			}
		}

		public bool HasNewData {
			get { return hasNewData; }
			set {
				hasNewData = value;
				if (value)
					isWorking = false;
				if (StatusChanged != null)
					StatusChanged (this, EventArgs.Empty);
			}
		}

		public string Id {
			get { return codon.PadId; }
		}

		public bool Visible {
			get {
				return Item.Visible;
			}
			set {
				Item.Visible = value;
			}
		}

		public bool AutoHide {
			get {
				return Item.Status == DockItemStatus.AutoHide;
			}
			set {
				if (value)
					Item.Status = DockItemStatus.AutoHide;
				else
					Item.Status = DockItemStatus.Dockable;
			}
		}

		public IDockItemLabelProvider DockItemLabelProvider {
			get { return Item.DockLabelProvider; }
			set { Item.DockLabelProvider = value; }
		}

		public bool ContentVisible {
			get { return workbench.IsContentVisible (codon); }
		}

		public bool Sticky {
			get {
				return workbench.IsSticky (codon);
			}
			set {
				workbench.SetSticky (codon, value);
			}
		}

		public DockItemToolbar GetToolbar (Gtk.PositionType position)
		{
			return Item.GetToolbar (position);
		}

		public void Activate (bool giveFocus)
		{
			CreateContent ();
			workbench.ActivatePad (codon, giveFocus);
		}

		void CreateContent ()
		{
			if (this.content == null) {
				this.content = codon.InitializePadContent (this);
			}
		}

		internal IMementoCapable GetMementoCapable ()
		{
			// Don't create the content if not already created
			return content as IMementoCapable;
		}

		internal void NotifyShown ()
		{
			if (PadShown != null)
				PadShown (this, EventArgs.Empty);
		}

		internal void NotifyHidden ()
		{
			if (PadHidden != null)
				PadHidden (this, EventArgs.Empty);
		}

		internal void NotifyContentShown ()
		{
			if (HasNewData)
				HasNewData = false;
			if (HasErrors)
				HasErrors = false;
			if (PadContentShown != null)
				PadContentShown (this, EventArgs.Empty);
		}

		internal void NotifyContentHidden ()
		{
			if (PadContentHidden != null)
				PadContentHidden (this, EventArgs.Empty);
		}

		internal void NotifyDestroyed ()
		{
			if (PadDestroyed != null)
				PadDestroyed (this, EventArgs.Empty);
		}

		public event EventHandler PadShown;
		public event EventHandler PadHidden;
		public event EventHandler PadContentShown;
		public event EventHandler PadContentHidden;
		public event EventHandler PadDestroyed;

		internal event EventHandler StatusChanged;
	}
}

*/