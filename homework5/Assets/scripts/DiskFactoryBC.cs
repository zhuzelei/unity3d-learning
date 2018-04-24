using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;


public class DiskFactoryBC : MonoBehaviour
{
    public GameObject disk;

    void Awake()
    {
        // 初始化预设对象  
        DiskFactory.getInstance().diskTemplate = disk;
    }
}
