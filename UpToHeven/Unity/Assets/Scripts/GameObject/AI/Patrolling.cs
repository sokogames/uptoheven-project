using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RotateObject))]
[RequireComponent(typeof(JumpSlerpObject))]
[RequireComponent(typeof(ObjectVision))]
public class Patrolling : Stategy {

	// Use this for initialization
	private ObjectVision objectVision;
	private JumpSlerpObject jumpSlerpObject;
	private RotateObject rotateObject;

	private JumperDirection currentDirection;

	void Start () {

		objectVision = GetComponent<ObjectVision> ();
		jumpSlerpObject = GetComponent<JumpSlerpObject> ();
		rotateObject = GetComponent<RotateObject> ();

		int startDir = Random.Range (1, 30);

		currentDirection = startDir >= 15 ? JumperDirection.left : JumperDirection.right;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void Action(){

		if (objectVision.isOnEdge (currentDirection) || objectVision.hasBarrier(currentDirection)) {

			currentDirection = currentDirection == JumperDirection.left ? JumperDirection.right : JumperDirection.left;
			rotateObject.RotateTo(currentDirection);

		} else {
			rotateObject.RotateTo(currentDirection);
			jumpSlerpObject.Jump(currentDirection);
		}

	}
}
