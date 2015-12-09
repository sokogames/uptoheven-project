using UnityEngine;
using System.Collections;

public class JumpSlerpAction : Action {

	public const string ACTION_JUMP = "jump";

	private JumpSlerpObject jumpObject;

	public JumpSlerpAction(GameObject receiver, string message = JumpAction.ACTION_JUMP):base(receiver,message){
	
		jumpObject = receiver.GetComponent<JumpSlerpObject> ();

		if (jumpObject == null) {
			Debug.LogError("JumpObject  scritp not assigned");
		}
	}

	public override void Do ()
	{
		jumpObject.Jump ();
	}

	public override IEnumerator IsDone(){
	
		while(!jumpObject.Done()){
		
			yield return null;

		}

	}
}
