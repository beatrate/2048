using UnityEngine;

[CreateAssetMenu]
public class ColorProfile : ScriptableObject
{
	[SerializeField]
	private IntColorDictionary textColors;

	public Color GetColorOf(int val)
	{
		return textColors.ContainsKey(val) ? textColors[val] : Color.cyan;
	}
}