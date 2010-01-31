Specifications
==============

This document contains the functionality specification of the
md2html product project, specifying details.

In this file, all lines beginning with a letter and a 3-digit number
specifies a feature or similar that will be directly implemented in
the program as a separate module or function.

Source reader
=============

The source reader is the module of the program that reads the
source Markdown file(s) from disk, and presents them to the
rest of the program in a manner fitting easy handling, by
providing them as a sequence of lines, where each line carries
extra meta-data specifying which file on disk, and while line
number in the file the line is from.

Preprocessor
============

The preprocessor reads data from the source reader and processes
directives, removing them from the data before passing it on to
the MarkdownSharp transformer code.

Some of the directives will be replaced with a different piece
of code that can be carried transparently through the MarkdownSharp
transformer, usually by way of a normal HTML comment, to be
picked up by the postprocessor.

Postprocessor
=============

The postprocessor reads the html produced by MarkdownSharp and
postprocesses it, most notably by fixing code blocks by
formatting them according to the specified options.

Directives
==========

D000: Directive format

The format of directives in the Markdown files is as follows:

    @@ directive [parameter1] [parameter2] ... [parameterN]

Directives may or may not need a parameter to function, depending
on the meaning of the directive.

The double-at characters must be separated from the directive name
by exactly one space.

The directive name must be specified using lower-case characters only.

The parameters, if any, must be separated by one or more spaces. If
a directive parameter should contain a space, you need to enclose
the parameter in double quotes, the "-character.

The directive parameter may also contain the quote-character, in which
case it must be escaped with a backslash. To let the parameter contain
a backslash, it too must be escaped by another backlash.

Examples:

    @@ title "This is the title of this document"
    @@ title "This is the title of C:\\md2html\\readme.md"
    @@ title "This is the title of \"C:\\md2html\\readme.md\""

Note the use of quotes and backslashes in the examples above.

D001: title directive
---------------------

This directive can be used from within the Markdown file itself
to specify which title the final HTML output should carry.

The directive name is "title", all lowercase. The directive takes
one argument, the title to use for the final output.

Examples:

    @@ title "This is the title of this document"

The title directive is removed from the Markdown code before passing
it on to the transformer module, and is then used in the postprocessor
to add a `<title>This is the title of this document</title>` HTML
tag to the `<head>...</head>` part of the document.

If more than one title directive is present in the file, the last
one wins, but each directive, beyond the first, will produce a
warning to this effect.

W001: Warning about multiple title directives
---------------------------------------------

This warning will be output when there are multiple `@@ title ...`
directives present in the Markdown code.