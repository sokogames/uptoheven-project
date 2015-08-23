using UnityEngine;
using System.Collections;

public class RotateAction : Action {

	public const string ACTION_LEFT = "action left";
	public const string ACTION_RIGTH = "action right";
	public const string ACTION_FACE_FORWARD = "action face forward";
	public const string ACTION_FACE_BACKWARD = "action face backward";
	public const string ACTION_FACE_LEFT = "action face left";
	public const string ACTION_FACE_RIGHT = "action face right";

	private RotateObject rotateObject;

	public RotateAction(GameObject receiver, string message):base(receiver,message){
	
		rotateObject = receiver.GetComponent<RotateObject> ();

		if (rotateObject == null) {
			Debug.LogError("RotateObject  scritp not assigned");
		}
	}

	public override void Do ()
	{
		switch(message){
		case RotateAction.ACTION_LEFT:
			rotateObject.RotateLeft();
			break;
		case RotateAction.ACTION_RIGTH:
			rotateObject.RotateRight();
			break;
		case RotateAction.ACTION_FACE_FORWARD:
			rotateObject.FaceForward();
			break;
		case RotateAction.ACTION_FACE_BACKWARD:
			rotateObject.FaceBackward();
			break;
		case RotateAction.ACTION_FACE_LEFT:
			rotateObject.FaceLeft();
			break;
		case RotateAction.ACTION_FACE_RIGHT:
			rotateObject.FaceRight();
			break;
		default :break;
		}
	}

	public override IEnumerator IsDone(){
	
		while(!rotateObject.Done()){
		
			yield return null;

		}
		
	}
}
