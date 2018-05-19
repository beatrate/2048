using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.DeInspektor.Attributes;

public class FieldWrapper : MonoBehaviour
{
	public UnityEvent OnGameOver;
	public UnityEvent OnWin;
	public IntEvent OnScoreChanged;

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
		field = new Field();
		StartGame();
	}

	private void Update()
	{
		if(field.Won || !field.HasMoves)
		{
			return;
		}
		foreach(Reaction reaction in reactions)
		{
			if(Input.GetKeyDown(reaction.Input))
			{
				Move(reaction.Result);
				break;
			}
		}
	}

	public void StartGame()
	{
		field.Clear();
		display.Clear();
		AddRandomCell();
		AddRandomCell();
		OnScoreChanged.Invoke(0);
	}

	[DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
	private void LogState()
	{
		Debug.Log(field.ToString());
	}


	private void AddRandomCell()
	{
		display.Add(field.AddRandom());
	}

	private void Move(Direction direction)
	{
		if(field.Won || !field.HasMoves)
		{
			return;
		}
		int previousScore = field.Score;
		List<PositionChange> changes = field.MakeMove(direction);
		if(field.Score != previousScore)
		{
			OnScoreChanged.Invoke(field.Score);
		}

		if(changes.Count != 0)
		{
			display.HandleMove(changes);
			AddRandomCell();
		}

		if(field.Won)
		{
			Debug.Log("Won");
			OnWin.Invoke();
		}
		else if(!field.HasMoves)
		{
			Debug.Log("Game Over");
			OnGameOver.Invoke();
		}
	}
}
