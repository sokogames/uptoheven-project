using UnityEngine;
using System.Collections;

public class StepPart : MonoBehaviour {

	public float removeTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Fall(){
		GetComponent<Rigidbody> ().isKinematic = false;
		Invoke ("Remove", removeTime);
	}
	void Remove(){
		Destroy (gameObject);
	}
}
