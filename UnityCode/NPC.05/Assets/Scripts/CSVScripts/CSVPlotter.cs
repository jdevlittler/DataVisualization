using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CSVPlotter : MonoBehaviour     //Class to get data from CSVFileReader, and eventually turn it into gameobjects that are plotted as a graph
{

    public string file;                                    //Variable to hold name of input file from CSVFileReader
    public List<Dictionary<string, object>> dataList;      //List to hold data from CSV reader

                                                
    public int colX = 0;                                   //Variables to assign column names to coordinates 
    public int colY = 1; 
    public int colZ = 2;

    public int colXX = 0;                                  //Variables to assign column names to coordinates for graph 2
    public int colYY = 3; 
    public int colZZ = 4;

    
    public string xName;                              //Actual column names
    public string yName;
    public string zName;
    public string xxName;                             //Actual column names for second graph
    public string yyName;
    public string zzName;

    public float xMin;                                //Variables to old min/mid/max values used to plot along the axis
    public float xMax;
    public float yMin;
    public float yMax;
    public float zMin;
    public float zMax;

    public float xxMin;                               //Variables to old min/mid/max values used to plot along the axis for second graph
    public float xxMax;
    public float yyMin;
    public float yyMax;
    public float zzMin;
    public float zzMax;

    
    public GameObject dataPrefab;                   //Prefab for the points of data to be instantiated
    public GameObject pointHolder;                  //Prefab for the points of data to be held in heirarchy, looks cleaner

    public float scale = 10;                        //Variable to set the scale of the graph


    void Start()                                    //Called when program is run
    {
        dataList = CSVFileReader.Read(file);       //Sets dataList to output from CSVFileReader
        //Debug.Log(dataList);                       //Test for data loading

        List<string> columns = new List<string>(dataList[4].Keys);          //List of strings filled with column names, used for reference points to get coordinates of data

        Debug.Log("There are " + columns.Count + "columns in CSV file");    //Prints no. keys(columns) with count

        foreach (string key in columns)                                     //Print each column name to console
        {
            Debug.Log("Column name: " + key);          
        }

   
        xName = columns[colX];                          //This takes the string within columns at the index specified by column variables and assigns it to name vars
        yName = columns[colY];
        zName = columns[colZ];      
        xxName = columns[colXX];                        //And the same for the second graph
        yyName = columns[colYY];
        zzName = columns[colZZ];

        xMax = findMaxVal(xName);                       //Passes the colmn name into findMaxVal function
        yMax = findMaxVal(yName);
        zMax = findMaxVal(zName);

        xMin = findMinVal(xName);                       //Passes the colmn name into findMixVal function
        yMin = findMinVal(yName);
        zMin = findMinVal(zName);

        xxMax = findMaxVal(xxName);                     //Passes the colmn name into findMaxVal function second graph
        yyMax = findMaxVal(yyName); 
        zzMax = findMaxVal(zzName);

        xxMin = findMinVal(xxName);                     ////Passes the colmn name into findMinVal function second graph
        yyMin = findMinVal(yyName);
        zzMin = findMinVal(zzName);

        labelText();                                    //Calls labelText function below.
       

        for (var i = 0; i < dataList.Count; i++)                                                            //Loops through the length of dataList with count, converts to float with ToSingle
        {

            float x = (System.Convert.ToSingle(dataList[i][xName]) - xMin) / (xMax - xMin);                 //Gets position using positioning formula and max/min/mid values
            float y = (System.Convert.ToSingle(dataList[i][yName]) - yMin) / (yMax - yMin);
            float z = (System.Convert.ToSingle(dataList[i][zName]) - zMin) / (zMax - zMin);
            
            //New object to scale data values for readability, used below
            Vector3 dataPosition = new Vector3(x, y, z) * scale;                                            //Creates new Vector3 using values multiplied by scale set above
            //Gives meaningful name to child clones
            string dataName = dataList[i][xName] + "   " + dataList[i][yName] + "  " + dataList[i][zName];  //Gives meaningful names to the datapoints (index = X, sepal length, Y, width = Z), later used to print currently selected
  

            //This checks the CSV file for the species, and depending on result, creates
            //a game object with a particular colour, then makes pointHolder the parent and sets the name to dataName above

            if (dataList[i].ContainsValue("setosa"))                                                            //Checks dataList for all rows containing setosa
            {
                GameObject dataSetosa = Instantiate(dataPrefab, dataPosition, Quaternion.identity);            //If true, instantiates game object with object set to the prefab, position created above, and rotation of 0 (Quaternion.identity)
                dataSetosa.GetComponent<Renderer>().material.color = new Color(255, 0, 0);                     //Change this colour to RED
                dataSetosa.transform.parent = pointHolder.transform;                                           //Make the new game object a child of our data holder prefab
                dataSetosa.transform.name = dataName;                                                          //Change the name of the game object to meaningful name above
            }
            else if (dataList[i].ContainsValue("versicolor"))                                                  //Checks dataList for all rows containing versicolor
            {
                GameObject dataVersicolor = Instantiate(dataPrefab, dataPosition, Quaternion.identity);        //If true, instantiates game object with object set to the prefab, position created above, and rotation of 0 (Quaternion.identity)
                dataVersicolor.GetComponent<Renderer>().material.color = new Color(0, 255, 0);                 //Changes colour to GREEN
                dataVersicolor.transform.parent = pointHolder.transform;                                       //Same parent
                dataVersicolor.transform.name = dataName;                                                      //Name
            }
            else if (dataList[i].ContainsValue("virginica"))                                                    //Checks dataList for all rows containing virginica
            {                                                                                                   //If true, instantiates game object with object set to the prefab, position created above, and rotation of 0 (Quaternion.identity)
                GameObject dataVirginica = Instantiate(dataPrefab, dataPosition, Quaternion.identity);          //Checks dataList for all rows containing virginica
                dataVirginica.GetComponent<Renderer>().material.color = new Color(0, 0, 255);                   //Changes colour to BLUE
                dataVirginica.transform.parent = pointHolder.transform;                                         //Same parent
                dataVirginica.transform.name = dataName;                                                        //Change name
            }
        }

        for (var i = 0; i < dataList.Count; i++)                                                               //THIS function is the exact same as above, only using data for the second set of axis
        {

            float x = (System.Convert.ToSingle(dataList[i][xxName]) - xxMin) / (xxMax - xxMin);
            float y = (System.Convert.ToSingle(dataList[i][yyName]) - yyMin) / (yyMax - yyMin);
            float z = (System.Convert.ToSingle(dataList[i][zzName]) - zzMin) / (zzMax - zzMin);

   
            Vector3 dataPosition = new Vector3(x, y, (z+2)) * scale;

            string dataName = dataList[i][xxName] + "   " + dataList[i][yyName] + "  " + dataList[i][zzName];

        
            /*This checks the CSV file for the species, and depending on result, creates
             a game object with a particular colour, then makes pointHolder the parent and gives
            meaningful names to children for cleaner Hierarchy */

            if (dataList[i].ContainsValue("setosa"))
            {
                GameObject dataSetosa = Instantiate(dataPrefab, dataPosition, Quaternion.identity);
                dataSetosa.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
                dataSetosa.transform.parent = pointHolder.transform;
                dataSetosa.transform.name = dataName;
                
            }
            else if (dataList[i].ContainsValue("versicolor"))
            {
                GameObject dataVersicolor = Instantiate(dataPrefab, dataPosition, Quaternion.identity);
                dataVersicolor.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
                dataVersicolor.transform.parent = pointHolder.transform;
                dataVersicolor.transform.name = dataName;
                
            }
            else if (dataList[i].ContainsValue("virginica"))
            {
                GameObject dataVirginica = Instantiate(dataPrefab, dataPosition, Quaternion.identity);
                dataVirginica.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
                dataVirginica.transform.parent = pointHolder.transform;
                dataVirginica.transform.name = dataName;             
            }
        }
    }

                                                                                //These methods are used for scaling the graph, in VR we need to scale down (so we dont walk a mile)
                                                                                //Method to find the maximum value in a column in the data list
    private float findMaxVal(string colName)    
    {
        float maxVal = Convert.ToSingle(dataList[0][colName]);                  //Creates float value to store the first column in the CSV file (index 0)
        for (var i = 0; i < dataList.Count; i++)                                //Loop through every row 
        {
            if (maxVal < Convert.ToSingle(dataList[i][colName]))                //If maxVal is less than the value in the row of CSV file (in colName)
            {
                maxVal = Convert.ToSingle(dataList[i][colName]);                //Set maxVal to this number
            }
        }
        return maxVal;                                                          //Return maxVal at the end of loop
    }
   
    private float findMinVal(string colName)                                    //This function will loop through the dictionary and override min value if a smaller value is found
    {
        float minVal = Convert.ToSingle(dataList[0][colName]);                  //Creates float value to store the first column in the CSV file (index 0)
        for (var i = 0; i < dataList.Count; i++)                                //Loop every row
        {
            if (Convert.ToSingle(dataList[i][colName]) < minVal)                //If mixVal is less than the value in the row of CSV file (in colName)
            {               
                minVal = Convert.ToSingle(dataList[i][colName]);                //Set minVal to colName
            }
        }
        return minVal;                                                          //Return minVal at the end of loop
    }

    private void labelText()                                                                                  //This function is used to set the label text.
    {   
        GameObject.Find("xLabel").GetComponent<TextMeshPro>().text = xName;                                            //Finds the text game objects using their names, and sets to the column names
        GameObject.Find("yLabel").GetComponent<TextMeshPro>().text = yName;
        GameObject.Find("zLabel").GetComponent<TextMeshPro>().text = zName;

        GameObject.Find("xMin").GetComponent<TextMeshPro>().text = xMin.ToString("0.0");                                //Finds text game objects, and sets min values toString
        GameObject.Find("yMin").GetComponent<TextMeshPro>().text = yMin.ToString("0.0");
        GameObject.Find("zMin").GetComponent<TextMeshPro>().text = zMin.ToString("0.0");

        GameObject.Find("xMax").GetComponent<TextMeshPro>().text = xMax.ToString("0.0");                                ////Finds text game objects, and sets max values toString
        GameObject.Find("yMax").GetComponent<TextMeshPro>().text = yMax.ToString("0.0");
        GameObject.Find("zMax").GetComponent<TextMeshPro>().text = zMax.ToString("0.0");

        GameObject.Find("xMid").GetComponent<TextMeshPro>().text = (xMin + (xMax - xMin) / 2f).ToString("0.0");         //Finds text game objects, and sets mid values toString
        GameObject.Find("yMid").GetComponent<TextMeshPro>().text = (yMin + (yMax - yMin) / 2f).ToString("0.0");
        GameObject.Find("zMid").GetComponent<TextMeshPro>().text = (zMin + (zMax - zMin) / 2f).ToString("0.0");

        GameObject.Find("xxLabel").GetComponent<TextMeshPro>().text = xxName;                                           //Exactly the same here but for the second pair of axis
        GameObject.Find("yyLabel").GetComponent<TextMeshPro>().text = yyName;   
        GameObject.Find("zzLabel").GetComponent<TextMeshPro>().text = zzName;

        GameObject.Find("xxMin").GetComponent<TextMeshPro>().text = xxMin.ToString("0.0");
        GameObject.Find("yyMin").GetComponent<TextMeshPro>().text = yyMin.ToString("0.0");
        GameObject.Find("zzMin").GetComponent<TextMeshPro>().text = zzMin.ToString("0.0");

        GameObject.Find("xxMax").GetComponent<TextMeshPro>().text = xxMax.ToString("0.0");
        GameObject.Find("yyMax").GetComponent<TextMeshPro>().text = yyMax.ToString("0.0");
        GameObject.Find("zzMax").GetComponent<TextMeshPro>().text = zzMax.ToString("0.0");

        GameObject.Find("xxMid").GetComponent<TextMeshPro>().text = (xxMin + (xxMax - xxMin) / 2f).ToString("0.0");
        GameObject.Find("yyMid").GetComponent<TextMeshPro>().text = (yyMin + (yyMax - yyMin) / 2f).ToString("0.0");
        GameObject.Find("zzMid").GetComponent<TextMeshPro>().text = (zzMin + (zzMax - zzMin) / 2f).ToString("0.0");
    }  
}
