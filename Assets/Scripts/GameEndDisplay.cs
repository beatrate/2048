using UnityEngine;
using DG.DeInspektor.Attributes;

public class GameEndDisplay : MonoBehaviour
{
	[SerializeField]
	private string gameOverMessage;
	[SerializeField]
	private string winMessage;
	[SerializeField]
	[DeEmptyAlert]
	private TMPro.TextMeshProUGUI textMesh;

	public void HandleGameOver()
	{
		textMesh.text = gameOverMessage;
		gameObject.SetActive(true);
	}

	public void HandleWin()
	{
		textMesh.text = winMessage;
		gameObject.SetActive(true);
	}
}