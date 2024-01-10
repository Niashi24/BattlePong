using UnityEngine;

public abstract class PaddleInput : MonoBehaviour
{
	public abstract float XDir { get; }
	
	public abstract bool Special { get;  }
}