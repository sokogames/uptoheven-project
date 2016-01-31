using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChoosePlayer : MonoBehaviour {

	public GameObject[] playerBodies;
	public float distanceX;

	private int index;
	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
			
	}

	void MoveLeft(){
	}
	void MoveRight(){
	}
	void Choose(){
	}
	void Init(){

		for (int i =0; i < playerBodies.Length; i++) {
	
			GameObject playerBody = (GameObject)Instantiate(playerBodies[i],Vector3.right * distanceX * i,Quaternion.identity);
			playerBody.transform.parent = transform;
			playerBody.layer = gameObject.layer;

		}

	}
}
