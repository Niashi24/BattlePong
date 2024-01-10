using System;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour, IBallTrigger
{
	public event Action<RaycastHit2D> OnScore;

	// private void OnCollisionEnter2D(Collision2D col)
	// {
	// 	if (col.gameObject.CompareTag("Ball"))
	// 		OnScore?.Invoke(col);
	// 	// Debug.Log(col.relativeVelocity);
	// }

	public (Vector2, Vector2) HitBall(BallScript ballScript, RaycastHit2D hit2D, Vector2 position, Vector2 direction)
	{
		OnScore?.Invoke(hit2D);
		return (position, direction);
	}
}