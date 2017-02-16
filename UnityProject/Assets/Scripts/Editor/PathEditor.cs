using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(Path), true)]
public class PathEditor : Editor
{
	[MenuItem("GameObject/New Path", false, 1)]
	public static void MakeNewPathGameObject()
	{
		GameObject GO = new GameObject("Path");
		GO.AddComponent<Path>();
	}
    
	private Path path;

	private SerializedProperty nodesArrayProp;

	void OnEnable()
	{
		path = (Path)target;

		nodesArrayProp = serializedObject.FindProperty("nodes");

		if(nodesArrayProp.arraySize < 4)
		{
			nodesArrayProp.arraySize = 4;

			nodesArrayProp.GetArrayElementAtIndex(0).FindPropertyRelative("position").vector3Value = new Vector3(0f, 1f, 0f);
			nodesArrayProp.GetArrayElementAtIndex(1).FindPropertyRelative("position").vector3Value = new Vector3(1f, 2f, 0f);
			nodesArrayProp.GetArrayElementAtIndex(2).FindPropertyRelative("position").vector3Value = new Vector3(-1f, 4f, 0f);
			nodesArrayProp.GetArrayElementAtIndex(3).FindPropertyRelative("position").vector3Value = new Vector3(0f, 5f, 0f);

			serializedObject.ApplyModifiedProperties();
		}
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GUI.enabled = false;
		EditorGUILayout.IntField("Number of segments", (nodesArrayProp.arraySize - 1) / 3);
		GUI.enabled = true;

		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();

		if(GUILayout.Button("Add"))
		{
			nodesArrayProp.arraySize += 3;

			//Position of the end point of the path
			Vector3 end = nodesArrayProp.GetArrayElementAtIndex(nodesArrayProp.arraySize - 4).FindPropertyRelative("position").vector3Value;

			Vector3 endForward = path.GetNormal(path.length);

			nodesArrayProp.GetArrayElementAtIndex(nodesArrayProp.arraySize - 3).FindPropertyRelative("position").vector3Value = end + endForward * 1f;
			nodesArrayProp.GetArrayElementAtIndex(nodesArrayProp.arraySize - 2).FindPropertyRelative("position").vector3Value = end + endForward * 3f;
			nodesArrayProp.GetArrayElementAtIndex(nodesArrayProp.arraySize - 1).FindPropertyRelative("position").vector3Value = end + endForward * 4f;
		}
		if(nodesArrayProp.arraySize <= 4)
		{
			GUI.enabled = false;
		}
		if(GUILayout.Button("Remove"))
		{
			nodesArrayProp.arraySize -= 3;
		}
		GUI.enabled = true;

		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
	}

	void OnSceneGUI()
	{
		int i = 0;
		foreach(SerializedProperty nodeProp in nodesArrayProp)
		{
			SerializedProperty posProp = nodeProp.FindPropertyRelative("position");

			Vector3 worldPos = path.transform.TransformPoint(posProp.vector3Value);
			Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);
			if(newWorldPos != worldPos)
			{
				//It moved, revalidate the path

				Vector3 newLocalpos = path.transform.InverseTransformPoint(newWorldPos);

				posProp.vector3Value = newLocalpos;

				if(serializedObject.FindProperty("forceSmooth").boolValue)
				{
					DoForceSmoothToNode(i, newLocalpos);
				}

				if(serializedObject.FindProperty("forceSmoothSpeed").boolValue)
				{
					DoForceSpeedToNode(i, newLocalpos);
                }

				//Use reflection to call Validate
				typeof(Path).GetMethod("Validate", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(path, null);
			}

			i++;
		}

		serializedObject.ApplyModifiedProperties();
	}

	private void DoForceSmoothToNode(int nodeId, Vector3 newLocalpos)
	{
		//Check to see if this node is a handle node (eg the curve might not go directly through this point)
		if(!IsNodeHandle(nodeId))
		{
			return;
		}

		//Get the matching node on the other side of the position node (if it exists)
		int pairedID = GetPairedHandleNodeFromHandleNode(nodeId);
		if(pairedID == -1)
		{
			return;
		}

		//Get the position of the position node
		int positionID = GetPositionNodeFromHandleNode(nodeId);
		SerializedProperty positionNode = nodesArrayProp.GetArrayElementAtIndex(positionID);
		Vector3 posNodePos = positionNode.FindPropertyRelative("position").vector3Value;

		//Get the other nodes current position and normal
		SerializedProperty handleNode = nodesArrayProp.GetArrayElementAtIndex(pairedID);
		Vector3 handleNodePos = handleNode.FindPropertyRelative("position").vector3Value;
		Vector3 handleNodeNormal = handleNodePos - posNodePos;

		//Get the new normal
		Vector3 normal = Vector3.Normalize(newLocalpos - posNodePos);

		//Rotate the other node so that it has an opposite normal
		Vector3 handleNodeNewNormal = -normal * handleNodeNormal.magnitude;

		//Apply the normal to the nodes position
		handleNode.FindPropertyRelative("position").vector3Value = handleNodeNewNormal + posNodePos;
	}

	private void DoForceSpeedToNode(int nodeId, Vector3 newLocalpos)
	{
		//Check to see if this node is a handle node (eg the curve might not go directly through this point)
		if(!IsNodeHandle(nodeId))
		{
			return;
		}

		//Get the matching node on the other side of the position node (if it exists)
		int pairedID = GetPairedHandleNodeFromHandleNode(nodeId);
		if(pairedID == -1)
		{
			return;
		}

		//Get the position of the position node
		int positionID = GetPositionNodeFromHandleNode(nodeId);
		SerializedProperty positionNode = nodesArrayProp.GetArrayElementAtIndex(positionID);
		Vector3 posNodePos = positionNode.FindPropertyRelative("position").vector3Value;

		//Get the other nodes current position and normal
		SerializedProperty handleNode = nodesArrayProp.GetArrayElementAtIndex(pairedID);
		Vector3 handleNodePos = handleNode.FindPropertyRelative("position").vector3Value;
		Vector3 handleNodeNormal = handleNodePos - posNodePos;

		//Get the new speed
		float speed = Vector3.Magnitude(newLocalpos - posNodePos);

		//Rotate the other node so that it has an opposite normal
		Vector3 handleNodeNewNormal = handleNodeNormal.normalized * speed;

		//Apply the normal to the nodes position
		handleNode.FindPropertyRelative("position").vector3Value = handleNodeNewNormal + posNodePos;
	}

	private bool IsNodeHandle(int nodeId)
	{
		return nodeId % 3 != 0;
    }

	private int GetPairedHandleNodeFromHandleNode(int handleId)
	{
		int localId = handleId % 3;
		if(localId == 0)
		{
			Debug.LogError("nodeID is not a handle!");
			return -1;
		}
		int otherHandleID = localId == 1 ? handleId - 2 : handleId + 2;
		if(otherHandleID > 0 && otherHandleID < nodesArrayProp.arraySize)
		{
			return otherHandleID;
		}
		
		//Node has no pair. It is the first or last handle
		return -1;
	}

	private int GetPositionNodeFromHandleNode(int handleId)
	{
		int localId = handleId % 3;
		if(localId == 0)
		{
			Debug.LogError("nodeID is not a handle!");
			return -1;
		}
		
		return localId == 1 ? handleId - 1 : handleId + 1;
	}
}
