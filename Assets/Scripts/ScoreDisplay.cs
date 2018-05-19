using System;
using UnityEngine;
using DG.DeInspektor.Attributes;

public class ScoreDisplay : MonoBehaviour
{
	[SerializeField]
	[DeEmptyAlert]
	private TMPro.TextMeshProUGUI textMesh;

	private void Start()
	{
		HandleScore(0);
	}

	public void HandleScore(int score)
	{
		textMesh.text = score.ToString();
	}
}