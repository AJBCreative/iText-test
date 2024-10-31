# iText-test
Experimenting with c# iTEXT SDK

The project can be opened with Visual Studio and uses .NET v 7.0
The result is a Windows Forms application with some basic functionality:

1. You can write some text in the input field and click the Generate button located bottom left; this will create a PDF within the project directory having the text you submitted.
2. Something I plan on expanding upon since it's kind of exciting; you can click the "GET mtg collection API" button and this will do the following:
   * call an open scryfall endpoint with a basic card search payload
   * return a JSON response that will be looped through for each card object returned
   * for each card object returned, write specific fields into a PDF as rendered text
   * for each card objects image url key, download the corresponding image file to the project directory and, add it to the PDF
  
     
