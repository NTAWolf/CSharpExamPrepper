using System;
using System.Text;
using System.Timers;

namespace ExamPrepper
{
	public partial class QuizWindow : Gtk.Window
	{
		private QuizConductor conductor;
		private QuestionAnswer currentQA;

		private const Gdk.Key hotkeyAcceptAnswer = Gdk.Key.a;
		private const Gdk.Key hotkeyRejectAnswer = Gdk.Key.r;
		private const Gdk.Key hotkeyShowAnswer = Gdk.Key.space;

		private enum QAState {ShowingAnswer, ShowingQuestion}
		private QAState state;
		private DateTime startTime;
		private Timer updateTitleTimer;
		private string baseTitle = "Quiz";

		public TimeSpan ElapsedTime
		{
			get 
			{
				DateTime now = DateTime.Now;
				return (now - startTime);
			}
		}

		public string ElapsedTimeString
		{
			get
			{
				TimeSpan t = ElapsedTime;
				StringBuilder s = new StringBuilder(12);

				if(t.Days > 0)
				{
					s.Append(t.Days + " day" + (t.Days != 1 ? "s" : "") + ", ");
				}
				if(t.Hours > 0 || t.Days > 0)
				{
					s.Append(t.Hours + " hour" + (t.Hours != 1 ? "s" : "") + ", ");
				}
				// Always show elapsed minutes
				s.Append(t.Minutes + " minute" + (t.Minutes != 1 ? "s" : ""));
			
				return s.ToString();
			}
		}

		public QuizWindow(QuizConductor conductor) : 
			base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.conductor = conductor;
			this.startTime = DateTime.Now;
			this.updateTitleTimer = new Timer(20000); // 20 s, so that we won't miss the actual change by much
			this.updateTitleTimer.Elapsed += OnTitleUpdateTimer;
			this.updateTitleTimer.Start();
			GoToNextQuestion();
		}

		protected void OnShowAnswerButtonClicked(object sender, EventArgs e)
		{
			ShowAnswer();
		}

		protected void OnAcceptAnswerButtonClicked(object sender, EventArgs e)
		{
			AcceptAnswer();
		}

		protected void OnRejectAnswerButtonClicked(object sender, EventArgs e)
		{
			RejectAnswer();
		}

		private void AcceptAnswer()
		{
			if(state != QAState.ShowingAnswer)
				return;

			conductor.MarkAsCorrectlyAnswered();
			GoToNextQuestion();
		}

		private void RejectAnswer()
		{
			if(state != QAState.ShowingAnswer)
				return;

			GoToNextQuestion();
		}

		private void GoToNextQuestion()
		{
			currentQA = conductor.NextQuestion;

			if(currentQA == null)
			{
				Finish();
				return;
			}
		
			ShowQuestion();
			UpdateProgressBar();
		}

		private void ShowQuestion()
		{
			state = QAState.ShowingQuestion;

			questionTextview.Buffer.Text = currentQA.Question;

			userResponseTextview.Buffer.Text = "";
			userResponseTextview.Sensitive = true;
			userResponseTextview.GrabFocus();
			answerTextview.Buffer.Text = "";
			answerTextview.Sensitive = false;

			acceptAnswerButton.Sensitive = false;
			rejectAnswerButton.Sensitive = false;
			showAnswerButton.Sensitive = true;
		}

		private void ShowAnswer()
		{
			if(state == QAState.ShowingAnswer)
				return;

			state = QAState.ShowingAnswer;

			answerTextview.Buffer.Text = currentQA.Answer;
			answerTextview.Sensitive = true;

			if(currentQA.HasImage)
			{
				string imagepath = System.IO.Path.Combine(conductor.BasePath, "images", currentQA.ImageFile);
				ShowImage(imagepath);
			}

			userResponseTextview.Sensitive = false;
			acceptAnswerButton.Sensitive = true;
			rejectAnswerButton.Sensitive = true;
			showAnswerButton.Sensitive = false;
		}
			
		private void UpdateProgressBar()
		{
			progressbar.Fraction = conductor.Progress;
			progressText.Text = conductor.NumberOfQuestionsLeft.ToString() + " questions left.";
		}

		private void UpdateTitle()
		{
			this.Title = baseTitle + " - " + ElapsedTimeString;
		}

		private void Finish()
		{
			UpdateProgressBar();

			questionTextview.Buffer.Text = String.Format(
				"No more questions. It took you {0} to complete the quiz. Take a break!",
				ElapsedTimeString);
			questionTextview.Sensitive = false;

			userResponseTextview.Buffer.Text = "";
			userResponseTextview.Sensitive = false;

			answerTextview.Buffer.Text = "";
			answerTextview.Sensitive = false;

			acceptAnswerButton.Sensitive = false;
			rejectAnswerButton.Sensitive = false;
			showAnswerButton.Sensitive = false;
		}

		private void ShowImage(string path)
		{
			ImageDisplayerWindow idw = new ImageDisplayerWindow(path);
			idw.Show();
		}

		protected void OnWindowKeyRelease(object o, Gtk.KeyReleaseEventArgs args)
		{
			if(args.Event.State == Gdk.ModifierType.ControlMask)
			{
				switch(args.Event.Key)
				{
					case hotkeyShowAnswer:
						ShowAnswer();
						break;
					case hotkeyAcceptAnswer:
						AcceptAnswer();
						break;
					case hotkeyRejectAnswer:
						RejectAnswer();
						break;
				}
			}
		}

		private void OnTitleUpdateTimer(Object source, ElapsedEventArgs e)
		{
			UpdateTitle();
		}
	}
}