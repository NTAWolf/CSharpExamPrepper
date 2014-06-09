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
			
		public bool IsImage
		{
			get;
			private set;
		}

		public QuestionAnswer(StreamReader rawText, string[] tags, string source)
		{
			IsImage = false;
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
			else if(rawText.Peek() == '[')
			{
				// Is image
				IsImage = true;
				string imageName = rawText.ReadLine();
				imageName = imageName.Substring(1, imageName.LastIndexOf(']') - 1);
				Answer = imageName;
				/*string imagePath = Path.Combine(basePath, "images", imageName);

				Console.WriteLine("Reading image path: " + imagePath);

				Image = new Gdk.Pixbuf(imagePath);

				Console.WriteLine("Finsihed reading image");
*/
				//Image = Gtk.Image.LoadFromResource(imagePath);
			}
			else
			{
				// Single line
				Answer = rawText.ReadLine();
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