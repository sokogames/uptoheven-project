using UnityEngine;
using System.Collections;

public class JumpAction : Action {

	public const string ACTION_JUMP = "jump";

	private JumpObject jumpObject;

	public JumpAction(GameObject receiver, string message = JumpAction.ACTION_JUMP):base(receiver,message){
	
		jumpObject = receiver.GetComponent<JumpObject> ();

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
