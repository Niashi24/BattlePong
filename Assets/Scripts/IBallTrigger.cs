using UnityEngine;

public interface IBallTrigger
{
	(Vector2, Vector2) HitBall(BallScript ballScript, RaycastHit2D hit2D, Vector2 position, Vector2 direction);
}