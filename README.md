ExamPrepper
===========

A small program made specifically for exam preparations - when you need to learn many things by heart, fast!

===========

# How to use
You must get or write a quiz that follows the quiz syntax specified below. Use a plain text format, i.e. .txt. Then run this program, use it to open the relevant file, (de)select the categories as to your liking, and start quizzing.

When quizzing, you are presented with a question in the top field. The middle text area allows you to write down an answer, and the bottom field shows the answer that you provided in your quiz text file.

When you have thought about an answer, and maybe written it down, click `Show Answer` to show the answer from the text file. If you think your answer is good enough, simply accept it by clicking the appropriate button. This will mark the answer as ... answered, and you will not encounter it again for the duration of the quiz. If you reject your current answer, e.g. because it turned out to be wrong when compared to the full answer from the text file, then that question will be put back in the question pool, and you will encounter it again later.

This way, the application helps you focus on the questions that you cannot readily answer, and beat you with them until you can.

# Hotkeys
At the moment, hotkeys are hardcoded. They are as follows:

- CTRL + SPACE: Show true answer
- CTRL + a: Accept your answer
- CTRL + r: Reject your answer

# Quiz syntax
When you create your own quiz, some syntax rules have to be obeyed.

- **?** at the beginning of a line indicates that the following is a question. The question can use any number of lines, as long as none of its lines begin with **#**.
- **#** at the beginning of a line indicates that the followin is an answer. The answer can span multiple lines, as seen next.
- **[** after **#** shows that the answer is an image. The image name is enclosed in the brackets, i.e. the line must end with **]**. The images are assumed to be in the same folder as the quiz .txt file, in a subfolder called "images".
- A line that isn't part of a question or an answer is treated as a Category, or Subject (same thing, different name) that the user can select or deselect when starting the quiz.
