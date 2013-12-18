using UnityEngine;
using System.Collections;

using System.Xml;
using System.Collections.Generic;

/// <summary>
/// This manager handles special GameObjects which are defined to be not
/// destroyed on scene load.
/// </summary>
public class PersistentManager : MonoBehaviour
{
	/// <summary>
	/// The config file to use as reference.
	/// </summary>
	public TextAsset ConfigFile;

	/// <summary>
	/// The scenes configuration.
	/// </summary>
	private Dictionary<string, string[]> _scenesConfig;
	
	void Start ()
	{
		// Load the config file if needed
		LoadConfigFile();
		
		// Destroy the GameObjects not needed anymore
		DestroyObjects();
		
		// Create the missing GameObjects
		CreateObjects();
		
		// Destroy the manage as it is no more needed
		Destroy(gameObject);
	}
	
	/// <summary>
	/// Loads the config file.
	/// </summary>
	private void LoadConfigFile()
	{
		_scenesConfig = new Dictionary<string, string[]>();
	
		// Load and parse the XML file
		var xmlData = new XmlDocument();
		xmlData.LoadXml(ConfigFile.text);
		
		// Get the list of scene nodes
		var sceneNodeList = xmlData.GetElementsByTagName("scene");
		
		// Loop through scenes
		 ;
		foreach(XmlNode node in sceneNodeList)
		{
			var currentSceneName = node.Attributes["name"].Value;
			
			// Fetch all objects defined in the config file
			var childList = node.ChildNodes;
			var childCount = node.ChildNodes.Count;
			var nameArray = new string[childCount];
			var i = 0;
			foreach(XmlNode child in childList)
			{
				nameArray[i] = child.Attributes["name"].Value;
				i++;
			}
			
			_scenesConfig[currentSceneName] = nameArray;
		}
	}
	
	/// <summary>
	/// Destroys the objects not needed anymore according to configuration.
	/// </summary>
	private void DestroyObjects()
	{
		// Retreive the current scene name
		var currentScene = Application.loadedLevelName;
		
		if(_scenesConfig.ContainsKey(currentScene))
		{
			// Retreive all "immortal" objects of the scene 
			DontDestroyMeOnLoad[] objects = (DontDestroyMeOnLoad[]) GameObject.FindObjectsOfType(typeof(DontDestroyMeOnLoad));
			
			// Check if they should be there
			var objectsToKeep = _scenesConfig[currentScene];
			var keepTabLength = objectsToKeep.Length;
			foreach(DontDestroyMeOnLoad current in objects)
			{
				var keepCurrent = false;
				var i = 0;
				var loop = true;
				// Loop through the config to search for the current object's name.
				// If it's found then we have to keep it on scene.
				while(i < keepTabLength && loop)
				{
					if(current.name == objectsToKeep[i])
					{
						// The object's name was found in the config, we have to skip it
						loop = false;
						keepCurrent = true;
					}
					i++;
				}
				
				if(!keepCurrent)
				{
					// Sorry bro, you're gonna die ! :-(
					Destroy(current.gameObject);
				}
			}
		}
	}
	
	/// <summary>
	/// Creates the missing objects on scene according to configuration.
	/// </summary>
	private void CreateObjects()
	{
		// Retreive the current scene name
		var currentScene = Application.loadedLevelName;
		
		if(_scenesConfig.ContainsKey(currentScene))
		{
			// The manager has config about the current scene
			// Search for missing objects
			foreach(string objectName in _scenesConfig[currentScene])
			{
				if(!GameObject.Find(objectName))
				{
					var resource = Resources.Load (objectName);
					#if UNITY_EDITOR
					// Safe check in editor only
					if(resource == null)
					{
						Debug.LogError("The asset " + objectName + " was not found in Resources directories.");
						continue;
					}
					#endif
					
					// The object is missing, create it and rename it to get rid of
					// the appended "(Clone)" suffix
					var o = GameObject.Instantiate(resource);
					o.name = objectName;
				}
			}
		}
	}

}
