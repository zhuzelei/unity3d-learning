using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace Com.MyGame
{
    public class Direction
    {
        public static bool left = true;
        public static bool right = false;
    }
    public class Operation
    {
        public static bool Add = true;
        public static bool Sub = false;
    }
    public class StartLocation
    {
        public static Vector3 Pri1 = new Vector3(8, 1, 3);
        public static Vector3 Pri2 = new Vector3(10, 1, 3);
        public static Vector3 Pri3 = new Vector3(12, 1, 3);
        public static Vector3 Dev1 = new Vector3(8, 1, 6);
        public static Vector3 Dev2 = new Vector3(10, 1, 6);
        public static Vector3 Dev3 = new Vector3(12, 1, 6);
        public static Vector3 Land1 = new Vector3(10, 0, 5);
        public static Vector3 Land2 = new Vector3(-10, 0, 5);

        public static Vector3 Boat1 = new Vector3(4, 0, 5);
        public static Vector3 Boat2 = new Vector3(-4, 0, 5);
        public static Vector3 Seat1L = new Vector3(4, 1, 4);
        public static Vector3 Seat1R = new Vector3(4, 1, 6);
        public static Vector3 Seat2L = new Vector3(-4, 1, 4);
        public static Vector3 Seat2R = new Vector3(-4, 1, 6);
    }

    public interface IUserActions
    {
        void boatMove();
        void priestsGetOn();
        void priestsGetOff();
        void devilsGetOn();
        void devilsGetOff();
    }


    public interface IGameJudge
    {
        void modifyBoatPriestsNum(bool isAdd);
        void modifyBoatDevilsNum(bool isAdd);
        void modifyBankPriestsNum(bool isLeftBank, bool isAdd);
        void modifyBankDevilsNum(bool isLeftBank, bool isAdd);
        void judgeTheGame(bool isBoatLeft);
    }

    public class mainSceneController : System.Object, IUserActions, IGameJudge
    {
        private static mainSceneController instance;
        private GenGameObject myGenGameObjects;

        private int BoatPriestsNum, BoatDevilsNum, BankLeftPriestsNum,
            BankRightPriestsNum, BankLeftDevilsNum, BankRightDevilsNum;  //人员数量，用于判断游戏胜负  

        public static mainSceneController getInstance()
        {
            if (instance == null)
                instance = new mainSceneController();
            return instance;
        }

        internal void setGenGameObjects(GenGameObject _myGenGameObjects)
        {
            if (myGenGameObjects == null)
            {
                myGenGameObjects = _myGenGameObjects;
                BoatPriestsNum = BoatDevilsNum = BankLeftPriestsNum = BankLeftDevilsNum = 0;
                BankRightPriestsNum = BankRightDevilsNum = 3;
            }
        }

        /** 
        * 实现IUserActions接口 
        **/
        public void boatMove()
        {
            myGenGameObjects.boatMove();
        }

        public void devilsGetOff()
        {
            myGenGameObjects.devilsGetOff();
        }

        public void devilsGetOn()
        {
            myGenGameObjects.devilsGetOn();
        }

        public void priestsGetOff()
        {
            myGenGameObjects.priestsGetOff();
        }

        public void priestsGetOn()
        {
            myGenGameObjects.priestsGetOn();
        }
        /** 
  * 实现IGameJudge接口 
  **/
        public void modifyBoatPriestsNum(bool isAdd)
        {
            if (isAdd)
                BoatPriestsNum++;
            else
                BoatPriestsNum--;
        }

        public void modifyBoatDevilsNum(bool isAdd)
        {
            if (isAdd)
                BoatDevilsNum++;
            else
                BoatDevilsNum--;
        }

        public void modifyBankDevilsNum(bool isLeftBank, bool isAdd)
        {
            if (isLeftBank)
            {
                if (isAdd)
                    BankLeftDevilsNum++;
                else
                    BankLeftDevilsNum--;
            }
            else
            {
                if (isAdd)
                    BankRightDevilsNum++;
                else
                    BankRightDevilsNum--;
            }
        }

        public void modifyBankPriestsNum(bool isLeftBank, bool isAdd)
        {
            if (isLeftBank)
            {
                if (isAdd)
                    BankLeftPriestsNum++;
                else
                    BankLeftPriestsNum--;
            }
            else
            {
                if (isAdd)
                    BankRightPriestsNum++;
                else
                    BankRightPriestsNum--;
            }
        }

        //public static int Game = 0;

        public void judgeTheGame(bool isBoatLeft)
        {
            if (isBoatLeft)
            {
                if ((BankLeftPriestsNum + BoatPriestsNum > 0
                    && BankLeftDevilsNum + BoatDevilsNum > BankLeftPriestsNum + BoatPriestsNum)
                    || (BankRightDevilsNum > BankRightPriestsNum && BankRightPriestsNum > 0))
                {
                    //Game = -1;
                    showGameText("Failed !");
                }

                if (BankLeftDevilsNum + BoatDevilsNum == 3 && BankLeftPriestsNum + BoatPriestsNum == 3)
                {
                    //Game = 1;
                    showGameText("Victory !");
                }
            }
            else
            {
                if ((BankRightPriestsNum + BoatPriestsNum > 0
                    && BankRightDevilsNum + BoatDevilsNum > BankRightPriestsNum + BoatPriestsNum)
                    || (BankLeftDevilsNum > BankLeftPriestsNum && BankLeftPriestsNum > 0))
                {
                    //Game = -1;
                    showGameText("Failed !");
                }
            }
        }


        void showGameText(string textContent)
        {
            GameObject Canvas = Camera.Instantiate(Resources.Load("Canvas")) as GameObject;
            GameObject GameText = Camera.Instantiate(Resources.Load("GameText"), Canvas.transform) as GameObject;
            GameText.transform.position = Canvas.transform.position;
            GameText.GetComponent<Text>().text = textContent;
        }
    }
}  
    


public class BaseCode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
