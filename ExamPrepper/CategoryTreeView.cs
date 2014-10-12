using System;
using Gtk;
using System.Collections.Generic;

namespace ExamPrepper
{
	/// <summary>
	/// Category picker class.
	/// Shows the user the available categories, and allows them to pick/unpick any set of categories.
	/// </summary>
	public class CategoryPicker
	{
		/// <summary>
		/// Gets a list of the selected categories.
		/// </summary>
		/// <value>The selected categories.</value>
		public List<QACategory> SelectedCategories
		{
			get
			{
				var states = GetToggleStates();
				List<QACategory> output = new List<QACategory>(categories.Count);

				for(int i = 0; i < states.Count; i++)
				{
					if(states[i])
					{
						output.Add(categories[i]);
					}
				}

				return output;
			}
		}

		private Gtk.TreeView treeview;
		private List<QACategory> categories;

		public CategoryPicker(Gtk.TreeView treeview, List<QACategory> categories)
		{
			this.treeview = treeview;
			this.categories = categories;
			PopulateTree();
		}

		void PopulateTree()
		{
			Gtk.ListStore subjectListStore = new Gtk.ListStore (typeof (bool), typeof (string));
			treeview.Model = subjectListStore;

			Gtk.TreeViewColumn includeColumn = new Gtk.TreeViewColumn ();
			Gtk.TreeViewColumn subjectColumn = new Gtk.TreeViewColumn();
			includeColumn.Title = "Include";
			subjectColumn.Title = "Subject";
			treeview.AppendColumn (includeColumn);
			treeview.AppendColumn (subjectColumn);

			for(int i = 0; i < categories.Count; i++)
			{
				var t = new Gtk.ToggleButton(i.ToString());
				subjectListStore.AppendValues (t, categories[i].ToString());
			}

			Gtk.CellRendererToggle toggleCell = new Gtk.CellRendererToggle();
			Gtk.CellRendererText textCell = new Gtk.CellRendererText ();

			includeColumn.PackStart (toggleCell, true);
			subjectColumn.PackStart (textCell, true);

			includeColumn.AddAttribute (toggleCell, "active", 0);
			subjectColumn.AddAttribute (textCell, "text", 1);

			toggleCell.Active = true;
			toggleCell.Toggled += ToggleHandler;

			SetAllToTrue();
		}

		void SetAllToTrue()
		{
			treeview.Model.Foreach(new TreeModelForeachFunc(delegate (TreeModel model, TreePath path, TreeIter iter)
			{	
				model.SetValue(iter, 0, true);
				return false;
			}));
		}

		List<bool> GetToggleStates()
		{
			List<bool> states = new List<bool>(categories.Count);

			treeview.Model.Foreach(new TreeModelForeachFunc(delegate (TreeModel model, TreePath path, TreeIter iter2)
			{	
				states.Add((bool)model.GetValue(iter2, 0));
				return false;
			}));

			return states;
		}

		void ToggleHandler(object o, ToggledArgs args)
		{
			TreeIter iter;

			treeview.Model.GetIter(out iter, new TreePath(args.Path));
			bool oldVal = (bool)treeview.Model.GetValue(iter, 0);
			treeview.Model.SetValue(iter, 0, !oldVal);
		}
	}
}

