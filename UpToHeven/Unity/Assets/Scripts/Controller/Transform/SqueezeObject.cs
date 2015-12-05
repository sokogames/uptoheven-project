using UnityEngine;
using System.Collections;

public class SqueezeObject : MonoBehaviour {

	public Vector3 fromScale;
	public Vector3 toScale;

	public float squeezeSpeed;
	public float unsqueezeSpeed;

	private bool squeeze;
	// Use this for initialization
	void Start () {
		squeeze = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (squeeze) {
			if(Vector3.Distance(transform.localScale, toScale) > 0.1f){
				transform.localScale = Vector3.Lerp(transform.localScale,toScale,Time.deltaTime * squeezeSpeed);
			}else{
				transform.localScale = toScale;
			}
		} else {
			if(Vector3.Distance(transform.localScale, fromScale) > 0.1f){
				transform.localScale = Vector3.Lerp(transform.localScale,fromScale,Time.deltaTime * unsqueezeSpeed);
			}else{
				transform.localScale = fromScale;
			}
		}
	}
	public void squeezeObject(){
		squeeze = true;
	}
	public void unsqueezeObject(){
		squeeze = false;
	}
}
