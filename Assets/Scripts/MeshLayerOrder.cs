using UnityEngine;

public class MeshLayerOrder : MonoBehaviour
{
	private RequireComponent MeshRenderer;

	private MeshRenderer meshRenderer;

	public int layer;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.sortingOrder = layer;
	}

	private void OnEnble()
	{
		meshRenderer.sortingOrder = layer;
	}

	public void SetSortingOrder(int newLayer)
	{
		meshRenderer.sortingOrder = newLayer;
	}
}
