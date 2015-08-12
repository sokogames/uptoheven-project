using UnityEngine;
using System.Collections;

public enum PatrollingDirection
{
	left,right
};

public class Patrolling : Stategy {

	// Use this for initialization
	public ObjectVision objectVision;
	public Moving moving;
	public PatrollingDirection direction;
	void Start () {

		moving.ToDirection (direction == PatrollingDirection.left ? MovingDirection.left : MovingDirection.right);
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
