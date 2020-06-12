using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointer : MonoBehaviour
{
    public float defaultLength = 30.0f;                 //Default length for pointer, handling visual aspects

    private LineRenderer lineRenderer = null;           //Reference for a line renderer

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();    //Sets lineRenderer var to a line renderer component
    }

    private void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);        //Access to line renderer: need to set 2 positions on it. Where its coming from and where its going
        lineRenderer.SetPosition(1, CalculateEnd());            //Set position, need to give an index. start from hand, set the positon to the end: Call calculate end                                                                                                                                                                       
    }                                                           //Physics pointer is going to be childed to the hand- so can transform.position of itsself 
   
    private Vector3 CalculateEnd()                             
    {
        //store a raycast hit, starter from function v
        RaycastHit hit = CreateForwardRayCast();               //Create forward raycast from controller. if we hit something, store in raycast hit
        //store end postion
        Vector3 endPosition = DefaultEnd(defaultLength);       //Get end positon, start with default end value using default length,

        if (hit.collider)                                      //If we hit something, update end position at point of the raycast hit
        {
            endPosition = hit.point;
        }

        return endPosition;                                    //Return final value
    }

    
    private RaycastHit CreateForwardRayCast()                         //Creates a raycast going forward from hand
    {
        RaycastHit hit;
    
        Ray ray = new Ray(transform.position, transform.forward);     //Creates ray from positon of hand going forward
        
        Physics.Raycast(ray, out hit, defaultLength);                 //Uses ray, getting data from raycast and storing in hit, and default length

        return hit;                                                   //Returns hit value
    }

                                                                         
    private Vector3 DefaultEnd(float length)                          //Calculates default end, if not hitting anything tells how far it should go.
    {
        return transform.position + (transform.forward * length);     //Starts from position of hand, goes forward * length passed in which is defaultLength
    }
}
