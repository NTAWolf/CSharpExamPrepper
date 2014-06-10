ExamPrepper
===========

A small program made specifically for exam preparations - when you need to learn many things by heart, fast!

===========

# Hotkeys
At the moment, hotkeys are hardcoded. They are as follows:

- CTRL + SPACE: Show true answer
- CTRL + a: Accept your answer
- CTRL + r: Reject your answer

# Implemented quiz syntax
When you create your own quiz, some syntax rules have to be obeyed.

- *?* at the beginning of a line indicates that the following is a question. The question can use any number of lines, as long as none of its lines begin with *#*.
- *#* at the beginning of a line indicates that the followin is an answer. The answer can span multiple lines, as seen next.
- *{* after *#* shows that the answer spans until the first following line that starts with a *}*.
- *[* after *#* shows that the answer is an image. The image name is enclosed in the brackets, i.e. the line must end with *]*. The images are assumed to be in the same folder as the quiz .txt file, in a subfolder called "images".
- Text not interpreted as a question, or an answer, or (in future implementations) the other specified elements, is ignored.

# Yet-to-be-implemented quiz syntax
The following keywords and symbols at the beginning of a line are reserved:

- *Source:*
- *====*
- *-*, to be used for tags
- *Tags:*