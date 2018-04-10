using UnityEngine;
using System.Collections;
using Com.MyGame;

public class PersonStatus : MonoBehaviour
{
    private Vector3 originalPos;
    public bool onBoatLeft, onBoatRight;
    public bool onBankLeft, onBankRight;

    private IGameJudge gameJudge;

    void Start()
    {
        originalPos = this.transform.position;
        onBoatLeft = false;
        onBoatRight = false;
        onBankLeft = false;
        onBankRight = true;

        gameJudge = mainSceneController.getInstance() as IGameJudge;
    }

    void Update()
    {

    }

    //人上船   bug !!已修复
    public void personSeatOnBoat(bool boatAtLeft, bool seatAtLeft)
    {
        if (seatAtLeft)
        {
            if (boatAtLeft)
                this.transform.position = StartLocation.Seat2L;
            else
                this.transform.position = StartLocation.Seat1L;
            onBoatLeft = true;
        }
        else
        {
            if (boatAtLeft)
                this.transform.position = StartLocation.Seat2R;
            else
                this.transform.position = StartLocation.Seat1R;
            onBoatRight = true;
        }
        onBankLeft = false;
        onBankRight = false;

        if (this.tag.Equals("Priest"))
        {
            gameJudge.modifyBoatPriestsNum(Operation.Add);
            gameJudge.modifyBankPriestsNum(boatAtLeft, Operation.Sub);
        }
        else
        {
            gameJudge.modifyBoatDevilsNum(Operation.Add);
            gameJudge.modifyBankDevilsNum(boatAtLeft, Operation.Sub);
        }
    }

    //人上岸（下船）  
    public void landTheBank(bool boatAtLeft)
    {
        if (boatAtLeft)
        {
            this.transform.position = new Vector3(-originalPos.x, originalPos.y, originalPos.z);
            onBankLeft = true;
        }
        else
        {
            this.transform.position = originalPos;
            onBankRight = true;
        }
        onBoatLeft = false;
        onBoatRight = false;

        if (this.tag.Equals("Priest"))
        {
            gameJudge.modifyBoatPriestsNum(Operation.Sub);
            gameJudge.modifyBankPriestsNum(boatAtLeft, Operation.Add);
        }
        else
        {
            gameJudge.modifyBoatDevilsNum(Operation.Sub);
            gameJudge.modifyBankDevilsNum(boatAtLeft, Operation.Add);
        }
    }
}