using UnityEngine;
using System.Collections;

public class RGBDisableAfterTime : MonoBehaviour {

	public float time;
	// Use this for initialization
	void Start () {
		Invoke ("Remove",time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Remove(){
		Destroy (gameObject);
	}
}
