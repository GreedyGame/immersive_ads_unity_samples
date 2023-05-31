using UnityEngine;
using System.Collections.Generic;
namespace PubScale.SdkOne.NativeAds.Hightower
{
	[System.Serializable]
	public struct PoolObjects
	{
		public string Name;
		public GameObject Prefab;
		public int PoolSize;
	}
	public class PoolingSystem : MonoBehaviour
	{
		Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();
		static PoolingSystem _instance;
		public static PoolingSystem instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<PoolingSystem>();
				}
				return _instance;
			}
		}
		public void InitPools(PoolObjects[] ObjectPools)
		{
			foreach (PoolObjects o in ObjectPools)
				CreatePool(o.Prefab, o.PoolSize);

		}
		public void CreatePool(GameObject prefab, int poolSize)
		{
			int poolKey = prefab.GetInstanceID();

			if (!poolDictionary.ContainsKey(poolKey))
			{
				poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

				GameObject poolHolder = new GameObject(prefab.name + " pool");
				poolHolder.transform.parent = transform;

				for (int i = 0; i < poolSize; i++)
				{
					ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
					poolDictionary[poolKey].Enqueue(newObject);
					newObject.SetParent(poolHolder.transform);
				}
			}
		}

		public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			int poolKey = prefab.GetInstanceID();

			if (poolDictionary.ContainsKey(poolKey))
			{
				ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
				poolDictionary[poolKey].Enqueue(objectToReuse);
				objectToReuse.Reuse(position, rotation);
				return objectToReuse.gameObject;
			}
			return null;
		}

		public class ObjectInstance
		{

			public GameObject gameObject;
			Transform transform;

			bool hasPoolObjectComponent;
			PoolObject poolObjectScript;

			public ObjectInstance(GameObject objectInstance)
			{
				gameObject = objectInstance;
				transform = gameObject.transform;
				gameObject.SetActive(false);

				if (gameObject.GetComponent<PoolObject>())
				{
					hasPoolObjectComponent = true;
					poolObjectScript = gameObject.GetComponent<PoolObject>();
				}
			}

			public void Reuse(Vector3 position, Quaternion rotation)
			{
				gameObject.SetActive(true);
				transform.position = position;
				transform.rotation = rotation;

				if (hasPoolObjectComponent)
				{
					poolObjectScript.OnObjectReuse();
				}
			}

			public void SetParent(Transform parent)
			{
				transform.parent = parent;
			}
		}
	}
}