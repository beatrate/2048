using UnityEngine;

public struct CellProfile
{
	public Vector2Int Position { get; }
	public int Score { get; }

	public CellProfile(Vector2Int position, int score)
	{
		Position = position;
		Score = score;
	}
}