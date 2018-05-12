using System.Collections.Generic;
using UnityEngine;
using DG.DeInspektor.Attributes;

public class FieldWrapper : MonoBehaviour
{
	private struct Reaction
	{
		public KeyCode Input { get; }
		public Direction Result { get; }

		public Reaction(KeyCode input, Direction result)
		{
			Input = input;
			Result = result;
		}
	}

	[SerializeField]
	[DeEmptyAlert]
	private FieldDisplay display;
	private Field field;
	private readonly Reaction[] reactions = {
		new Reaction(KeyCode.W, Direction.Up), new Reaction(KeyCode.S, Direction.Down),
		new Reaction(KeyCode.A, Direction.Left), new Reaction(KeyCode.D, Direction.Right)
	};

	private void Start()
	{
		field = new Field(new int[,]{
			{ 0, 2, 0, 2 },
			{ 4, 4, 0, 2 },
			{ 0, 2, 0, 0 },
			{ 32, 16, 32, 16 }
		});
		Debug.Log(field);
		for(int x = 0; x < 4; ++x)
		{
			for(int y = 0; y < 4; ++y)
			{
				int score = field[x, y];
				if(score != 0)
				{
					display.Add(x, y, score);
				}
			}
		}
	}

	private void Update()
	{
		foreach(Reaction reaction in reactions)
		{
			if(Input.GetKeyDown(reaction.Input))
			{
				display.HandleMove(field.MakeMove(reaction.Result));
				Debug.Log(field);
				break;
			}
		}
	}
}
