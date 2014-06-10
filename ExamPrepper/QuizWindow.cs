using System;

namespace ExamPrepper
{
	public partial class QuizWindow : Gtk.Window
	{
		private QuizConductor conductor;
		private QuestionAnswer currentQA;

		private const Gdk.Key hotkeyAcceptAnswer = Gdk.Key.a;
		private const Gdk.Key hotkeyRejectAnswer = Gdk.Key.r;
		private const Gdk.Key hotkeyShowAnswer = Gdk.Key.space;

		private enum State {ShowingAnswer, ShowingQuestion}
		private State state;

		public QuizWindow(QuizConductor conductor) : 
			base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.conductor = conductor;
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
			if(state != State.ShowingAnswer)
				return;

			conductor.MarkAsCorrectlyAnswered();
			GoToNextQuestion();
		}

		private void RejectAnswer()
		{
			if(state != State.ShowingAnswer)
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
			state = State.ShowingQuestion;

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
			if(state == State.ShowingAnswer)
				return;

			state = State.ShowingAnswer;

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

		private void Finish()
		{
			UpdateProgressBar();

			questionTextview.Buffer.Text = "No more questions. Take a break!";
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
	}
}