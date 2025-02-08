# WikipediaReferenceCleaner
Project for automatically cleaning and prettifying the reference section of Wikipedia articles.

Wikipedia articles often have a lot of references. This in itself is good, but they tend to clutter the wikitext, making it difficult to read and edit the article source code.
This project contains a Windows client program for cleaning up the article references by putting them in a reflist template at the bottom of the article, and formatting the references for easier read and edit.

## Points of interest
* The order of the reference data (URL, Title, Date, e.t.c) can be changed by editing the *ReferenceDataSorter* class.
* The test of the main use case (reading and writing references from/to wikitext) can be found in the *ReferencesReadAndWriteTests* class.
* The parsing from wikitext to in-memory Reference-objects are done in the *RefListReader* class.
* The code setting the desired format of the references can be found in the *RefListWriter* class.

## v0.1
The first edition and a MVP (Minimal Viable Product). To use this version:
1. Put the reflist template in a textfile named *wp-ref-cleaner-input.txt* and put it on the desktop on your computer.
2. Run the client.
3. The cleaned wikitext can be found in the file *wp-ref-cleaner-output.txt* on your desktop.

## v0.2
1. Updated license file with name and year.
2. Added few console messages to make the process more descriptive.

## v0.3
Made improvements on the message handling.
1. Moved the messages to an event-based Messenger class that can be subscribed.
2. Added colour to error- and result-messages.
3. Changed the code to handle errors by sending a message and returning false, instead of relying on exceptions.
4. Moved over-arcing processing code to separate class (*ReferenceProcessor*) so only GUI-code remains in the *Program* class.
