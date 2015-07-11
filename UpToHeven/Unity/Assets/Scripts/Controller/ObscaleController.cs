using UnityEngine;
using System.Collections;

public enum ObscaleType{ StaticObscale, DynamicObscale };

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
		return Instantiate(staticObscalesPrefs[0]);
	}
}
