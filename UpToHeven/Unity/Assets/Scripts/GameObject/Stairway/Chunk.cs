using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

	public Chunk nextChunk;
	public bool fallOnStart = false;

	public float fallDelta;
	public string stepPrefix;
	public string stepPartPrefix;

	private Transform toPosition;
	private float toPositionDistance;
	// Use this for initialization
	void Start () {
		if (fallOnStart) {
			Fall ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Fall(Transform toPosition, float toPositionDistance){
		this.toPosition = toPosition;
		this.toPositionDistance = toPositionDistance;
		Fall ();
	}
	public void Fall(){
		StartCoroutine ("FallCycle");
	}
	IEnumerator  FallCycle(){
		Transform step;
		Transform stepPart;
		float currentFallDelta;


		int stepIndex = 0;

		while (step = transform.Find(stepPrefix + stepIndex++.ToString())) {

			int stepPartIndex = 0;

			currentFallDelta = (toPosition != null 
			                    && 
			                    (toPosition.position.z - step.transform.position.z) > toPositionDistance
			                    ) 
								? 0 : fallDelta;

			yield return new WaitForSeconds (currentFallDelta);

			while (stepPart = step.Find(stepPartPrefix + stepPartIndex++.ToString())) {
				yield return new WaitForSeconds (currentFallDelta);

				stepPart.GetComponent<StepPart> ().Fall ();
			}
		}

		if (nextChunk) {
			nextChunk.Fall (toPosition,toPositionDistance);
		}

		Destroy (gameObject);
	}
}
