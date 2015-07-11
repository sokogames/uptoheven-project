using UnityEngine;
using System.Collections;

public class RemoveAfterTime : MonoBehaviour {
	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartRemoving(float time){
		Invoke("Remove",time);
	}
	private void Remove(){
		Destroy(gameObject);
	}
}
