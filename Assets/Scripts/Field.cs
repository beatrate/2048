using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
	private const int WinScore = 2048;
	private const int Dimension = 4;
	private const float ChanceOf4 = 0.1f;

	private int[,] field = new int[Dimension, Dimension];

	public Field()
	{

	}

	public Field(int[,] fieldPreset)
	{
		field = fieldPreset;
	}

	public int this[int x, int y] => field[y, x];
	public bool Won { get; private set; } = false;
	public bool HasMoves { get; private set; } = true;
	public int Score { get; private set; } = 0;

	public void Clear()
	{
		Array.Clear(field, 0, field.Length);
		Won = false;
		HasMoves = true;
		Score = 0;
	}

	public List<PositionChange> MakeMove(Direction direction)
	{
		switch(direction)
		{
			case Direction.Left:
				return MoveHorizontal(0, Dimension, 1);
			case Direction.Right:
				return MoveHorizontal(Dimension - 1, -1, -1);
			case Direction.Up:
				return MoveVertical(0, Dimension, 1);
			case Direction.Down:
				return MoveVertical(Dimension - 1, -1, -1);
			default:
				throw new NotImplementedException(direction.ToString());
		}
	}

	public CellProfile AddRandom()
	{
		List<Vector2Int> empty = FindEmpty();
		if(empty.Count == 0)
		{
			throw new InvalidOperationException("Cannot spawn cells in full Field");
		}
		float random = UnityEngine.Random.value;
		Vector2Int coordinates = empty[UnityEngine.Random.Range(0, empty.Count - 1)];
		int score = random < ChanceOf4 ? 4 : 2;
		field[coordinates.y, coordinates.x] = score;

		if(empty.Count == 1)
		{
			HasMoves = false;
			for(int y = 0; y < Dimension; ++y)
			{
				for(int x = 0; x < Dimension; ++x)
				{
					if(HasEqualNeighbours(x, y))
					{
						HasMoves = true;
						break;
					}
				}
			}
		}
		return new CellProfile(coordinates, score);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if((obj == null) || (GetType() != obj.GetType()))
		{
			return false;
		}
		Field other = (Field)obj;
		for(int i = 0; i < Dimension; ++i)
		{
			for(int j = 0; j < Dimension; ++j)
			{
				if(field[i, j] != other.field[i, j])
				{
					return false;
				}
			}
		}
		return true;
	}

	public override string ToString()
	{
		StringBuilder value = new StringBuilder();
		value.Append("Field\n");
		for(int row = 0; row < Dimension; ++row)
		{
			value.Append("{");
			for(int column = 0; column < Dimension; ++column)
			{
				value.Append(field[row, column]);
				if(column != Dimension - 1)
				{
					value.Append(" ");
				}
			}
			value.Append("}");
			if(row != Dimension - 1)
			{
				value.Append(", \n");
			}
		}

		return value.ToString();
	}

	private List<Vector2Int> FindEmpty()
	{
		List<Vector2Int> empty = new List<Vector2Int>();
		for(int y = 0; y < Dimension; ++y)
		{
			for(int x = 0; x < Dimension; ++x)
			{
				if(field[y, x] == 0)
				{
					empty.Add(new Vector2Int(x, y));
				}
			}
		}
		return empty;
	}

	private bool HasEqualNeighbours(int x, int y)
	{
		int score = field[y, x];
		if((x > 0) && (field[y, x - 1] == score))
		{
			return true;
		}
		if((x < Dimension - 1) && (field[y, x + 1] == score))
		{
			return true;
		}
		if((y > 0) && (field[y - 1, x] == score))
		{
			return true;
		}
		if((y < Dimension - 1) && (field[y + 1, x] == score))
		{
			return true;
		}
		return false;
	}

	private List<PositionChange> MoveVertical(int start, int end, int step)
	{
		List<PositionChange> changes = new List<PositionChange>();
		for(int column = 0; column < Dimension; ++column)
		{
			int edgeRow = -1;
			int lastScore = -1;
			for(int row = start; row != end; row += step)
			{
				if(field[row, column] == 0)
				{
					if(edgeRow == -1)
					{
						edgeRow = row;
					}
					continue;
				}

				int currentRow = row;
				int destinationRow = -1;
				if(edgeRow != -1)
				{
					destinationRow = edgeRow;
					currentRow = edgeRow;
					field[edgeRow, column] = field[row, column];
					field[row, column] = 0;
					edgeRow += step;
				}
				if(field[currentRow, column] == lastScore)
				{
					destinationRow = currentRow - step;

					field[currentRow - step, column] *= 2;
					int cellScore = field[currentRow - step, column];
					Score += cellScore;
					if(cellScore == WinScore)
					{
						Won = true;
					}
					field[currentRow, column] = 0;

					edgeRow = currentRow;
					lastScore = -1;
				}
				else
				{
					lastScore = field[destinationRow == -1 ? row : destinationRow, column];
				}

				if(destinationRow != -1)
				{
					changes.Add(new PositionChange(new Vector2Int(column, row), new Vector2Int(column, destinationRow)));
				}
			}
		}

		return changes;
	}

	private List<PositionChange> MoveHorizontal(int start, int end, int step)
	{
		List<PositionChange> changes = new List<PositionChange>();
		for(int row = 0; row < Dimension; ++row)
		{
			int edgeColumn = -1;
			int lastScore = -1;
			for(int column = start; column != end; column += step)
			{
				if(field[row, column] == 0)
				{
					if(edgeColumn == -1)
					{
						edgeColumn = column;
					}
					continue;
				}

				int currentColumn = column;
				int destinationColumn = -1;
				if(edgeColumn != -1)
				{
					destinationColumn = edgeColumn;
					currentColumn = edgeColumn;
					field[row, edgeColumn] = field[row, column];
					field[row, column] = 0;
					edgeColumn += step;
				}
				if(field[row, currentColumn] == lastScore)
				{
					destinationColumn = currentColumn - step;

					field[row, currentColumn - step] *= 2;
					int cellScore = field[row, currentColumn - step];
					Score += cellScore;
					if(cellScore == WinScore)
					{
						Won = true;
					}
					field[row, currentColumn] = 0;

					edgeColumn = currentColumn;
					lastScore = -1;
				}
				else
				{
					lastScore = field[row, destinationColumn == -1 ? column : destinationColumn];
				}

				if(destinationColumn != -1)
				{
					changes.Add(new PositionChange(new Vector2Int(column, row), new Vector2Int(destinationColumn, row)));
				}
			}
		}

		return changes;
	}
}