using UnityEngine;
using System.Collections;

public class DarkOrLight : MonoBehaviour {

	public Light gameLight;
	public bool isDark = false;
	public float darkItesity;
	public float lerpSpeed = 1.0f;
	public float darkRation = 0.3f;
	public float darkPeriodDuration = 10.0f;

	private float lightIntesity; 
	private float toIntensity;
	// Use this for initialization
	void Start () {
		lightIntesity = gameLight.intensity;
		toIntensity = lightIntesity;
		isDark = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(gameLight.intensity - toIntensity) > 0.05f) {
			gameLight.intensity = Mathf.Lerp(gameLight.intensity,toIntensity,Time.deltaTime * lerpSpeed);
		}
	}

	public void StartDarkOrLight(){
		StartCoroutine ("DarkOrLightCoroutine");
	}

	public void StopDarkOrLight(){
		StopCoroutine ("DarkOrLightCoroutine");
	}

	IEnumerator DarkOrLightCoroutine(){

		while(true){

			yield return new WaitForSeconds(darkPeriodDuration);

			if(Random.Range(0.0f, 1.0f) < darkRation){
				isDark = true;
				toIntensity = darkItesity;
			}else{
				isDark = false;
				toIntensity = lightIntesity;
			}
		}

	}
	void TurnOnLamps(){
		
	}
}
