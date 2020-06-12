using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HapticInput : BaseInput
{
                                                                                    //Unity eventSystem revloves around having an eventcamera, to know where cursor is on screen
    public Camera eventCamera = null;                                               //In VR, no cursor, but dummy camera attached to hand- cursor positon in the middle of that camera

    public OVRInput.RawButton clickButton = OVRInput.RawButton.A;                   //Reference to which button clickButton is assigned to

    public OVRInput.Controller controller = OVRInput.Controller.All;                //Designate which controller you want to be checking for- can change in inspector


    protected override void Awake()                                                 //This component is attatched to event system
    {
        GetComponent<BaseInputModule>().inputOverride = this;                       //This script inherits from baseinput, so can override to .this;
    }

    public override bool GetMouseButton(int button)                                 //Gets the value of whatever click button you have
    {   
        return OVRInput.Get(clickButton, controller);                               
    }

    public override bool GetMouseButtonDown(int button)                             //Returns value of click button when pressed
    {       
        return OVRInput.GetDown(clickButton, controller);
    }

    public override bool GetMouseButtonUp(int button)                              //Returns value of click button when press is released
    {
        return OVRInput.GetUp(clickButton, controller);
    }

    public override Vector2 mousePosition                                          //Mouse position, uses center- needs event camera to get pixel width and height to cast out of center of camera
    {
        get
        {
            return new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);
        }
    }

}
