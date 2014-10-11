using System;
using Gtk;
using System.Collections.Generic;

namespace ExamPrepper
{
	public class CategoryPicker
	{
		public List<QACategory> SelectedCategories
		{
			get
			{
				return null;
			}
		}

		public String cat2str
		{
			get
			{
				String str = "";
				foreach(var c in categories)
				{
					str += c.Category + "\n";
				}

				return str;
			}
		}

		private Gtk.TreeView treeview;
		private List<QACategory> categories;
		private Gtk.ToggleButton[] toggles;

		public CategoryPicker(Gtk.TreeView treeview, List<QACategory> categories)
		{
			Console.WriteLine("Building categorypicker");
			this.treeview = treeview;
			this.categories = categories;
			PopulateTree();
		}

		void PopulateTree()
		{
			Gtk.TreeViewColumn includeColumn = new Gtk.TreeViewColumn ();
			includeColumn.Title = "Include";

			Gtk.TreeViewColumn subjectColumn = new Gtk.TreeViewColumn ();
			subjectColumn.Title = "Subject";

			treeview.AppendColumn (includeColumn);
			treeview.AppendColumn (subjectColumn);

			// Create a model that will hold two strings - Artist Name and Song Title
			Gtk.ListStore subjectListStore = new Gtk.ListStore (typeof (Gtk.ToggleButton), typeof (string));

			// Assign the model to the TreeView
			treeview.Model = subjectListStore;

			toggles = new Gtk.ToggleButton[categories.Count];
			for(int i = 0; i < toggles.Length; i++)
			{
				var t = new Gtk.ToggleButton();
				//t.T
				toggles[i] = t;
				subjectListStore.AppendValues (t, categories[i].Category);
			}


			//subjectListStore.AppendValues ("Garbage", "Dog New Tricks");


			Gtk.CellRendererToggle artistNameCell = new Gtk.CellRendererToggle();
			// Create the text cell that will display the artist name
			//Gtk.CellRendererText artistNameCell = new Gtk.CellRendererText ();

			// Add the cell to the column
			includeColumn.PackStart (artistNameCell, true);

			// Do the same for the song title column
			Gtk.CellRendererText songTitleCell = new Gtk.CellRendererText ();
			subjectColumn.PackStart (songTitleCell, true);

			// Tell the Cell Renderers which items in the model to display
			includeColumn.AddAttribute (artistNameCell, "toggle", 0); // g_object_set_property: object class `GtkCellRendererToggle' has no property named `toggle'


			subjectColumn.AddAttribute (songTitleCell, "text", 1);


			Console.WriteLine("Finished populating tree");
		}
	}
}

