using System;
using System.IO;
using System.Collections.Generic;
using Gtk;

namespace ExamPrepper
{
	public class QuizConductor
	{
		//public enum Mode {Random, Forward, Backward}

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

		public QuizConductor(StreamReader rawText)
		{
			ReadQAs(rawText);
			InitiateUnusedQAs();
		}

		public void ReadQAs(StreamReader rawText)
		{
			questionAnswers = new List<QuestionAnswer>(50);

			while(false == rawText.EndOfStream)
			{
				// Determine source, tags

				// For now, disregard source, tags

				// Disregard all lines not starting with ?

				// Create & store questions
				while(rawText.Peek() != '?')
				{
					rawText.ReadLine();
				}

				QuestionAnswer temp = new QuestionAnswer(rawText, null, null);

				Console.WriteLine("Adding question:" 
					+ System.Environment.NewLine 
					+ temp.ToString()
					+ System.Environment.NewLine
					+ "---------"
					+ System.Environment.NewLine);

				questionAnswers.Add(temp);
			}

			Console.WriteLine("Total question count: " + questionAnswers.Count);
		}

		private QuestionAnswer GetNextQuestionAnswer()//Mode mode)
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
			/*switch(mode)
			{
			case Mode.Random:
				output = unusedQAs[]
				break;
			case Mode.Forward:
				break;
			case Mode.Backward:
				break;
			}*/
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