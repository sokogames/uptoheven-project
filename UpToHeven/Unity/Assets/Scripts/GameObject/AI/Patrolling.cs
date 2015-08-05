using UnityEngine;
using System.Collections;

public class Patrolling : Stategy {

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

		if (objectVision.isOnEdge () || objectVision.hasBarrier()) {
			moving.Reverse ();
		} else {
			moving.Jump();
		}

	}
}
