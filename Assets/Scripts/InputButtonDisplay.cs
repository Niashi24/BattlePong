
using UnityEngine;
using UnityEngine.U2D;

public class InputButtonDisplay : MonoBehaviour
{
	[SerializeField]
	private SpriteShapeRenderer spriteShapeRenderer;

	[SerializeField]
	private LineRenderer[] outlines;

	[SerializeField]
	private Color highlightColor = Color.gray, unHighlightColor = Color.clear;

	[SerializeField]
	private Color activeOutlineColor, inactiveOutlineColor;

	public void SetHighlight(bool highlight, bool active)
	{
		foreach (var line in outlines)
			line.startColor = line.endColor = active ? activeOutlineColor : inactiveOutlineColor;
		spriteShapeRenderer.color = highlight && active ? highlightColor : unHighlightColor;
	}
}