using UnityEngine;
using DG.DeInspektor.Attributes;
using TMPro;

public class Cell : MonoBehaviour
{
	[SerializeField]
	[DeEmptyAlert]
	private new SpriteRenderer renderer;
	[SerializeField]
	[DeEmptyAlert]
	private TextMeshPro text;
	[SerializeField]
	[DeEmptyAlert]
	private ColorProfile textColors;
	[SerializeField]
	[DeEmptyAlert]
	private ColorProfile cellColors;

	public int Score { get; set; }

	public void Redraw()
	{
		text.text = Score.ToString();
		text.color = textColors.GetColorOf(Score);
		renderer.color = cellColors.GetColorOf(Score);
	}
}