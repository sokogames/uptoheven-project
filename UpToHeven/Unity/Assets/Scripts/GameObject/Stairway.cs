using UnityEngine;
using System.Collections;

public class Stairway : MonoBehaviour {

	public GameObject stepPartPref;
	public int stepPartCount = 15;
	public GameObject[] steps;

	private ObscaleController obscaleController;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Generate(ObscaleController obscaleController){

		this.obscaleController = obscaleController;

		Vector3 size = stepPartPref.GetComponent<Renderer> ().bounds.size;

		int index = 0;
		int i = 0;
		GameObject stepPart;

		for (i = 0; i < steps.Length; i++) {
			steps[i] = new GameObject();
			steps[i].transform.parent = transform;
		}

		foreach (GameObject step in steps) {
			for(i = 0; i < stepPartCount; i++){
				stepPart = (GameObject)Instantiate(stepPartPref);
				stepPart.transform.localPosition = new Vector3(i * size.x ,-size.y * 0.5f,0);
				stepPart.transform.parent = step.transform;
				stepPart.GetComponent<DisableKinematicAfterTime>().disableKinematicAfterTime((stepPartCount * index + i) / 10.0f);
				stepPart.GetComponent<RemoveAfterTime>().StartRemoving((stepPartCount * index + i) / 10.0f + 0.5f);
				int rand = Random.Range (0,5);

				if(rand == 3){
					GameObject obscale = obscaleController.Obscale(ObscaleType.StaticObscale);
					obscale.transform.parent = step.transform;
					obscale.transform.localPosition = new Vector3(i ,1 ,0);
				}
			}
			step.transform.position  = new Vector3(0,index * size.x * 0.2f ,index * size.x);
			index++;
		}
	}
}
