using System.Collections.Generic;
using UnityEngine;

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

	private Field field;
	private readonly Reaction[] reactions = {
		new Reaction(KeyCode.W, Direction.Up), new Reaction(KeyCode.S, Direction.Down),
		new Reaction(KeyCode.A, Direction.Left), new Reaction(KeyCode.D, Direction.Right)
	};

	private void Awake()
	{
		field = new Field(new int[,]{
			{ 0, 2, 0, 2 },
			{ 4, 4, 0, 2 },
			{ 0, 2, 0, 0 },
			{ 32, 16, 32, 16 }
		});
		Debug.Log(field);
	}

	private void Update()
	{
		foreach(Reaction reaction in reactions)
		{
			if(Input.GetKeyDown(reaction.Input))
			{
				field.MakeMove(reaction.Result);
				Debug.Log(field);
				break;
			}
		}
	}
}
