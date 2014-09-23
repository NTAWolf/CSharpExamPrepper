using System;
using System.Collections.Generic;
using System.IO;

namespace ExamPrepper
{
	public class QACategory
	{
		public string Category
		{
			get;
			private set;
		}

		public List<QuestionAnswer> QuestionAnswers;

		public int Count
		{
			get {
				return QuestionAnswers.Count;
			}
		}

		public QACategory(StreamReader stream)
		{
			QuestionAnswers = new List<QuestionAnswer>(20);
			ExtractCategory(stream);
			ExtractQAs(stream);

		}

		void ExtractCategory(StreamReader stream)
		{
			Category = stream.ReadLine();
			Console.WriteLine("Category noted: " + Category);
		}

		void ExtractQAs(StreamReader stream)
		{
			ConsumeEmptyLines(stream);

			while(
				false == stream.EndOfStream &&
				(stream.Peek() == '?' ||
				 stream.Peek() == '#'))
			{
				// Create & store questions	
				QuestionAnswer temp = new QuestionAnswer(stream);
				QuestionAnswers.Add(temp);

				ConsumeEmptyLines(stream);
			}

		}

		void ConsumeEmptyLines(StreamReader stream)
		{
			while( 
			   (stream.Peek() == '\n' ||
				stream.Peek() == '\r' ||
				stream.Peek() == '\f') &&
				stream.Peek() > -1)
			{
				if(stream.EndOfStream)
					break;

				Console.WriteLine("QACategory removing nonessential line: " + stream.ReadLine());
			}
		}
	}
}

