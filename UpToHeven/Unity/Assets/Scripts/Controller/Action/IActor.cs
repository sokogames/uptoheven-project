using UnityEngine;
using System.Collections;

public interface IActor {

	void Jump();
	void ReadyForJump(MovingDirection direction);
}
