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

			ExtractQuestion(rawText);
			ExtractAnswer(rawText);
		}

		void ExtractQuestion(StreamReader rawText)
		{
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
			Question = Question.Trim();
		}

		void ExtractAnswer(StreamReader rawText)
		{
			// Consume '#'		
			rawText.Read();

			// Consume whitespace before a possible keyword
			ConsumeWhitespace(rawText);

			// Determine if multiline, or image
			if(rawText.Peek() == '{') // Multiline
			{
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
			else if(rawText.Peek() == '[') // Has image
			{
				string fullLine = rawText.ReadLine();
				int startBracket = fullLine.IndexOf('[');
				// Yes, I know this can only be index 0, but it might be made more flexible in the future.
				int endBracket = fullLine.IndexOf(']');
				HasImage = true;
				ImageFile = fullLine.Substring(startBracket + 1, endBracket - 1);
				Answer = fullLine.Substring(endBracket + 1);
			}
			else // Single line
			{
				Answer = rawText.ReadLine();
			}
			Answer = Answer.Trim();
		}

		private void ConsumeWhitespace(StreamReader stream)
		{
			while(Char.IsWhiteSpace((char)stream.Peek())
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