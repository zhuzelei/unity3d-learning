using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBeh : MonoBehaviour {

    public Transform Planet;
    public Transform Origin;
    public float speed;
    public float AroundSpeed;
    public float AroundY, AroundZ;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 axis = new Vector3(0, AroundY, AroundZ);
        Planet.transform.RotateAround(Origin.position, axis, AroundSpeed * Time.deltaTime);
        Planet.transform.Rotate(Vector3.up * speed * Time.deltaTime);
	}
}
