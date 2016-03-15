using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LampLight : MonoBehaviour {

	public Light lampLight;
	public const float lightIntensity = 3.22f;
	public const float bounceIntensity = 0.0f;
	public float lerpSpeed = 1.0f;
	public float darkOrLightCheckDuration = 1.0f;
	private const float darkLightIntensity = 0.0f;
	private float toLightIntensity;

	private bool _lightOn = false;

	[SerializeField]
	public bool LightOn{

		get{
			return _lightOn;
		}

		set{
			_lightOn = value;
			toLightIntensity = value ? LampLight.lightIntensity : LampLight.darkLightIntensity;	

			if (value) {
				lampLight.enabled = true;
			}
		}
	}
	// Use this for initialization
	void Start () {
		lampLight.intensity = LampLight.darkLightIntensity;
		lampLight.bounceIntensity = bounceIntensity;
		StartCoroutine ("CheckDarkOrLight");
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(lampLight.intensity - toLightIntensity) > 0.05f) {
			lampLight.intensity = Mathf.Lerp(lampLight.intensity,toLightIntensity,Time.deltaTime * lerpSpeed);
		}
	}

	IEnumerator CheckDarkOrLight(){
		while (true) {
			yield return new WaitForSeconds (darkOrLightCheckDuration);
			LightOn = Object.FindObjectOfType<DarkOrLight> ().isDark;

			if (lampLight.intensity < 0.06f && !LightOn) {
				lampLight.enabled = false;
			}
		}
	}
}
