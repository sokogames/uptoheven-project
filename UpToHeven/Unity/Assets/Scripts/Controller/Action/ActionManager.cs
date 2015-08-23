using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour {
	
	public int MaxAcionsInQueue = 3;

	private Queue <Action> actions = new Queue<Action>();
	private Action currentAction;

	void Start () {
		currentAction = null;
	}
	
	void Update(){

		if (currentAction == null && actions.Count > 0) {


			currentAction = actions.Dequeue();
			currentAction.Do();

			StartCoroutine("StartAndWaitForCurrenAction");

		}
	}

	public void AddAction(Action action){

		if(MaxAcionsInQueue < actions.Count){
			return;
		}

		actions.Enqueue (action);

	}
	public IEnumerator StartAndWaitForCurrenAction(){
	
		yield return StartCoroutine (currentAction.IsDone ());

		currentAction = null;
	
	}
}
