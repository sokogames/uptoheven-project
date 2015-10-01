using UnityEngine;
using System.Collections;

public class MovingAI : MonoBehaviour {

	public Moving movingScript;
	public float actionTime;
	public Stategy strategy;

	public int stepsToRest = 0;
	public float restTiem = 1.0f;


	private float currentActionTime;
	private int stepCounter;
	// Use this for initialization
	void Start () {
		currentActionTime = Random.Range (0.0f, (float)actionTime * 2);
		stepCounter = 0;
		StartCoroutine("DoAction");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator DoAction(){
	
		while (true) {
			if(stepsToRest > 0 ){
				Debug.Log (stepsToRest);	
				Debug.Log (stepCounter);
				Debug.Log ("=====");
			}

			yield return new WaitForSeconds (currentActionTime);

			currentActionTime = (stepCounter == stepsToRest && stepsToRest > 0 ? restTiem : actionTime);

			strategy.Action();		

			if(stepsToRest > 0){
				stepCounter = ++stepCounter % (stepsToRest + 1);
			}
		}
	
	}
}
