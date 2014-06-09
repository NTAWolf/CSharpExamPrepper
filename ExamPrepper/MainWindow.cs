using System;
using Gtk;
using ExamPrepper;
using System.IO;

public partial class MainWindow: Gtk.Window
{
	private QuizConductor qc;
	private QuizWindow qw;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnStartButtonClicked(object sender, EventArgs e)
	{
		if(qc == null)
			return;
		
		qw = new QuizWindow(qc);
		qw.Show();
		//throw new NotImplementedException();
	}

	protected void OnOpenQuizFileButtonClicked(object sender, EventArgs e)
	{
		StreamReader stream;
		AskForQuizFile(out stream);
		qc = new QuizConductor(stream);
		stream.Close();
		//throw new NotImplementedException();
	}

	public void AskForQuizFile(out StreamReader rawText)
	{
		Gtk.FileChooserDialog fc =
			new Gtk.FileChooserDialog("Choose the quiz to open",
				this,
				FileChooserAction.Open,
				"Cancel",ResponseType.Cancel,
				"Open",ResponseType.Accept);

		if (fc.Run() == (int)ResponseType.Accept) 
		{
			rawText = new StreamReader(fc.Filename);
			//System.IO.FileStream file = System.IO.File.OpenRead(fc.Filename);
			//file.Close();
		}
		else
		{
			throw new ApplicationException("Could for some reason not read the wanted file: " + fc.Filename);
		}
		//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
		fc.Destroy();
	}
}
