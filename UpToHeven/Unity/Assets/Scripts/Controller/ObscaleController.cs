using UnityEngine;
using System.Collections;

public enum ObscaleType{ StaticObscale, StaticObscaleDouble, DynamicObscalePatrolling, DynamicObscaleDown, Handrail };

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
		if(type == ObscaleType.Handrail){
			return Instantiate(staticObscalesPrefs[2]);
		}
		if(type == ObscaleType.DynamicObscalePatrolling){
			return Instantiate(dynamicObscalePrefs[0]);
		}
		if(type == ObscaleType.DynamicObscaleDown){
			return Instantiate(dynamicObscalePrefs[1]);
		}
		return staticObscalesPrefs[0];
	}
}
