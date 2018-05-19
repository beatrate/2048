using UnityEngine;

/// <summary>
/// Description of cell movement.
/// </summary>
public class PositionChange
{
	/// <summary>
	/// Where the move started.
	/// </summary>
	public Vector2Int Start { get; set; } = Vector2Int.zero;
	/// <summary>
	/// Where the move stopped.
	/// </summary>
	public Vector2Int End { get; set; } = Vector2Int.zero;

	public PositionChange()
	{

	}

	public PositionChange(Vector2Int start, Vector2Int end)
	{
		Start = start;
		End = end;
	}

	public override string ToString()
	{
		return $"start:{Start} end:{End}";
	}
}