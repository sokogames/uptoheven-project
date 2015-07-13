using UnityEngine;
using System.Collections;

public class Stairway : MonoBehaviour {

	public GameObject stepPartPref;
	public int stepPartCount = 15;
	public GameObject[] steps;
	public float stepPartCrashTime;

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
			//if is first obscale on this step;
			bool firstObscale = true;
			bool obscalesFinished = false;
			for(i = 0; i < stepPartCount; i++){
				stepPart = (GameObject)Instantiate(stepPartPref);
				stepPart.transform.localPosition = new Vector3(i * size.x ,-size.y * 0.5f,0);
				stepPart.transform.parent = step.transform;
				stepPart.GetComponent<DisableKinematicAfterTime>().disableKinematicAfterTime((stepPartCount * index + i) * stepPartCrashTime);
				stepPart.GetComponent<RemoveAfterTime>().StartRemoving((stepPartCount * index + i) * stepPartCrashTime + 0.5f);

				if(!obscalesFinished){
					int rand = Random.Range (0,6);

					if(rand == 3){

						if(firstObscale){
							int randForType = Random.Range(0,4);

							if(randForType >= 3){
								createObscale(ObscaleType.DynamicObscalePatrolling,step.transform,i);
								obscalesFinished = true;

								continue;
							}
						}

						createObscale(ObscaleType.StaticObscale,step.transform,i);

						firstObscale = false;
					}
				}
			}
			step.transform.position  = new Vector3(0,index * size.x * 0.2f ,index * size.x);
			index++;
		}
	}
	private GameObject createObscale(ObscaleType type, Transform parent, int index){

		GameObject obscale = obscaleController.Obscale(type);
		obscale.transform.parent = parent;
		obscale.transform.localPosition = new Vector3(index ,1 ,0);

		return obscale;
	}
}
