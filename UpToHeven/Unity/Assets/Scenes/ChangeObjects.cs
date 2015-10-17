using UnityEngine;
using System.Collections;

public class ChangeObjects : MonoBehaviour {

	public GameObject newStep;
	// Use this for initialization
	void Start () {
	
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("StepPart")) {
			
			/*if( hasChild(obj.transform, "futur_step")){
				GameObject gameObject = (GameObject) Instantiate (newStep,obj.transform.position,obj.transform.rotation);
				gameObject.name = obj.name;
				gameObject.transform.parent = obj.transform.parent;
				//obj.GetComponent<BoxCollider>().isTrigger = true;
				//BoxCollider collider =  obj.AddComponent<BoxCollider>();
				//collider.center = new Vector3(0,-0.5f,0);
				//collider.size = new Vector3(1,1,1);
				Destroy(obj);
			}*/
			foreach(Transform child in obj.transform){
				
				if(child.gameObject.name == "StaticObscaleIron"){
					child.gameObject.tag = "Iron";
				}
				
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool hasChild(Transform tr, string childName){
	
		foreach(Transform child in tr){
		
			if(child.gameObject.name == childName){
				return true;
			}
		
		}

		return false;
	
	}
}
