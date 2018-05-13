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

	[DeMethodButton(mode = DeButtonMode.NoPlayMode)]
	public void Setup()
	{
		SetStart();
		Clear();
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

	[DeMethodButton(mode = DeButtonMode.NoPlayMode)]
	public void Clear()
	{
		while(backgroundParent.childCount != 0)
		{
			DestroyImmediate(backgroundParent.GetChild(0).gameObject);
		}
	}

	public void Add(int x, int y, int score)
	{
		GameObject cell = Instantiate(activeCellPrefab, PositionOf(x, y), activeCellPrefab.transform.rotation, activeParent);
		cells[x, y] = cell.GetComponent<Cell>();
		cells[x, y].Score = score;
		cells[x, y].Redraw();
	}

	public void HandleMove(List<PositionChange> changes)
	{
		foreach(PositionChange change in changes)
		{
			Debug.Log(change);
			Debug.Assert(cells[change.Start.x, change.Start.y] != null, "Cell doesnt exist in display");

			Vector2 endPosition = PositionOf(change.End);

			if(change.CollapsePoint != null)
			{
				Debug.Assert(cells[change.CollapsePoint.Value.x, change.CollapsePoint.Value.y] != null, "Collapsed cell doesnt exist");
				Cell consumed = cells[change.Start.x, change.Start.y];
				cells[change.Start.x, change.Start.y] = null;
				consumed.transform.DOLocalMove(endPosition, transitionLength).OnComplete(() =>
				{
					Destroy(consumed.gameObject);
				});

				Cell moved = cells[change.CollapsePoint.Value.x, change.CollapsePoint.Value.y];
				moved.Score *= 2;
				if(change.CollapsePoint != change.End)
				{
					cells[change.End.x, change.End.y] = moved;
					cells[change.CollapsePoint.Value.x, change.CollapsePoint.Value.y] = null;
				}

				moved.transform.DOLocalMove(endPosition, transitionLength).OnComplete(() =>
				{
					moved.Redraw();
				});
			}
			else
			{
				Cell moved = cells[change.Start.x, change.Start.y];
				cells[change.End.x, change.End.y] = moved;
				cells[change.Start.x, change.Start.y] = null;
				moved.transform.DOLocalMove(endPosition, transitionLength);
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