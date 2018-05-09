using System;
using System.Text;
using System.Collections.Generic;

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

	public void MakeMove(Direction direction)
	{
		Move(direction);
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

	private void Move(Direction direction)
	{
		switch(direction)
		{
			case Direction.Left:
				CollapseHorizontal(0, Dimension, 1);
				ShiftHorizontal(0, Dimension, 1);
				break;
			case Direction.Right:
				CollapseHorizontal(Dimension - 1, -1, -1);
				ShiftHorizontal(Dimension - 1, -1, -1);
				break;
			case Direction.Up:
				CollapseVertical(0, Dimension, 1);
				ShiftVertical(0, Dimension, 1);
				break;
			case Direction.Down:
				CollapseVertical(Dimension - 1, -1, -1);
				ShiftVertical(Dimension - 1, -1, -1);
				break;
		}
	}

	private void CollapseVertical(int start, int end, int step)
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

	private void CollapseHorizontal(int start, int end, int step)
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

	private void ShiftVertical(int start, int end, int step)
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
					field[edge, column] = field[row, column];
					field[row, column] = 0;
					edge += step;
				}
			}
		}
	}

	private void ShiftHorizontal(int start, int end, int step)
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
					field[row, edge] = field[row, column];
					field[row, column] = 0;
					edge += step;
				}

			}
		}
	}

	private void CollapseRight()
	{
		for(int row = 0; row < Dimension; ++row)
		{
			int last = -1;
			for(int column = Dimension - 1; column >= 0; --column)
			{
				if(field[row, column] == 0)
				{
					continue;
				}
				if(last == -1)
				{
					last = column;
				}
				else if(field[row, last] == field[row, column])
				{
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

	private void ShiftRight()
	{
		for(int row = 0; row < Dimension; ++row)
		{
			int rightmost = -1;
			for(int column = Dimension - 1; column >= 0; --column)
			{
				if(rightmost == -1)
				{
					if(field[row, column] == 0)
					{
						rightmost = column;
					}
					continue;
				}
				if((field[row, column] != 0) && (rightmost != -1))
				{
					field[row, rightmost] = field[row, column];
					field[row, column] = 0;
					rightmost = column == 0 ? -1 : rightmost - 1;
				}

			}
		}
	}
}