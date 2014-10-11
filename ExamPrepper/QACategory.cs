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
		public List<QACategory> Subcategories;

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


			Console.WriteLine("Category built: " + Category);
			Console.WriteLine("QAs in category:");
			foreach(var v in QuestionAnswers) 
			{
				Console.WriteLine ("? " + v.Question);
				Console.WriteLine ("# " + v.Answer);
			}
			Console.WriteLine ("end of category");
			Console.WriteLine ();
		}

		void ExtractCategory(StreamReader stream)
		{
			Category = stream.ReadLine();
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
				stream.ReadLine();

				// Console.WriteLine("QACategory removing nonessential line: " + stream.ReadLine());
			}
		}
	}
}

