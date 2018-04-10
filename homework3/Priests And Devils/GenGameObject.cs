using UnityEngine;
using System.Collections;
using Com.MyGame;
using System.Collections.Generic;

public class GenGameObject : MonoBehaviour
{
    public List<GameObject> Priests, Devils;
    public GameObject boat, bankLeft, bankRight;

    private BoatBehaviour myBoatBehaviour;

    void Start()
    {
        Priests = new List<GameObject>();
        
        for (int i = 0; i < 3; i++)
        {
            GameObject priests = (GameObject)Instantiate(Resources.Load("Priest"));
            priests.name = "Priest " + (i + 1);
            priests.AddComponent<PersonStatus>();
            Priests.Add(priests);
        }
        
        Priests[0].transform.position = StartLocation.Pri1;
        Priests[1].transform.position = StartLocation.Pri2;
        Priests[2].transform.position = StartLocation.Pri3;

        Devils = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject devils = (GameObject)Instantiate(Resources.Load("Devil"));
            devils.name = "Devil " + (i + 1);
            devils.AddComponent<PersonStatus>();
            Devils.Add(devils);
        }
        Devils[0].transform.position = StartLocation.Dev1;
        Devils[1].transform.position = StartLocation.Dev2;
        Devils[2].transform.position = StartLocation.Dev3;

        boat = (GameObject)Instantiate(Resources.Load("Boat"));
        boat.name = "Boat";
        boat.AddComponent<BoatBehaviour>();
        myBoatBehaviour = boat.GetComponent<BoatBehaviour>();
        //boat.transform.localScale = new Vector3(3, 1, 1);
        boat.transform.position = StartLocation.Boat1;

        bankLeft = (GameObject)Instantiate(Resources.Load("Land"));
        bankLeft.name = "BankLeft";
        //bankLeft.transform.Rotate(new Vector3(0, 0, 90));
        //bankLeft.transform.localScale = new Vector3(1, 4, 1);
        bankLeft.transform.position = StartLocation.Land2;

        bankRight = (GameObject)Instantiate(Resources.Load("Land"));
        bankRight.name = "BankRight";
        //bankRight.transform.Rotate(new Vector3(0, 0, 90));
        //bankRight.transform.localScale = new Vector3(1, 4, 1);
        bankRight.transform.position = StartLocation.Land1;

        mainSceneController.getInstance().setGenGameObjects(this);
    }

    void Update()
    {

    }

    public void boatMove()
    {
        myBoatBehaviour.setBoatMove();
    }

    //牧师上船  
    public void priestsGetOn()
    {
        if (myBoatBehaviour.isMoving)
            return;

        if (!myBoatBehaviour.isBoatAtLeftSide())
        {  //船在右侧  
            for (int i = 0; i < Priests.Count; i++)
            {
                if (Priests[i].GetComponent<PersonStatus>().onBankRight)
                {
                    //右侧岸上有牧师  
                    detectEmptySeat(true, i, Direction.right);
                    break;
                }
            }
        }
        else
        {  //船在左侧  
            for (int i = 0; i < Priests.Count; i++)
            {
                if (Priests[i].GetComponent<PersonStatus>().onBankLeft)
                {
                    //左侧岸上有牧师  
                    detectEmptySeat(true, i, Direction.left);
                    break;
                }
            }
        }
    }
    //恶魔上船  
    public void devilsGetOn()
    {
        if (myBoatBehaviour.isMoving)
            return;

        if (!myBoatBehaviour.isBoatAtLeftSide())
        {  //船在右侧  
            for (int i = 0; i < Devils.Count; i++)
            {
                if (Devils[i].GetComponent<PersonStatus>().onBankRight)
                {
                    //右侧岸上有恶魔  
                    detectEmptySeat(false, i, Direction.right);
                    break;
                }
            }
        }
        else
        {  //船在左侧  
            for (int i = 0; i < Devils.Count; i++)
            {
                if (Devils[i].GetComponent<PersonStatus>().onBankLeft)
                {
                    //左侧岸上有恶魔  
                    detectEmptySeat(false, i, Direction.left);
                    break;
                }
            }
        }
    }
    //当岸上有牧师/恶魔的时候，检测船上是否有空位  
    void detectEmptySeat(bool isPriests, int index, bool boatDir)
    {
        if (myBoatBehaviour.isLeftSeatEmpty())
        {        //船上左位置没人  
            seatThePersonAndModifyBoat(isPriests, index, boatDir, Direction.left);
        }
        else if (myBoatBehaviour.isRightSeatEmpty())
        {  //船上左位置有人，右位置没人  
            seatThePersonAndModifyBoat(isPriests, index, boatDir, Direction.right);
        }
    }
    //牧师/恶魔上船，并调整船的属性  
    void seatThePersonAndModifyBoat(bool isPriests, int index, bool boatDir, bool seatDir)
    {
        if (isPriests)
        {
            Priests[index].GetComponent<PersonStatus>().personSeatOnBoat(boatDir, seatDir);
            Priests[index].transform.parent = boat.transform;
        }
        else
        {
            Devils[index].GetComponent<PersonStatus>().personSeatOnBoat(boatDir, seatDir);
            Devils[index].transform.parent = boat.transform;
        }
        myBoatBehaviour.seatOnPos(seatDir);
    }


    //牧师下船  
    public void priestsGetOff()
    {
        if (myBoatBehaviour.isMoving)
            return;

        if (!myBoatBehaviour.isBoatAtLeftSide())
        {  //船在右侧  
            for (int i = Priests.Count - 1; i >= 0; i--)
            {
                if (detectIfPeopleOnBoat(true, i, Direction.right))
                    break;
            }
        }
        else
        {  //船在左侧  
            for (int i = Priests.Count - 1; i >= 0; i--)
            {
                if (detectIfPeopleOnBoat(true, i, Direction.left))
                    break;
            }
        }
    }
    //恶魔下船  
    public void devilsGetOff()
    {
        if (myBoatBehaviour.isMoving)
            return;

        if (!myBoatBehaviour.isBoatAtLeftSide())
        {  //船在右侧  
            for (int i = Devils.Count - 1; i >= 0; i--)
            {
                if (detectIfPeopleOnBoat(false, i, Direction.right))
                    break;
            }
        }
        else
        {  //船在左侧  
            for (int i = Devils.Count - 1; i >= 0; i--)
            {
                if (detectIfPeopleOnBoat(false, i, Direction.left))
                    break;
            }
        }
    }
    //检测是否有牧师/恶魔在船上  
    bool detectIfPeopleOnBoat(bool isPriests, int i, bool boatDir)
    {
        if (isPriests)
        {
            if (Priests[i].GetComponent<PersonStatus>().onBoatLeft
            || Priests[i].GetComponent<PersonStatus>().onBoatRight)
            {
                //在船上  
                myBoatBehaviour.jumpOutOfPos(Priests[i].GetComponent<PersonStatus>().onBoatLeft);
                Priests[i].GetComponent<PersonStatus>().landTheBank(boatDir);
                Priests[i].transform.parent = boat.transform.parent;

                return true;
            }
            return false;
        }
        else
        {
            if (Devils[i].GetComponent<PersonStatus>().onBoatLeft
            || Devils[i].GetComponent<PersonStatus>().onBoatRight)
            {
                //在船上  
                myBoatBehaviour.jumpOutOfPos(Devils[i].GetComponent<PersonStatus>().onBoatLeft);
                Devils[i].GetComponent<PersonStatus>().landTheBank(boatDir);
                Devils[i].transform.parent = boat.transform.parent;

                return true;
            }
            return false;
        }
    }

}