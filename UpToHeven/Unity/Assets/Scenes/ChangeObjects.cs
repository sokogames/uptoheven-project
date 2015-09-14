using UnityEngine;
using System.Collections;

public class ChangeObjects : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("StaticObscale")) {
		
			if( hasChild(obj.transform, "futur_step")){
				obj.GetComponent<BoxCollider>().isTrigger = true;
				BoxCollider collider =  obj.AddComponent<BoxCollider>();
				collider.center = new Vector3(0,-0.5f,0);
				collider.size = new Vector3(1,1,1);
				Debug.Log(obj.transform.parent.gameObject.name);
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
