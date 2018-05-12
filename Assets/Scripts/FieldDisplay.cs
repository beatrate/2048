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
	}

	public void HandleMove(List<PositionChange> changes)
	{
		foreach(PositionChange change in changes)
		{
			if(change.CollapsePoint != null)
			{

			}
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