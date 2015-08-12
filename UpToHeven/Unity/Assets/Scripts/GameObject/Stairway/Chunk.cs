using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

	public bool fall = false;

	public float fallDelta;
	public string stepPrefix;
	public string stepPartPrefix;
	// Use this for initialization
	void Start () {
		if (fall) {
			Fall ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Fall(){
		StartCoroutine ("fallChilds");
	}
	IEnumerator  fallChilds(){
		Transform step;
		Transform stepPart;

		int stepIndex = 0;

		while (step = transform.FindChild(stepPrefix + stepIndex++.ToString())) {

			int stepPartIndex = 0;

			yield return new WaitForSeconds (fallDelta);

			while (stepPart = step.FindChild(stepPartPrefix + stepPartIndex++.ToString())) {
				yield return new WaitForSeconds (fallDelta);

				stepPart.GetComponent<StepPart> ().Fall ();
			}
		}
	}
}
