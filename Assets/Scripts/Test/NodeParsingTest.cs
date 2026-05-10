using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

[RequireComponent(typeof(MeshFilter))]
public class NodeParsingTest: MonoBehaviour {
    [SerializeField] private TextAsset _text;
    [SerializeField, Range(1, 20)] private int _layerCnt = 5;
    [SerializeField] private bool _fillColor = false;
    private Node _root;
    
    private void Awake() { 
        
        var factory = new NodeFactory();
        _root = factory.Generate(_text.text.Split('\n'));
        GetComponent<MeshFilter>().mesh = MakeMesh(_root);
    }

    private Vector3 pos = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            pos = Camera.main.transform.position;
            direction = Camera.main.transform.forward * 100;
            Node.Boxes.Clear();
            var result = _root.IsThrough(new() { Origin = pos, Length = direction });
            Debug.Log(result);
        }
        Debug.DrawRay(pos, direction, Color.red);
    }

    private Mesh MakeMesh(Node pNode) {
        var vertices = GetTriangles(pNode).ToArray();
        var triangles = Enumerable.Range(0, vertices.Length / 3)
            .SelectMany(v => new[]{3 * v, 3 * v + 1, 3 * v + 2})
            .ToArray();
        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    } 

    private List<(Vector3, Vector3)> GetBox(Node pNode, int pMaxDeep, int pDeep = 0) {
        var list = new List<(Vector3, Vector3)>();
        if (pMaxDeep <= pDeep) return list;
        
        if (pNode.Triangle == null) {
            var center = (pNode.AABBMax + pNode.AABBMin) / 2;
            var length = center - pNode.AABBMin;
            list.Add(new(center, length));
        }
        if (pNode.Left != null) 
            list.AddRange(GetBox(pNode.Left, pMaxDeep, pDeep + 1));
        if(pNode.Right != null)
            list.AddRange(GetBox(pNode.Right, pMaxDeep, pDeep + 1));
        return list;
    }

    private List<Vector3> GetTriangles(Node pNode) {
        var list = new List<Vector3>();
        if (pNode.Triangle != null) 
            list.AddRange(pNode.Triangle);
        if (pNode.Left != null) 
            list.AddRange(GetTriangles(pNode.Left));
        if(pNode.Right != null)
            list.AddRange(GetTriangles(pNode.Right));
        return list;
    }
    
    private void OnDrawGizmos() {
        if (_root == null) return;
        foreach (var box in Node.Boxes /*GetBox(_root, _layerCnt)*/) {
            var r = new Random((int)((box.Item1.x * box.Item2.x + box.Item1.z * box.Item2.z + box.Item1.z * box.Item2.z) *1000));
            var color = new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
            Gizmos.color = color;
            if(_fillColor)
                Gizmos.DrawCube(box.Item1, box.Item2 * 2);
            else
                Gizmos.DrawWireCube(box.Item1, box.Item2 * 2);
        }
    }
}