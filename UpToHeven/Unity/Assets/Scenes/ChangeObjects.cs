using UnityEngine;
using System.Collections;

public class ChangeObjects : MonoBehaviour {

	public GameObject newStep;
	public Transform objectContainer;

	public GameObject dog;
	public GameObject pudel;
	public GameObject cat;
	public GameObject grandMom;
	public GameObject person;
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
			//foreach(Transform child in obj.transform){
				
			//	if(child.gameObject.name == "StaticObscaleIron"){
			//		child.gameObject.tag = "Iron";
			//	}
				
			//}
		}
		Loop (objectContainer);

	}

	void Loop(Transform container){
		switch(container.name){
		case "Boy":  ReplaceObjects(container.gameObject, person); break;
		case "Cat": ReplaceObjects(container.gameObject, cat); break;
		case "Dog": ReplaceObjects(container.gameObject, dog); break;
		case "Pudel": ReplaceObjects(container.gameObject, pudel); break;
		case "GrandMom": ReplaceObjects(container.gameObject, grandMom); break;
		case "Boy(Clone)":  container.gameObject.name = "Boy"; break;
		case "Cat(Clone)": container.gameObject.name = "Cat"; break;
		case "Dog(Clone)": container.gameObject.name = "Dog"; break;
		case "Pudel(Clone)": container.gameObject.name = "Pudel"; break;
		case "GrandMom(Clone)": container.gameObject.name = "GrandMom"; break;
		case "Proto_101_unit_ground": container.localScale = new Vector3(1,60,1); container.Translate(new Vector3(0,-3.571429f,0)); break;
		case "Mous_cub_001": container.localScale = new Vector3(1,60,1); container.Translate(new Vector3(0,-7.143f,0)); break;
		case "futur_step_2": container.localScale = new Vector3(1,60,1); container.Translate(new Vector3(0,-7.143f,0)); break;
		default: 
			foreach(Transform child in container){
				Loop(child);
			}
			break;
		}
	}

	void ReplaceObjects(GameObject currObj, GameObject ToObj){


		GameObject newObejct = (GameObject)Instantiate(ToObj,currObj.transform.position,currObj.transform.rotation);
		newObejct.transform.parent = currObj.transform.parent;

		Destroy (currObj);
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
