using System;
using System.IO;

namespace ExamPrepper
{
	public class QuestionAnswer
	{
		public string Question
		{
			get;
			private set;
		}

		public string Answer
		{
			get;
			private set;
		}

		public string[] Tags
		{
			get;
			private set;
		}

		public string Source
		{
			get;
			private set;
		}

		public QuestionAnswer(StreamReader rawText, string[] tags, string source)
		{
			Tags = tags;
			Source = source;

			// Consume '?'
			if(rawText.Peek() == '?')
			{
				rawText.Read();
			}

			// Get question
			Question = rawText.ReadLine();
			while(rawText.Peek() != '#')
			{
				Question += System.Environment.NewLine + rawText.ReadLine();
			}

			// Consume '#'		
			rawText.Read();

			// Determine if multiline
			if(rawText.Peek() == '{')
			{
				// Multiline
				// Consume {
				rawText.Read();

				// Read multiline content
				Answer = rawText.ReadLine();
				while(rawText.Peek() != '}')
				{
					Answer += System.Environment.NewLine + rawText.ReadLine();
				}
				// Consume }
				rawText.Read();
			}
			else
			{
				// Single line
				Answer = rawText.ReadLine();
			}
		}

		public override string ToString()
		{
			return string.Format("?{0}" + System.Environment.NewLine +
				"#{1}" + System.Environment.NewLine + 
				"Tags: {2}", Question, Answer, Tags);
		}
	}
}