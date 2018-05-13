using UnityEngine;

/// <summary>
/// Description of cell movement.
/// </summary>
public class PositionChange
{
	/// <summary>
	/// Where the move started.
	/// </summary>
	public Vector2Int Start { get; set; }
	/// <summary>
	/// Position of merged cell.
	/// </summary>
	public Vector2Int? CollapsePoint { get; set; }
	/// <summary>
	/// Where the move stopped.
	/// </summary>
	public Vector2Int End { get; set; }

	public PositionChange(Vector2Int start, Vector2Int end, Vector2Int? collapsePoint = null)
	{
		Start = start;
		End = end;
		CollapsePoint = collapsePoint;
	}

	public override string ToString()
	{
		return $"start:{Start} collapse:{CollapsePoint} end:{End}";
	}
}