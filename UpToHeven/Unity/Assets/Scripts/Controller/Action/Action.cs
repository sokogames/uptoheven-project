using UnityEngine;
using System.Collections;

public abstract class Action {

	protected GameObject receiver;
	protected string message; 

	public Action(GameObject reciever, string message){
		this.receiver = receiver;
		this.message = message;

	}
	public abstract void Do ();
	public abstract IEnumerator IsDone();
}
