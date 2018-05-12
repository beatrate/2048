using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
	private const int Dimension = 4;
	private int[,] field = new int[Dimension, Dimension];

	public Field()
	{

	}

	public Field(int[,] fieldSetup)
	{
		field = fieldSetup;
	}

	public int this[int x, int y]
	{
		get
		{
			return field[y, x];
		}
		set
		{
			field[y, x] = value;
		}
	}

	public List<PositionChange> MakeMove(Direction direction)
	{
		List<PositionChange> changes = new List<PositionChange>();
		switch(direction)
		{
			case Direction.Left:
				CollapseHorizontal(changes, 0, Dimension, 1);
				ShiftHorizontal(changes, 0, Dimension, 1);
				break;
			case Direction.Right:
				CollapseHorizontal(changes, Dimension - 1, -1, -1);
				ShiftHorizontal(changes, Dimension - 1, -1, -1);
				break;
			case Direction.Up:
				CollapseVertical(changes, 0, Dimension, 1);
				ShiftVertical(changes, 0, Dimension, 1);
				break;
			case Direction.Down:
				CollapseVertical(changes, Dimension - 1, -1, -1);
				ShiftVertical(changes, Dimension - 1, -1, -1);
				break;
		}
		return changes;
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
				value.Append(", ");
			}
		}

		return value.ToString();
	}

	private void CollapseVertical(List<PositionChange> changes, int start, int end, int step)
	{
		for(int column = 0; column < Dimension; ++column)
		{
			int last = -1;
			for(int row = start; row != end; row += step)
			{
				if(field[row, column] == 0)
				{
					continue;
				}
				if((last != -1) && (field[last, column] == field[row, column]))
				{
					changes.Add(new PositionChange(new Vector2Int(column, row), new Vector2Int(column, last), new Vector2Int(column, last)));
					field[last, column] *= 2;
					field[row, column] = 0;
					last = -1;
				}
				else
				{
					last = row;
				}
			}
		}
	}

	private void CollapseHorizontal(List<PositionChange> changes, int start, int end, int step)
	{
		for(int row = 0; row < Dimension; ++row)
		{
			int last = -1;
			for(int column = start; column != end; column += step)
			{
				if(field[row, column] == 0)
				{
					continue;
				}
				if((last != -1) && (field[row, last] == field[row, column]))
				{
					changes.Add(new PositionChange(new Vector2Int(column, row), new Vector2Int(last, row), new Vector2Int(last, row)));
					field[row, last] *= 2;
					field[row, column] = 0;
					last = -1;
				}
				else
				{
					last = column;
				}
			}
		}
	}

	private void ShiftVertical(List<PositionChange> changes, int start, int end, int step)
	{
		for(int column = 0; column < Dimension; ++column)
		{
			int edge = -1;
			for(int row = start; row != end; row += step)
			{
				if(edge == -1)
				{
					if(field[row, column] == 0)
					{
						edge = row;
					}
					continue;
				}
				if((field[row, column] != 0) && (edge != -1))
				{
					PositionChange changeWithCollapse = changes.Find(change => change.End.x == row && change.End.y == column);
					if(changeWithCollapse != null)
					{
						changeWithCollapse.End.Set(edge, column);
					}
					else
					{
						changes.Add(new PositionChange(new Vector2Int(row, column), new Vector2Int(edge, column)));
					}
					field[edge, column] = field[row, column];
					field[row, column] = 0;
					edge += step;
				}
			}
		}
	}

	private void ShiftHorizontal(List<PositionChange> changes, int start, int end, int step)
	{
		for(int row = 0; row < Dimension; ++row)
		{
			int edge = -1;
			for(int column = start; column != end; column += step)
			{
				if(edge == -1)
				{
					if(field[row, column] == 0)
					{
						edge = column;
					}
					continue;
				}
				if((field[row, column] != 0) && (edge != -1))
				{
					PositionChange changeWithCollapse = changes.Find(change => change.End.x == row && change.End.y == column);
					if(changeWithCollapse != null)
					{
						changeWithCollapse.End.Set(row, edge);
					}
					else
					{
						changes.Add(new PositionChange(new Vector2Int(row, column), new Vector2Int(row, edge)));
					}
					field[row, edge] = field[row, column];
					field[row, column] = 0;
					edge += step;
				}

			}
		}
	}
}