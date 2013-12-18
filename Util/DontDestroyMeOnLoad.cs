using UnityEngine;
using System.Collections;

/// <summary>
/// Prevents the the GameObject this script is attached to to
/// be destroyed on scene load.
/// </summary>
public class DontDestroyMeOnLoad : MonoBehaviour
{

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
