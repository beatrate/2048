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

	public int Score
	{
		get
		{
			return score;
		}
		set
		{
			score = value;
			text.text = value.ToString();
			Recolor(value);
		}
	}
	private int score;

	private void Recolor(int currentScore)
	{
		text.color = textColors.GetColorOf(currentScore);
		renderer.color = cellColors.GetColorOf(currentScore);
	}
}