using UnityEngine;
using System.Collections;

public class MovingAI : MonoBehaviour {

	public Moving movingScript;
	public float actionTime;
	public Stategy strategy;

	private float currentActionTime;
	// Use this for initialization
	void Start () {
		currentActionTime = Random.Range (0.0f, actionTime);
		StartCoroutine("DoAction");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator DoAction(){
	
		while (true) {
				
			yield return new WaitForSeconds (currentActionTime);

			currentActionTime = actionTime;

			strategy.Action();		
				
		}
	
	}
}
