using System;
using System.IO;
using System.Collections.Generic;
using Gtk;

namespace ExamPrepper
{
	public class QuizConductor
	{
		private List<QuestionAnswer> questionAnswers;
		private List<QuestionAnswer> unusedQAs;
		private List<QACategory> categories;
		private QuestionAnswer currentQA = null;

		public List<QACategory> Categories
		{
			get
			{
				return categories;
			}
		}

		public int NumberOfQuestionsLeft
		{
			get
			{
				return unusedQAs.Count;
			}
		}

		public int NumberOfQuestions
		{
			get
			{
				return questionAnswers.Count;
			}
		}

		public double Progress
		{
			get
			{
				return 1.0 - (double)NumberOfQuestionsLeft / (double)NumberOfQuestions;
			}
		}

		public QuestionAnswer NextQuestion
		{
			get
			{
				if(unusedQAs.Count > 0)
				{
					return GetNextQuestionAnswer();
				}
				else
				{
					return null;
				}
			}
		}

		public string BasePath
		{
			get;
			private set;
		}

		public QuizConductor(StreamReader rawText, string basePath)
		{
			BasePath = basePath;
			ReadQAs(rawText);
			UseQAsInCategories(categories);
			InitiateUnusedQAs();
		}

		public QuizConductor(List<QACategory> categories)
		{
			this.categories = categories;
			UseQAsInCategories(categories);
			InitiateUnusedQAs();
		}

		public void ReadQAs(StreamReader rawText)
		{
			categories = new List<QACategory>(10);
			RemoveNonessentialLines(rawText);

			while(false == rawText.EndOfStream)
			{
				QACategory temp = new QACategory(rawText);
				categories.Add(temp);
				RemoveNonessentialLines(rawText);
			}
		}

		private void UseQAsInCategories(List<QACategory> categories)
		{
			questionAnswers = new List<QuestionAnswer>(50);
			foreach(var c in categories)
			{
				foreach(var qa in c.QuestionAnswers)
				{
					questionAnswers.Add(qa);
				}
			}
		}

		private void RemoveNonessentialLines(StreamReader stream)
		{
			while( "\n\r\f".Contains(stream.Peek().ToString()) )
			{
				if(stream.EndOfStream)
					break;
				stream.ReadLine();
			}
		}

		private QuestionAnswer GetNextQuestionAnswer()
		{ 
			QuestionAnswer qa;

			qa = GetRandomElement(unusedQAs);

			if(currentQA != null && unusedQAs.Count > 1)
			{
				while(currentQA == qa)
				{
					qa = GetRandomElement(unusedQAs);
				}
			}

			currentQA = qa;

			return currentQA;
		}

		private QuestionAnswer GetRandomElement(List<QuestionAnswer> list)
		{
			Random rnd = new Random();
			return list[(int)(unusedQAs.Count * rnd.NextDouble())];
		}

		public void MarkAsCorrectlyAnswered()
		{
			if(currentQA == null)
				return;

			MarkAsCorrectlyAnswered(currentQA);
		}

		public void MarkAsCorrectlyAnswered(QuestionAnswer qa)
		{
			unusedQAs.Remove(qa);
		}

		private void InitiateUnusedQAs()
		{
			unusedQAs = new List<QuestionAnswer>(questionAnswers);
		}
	}
}