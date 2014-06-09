using System;

namespace ExamPrepper
{
	public partial class ImageDisplayerWindow : Gtk.Window
	{
		public ImageDisplayerWindow(string imagePath) :
			base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			imageArea.Pixbuf = (new Gtk.Image(imagePath)).Pixbuf;
		}
	}
}

