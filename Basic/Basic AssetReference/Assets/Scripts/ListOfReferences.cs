﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ListOfReferences : MonoBehaviour {

	public List<AssetReference> shapes;

	bool m_IsReady = false;
	int m_ToLoadCount;

	int currentIndex = 0;
	// Use this for initialization
	void Start ()
	{
		m_ToLoadCount = shapes.Count;
		foreach (var shape in shapes)
		{
			shape.LoadAssetAsync<GameObject>().Completed += OnShapeLoaded;
		}
	}

	void OnShapeLoaded(AsyncOperationHandle<GameObject> obj)
	{
		m_ToLoadCount--;
		if (m_ToLoadCount <= 0)
			m_IsReady = true;
	}

	public void SpawnAThing()
	{
		Debug.Log("ListOfReferences::SpawnAThing called (Instantiate)");
		if (m_IsReady && shapes[currentIndex].Asset != null)
		{
			for(int count = 0; count <= currentIndex; count++)
				GameObject.Instantiate(shapes[currentIndex].Asset);
			currentIndex++;
			if (currentIndex >= shapes.Count)
				currentIndex = 0;

		}
	}

	void OnDestroy()
	{
		Debug.Log("ListOfReferences::OnDestroy called (ReleaseAsset)");
		foreach (var shape in shapes)
		{
			// Debug.Log("shape.ReleaseAsset()");
			shape.ReleaseAsset();
		}
	}
}
