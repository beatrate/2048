using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.DeInspektor.Attributes;
using DG.Tweening;

public class FieldDisplay : MonoBehaviour
{
	[SerializeField]
	private int width;
	[SerializeField]
	private int height;
	[SerializeField]
	private float elementSize;
	[SerializeField]
	private float transitionLength;
	[SerializeField]
	private float popupStrength;
	[SerializeField]
	private float popupLength;
	[SerializeField]
	[HideInInspector]
	private Vector2 start;
	[SerializeField]
	[DeEmptyAlert]
	private GameObject cellPrefab;
	[SerializeField]
	[DeEmptyAlert]
	private GameObject activeCellPrefab;
	[SerializeField]
	[DeEmptyAlert]
	private Transform activeParent;
	[SerializeField]
	[DeEmptyAlert]
	private Transform backgroundParent;
	private Cell[,] cells;

	private void OnValidate()
	{
		SetStart();
	}

	private void Awake()
	{
		cells = new Cell[width, height];
	}

	public void Clear()
	{
		for(int x = 0; x < width; ++x)
		{
			for(int y = 0; y < height; ++y)
			{
				if(cells[x, y] == null)
				{
					continue;
				}
				Destroy(cells[x, y].gameObject);
				cells[x, y] = null;
			}
		}
	}

	public void Add(int x, int y, int score)
	{
		GameObject cellObject = Instantiate(activeCellPrefab, PositionOf(x, y), activeCellPrefab.transform.rotation, activeParent);
		cellObject.SetActive(false);
		Cell cell = cellObject.GetComponent<Cell>();
		cell.Score = score;
		cell.Redraw();
		cells[x, y] = cell;
		cellObject.transform.DOPunchScale(new Vector3(popupStrength, popupStrength, popupStrength), popupLength, 1, 0)
			.SetDelay(transitionLength)
			.OnStart(() => cellObject.SetActive(true));
	}

	public void Add(CellProfile cellProfile)
	{
		Add(cellProfile.Position.x, cellProfile.Position.y, cellProfile.Score);
	}

	public void HandleMove(List<PositionChange> changes)
	{
		foreach(PositionChange change in changes)
		{
			Debug.Assert(cells[change.Start.x, change.Start.y] != null, $"Cell {change.Start} doesnt exist in display\n Current state {ToString()}");

			Vector2 endPosition = PositionOf(change.End);
			Cell moved = cells[change.Start.x, change.Start.y];
			// Simple move to an empty cell.
			if(cells[change.End.x, change.End.y] == null)
			{
				cells[change.End.x, change.End.y] = moved;
				cells[change.Start.x, change.Start.y] = null;
				moved.transform.DOLocalMove(endPosition, transitionLength);
			}
			else
			{
				// Move with 2 cells merging.
				Cell receiver = cells[change.End.x, change.End.y];
				receiver.Score *= 2;
				cells[change.Start.x, change.Start.y] = null;
				moved.transform.DOLocalMove(endPosition, transitionLength).OnComplete(() =>
				{
					receiver.Redraw();
					Destroy(moved.gameObject);
				});
			}
		}
	}

	public override string ToString()
	{
		StringBuilder value = new StringBuilder();
		value.Append("FieldDisplay\n");
		for(int y = 0; y < height; ++y)
		{
			value.Append("{");
			for(int x = 0; x < width; ++x)
			{
				value.Append(cells[x, y] != null ? cells[x, y].Score.ToString() : "null");
				if(x != width - 1)
				{
					value.Append(" ");
				}
			}
			value.Append("}");
			if(y != height - 1)
			{
				value.Append(", \n");
			}
		}

		return value.ToString();
	}

	[DeMethodButton(mode = DeButtonMode.NoPlayMode)]
	private void Setup()
	{
		SetStart();
		Reset();
		if(cellPrefab == null)
		{
			return;
		}
		for(int x = 0; x < width; ++x)
		{
			for(int y = 0; y < height; ++y)
			{
				GameObject child = PrefabUtility.InstantiatePrefab(cellPrefab) as GameObject;
				child.transform.parent = backgroundParent;
				child.transform.localPosition = PositionOf(x, y);
			}
		}
	}

	[DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
	private void LogState()
	{
		Debug.Log(ToString());
	}

	[DeMethodButton(mode = DeButtonMode.NoPlayMode)]
	private void Reset()
	{
		while(backgroundParent.childCount != 0)
		{
			DestroyImmediate(backgroundParent.GetChild(0).gameObject);
		}
	}


	private Vector2 PositionOf(int x, int y)
	{
		return new Vector2(start.x + elementSize * x, start.y - elementSize * y);
	}

	private Vector2 PositionOf(Vector2Int fieldPosition)
	{
		return PositionOf(fieldPosition.x, fieldPosition.y);
	}

	private void SetStart()
	{
		start = new Vector2(
			-elementSize * (width / 2 - (width % 2 == 0 ? 0.5f : 0)),
			elementSize * (height / 2 - (height % 2 == 0 ? 0.5f : 0)));
	}
}