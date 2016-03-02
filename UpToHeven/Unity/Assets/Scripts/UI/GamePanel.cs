using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

	const float buttonOffsetY = -40; 

	public Player player;
	private Text text;
	private string comicsName = "Comics";

	private 

	// Use this for initialization
	void Start () {
		text = transform.GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = Mathf.Max (231,(player.maxPosition)).ToString();
	}

	public void ClickPlay(ButtonScrollDown sender){
		sender.ScrollDown ();
		Invoke ("StartGame", 12.0f);
		Invoke ("Fade", 0.2f);
	}
	public void ClickPlayer(ButtonScrollDown sender){

	}
	public void ClickScore(ButtonScrollDown sender){

	}
	public void ClickSettings(ButtonScrollDown sender){

	}
	void StartGame(){
		FadeAllChild (transform.FindChild (comicsName).transform, 0.2f);
		GameObject.Find ("_main").GetComponent<GameController> ().StartGame ();
	}
	void Fade(){
		FadeAllChild(transform,0.7f,comicsName);
	}
	void FadeAllChild(Transform parent, float duration, string exceptName = ""){
		foreach(Transform tr in parent){

			if(tr.gameObject.name == exceptName) continue;

			Image img = tr.GetComponent<Image>();
			if(img){
				img.CrossFadeColor(new Color(255,255,255,0), duration, false,true);
			}
			Text text = tr.GetComponent<Text>();
			if(text){
				text.CrossFadeColor(new Color(255,255,255,0), duration, false,true);
			}
			Button button = tr.GetComponent<Button>();
			if(button){
				button.interactable = false;
			}
			FadeAllChild(tr,duration,exceptName);
		}
	}
}
