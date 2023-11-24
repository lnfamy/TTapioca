using UnityEngine;

public class CamFollow : MonoBehaviour
{
	private double borderUp = -3.5, borderDown = -20.5, borderRight = 15.5;
	private int borderLeft = 2;
	public Transform target;
	public Vector3 offset;
	public float dampTime = .15f;
	private Vector3 velocity = Vector3.zero;
	private Transform moveTo;
	private Vector3 camPos;


	void Update ()
	{
		camPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

		Vector3 moveTo = new Vector3 (
			                 Mathf.Clamp (target.position.x + offset.x, (float)borderLeft, (float)borderRight),
			Mathf.Clamp (target.position.y + offset.y,target.position.y + offset.y,target.position.y + offset.y),
			                 Mathf.Clamp (target.position.z + offset.z, (float)borderDown, (float)borderUp)
		                 );

		transform.position = Vector3.SmoothDamp (camPos, moveTo, ref velocity, dampTime);
	

	}
}