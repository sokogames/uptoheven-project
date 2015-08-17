using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine ("AddChunk");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator AddChunk(){

		while (true) {

			yield return new WaitForSeconds (2.0f);

			Stairway stairway = GameObject.Find ("Stairway").GetComponent<Stairway> ();

			stairway.addRandomChunk ();

		}
	}
}
