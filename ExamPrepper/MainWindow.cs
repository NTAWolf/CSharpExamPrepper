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
	}

	protected void OnOpenQuizFileButtonClicked(object sender, EventArgs e)
	{
		string quizPath = AskForQuizFilePath();
		Console.WriteLine("Reading quiz from " + quizPath);
		using(StreamReader quizStream = new StreamReader(quizPath))
		{
			string basePath = System.IO.Path.GetDirectoryName(quizPath);
			qc = new QuizConductor(quizStream, basePath);
		}

		Console.WriteLine("Finished reading quiz");
	}

	public string AskForQuizFilePath()
	{
		string output;

		Gtk.FileChooserDialog fc =
			new Gtk.FileChooserDialog("Choose the quiz to open",
				this,
				FileChooserAction.Open,
				"Cancel",ResponseType.Cancel,
				"Open",ResponseType.Accept);

		if (fc.Run() == (int)ResponseType.Accept) 
		{
			output = fc.Filename;
		}
		else
		{
			throw new ApplicationException("Could for some reason not read the wanted file: " + fc.Filename);
		}

		//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
		fc.Destroy();

		return output;
	}
}
