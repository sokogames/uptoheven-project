using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

	const float buttonOffsetY = -40; 

	public Player player;
	private Text text;

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
		Invoke ("StartGame", 0.5f);
		Invoke ("Fade", 0.2f);
	}
	public void ClickPlayer(ButtonScrollDown sender){

	}
	public void ClickScore(ButtonScrollDown sender){

	}
	public void ClickSettings(ButtonScrollDown sender){

	}
	void StartGame(){
		GameObject.Find ("_main").GetComponent<GameController> ().StartGame ();
	}
	void Fade(){
			FadeAllChild(transform,0.5f);
	}
	void FadeAllChild(Transform parent, float duration){
		foreach(Transform tr in parent){
			Image img = tr.GetComponent<Image>();
			if(img){
				img.CrossFadeColor(new Color(255,255,255,0), duration, false,true);
			}
			FadeAllChild(tr,duration);
		}
	}
}
