﻿using UnityEngine;
using System.Collections;

public class Patrolling : Strategy {

	// Use this for initialization
	public ObjectVision objectVision;
	public Moving moving;
	void Start () {

		int rand = Random.Range (0, 2);
		if (rand == 1) {
			moving.RotateLeft();	
		} else {
			moving.RotateRight();
		}
				
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void Action(){

		if (objectVision.isOnEdge ()) {
			moving.Reverse ();
		} else {
			moving.Jump();
		}

	}
}
