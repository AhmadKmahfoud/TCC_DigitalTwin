using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IK_control : MonoBehaviour
{
    #region Variables
    [Space(5)][Header("TP Minus Buttons")]
    public Button BtnMinus01;
    public Button BtnMinus02, BtnMinus03, BtnMinus04, BtnMinus05, BtnMinus06;

    [Space(5)][Header("TP Plus Buttons")]
    public Button BtnPlus01;
    public Button BtnPlus02, BtnPlus03, BtnPlus04, BtnPlus05, BtnPlus06;

    public Transform target;
    public bool IKenabled = false;
    private Vector3 initialPosition; // Posição inicial do alvo
    public float positionStep = 0.01f;
    public float rotationStep = 0.3f;

    // Flags para os botões
    private bool isPlus01Pressed, isMinus01Pressed, isPlus02Pressed, isMinus02Pressed, isPlus03Pressed, isMinus03Pressed;
    private bool isPlus04Pressed, isMinus04Pressed, isPlus05Pressed, isMinus05Pressed, isPlus06Pressed, isMinus06Pressed;

    #endregion

    void Start(){
        initialPosition = target.position;
    }

    public void EnableIKControl(){
        IKenabled = true;
    }

    public void DisableIKControl(){
        IKenabled = false;
    }

    void Update()
    {
        if(IKenabled){
            UpdatePosition();
            UpdateRotation();
        }
    }

    void UpdatePosition()
    {
        Vector3 positionChange = Vector3.zero;

        if (isPlus01Pressed) positionChange.x += positionStep;
        if (isMinus01Pressed) positionChange.x -= positionStep;
        if (isPlus02Pressed) positionChange.y += positionStep;
        if (isMinus02Pressed) positionChange.y -= positionStep;
        if (isPlus03Pressed) positionChange.z += positionStep;
        if (isMinus03Pressed) positionChange.z -= positionStep;

        target.position += positionChange;
    }

    void UpdateRotation()
    {
        Vector3 rotationChange = Vector3.zero;

        if (isPlus04Pressed) rotationChange.x += rotationStep;
        if (isMinus04Pressed) rotationChange.x -= rotationStep;
        if (isPlus05Pressed) rotationChange.y += rotationStep;
        if (isMinus05Pressed) rotationChange.y -= rotationStep;
        if (isPlus06Pressed) rotationChange.z += rotationStep;
        if (isMinus06Pressed) rotationChange.z -= rotationStep;

        if (rotationChange != Vector3.zero)
        {
            Quaternion deltaRotation = Quaternion.Euler(rotationChange);
            target.localRotation = target.localRotation * deltaRotation;
        }
    }

    public void Plus01Is(){isPlus01Pressed = true;}
    public void Plus01IsNot(){isPlus01Pressed = false;}
    public void Minus01Is(){isMinus01Pressed = true;}
    public void Minus01IsNot(){isMinus01Pressed = false;}

    public void Plus02Is(){isPlus02Pressed = true;}
    public void Plus02IsNot(){isPlus02Pressed = false;}
    public void Minus02Is(){isMinus02Pressed = true;}
    public void Minus02IsNot(){isMinus02Pressed = false;}

    public void Plus03Is(){isPlus03Pressed = true;}
    public void Plus03IsNot(){isPlus03Pressed = false;}
    public void Minus03Is(){isMinus03Pressed = true;}
    public void Minus03IsNot(){isMinus03Pressed = false;}

    public void Plus04Is(){isPlus04Pressed = true;}
    public void Plus04IsNot(){isPlus04Pressed = false;}
    public void Minus04Is(){isMinus04Pressed = true;}
    public void Minus04IsNot(){isMinus04Pressed = false;}

    public void Plus05Is(){isPlus05Pressed = true;}
    public void Plus05IsNot(){isPlus05Pressed = false;}
    public void Minus05Is(){isMinus05Pressed = true;}
    public void Minus05IsNot(){isMinus05Pressed = false;}

    public void Plus06Is(){isPlus06Pressed = true;}
    public void Plus06IsNot(){isPlus06Pressed = false;}
    public void Minus06Is(){isMinus06Pressed = true;}
    public void Minus06IsNot(){isMinus06Pressed = false;}
}