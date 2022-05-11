using UnityEngine;
using UnityEngine.AddressableAssets;

public class SelfDestruct : MonoBehaviour {

	public float lifetime = 2f;

	void Start()
	{
		Debug.Log("SelfDestruct::Start called");
		Invoke("Release", lifetime);
		Debug.Log("SelfDestruct::Start end");
	}

	void Release()
	{
		Debug.Log("SelfDestruct::Release called");
		if (!Addressables.ReleaseInstance(gameObject))
		{
			Debug.Log("Addressables.ReleaseInstance fail (Destroy call)");
			Destroy(gameObject);
		}
		else
		{
			Debug.Log("Addressables.ReleaseInstance success");
		}
	}
}
