using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)                                  //Passes through event data when the raycast enters a game object 
    {
        FindPartnerEnter();                                                                 //Calls function to find corresponding data point
        transform.localScale = transform.localScale + new Vector3(0.3f, 0.3f, 0.3f);        //Increases the size of current game object
    }

    public void OnPointerExit(PointerEventData eventData)                                   //Passes through event data on raycast exit a game object
    {
        FindPartnerExit();                                                                  //Function to execuse on corresponsing data point   
        transform.localScale = transform.localScale - new Vector3(0.3f, 0.3f, 0.3f);        //Decreases the size back to previous
    }
    public void FindPartnerEnter()                                                                              //Function to find the corresponding data point 
    {         
        var nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex());            //Stores the current index of the game object in a Transform variable called nextObj
      
        if(nextObj.GetSiblingIndex() <= 150) {                                                                 //Checks if the index is less than 150 to decide which graph to operate on
            nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() + 150);      //If current index is less than 150, add 150 on to select the opposite graph 
        }
        else if(nextObj.GetSiblingIndex() >= 150)                                                               //Checks if the index is greater than 150 to decide which graph to operate on
        {
            nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 150);       //If current index is less than 150, add 150 on to select the opposite graph 
        }
        nextObj.transform.localScale = nextObj.transform.localScale + new Vector3(0.3f, 0.3f, 0.3f);            //Depending on which partner object was selected, increase the size
    }

    public void FindPartnerExit()
    {       
        var nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex());           //Stores the current index of the game object in a Transform variable called nextObj
       
        if (nextObj.GetSiblingIndex() <= 150)                                                                 //Checks if the index is less than 150 to decide which graph to operate on
        {
            nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() + 150);     //If current index is less than 150, add 150 on to select the opposite graph 
        }
        else if (nextObj.GetSiblingIndex() >= 150)                                                            //Checks if the index is greater than 150 to decide which graph to operate on
        {
            nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 150);     //If current index is less than 150, add 150 on to select the opposite graph 
        }
        nextObj.transform.localScale = nextObj.transform.localScale - new Vector3(0.3f, 0.3f, 0.3f);          //Depending on which partner object was selected, decrease the size
    }

    public void OnPointerDown(PointerEventData eventData)                                                     //Passes through event data when a button is pressed
    {
        if (OVRInput.Get(OVRInput.RawButton.A))                                                               //Selection of button on Oculus controller- Raw button A
        {        
            SelectCorresponding();                                                                            //If A is pressed call SelectCorresponding();
        }
        else if (OVRInput.Get(OVRInput.RawButton.B))                                                          //If B is pressed call DeleteData();
        {
            DeleteData();
        }      
    }
    public void SelectCorresponding()                                                                            //Function to select game object and corresponding object
    {
        var selectObj = gameObject.name;                                                                         //Creates variable to store current objects name
        
        GameObject.Find("Data").GetComponent<TextMeshPro>().text = selectObj;                                    //Finds Text game object named "Data" and updates the current objects to current name, set values in plotter
        
        var twinObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex());              //Creates variable to store the index of corresponding data point
       
        if (twinObj.GetSiblingIndex() <= 150)                                                                    //Again checks if index <= 150
        {
            twinObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() + 150);       //If so add 150 for corresponsing graph
            GameObject.Find("TopLength").GetComponent<TextMeshPro>().text = "S.Length";                         //Change the names of Text Game objects to fit current loaded data point values
            GameObject.Find("TopWidth").GetComponent<TextMeshPro>().text = "S.Width";
            GameObject.Find("BotLength").GetComponent<TextMeshPro>().text = "P.Length";
            GameObject.Find("BotWidth").GetComponent<TextMeshPro>().text = "P.Width";

        }
        else if (twinObj.GetSiblingIndex() >= 150)                                                              //Again checks if index >= 150
        {
            twinObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 150);       //If so subtract 150 for corresponsing graph
            GameObject.Find("BotLength").GetComponent<TextMeshPro>().text = "S.Length";                         //Change the names of Text Game objects to fit current loaded data point values
            GameObject.Find("BotWidth").GetComponent<TextMeshPro>().text = "S.Width";
            GameObject.Find("TopLength").GetComponent<TextMeshPro>().text = "P.Length";
            GameObject.Find("TopWidth").GetComponent<TextMeshPro>().text = "P.Width";
        }
        
        GameObject.Find("OtherData").GetComponent<TextMeshPro>().text = twinObj.ToString();                     //Change the Text object named "OtherData" to the data values set to name in CSVPlotter
    }

    public void DeleteData()                                                                                    //Function to remove data points
    {                  
        var nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex());             //Stores current object index in nextObj
        
        if (nextObj.GetSiblingIndex() <= 150)                                                                   //Check again which graph this belongs to
        {
            nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() + 150);       //If left graph, add 150 for right
        }
        else if (nextObj.GetSiblingIndex() >= 150)                                                              
        {
            nextObj = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 150);      //If right graph, subtract 150 for left
        }

        Destroy(GetComponent<MeshRenderer>());                                                                 //Delete from scene including collisions
        Destroy(GetComponent<SphereCollider>());
        Destroy(nextObj.GetComponent<MeshRenderer>());
        Destroy(nextObj.GetComponent<SphereCollider>());
        GameObject.Find("Data").GetComponent<TextMeshPro>().text = "Deleted.";                                  //Update currently selected string values set to empty.
        GameObject.Find("OtherData").GetComponent<TextMeshPro>().text = "";
        GameObject.Find("TopLength").GetComponent<TextMeshPro>().text = "";
        GameObject.Find("TopWidth").GetComponent<TextMeshPro>().text = "";
        GameObject.Find("BotLength").GetComponent<TextMeshPro>().text = "";
        GameObject.Find("BotWidth").GetComponent<TextMeshPro>().text = "";
    }
 
}
