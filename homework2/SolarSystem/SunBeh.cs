using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBeh : MonoBehaviour {

    public Transform Sun;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Sun.Rotate(Vector3.up * 10 * Time.deltaTime);
	}
}
