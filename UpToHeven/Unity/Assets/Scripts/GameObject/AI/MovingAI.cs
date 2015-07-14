using UnityEngine;
using System.Collections;

public class MovingAI : MonoBehaviour {

	public Moving movingScript;
	public float actionTime;
	public Stategy strategy;
	// Use this for initialization
	void Start () {
		StartCoroutine("DoAction");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator DoAction(){
	
		while (true) {
				
			yield return new WaitForSeconds (actionTime);

			strategy.Action();		
				
		}
	
	}
}
