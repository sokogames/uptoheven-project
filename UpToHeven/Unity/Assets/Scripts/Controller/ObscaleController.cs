using UnityEngine;
using System.Collections;

public enum ObscaleType{ StaticObscale, StaticObscaleDouble, DynamicObscalePatrolling , };

public class ObscaleController : MonoBehaviour {
	
	public GameObject[] staticObscalesPrefs;
	public GameObject[] dynamicObscalePrefs;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject Obscale(ObscaleType type){
		if(type == ObscaleType.StaticObscale){
			return Instantiate(staticObscalesPrefs[0]);
		}
		if(type == ObscaleType.StaticObscaleDouble){
			return Instantiate(staticObscalesPrefs[1]);
		}
		if(type == ObscaleType.DynamicObscalePatrolling){
			return Instantiate(dynamicObscalePrefs[1/*Random.Range(0,2)*/]);
		}

		return staticObscalesPrefs[0];
	}
}
