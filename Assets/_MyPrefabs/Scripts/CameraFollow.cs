using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public bool following=true;
	public GameObject followTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (followTarget && following) {
			transform.position = followTarget.transform.position;
		}
	}
}
