


// 원래 코드.

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
// using UnityEngine.UI;

// public class AddObject : MonoBehaviour {
//
//     public string addressToAdd;
//     Button m_AddButton;
//
//     void Start()
//     {
//         
//         m_AddButton = GetComponent<Button>();
//         m_AddButton.onClick.AddListener(OnButtonClick);
//     }
//
//     void OnButtonClick()
//     {
//         // Addressables.ReleaseInstance()
//         var randSpot = new Vector3(Random.Range(-5, 1), Random.Range(-10, 10), Random.Range(0, 100));
//         var sss = Addressables.InstantiateAsync("ball", randSpot, Quaternion.identity);
//     }
// }






using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddObject : MonoBehaviour {

	enum eAssetAPIType
	{
		Addressables_Instantiate,
		Addressables_LoadAssetAsync,
		AssetRef_InstantiateAsync,
		AssetRef_LoadAssetAsync,
	}
	public string addressToAdd;
	public AssetReference _assetReference;
	
	Button m_AddButton;
	bool m_ReadyToLoad = true;
	private AsyncOperationHandle<GameObject> asyncOperationHandle;
	private eAssetAPIType type = eAssetAPIType.Addressables_LoadAssetAsync;
	private GameObject obj;
	private GameObject objInstantiate;
	
	void Start()
	{
        
		m_AddButton = GetComponent<Button>();
		m_AddButton.onClick.AddListener(OnButtonClick);
	}

	void OnButtonClick()
	{
		var randSpot = new Vector3(Random.Range(-5, 1), Random.Range(-10, 10), Random.Range(0, 100));
		switch (type)
		{
			case eAssetAPIType.Addressables_Instantiate:
			{
				Debug.Log("Addressables.InstantiateAsync case");
				if (m_ReadyToLoad)
				{
					asyncOperationHandle = Addressables.InstantiateAsync("ball", randSpot, Quaternion.identity);
					m_ReadyToLoad = false;
					Debug.Log("Addressables.InstantiateAsync After");
				}
				else
				{
					Addressables.ReleaseInstance(asyncOperationHandle);
					m_ReadyToLoad = true;
					Debug.Log("Addressables.ReleaseInstance After");
				}
			}
				break;

			case eAssetAPIType.Addressables_LoadAssetAsync:
			{
				Debug.Log("Addressables_LoadAssetAsync case");
				if (m_ReadyToLoad)
				{
					Addressables.LoadAssetAsync<GameObject>(_assetReference).Completed += handle =>
					{
						if (handle.Status == AsyncOperationStatus.Succeeded)
						{
							asyncOperationHandle = handle;
							m_ReadyToLoad = false;
							obj = handle.Result;
							objInstantiate = GameObject.Instantiate(obj, randSpot, Quaternion.identity);
							
							Debug.Log($"handle.Result : ${handle.Result}");
							Debug.Log($"obj : ${obj}");
							Debug.Log($"objInstantiate : ${objInstantiate}");
							Debug.Log("Addressables.LoadAssetAsync Succeeded");
						}
					};
					
					Debug.Log("Addressables.LoadAssetAsync After");
				}
				else
				{
					// succeed 
					Addressables.Release(asyncOperationHandle);
					
					// Addressables.LoadAssetsAsync
					// warn, error ??
					// Addressables.Release(obj);
					// Addressables.Release(objInstantiate);
					// Addressables.Release(_assetReference);
					// _assetReference.ReleaseAsset();
					
					GameObject.Destroy(objInstantiate);
					objInstantiate = null;
					m_ReadyToLoad = true;
					Debug.Log("Addressables.Release After");
				}
			}
				break;

			case eAssetAPIType.AssetRef_InstantiateAsync:
			{
				Debug.Log("AssetRef_InstantiateAsync case");
			}
				break;
			
			case eAssetAPIType.AssetRef_LoadAssetAsync:
			{
				Debug.Log("AssetRef_LoadAssetAsync case");
			}
				break;
			
			default:
				break;
		}

	}
}