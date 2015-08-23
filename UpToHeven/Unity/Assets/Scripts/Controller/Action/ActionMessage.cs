
public class ActionMessage{

	public TransformType type;
	public RotationType rotationType;

	public ActionMessage(TransformType type, RotationType rotationType = RotationType.ToForward){
		this.type = type;
		this.rotationType = rotationType;
	}

}
