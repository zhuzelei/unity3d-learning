using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGUI : MonoBehaviour
{
    //父按钮
    public RectTransform rectBloodPos;
    //子按钮
    public RectTransform blood;
    //初始减少血量
    public int reduceBlood = 2;
    void Update()
    {
        rectBloodPos.anchoredPosition = new Vector2( 0,0 );
        //这里需要用到下面的ExtendsionMethod，否则不能直接设置Right属性
        blood.GetComponent<RectTransform>().Right(reduceBlood);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 30, 40, 20), "加血"))
        {
            reduceBlood -= 10;
            if (reduceBlood < 0)
            {
                reduceBlood = 0;
            }
        }
        if (GUI.Button(new Rect(70, 30, 40, 20), "减血"))
        {
            reduceBlood += 10;
            if (reduceBlood > 200)
            {
                reduceBlood = 200;
            }
        }
    }
}