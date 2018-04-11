using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.Mygame;

public class GenGameObject : MonoBehaviour
{
    Stack<GameObject> priests_start = new Stack<GameObject>();
    Stack<GameObject> priests_end = new Stack<GameObject>();
    Stack<GameObject> devils_start = new Stack<GameObject>();
    Stack<GameObject> devils_end = new Stack<GameObject>();
    
    GameObject[] boat = new GameObject[2];
    GameObject boat_obj;
    int side = 1;
    public float speed = 6f;
    float gap = 1.2f;
    
    Vector3 shoreStartPos = new Vector3(-19, 0, 0);
    Vector3 shoreEndPos = new Vector3(8, 0, 0);
    Vector3 boatStartPos = new Vector3(-11, 0, 0);
    Vector3 boatEndPos = new Vector3(-1, 0, 0);
    Vector3 priestStartPos = new Vector3(-17, 2f, 0);
    Vector3 priestEndPos = new Vector3(4, 2f, 0);
    Vector3 devilStartPos = new Vector3(-23, 2f, 0);
    Vector3 devilEndPos = new Vector3(10, 2f, 0);
   
    void Start()
    {
        GameSceneController.GetInstance().setGenGameObject(this);
        Instantiate(Resources.Load("Directional Light"));
        Instantiate(Resources.Load("Land"), shoreStartPos, Quaternion.identity);
        Instantiate(Resources.Load("Land"), shoreEndPos, Quaternion.identity);
        boat_obj = Instantiate(Resources.Load("Boat"), boatStartPos, Quaternion.identity) as GameObject;
        for (int i = 0; i < 3; ++i)
        {
            GameObject priest = Instantiate(Resources.Load("Priest")) as GameObject;
            priest.transform.position = getCharacterPosition(priestStartPos, i);
            priests_start.Push(priest);
            GameObject devil = Instantiate(Resources.Load("Devil")) as GameObject;
            devil.transform.position = getCharacterPosition(devilStartPos, i);
            devils_start.Push(devil);
        }
    }
    
    int boatCapacity()
    {
        int capacity = 0;
        for (int i = 0; i < 2; ++i)
            if (boat[i] == null) capacity++;
        return capacity;
    }
    
    void getOnTheBoat(GameObject obj)
    {
        if (boatCapacity() != 0)
        {
            obj.transform.parent = boat_obj.transform;
            Vector3 target = new Vector3();
            if (boat[0] == null)
            {
                boat[0] = obj;
                target = boat_obj.transform.position + new Vector3(-0.8f, 1.2f, 0);
            }
            else
            {
                boat[1] = obj;
                target = boat_obj.transform.position + new Vector3(0.8f, 1.2f, 0);
            }
            SSActionManager.GetInstance().ApplyCCMoveToYZAction(obj, target, speed);
        }
    }
 
    public void moveBoat()
    {
        if (boatCapacity() != 2)
        {
            if (side == 1)
            {
                SSActionManager.GetInstance().ApplyCCMoveToAction(boat_obj, boatEndPos, speed);
                side = 2;
            }
            else if (side == 2)
            {
                SSActionManager.GetInstance().ApplyCCMoveToAction(boat_obj, boatStartPos, speed);
                side = 1;
            }
        }
    }
    
    public void getOffTheBoat(int bside)
    {
        if (boat[bside] != null)
        {
            boat[bside].transform.parent = null;
            Vector3 target = new Vector3();
            if (side == 1)
            {
                if (boat[bside].tag == "Priest")
                {
                    priests_start.Push(boat[bside]);
                    target = getCharacterPosition(priestStartPos, priests_start.Count - 1);
                }
                else if (boat[bside].tag == "Devil")
                {
                    devils_start.Push(boat[bside]);
                    target = getCharacterPosition(devilStartPos, devils_start.Count - 1);
                }
            }
            else if (side == 2)
            {
                if (boat[bside].tag == "Priest")
                {
                    priests_end.Push(boat[bside]);
                    target = getCharacterPosition(priestEndPos, priests_end.Count - 1);
                }
                else if (boat[bside].tag == "Devil")
                {
                    devils_end.Push(boat[bside]);
                    target = getCharacterPosition(devilEndPos, devils_end.Count - 1);
                }
            }
            SSActionManager.GetInstance().ApplyCCMoveToYZAction(boat[bside], target, speed);
            boat[bside] = null;
        }
    }
    
    public void priestStartOnBoat()
    {
        if (priests_start.Count != 0 && boatCapacity() != 0 && side == 1) getOnTheBoat(priests_start.Pop());
    }
    public void priestEndOnBoat()
    {
        if (priests_end.Count != 0 && boatCapacity() != 0 && side == 2) getOnTheBoat(priests_end.Pop());
    }
    public void devilStartOnBoat()
    {
        if (devils_start.Count != 0 && boatCapacity() != 0 && side == 1) getOnTheBoat(devils_start.Pop());
    }
    public void devilEndOnBoat()
    {
        if (devils_end.Count != 0 && boatCapacity() != 0 && side == 2) getOnTheBoat(devils_end.Pop());
    }

    
    Vector3 getCharacterPosition(Vector3 pos, int index) { return new Vector3(pos.x + gap * index, pos.y, pos.z); }

    
    void Update()
    {
        GameSceneController scene = GameSceneController.GetInstance();
        int pOnb = 0, dOnb = 0;
        int priests_s = 0, devils_s = 0, priests_e = 0, devils_e = 0;

        if (priests_end.Count == 3 && devils_end.Count == 3)
        {
            scene.setMessage("Win!");
            return;
        }

        for (int i = 0; i < 2; ++i)
        {
            if (boat[i] != null && boat[i].tag == "Priest") pOnb++;
            else if (boat[i] != null && boat[i].tag == "Devil") dOnb++;
        }
        if (side == 1)
        {
            priests_s = priests_start.Count + pOnb;
            devils_s = devils_start.Count + dOnb;
            priests_e = priests_end.Count;
            devils_e = devils_end.Count;
        }
        else if (side == 2)
        {
            priests_s = priests_start.Count;
            devils_s = devils_start.Count;
            priests_e = priests_end.Count + pOnb;
            devils_e = devils_end.Count + dOnb;
        }
        if ((priests_s != 0 && priests_s < devils_s) || (priests_e != 0 && priests_e < devils_e)) scene.setMessage("Lose!");
    }
}
