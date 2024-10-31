# iText-test
Experimenting with c# iTEXT SDK
Latest - 2024-10-31
![image](https://github.com/user-attachments/assets/3b1070b6-fec9-4b2a-a3e5-5468096a3210)


The project can be opened with Visual Studio and uses .NET v 7.0
The result is a Windows Forms application with some basic functionality:

1. You can write some text in the input field and click the Generate button located bottom left; this will create a PDF within the project directory having the text you submitted.
2. Something I plan on expanding upon since it's kind of exciting; you can click the "GET mtg collection API" button and this will do the following:
   * call an open scryfall endpoint with a basic card search payload
   * return a JSON response that will be looped through for each card object returned
   * for each card object returned, write specific fields into a PDF as rendered text
   * for each card objects image url key, download the corresponding image file to the project directory and, add it to the PDF

3. NEW! Combo Box added:
   * when loading the application a web request will be made against ScryFall web API to return all sets
   * All set codes and names are pushed to the combobox
   * After selecting a set and clicking the button the program will make another API call, fetching card objects for the selected set
   * Each card image is then pushed to a table, 3 columns wide and, given the card sizes, each page renders 3 cards per row, 3 rows long, ex:
    ![image](https://github.com/user-attachments/assets/f1d54a32-59c6-4c8f-93e0-4c0e3db6da32)

4. Progress bar has been added!
   * for each loop writing information, the progress bar increments
   * Success message when progress bar has completed
     
![image](https://github.com/user-attachments/assets/f6f75331-5246-4c55-8e5d-8b5ac6755eb3)



  
     
