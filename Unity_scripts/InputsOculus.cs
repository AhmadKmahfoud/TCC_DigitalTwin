using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsOculus : MonoBehaviour
{   
    public OVRPlayerController player;
    void Update()
    {
        //Pular
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //player.Jump();
        }

        //Controlando Robo
        if ((OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) && (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        {
            player.EnableLinearMovement = false;
            player.EnableRotation = false;
        }
        else
        {
            player.EnableLinearMovement = true;
            player.EnableRotation = true;
        }
    }
}