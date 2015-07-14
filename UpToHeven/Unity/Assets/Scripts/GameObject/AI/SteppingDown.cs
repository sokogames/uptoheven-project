using UnityEngine;
using System.Collections;

public class SteppingDown : Stategy {

	// Use this for initialization
	public ObjectVision objectVision;
	public Moving moving;
	void Start () {
		moving.ToDirection (MovingDirection.back);
	}
	
	// Update is called once per frame
	void Update () {

	}
	public override void Action(){

		Debug.Log (moving.currentDirection);

		if (moving.currentDirection != MovingDirection.back) {
			moving.ToDirection(MovingDirection.back);	
		}

		if (objectVision.hasBarrier () || objectVision.isOnEdge()) {
			FindDir ();
		}
		moving.Jump ();
	}
	private void FindDir(){
		int rand = Random.Range (0, 2);
		MovingDirection toDir = rand == 1 ? MovingDirection.left : MovingDirection.right;

		moving.ToDirection (toDir);

		if (objectVision.hasBarrier ()) {
			toDir = toDir == MovingDirection.left ? MovingDirection.right : MovingDirection.left;
			moving.ToDirection (toDir);
		}
		//no left side no right side
		if (objectVision.hasBarrier () || objectVision.isOnEdge()) {
			toDir = MovingDirection.forward;
			moving.ToDirection (toDir);
		}
	}
}
