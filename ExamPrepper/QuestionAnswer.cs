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

		public QuestionAnswer(StreamReader rawText)
		{
			HasImage = false;

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

			//Console.WriteLine("Question noted: " + Question);
		}

		void ExtractAnswer(StreamReader rawText)
		{
			// Consume '#'
			rawText.Read();
			Answer = "";

			// Determine if answer is in the form of an image
			if(rawText.Peek() == '[') // Has image
			{
				string fullLine = rawText.ReadLine();
				int startBracket = fullLine.IndexOf('[');
				// Yes, I know this can only be index 0, but it might be made more flexible in the future.
				int endBracket = fullLine.IndexOf(']');
				HasImage = true;
				ImageFile = fullLine.Substring(startBracket + 1, endBracket - 1);
				Answer = fullLine.Substring(endBracket + 1);
			}

			// Consume lines until we reach an empty line, or a new question
			while(
				rawText.Peek() != '\n' &&
				rawText.Peek() != '\r' &&
				rawText.Peek() != '\f' &&
				rawText.Peek() != '?' &&
				rawText.Peek() > -1
				)
			{
				//Console.WriteLine("Peek giving " + rawText.Peek() + " not an empty line or a new question, nor end of file");
				Answer += System.Environment.NewLine + rawText.ReadLine();
			}
				
			Answer = Answer.Trim();
			//Console.WriteLine("Answer noted: " + Answer);
		}

		public override string ToString()
		{
			return string.Format("?{0}" + System.Environment.NewLine +
				"#{1}" + System.Environment.NewLine, 
				Question, Answer);
		}
	}
}