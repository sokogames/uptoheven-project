using UnityEngine;
using System.Collections;

public class Stairway : MonoBehaviour {

	public GameObject stepPartPref;
	public int stepPartCount = 15;

	public float stepPartCrashTime;
	public bool crashSteps;
	public int initialStepsCount;
	public int lastStepIndex;

	private ObscaleController obscaleController;
	private Vector3 size;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Generate(ObscaleController obscaleController){

		lastStepIndex = 0;

		this.obscaleController = obscaleController;

		size = stepPartPref.GetComponent<Renderer> ().bounds.size;

		int index = 0;
		int i = 0;
		GameObject stepPart;

		for(i = 0; i < initialStepsCount; i++) {
			//if is first obscale on this step;
			GameObject step = createStepAtPosition(i);
		}
		lastStepIndex = i;
	}
	private GameObject createObscale(ObscaleType type, Transform parent, int index){

		GameObject obscale = obscaleController.Obscale(type);
		obscale.transform.parent = parent;
		obscale.transform.localPosition = new Vector3(index ,1 ,0);

		return obscale;
	}
	public void AddNextStep(){

		createStepAtPosition (lastStepIndex);

		lastStepIndex++;
	}
	private GameObject createStepAtPosition(int index){
			GameObject step = new GameObject ();
			bool firstObscale = true;
			bool obscalesFinished = false;
			for(int i = 0; i < stepPartCount; i++){
				GameObject stepPart = (GameObject)Instantiate(stepPartPref);
				stepPart.transform.localPosition = new Vector3(i * size.x ,-size.y * 0.5f,0);
				stepPart.transform.parent = step.transform;
				if(crashSteps){
					stepPart.GetComponent<DisableKinematicAfterTime>().disableKinematicAfterTime((stepPartCount * index + i) * stepPartCrashTime);
					stepPart.GetComponent<RemoveAfterTime>().StartRemoving((stepPartCount * index + i) * stepPartCrashTime + 0.5f);
				}
				if(!obscalesFinished){
					int rand = Random.Range (0,6);
					
					if(rand >= 4){
						
						if(firstObscale){
							int randForType = Random.Range(0,4);
							
							if(randForType >= 2){
								createObscale(ObscaleType.DynamicObscalePatrolling,step.transform,i);
								obscalesFinished = true;
								
								continue;
							}
						}
						
						
						createObscale(Random.Range (0,2) == 1 ? ObscaleType.StaticObscale : ObscaleType.StaticObscaleDouble,step.transform,i);
						
						firstObscale = false;
					}
				}
			}
			step.transform.parent = transform;
			step.transform.localPosition  = new Vector3(0,index * size.x * 0.2f ,index * size.x);
			return step;
	}
}
