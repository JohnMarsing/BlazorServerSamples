# Markdown Notes


### [MarkdownEditor <sup> by Mads Kristensen</sup>](https://github.com/madskristensen/MarkdownEditor)
- Features
- Powered by Markdig - the best markdown parser
- Syntax highlighting
- Live preview window with scroll sync
- Mermaid chart support
- CommonMark and GitHub flavored Markdown
- High-DPI support
- Drag 'n drop of images supported
- Paste image from clipboard directly onto document
- Outlining/folding of code blocks
- Keyboard shortcuts
- Brace completion with type-through
- Lightning fast
- Auto-generate HTML files

**Auto-generate HTML files**
- See https://github.com/madskristensen/MarkdownEditor#auto-generate-html-files
- By right-clicking any Markdown file in Solution Explorer, you can turn on automatic generation of a HTML file.

It will wrap the output rendered markdown in a HTML template that looks like this:

 ```html
<!DOCTYPE html>
<html>
<head>
    <title>[title]</title>
</head>
<body>

    [content]

</body>
</html>
```

You can provide your own HTML template by dropping a file with the name **md-template.html** in the same or parent folder to the markdown file. If not found, it will search %userprofile%.

Just make sure to include the [content] token in the template. And the [title] token is optional if you need a title.




**Convert to Light Bulbs**
- Link
- Image
- Blockquote
- Code Block
- Bullet/Numbered List
- Task List

GitHub and other flavors
Advanced markdown extensions are supported to give more features to the syntax. 
This includes pipe tables, emoji, mathematics and a lot more.

Live preview can be disabled in the settings.

The syntax highlighter is powered by Prism

Custom stylesheets


**Short Cut Keys**

 Key(s)      | Description 
 ----------- | ----------- 
 <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>C</kbd> | wraps the selected text in a code block.       
 <kbd>Ctrl</kbd> + <kbd>Space</kbd> | checks and unchecks task list items.
 <kbd>Ctrl</kbd> + <kbd>B</kbd> | makes the selected text **bold** by wrapping it with **.
 <kbd>Ctrl</kbd> + <kbd>I</kbd> | makes the selected text *italic* by wrapping it with _.
 <kbd>Ctrl</kbd> + <kbd>K,C</kbd> | wraps the selection with HTML comments. NO WORKY
 <kbd>Ctrl</kbd> + <kbd>K,U</kbd> | removes HTML comments surrounding the selection/caret. NO WORKY


- [x] task list item

---

## Tables

**Verbose**

| Syntax      | Description |
| ----------- | ----------- |
| Header      | Title       |
| Paragraph   | Text        |

**Shorthand**

 Syntax      | Description 
 ----------- | ----------- 
 Header      | Title       
 Paragraph   | Text        

 ```
 :-: Center
 --: Right align
 ```

 ## Fonts
- <font color="#FF0010">red</font>


# GitHub Style Checklists

-␣[␣]␣(A)␣Item
-␣[␣]␣(B)␣Item


The key combination (Ctrl+K, Ctrl+Q is bound to command (Comment Selection) which is not currently available.

---
# Mermaid

https://mermaid-js.github.io/mermaid/#/README

### Graph
```mermaid
  graph TD;
      A-->B;
      A-->C;
      B-->D;
      C-->D
```

# Epsilon Notes
- [Search.brave.com epsilon notes](http://example.com)
- [EpsilonExpert.com](http://epsilonexpert.com/e/news/version_2_27.php)