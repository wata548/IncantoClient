using System;
using System.Collections.Generic;
using Extension.Test;
using UnityEngine;
using Random = System.Random;

public class ShowBVH : MonoBehaviour {
	[SerializeField] private TextAsset _asset;
	[SerializeField] private bool _wire = false;
	private Node _root;
	
	[TestMethod]
	void Start() {
		var ls = _asset.text.Split('\n');
		var idx = 0;
		var boxes = new Stack<Node>();
		_root = new Node();
		De(_root, ls[idx++]);
		_root.l = new();
		_root.r = new();
		boxes.Push(_root.l); 
		boxes.Push(_root.r);
        
		while (boxes.Count > 0) {
			var t = boxes.Pop();
			if (De(t, ls[idx++])) {
				t.l = new();
				t.r = new();
				boxes.Push(t.l);
				boxes.Push(t.r);
			}
		}

		bool De(Node pNode, string pCon) {
			if (pCon[0] == '^') {
				var vs = pCon[1..].Split('|');
				pNode.A = DeV(vs[0].Split(", "));
				pNode.B = DeV(vs[1].Split(", "));
				pNode.C = DeV(vs[2].Split(", "));
				return false;
			}

			var temp = pCon.Split('|');
			pNode.Min = DeV(temp[0].Split(", "));
			pNode.Max = DeV(temp[1].Split(", "));
			return true;
		}

		Vector3 DeV(string[] pCons) =>
			new(float.Parse(pCons[0]), float.Parse(pCons[1]), float.Parse(pCons[2]));
		
	}

	private void OnDrawGizmos() {
		if (_root == null)
			Start();
		foreach (var (min, max) in _root.GetAABB()) {
			var center = (min + max) / 2 + transform.position;
			var size = max - min;
			var r = new Random((int)((center.x + 82 *center.y + 53546 * center.z) * size.x * 1000));
			var c = new Color((float)r.NextDouble() % 1, (float)r.NextDouble() % 1, (float)r.NextDouble() % 1);
			Gizmos.color = c;
			if(_wire)
				Gizmos.DrawWireCube(center, size);
			else 
				Gizmos.DrawCube(center, size);
		}   
	}
}


public class Node {
	public Node l = null, r = null;
	public Vector3 Min, Max;
	public Vector3 A, B, C;

	public List<(Vector3, Vector3)> GetAABB() {
		var result = new List<(Vector3, Vector3)>();
		if (l == null)
			return result;
		result.Add((Min, Max));
		result.AddRange(l.GetAABB());
		result.AddRange(r.GetAABB());
		return result;
	}
}