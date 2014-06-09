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
		private QuestionAnswer currentQA = null;


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
			InitiateUnusedQAs();
		}

		public void ReadQAs(StreamReader rawText)
		{
			questionAnswers = new List<QuestionAnswer>(50);

			RemoveNonessentialLines(rawText);

			while(false == rawText.EndOfStream)
			{
				// Determine source, tags
				// For now, disregard source, tags
				// 		Disregard all lines not starting with ?

				// Create & store questions	
				QuestionAnswer temp = new QuestionAnswer(rawText, null, null);
				questionAnswers.Add(temp);

				RemoveNonessentialLines(rawText);
			}

			Console.WriteLine("Total question count: " + questionAnswers.Count);
		}

		private void RemoveNonessentialLines(StreamReader stream)
		{
			while(stream.Peek() != '?')
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
					Console.WriteLine("currentQA == output");
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