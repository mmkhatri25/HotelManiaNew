using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	public static PoolManager Instance;

	public Transform poolContainer;

	[SerializeField]
	private Dictionary<int, Queue<GameObject>> pool = new Dictionary<int, Queue<GameObject>>();

	[Header("Prefabs without reference in objects")]
	public GameObject coinParticles;

	public GameObject siren;

	public int numberOfCoinParticles;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(this);
		}
		AddInstancesToPool(coinParticles, numberOfCoinParticles);
		AddInstancesToPool(siren, 1);
	}

	public void AddInstancesToPool(GameObject prefab, int totalNumber)
	{
		for (int i = 0; i < totalNumber; i++)
		{
			AddInstanceToPool(prefab);
			print("here i am instantiate - "+ i);
		}
	}

	private void AddInstanceToPool(GameObject prefab, Transform parent = null)
	{
		int instanceID = prefab.GetInstanceID();
		prefab.SetActive(value: false);
		prefab.SetActive(value: false);
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
		prefab.SetActive(value: true);
		gameObject.name = prefab.name;
		if (parent != null)
		{
			gameObject.transform.parent = parent;
		}
		else
		{
			gameObject.transform.parent = poolContainer;
		}
		if (!pool.ContainsKey(instanceID))
		{
			pool.Add(instanceID, new Queue<GameObject>());
		}
		pool[instanceID].Enqueue(gameObject);
	}

	private GameObject GetPooledInstance(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
	{
		int instanceID = prefab.GetInstanceID();
		GameObject gameObject = null;
		if (pool.ContainsKey(instanceID) && pool[instanceID].Count != 0)
		{
			if (!pool[instanceID].Peek().activeSelf)
			{
				gameObject = pool[instanceID].Dequeue();
				pool[instanceID].Enqueue(gameObject);
			}
			else
			{
				AddInstanceToPool(prefab);
				for (int i = 0; i < pool[instanceID].Count - 1; i++)
				{
					pool[instanceID].Enqueue(pool[instanceID].Dequeue());
				}
				gameObject = pool[instanceID].Dequeue();
				pool[instanceID].Enqueue(gameObject);
			}
		}
		else
		{
			AddInstanceToPool(prefab);
			gameObject = pool[instanceID].Dequeue();
			pool[instanceID].Enqueue(gameObject);
		}
		gameObject.transform.position = position;
		gameObject.transform.rotation = rotation;
		gameObject.SetActive(value: true);
		return gameObject;
	}

	public GameObject InstantiatePooled(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return GetPooledInstance(prefab, position, rotation);
	}

	public GameObject InstantiatePooled(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
	{
		GameObject pooledInstance = GetPooledInstance(prefab, position, rotation, parent);
		pooledInstance.transform.SetParent(parent, worldPositionStays: true);
		return pooledInstance;
	}

	public void ObjectBackToPool(GameObject obj)
	{
		obj.transform.SetParent(poolContainer, worldPositionStays: false);
		obj.transform.position = Vector3.zero;
		obj.SetActive(value: false);
	}
}
