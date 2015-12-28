using UnityEngine;
using System.Collections;

public class MovingAI : MonoBehaviour {
	
	public float actionTime;
	public Stategy strategy;

	public int stepsToRest = 0;
	public float restTiem = 1.0f;


	private float currentActionTime;
	private int stepCounter;
	private PlaySoundOnDistance playSoundOnDistance;
	// Use this for initialization
	void Start () {
		currentActionTime = Random.Range (0.0f, (float)actionTime * 2);
		stepCounter = 0;
		StartCoroutine("DoAction");
		playSoundOnDistance = GetComponent<PlaySoundOnDistance> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator DoAction(){
	
		while (true) {

			yield return new WaitForSeconds (currentActionTime);

			currentActionTime = (stepCounter == stepsToRest && stepsToRest > 0 ? restTiem : actionTime);

			strategy.Action();		

			if(stepsToRest > 0){
				stepCounter = ++stepCounter % (stepsToRest + 1);
			}

			if(playSoundOnDistance){
				playSoundOnDistance.Play();
			}
		}
	
	}
}
