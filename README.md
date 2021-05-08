# Introduction
.Net core 3.1 Command line tool

Processes all files in its current work directory and replaces build pipe variables in these files with the actual content.
It replaces $(variable) with the intended content, by leveraging the environment variables as specified in https://docs.microsoft.com/en-us/azure/devops/pipelines/process/variablesUsed .

Good luck, have fun!

# Build
Build the VarReplacerCmd project to get the command line executable.

# Usage
Type the command and add the help argument to get some more info.

`VarReplacerCmd -h`

Complete syntax

`varreplacercmd -f filepattern [+ filepattern [+ ...]] [-s]`

Option | Usage
------ | -----
-f | The file(s) to be manipulated. May use wildcards, but doesn't support regex.
-s | Also look into the sub directories.
-h | Help

# Remarks
* Needs .Net Core 3.1 to run.
* Intended for use on the Azure DevOps pipe to replace build variables in any document type, as long as it is a text file.
* Can conflict with file content if the characters $() already have a special meaning in the document. However it uses a very narrow regex \$\(([\w.]*)\) that only searches per line so you should be save if you are only using the characters seperately.
