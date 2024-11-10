using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RVFRL_robo : MonoBehaviour
{
    public Robo RVFRL = new Robo("RVFRL", 6);
    public float[] HomeAngles = { 0, 30, -30, 0, -90, 0 };

    public float[] LowerLimitAngles = { -240, -120, -164, -200, -120, -360 };
    public float[] UpperLimitAngles = { 240, 120, -0, 200, 120, 360 };

    private bool btnJ1plusIsPressed, btnJ1minusIsPressed, btnJ2plusIsPressed, btnJ2minusIsPressed,
                 btnJ3plusIsPressed, btnJ3minusIsPressed, btnJ4plusIsPressed, btnJ4minusIsPressed,
                 btnJ5plusIsPressed, btnJ5minusIsPressed, btnJ6plusIsPressed, btnJ6minusIsPressed;

    public void Start(){
        RVFRL.RoboInitiate(HomeAngles, LowerLimitAngles, UpperLimitAngles);
    }

    public void Update(){
        //RVFRL.Selection();
        //RVFRL.Manipulate();
        if(btnJ1plusIsPressed){RVFRL.TP_Manipulate(1, true);}
        if(btnJ1minusIsPressed){RVFRL.TP_Manipulate(1, false);}
        if(btnJ2plusIsPressed){RVFRL.TP_Manipulate(2, true);}
        if(btnJ2minusIsPressed){RVFRL.TP_Manipulate(2, false);}
        if(btnJ3plusIsPressed){RVFRL.TP_Manipulate(3, true);}
        if(btnJ3minusIsPressed){RVFRL.TP_Manipulate(3, false);}
        if(btnJ4plusIsPressed){RVFRL.TP_Manipulate(4, true);}
        if(btnJ4minusIsPressed){RVFRL.TP_Manipulate(4, false);}
        if(btnJ5plusIsPressed){RVFRL.TP_Manipulate(5, true);}
        if(btnJ5minusIsPressed){RVFRL.TP_Manipulate(5, false);}
        if(btnJ6plusIsPressed){RVFRL.TP_Manipulate(6, true);}
        if(btnJ6minusIsPressed){RVFRL.TP_Manipulate(6, false);}
    }

    public void J1plusIs(){btnJ1plusIsPressed=true;}
    public void J1plusIsNot(){btnJ1plusIsPressed=false;}
    public void J1minusIs(){btnJ1minusIsPressed=true;}
    public void J1minusIsNot(){btnJ1minusIsPressed=false;}

    public void J2plusIs(){btnJ2plusIsPressed=true;}
    public void J2plusIsNot(){btnJ2plusIsPressed=false;}
    public void J2minusIs(){btnJ2minusIsPressed=true;}
    public void J2minusIsNot(){btnJ2minusIsPressed=false;}

    public void J3plusIs(){btnJ3plusIsPressed=true;}
    public void J3plusIsNot(){btnJ3plusIsPressed=false;}
    public void J3minusIs(){btnJ3minusIsPressed=true;}
    public void J3minusIsNot(){btnJ3minusIsPressed=false;}

    public void J4plusIs(){btnJ4plusIsPressed=true;}
    public void J4plusIsNot(){btnJ4plusIsPressed=false;}
    public void J4minusIs(){btnJ4minusIsPressed=true;}
    public void J4minusIsNot(){btnJ4minusIsPressed=false;}

    public void J5plusIs(){btnJ5plusIsPressed=true;}
    public void J5plusIsNot(){btnJ5plusIsPressed=false;}
    public void J5minusIs(){btnJ5minusIsPressed=true;}
    public void J5minusIsNot(){btnJ5minusIsPressed=false;}

    public void J6plusIs(){btnJ6plusIsPressed=true;}
    public void J6plusIsNot(){btnJ6plusIsPressed=false;}
    public void J6minusIs(){btnJ6minusIsPressed=true;}
    public void J6minusIsNot(){btnJ6minusIsPressed=false;}
}