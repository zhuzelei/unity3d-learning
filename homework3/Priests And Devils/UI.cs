using UnityEngine;
using System.Collections;
using Com.MyGame;

public class UI : MonoBehaviour
{
    IUserActions myActions;
    float btnWidth = (float)Screen.width / 10.0f;
    float btnHeight = (float)Screen.height / 10.0f;

    void Start()
    {
        myActions = mainSceneController.getInstance() as IUserActions;
    }

    void Update()
    {

    }

    void OnGUI()
    {
        

        if (GUI.Button(new Rect(155, 350, btnWidth, btnHeight), "Priests GetOn"))
        {
            myActions.priestsGetOn();
        }
        if (GUI.Button(new Rect(305, 350, btnWidth, btnHeight), "Priests GetOff"))
        {
            myActions.priestsGetOff();
        }
        if (GUI.Button(new Rect(455, 350, btnWidth, btnHeight), "Go!"))
        {
            myActions.boatMove();
        }
        if (GUI.Button(new Rect(605, 350, btnWidth, btnHeight), "Devils GetOn"))
        {
            myActions.devilsGetOn();
        }
        if (GUI.Button(new Rect(755, 350, btnWidth, btnHeight), "Devils GetOff"))
        {
            myActions.devilsGetOff();
        }

    }
}