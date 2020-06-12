using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class CSVFileReader                           //This class parses a comma seperated value (CSV) file, converting the values into integer or floats if applicable, and returns in a dictionary list
{
    static char[] trimChar = { '\"' };                                          //Sets delimiters to trim 
    static string RegExSplit = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";       //Regular expressions to trim
    static string RegExLineSplit = @"\r\n|\n\r|\n|\r";                          //Regex to split on line

    public static List<Dictionary<string, object>> Read(string file)            //Method to read the CSV file, passing through string key and object as value for collection, returns file as a string list
    {
        //Debug.Log("CSVReader is reading " + file);                            // Print error check to ensure parsed correctly
        var list = new List<Dictionary<string, object>>();                      //Declare dictionary list, string object as key value

        TextAsset data = Resources.Load(file) as TextAsset;                     //Constructor to create a TextAsset, loading file parameter of function stored in "data"
        // Debug.Log("Data loaded:" + data);                                    // Print raw data, make sure parsed correctly

        var lines = Regex.Split(data.text, RegExLineSplit);                  // Split text stored in data into lines using RegExLineSplit
        if (lines.Length <= 1) return list;                                  //Check there's more than one line, return list if not
        var header = Regex.Split(lines[0], RegExSplit);                      //Split headers
                                                                             // Loop through length of lines
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], RegExSplit);                  //Split lines to store in string array (var)
            if (values.Length == 0 || values[0] == "") continue;             //Continue past loop if values array is empty

            var entry = new Dictionary<string, object>();                    //Declares dictionary object stored in "entry"

                                                                             
            for (var j = 0; j < header.Length && j < values.Length; j++)     // Loops through string array of headers
            {
                string value = values[j];                                                   // Set local variable to loop over x amount of headers
                value = value.TrimStart(trimChar).TrimEnd(trimChar).Replace("\\", "");      //Trim characters specified in trimChar variable
                object finalvalue = value;                                                  //Set to final value as an object

                int n;                                                                      // Create a int variable, to hold value if int
                float f;                                                                    // Create a float variable, to hold value if float

                                                                                            // If-else statement to attempt to error check parse value into int or float
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);                                         //Add Dictionary header as string + entry object to collection
        }
        return list;                                                 //Return list of entries added above
    }
}
       

        



