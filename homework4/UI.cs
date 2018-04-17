using UnityEngine;
using System.Collections;
using Com.HitGame;

public class UI : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    float beginButtonX = Screen.width / 2 - Screen.width / 8;
    float beginButtonY = Screen.height / 2 - Screen.width / 16;
    float beginButtonW = Screen.width / 4;
    float beginButtonH = Screen.height / 8;
    int clicked = 0;
   
    void OnGUI()
    {
        // font style
        GUIStyle Mystyle = new GUIStyle();
        Mystyle.fontSize = 20;
        Mystyle.normal.background = null;
        GUI.skin.button.fontSize = 30;
        //start button
        if (clicked == 0)
        {
            if (GUI.Button(new Rect(beginButtonX, beginButtonY, beginButtonW, beginButtonH), "Start"))
            {
                myFactory.GetInstance().Round = 1;
                clicked = 1;
            }
        }
        
        // Round
        string round = "Round: " + myFactory.GetInstance().Round.ToString();
        GUI.Label(new Rect(10 , 0, beginButtonW, beginButtonH), round, Mystyle);
        //Score
        string score = "Score: " + myFactory.GetInstance().Score.ToString();
        GUI.Label(new Rect(10 , beginButtonH, beginButtonW, beginButtonH), score, Mystyle);
        //State
        string state;
        if(myFactory.GetInstance().GameState == 1)
        {
             state = "State: Ready";
            GUI.Label(new Rect(10, beginButtonH * 2, beginButtonW, beginButtonH * 2), state, Mystyle);
        }
        else if(myFactory.GetInstance().GameState == 2)
        {
             state = "State: Playing";
            GUI.Label(new Rect(10, beginButtonH * 2, beginButtonW, beginButtonH * 2), state, Mystyle);
        }
        else if(myFactory.GetInstance().GameState == 3)
        {
            state = "State: GameOver !";
            GUI.Label(new Rect(10, beginButtonH * 2, beginButtonW, beginButtonH * 2), state, Mystyle);
        }
        //string state = "State:" + myFactory.GetInstance().State.ToString();
        string Lose = "LoseTime: " + myFactory.GetInstance().LoseNum.ToString();
        GUI.Label(new Rect(10, beginButtonH * 3, beginButtonW, beginButtonH * 3), Lose, Mystyle);

    }

}


