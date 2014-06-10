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
			
		public bool HasImage
		{
			get;
			private set;
		}

		public string ImageFile
		{
			get;
			private set;
		}

		public QuestionAnswer(StreamReader rawText, string[] tags, string source)
		{
			HasImage = false;
			Tags = tags;
			Source = source;

			// Consume '?'
			if(rawText.Peek() == '?')
			{
				rawText.Read();
			}

			// Consume whitespace
			ConsumeWhitespace(rawText);

			// Get question
			Question = rawText.ReadLine();
			while(rawText.Peek() != '#')
			{
				Question += System.Environment.NewLine + rawText.ReadLine();
			}

			// Consume '#'		
			rawText.Read();

			// Consume whitespace
			ConsumeWhitespace(rawText);

			// Determine if multiline, or image
			if(rawText.Peek() == '{')
			{
				Console.WriteLine("Is multiline!");
				// Multiline
				// Consume {
				rawText.Read();
				ConsumeWhitespace(rawText);

				// Read multiline content
				Answer = rawText.ReadLine();
				while(rawText.Peek() != '}')
				{
					Answer += System.Environment.NewLine + rawText.ReadLine();
				}
				// Consume }
				rawText.Read();
			}
			else if(rawText.Peek() == '[')
			{
				Console.WriteLine("Has image!");
				string fullLine = rawText.ReadLine();
				int startBracket = fullLine.IndexOf('['); // Yes, I know this can only be index 0, but it might be made more flexible in the future.
				int endBracket = fullLine.IndexOf(']');

				HasImage = true;
				ImageFile  = fullLine.Substring(startBracket + 1,  endBracket - 1);
				Answer = fullLine.Substring(endBracket + 1).Trim();
			}
			else
			{
				// Single line
				Answer = rawText.ReadLine();
				Console.WriteLine("is singleline!");
			}
		}

		private void ConsumeWhitespace(StreamReader stream)
		{
			while(
				(
				   stream.Peek() == ' ' 
				|| stream.Peek() == '\t' 
				|| stream.Peek() == '\r' 
				|| stream.Peek() == '\n'
				)
				&& stream.EndOfStream == false)
			{
				stream.Read();
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