using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionQueue : MonoBehaviour{

	public IActor actor; 
	List <Action> actions = new List<Action>();
	// Use this for initialization
	private bool actionAddressingFinished;
	void Start () {
		actionAddressingFinished = true;
	}
	void Update(){

	}
	public void AddAction(Action action){
		if (actor == null) {
			Debug.LogError ("Actor not assigned to ActionQueue");
		}
		actions.Add (action);

		//if only actions is that
		if (actionAddressingFinished) {
			StartCoroutine("AddressActions");
		}
	}
	private IEnumerator AddressActions(){

		actionAddressingFinished = false;

		while (actions.Count > 0) {
		
			Action action = actions[0];

			switch(action.type){
			case ActionType.jump : actor.Jump();break;
			case ActionType.readyForJump : actor.ReadyForJump(action.jumpDirection);break;
			default: break;
			}

			actions.RemoveAt(0);

			yield return new WaitForSeconds(action.actionTime);

		}

		actionAddressingFinished = true;

	}
}
