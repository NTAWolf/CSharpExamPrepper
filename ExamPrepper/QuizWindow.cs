using System;

namespace ExamPrepper
{
	public partial class QuizWindow : Gtk.Window
	{
		private QuizConductor conductor;
		private string answer;

		public QuizWindow(QuizConductor conductor) : 
			base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.conductor = conductor;
			ShowQuestion();
		}

		protected void OnShowAnswerButtonClicked(object sender, EventArgs e)
		{
			answerTextview.Buffer.Text = "Thanx!";
			EvaluateAnswer();
		}

		protected void OnAcceptAnswerButtonClicked(object sender, EventArgs e)
		{
			//throw new NotImplementedException();
			// Set answer as satisfactory: Discard the QA.

			conductor.MarkAsCorrectlyAnswered();
			GoToNextQuestion();
		}

		protected void OnRejectAnswerButtonClicked(object sender, EventArgs e)
		{
			//throw new NotImplementedException();
			// Set answer as not satisfactory
			GoToNextQuestion();
		}

		private void GoToNextQuestion()
		{
			QuestionAnswer qa = conductor.NextQuestion;

			questionTextview.Buffer.Text = qa.Question;
			answerTextview.Buffer.Text = "";
			answer = qa.Answer;
			ShowQuestion();
		}

		private void ShowQuestion()
		{
			userResponseTextview.Buffer.Text = "";
			userResponseTextview.Sensitive = true;
			answerTextview.Sensitive = false;
			acceptAnswerButton.Sensitive = false;
			rejectAnswerButton.Sensitive = false;
			showAnswerButton.Sensitive = true;
		}

		private void EvaluateAnswer()
		{
			userResponseTextview.Sensitive = false;
			answerTextview.Buffer.Text = answer;
			answerTextview.Sensitive = true;
			acceptAnswerButton.Sensitive = true;
			rejectAnswerButton.Sensitive = true;
			showAnswerButton.Sensitive = false;
		}

		private void UpdateQuestionsLeftDisplay()
		{

		}
	}
}

