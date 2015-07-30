using UnityEngine;
using System.Collections;

public class DisableKinematicAfterTime : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		if (! GetComponent<Rigidbody> ()) {
			Debug.LogError("Rigidbody not assigned");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void disableKinematicAfterTime(float time){
		Invoke ("disableKinematic",time);
	}
	private void  disableKinematic(){
		GetComponent<Rigidbody> ().isKinematic = false;
		GetComponent<RemoveAfterTime> ().enabled = true;
	}
}
