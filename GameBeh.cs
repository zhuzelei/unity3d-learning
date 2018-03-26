using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBeh : MonoBehaviour {

    int GameStart = 0;
    int Turn = 1;
    int result = 0;
    int[,] state = new int[3, 3];
    public Texture2D img1;
    public Texture2D img2;



    // Use this for initialization
    void Start() {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                state[x, y] = 0;
            }
        }

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(460, 300, 50, 30), "replay"))
        {
            Start();
        }
        int result = check();
        if(result == 1)
        {
            GUI.Label(new Rect(460, 50, 100, 50), "X wins !");
        }
        else if(result == -1)
        {
            GUI.Label(new Rect(460, 50, 100, 50), "O wins !");
        }
        else if(result == 3)
        {
            GUI.Label(new Rect(460, 50, 100, 50), "Nobody wins !");
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (state[x, y] == 1)
                {
                    GUI.Button(new Rect(410 + x * 50, 50 + Screen.height / 9 * 2 + y * 50, 50, 50), "X");
                }
                else if (state[x, y] == -1)
                {
                    GUI.Button(new Rect(410 + x * 50, 50 + Screen.height / 9 * 2 + y * 50, 50, 50), "O");
                }

                if (GUI.Button(new Rect(410 + x * 50, 50 + Screen.height / 9 * 2 + y * 50, 50, 50), ""))
                {
                    if (result == 0)
                    {
                        if (Turn == 1)
                        {
                            state[x, y] = 1;
                        }
                        else if (Turn == -1)
                        {
                            state[x, y] = -1;
                        }
                        Turn = -Turn;
                    }
                }
            }
        }
    }
        // check 
        int check()
        {
            for (int x = 0; x < 3; x++)
            {
                if (state[x, 0] != 0 && state[x, 1] == state[x, 0] && state[x, 2] == state[x, 1])
                {
                    return state[x, 0];
                }
            }
            for (int y = 0; y < 3; y++)
            {
                if (state[0, y] != 0 && state[1, y] == state[0, y] && state[2, y] == state[1, y])
                {
                    return state[0, y];
                }
            }

            if (state[1, 1] != 0 &&
            state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2] ||
            state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0])
            {
                return state[1, 1];
            }

        bool tie = true;
        for(int x=0; x < 3; x++)
        {
            for(int y=0; y < 3; y++)
            {
                if(state[x,y] == 0)
                {
                    tie = false;
                }
            }
        }
        if(tie == true)
        {
            return 3;
        }

        return 0;

        }
}

