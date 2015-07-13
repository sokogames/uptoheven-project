using UnityEngine;
using System.Collections;

public enum ActionType{readyForJump,jump}; 
public class Action{

	public ActionType type;
	public MovingDirection jumpDirection;
	public float actionTime;

	public Action(ActionType type, float actionTime, MovingDirection direction = MovingDirection.forward){

		this.type = type;
		this.jumpDirection = direction;
		this.actionTime = actionTime;

	}
}
