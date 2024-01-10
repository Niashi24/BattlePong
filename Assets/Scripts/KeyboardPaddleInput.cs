using UnityEngine;

namespace DefaultNamespace
{
	public class KeyboardPaddleInput : PaddleInput
	{
		public override float XDir => Input.GetAxisRaw("Horizontal");
		public override bool Special => Input.GetKey(KeyCode.Space);
	}
}