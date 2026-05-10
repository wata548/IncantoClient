using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

public static class ExVector {
    public static float Dot(this Vector3 lhs, Vector3 rhs) =>
        lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;

    public static Vector3 Cross(this Vector3 lhs, Vector3 rhs) =>
        new(lhs.y * rhs.z - lhs.z * rhs.y,
            lhs.x * rhs.z - lhs.z * rhs.x,
            lhs.x * rhs.y - lhs.y * rhs.x);
}

public class Ray {
    public Vector3 Origin { get; set; }
    public Vector3 Length { get; set; }
}

public class Node {
    public Vector3 AABBMin { get; set; }
    public Vector3 AABBMax { get; set; }
    public Vector3[] Triangle { get; set; }
    public Node Left { get; private set; }
    public Node Right { get; private set; }
    public static List<(Vector3, Vector3)> Boxes = new();

    public void SetLeft(Node pNode) => Left = pNode;
    public void SetRight(Node pNode) => Right = pNode;

    public bool IsThrough(Ray pRay) {
        if (Triangle != null)
            return IsThroughTriangle(pRay);
        Boxes.Add(((AABBMin + AABBMax) / 2, (AABBMax - (AABBMin + AABBMax) / 2)));   
        var result = false;
        if (Left.IsThroughAABB(pRay))
            result |= Left.IsThrough(pRay);
        if (Right.IsThroughAABB(pRay))
            result |= Right.IsThrough(pRay);
        return result;
    }
    
    private bool IsThroughAABB(Ray pRay) {
        var tRange = new Vector2(AABBMin.x, AABBMax.x);
        tRange -= Vector2.one * pRay.Origin.x;
        tRange /= pRay.Length.x;
        if (pRay.Length.x < 0)
            (tRange.x, tRange.y) = (tRange.y, tRange.x);
        
        var ytRange = new Vector2(AABBMin.y, AABBMax.y);
        ytRange -= Vector2.one * pRay.Origin.y;
        ytRange /= pRay.Length.y;
        if (pRay.Length.y < 0)
            (ytRange.x, ytRange.y) = (ytRange.y, ytRange.x);

        tRange.x = Math.Max(tRange.x, ytRange.x);
        tRange.y = Math.Min(tRange.y, ytRange.y);
        if (tRange.x > tRange.y)
            return false;

        var ztRange = new Vector2(AABBMin.z, AABBMax.z);
        ztRange -= Vector2.one * pRay.Origin.z;
        ztRange /= pRay.Length.z;
        if (pRay.Length.z < 0)
            (ztRange.x, ztRange.y) = (ztRange.y, ztRange.x);
        
        tRange.x = Math.Max(tRange.x, ztRange.x);
        tRange.y = Math.Min(tRange.y, ztRange.y);
        if (tRange.x > tRange.y)
            return false;
        
        return true;
    }
    
    private bool IsThroughTriangle(Ray pRay) {
        Debug.Log("Fuck");
        if (Triangle == null)
            return false;
        var b = Triangle[2] - Triangle[0];
        var c = Triangle[1] - Triangle[0];
        var n = b.Cross(c);
        var scalar = n.Dot(Triangle[0] - pRay.Origin);
        var tConstant = n.Dot(pRay.Length);
        //check if ray parallel to triangle plane
        if (Math.Abs(tConstant) < 0.0001f)
            return false;
        
        var t = scalar / tConstant;
        var pos = pRay.Origin + t * pRay.Length;
        
        bool? result = null;
        var temp = GetCCW(Triangle[1] - Triangle[0], pos - Triangle[0]);
        result = temp;
        temp = GetCCW(Triangle[2] - Triangle[1], pos - Triangle[1]);
        if (temp != null && result != null && result != temp)
            return false;
        result = temp;
        temp = GetCCW(Triangle[0] - Triangle[2], pos - Triangle[2]);
        if (temp != null && result != null && result != temp)
            return false;
        return true;

        bool? GetCCW(Vector3 pP0, Vector3 pP1) {
            var v= pP0.Cross(pP1).Dot(n);
            return v == 0 ? null : v > 0;
        }
    }
}

public class NodeFactory {
    private const string RangePattern = @"Min: \((?<Min>.*)\), Max: \((?<Max>.*)\)";
    private const string TrianglePattern = @"Triangle: \{A: \((?<A>.*)\), B: \((?<B>.*)\), C: \((?<C>.*)\)\}";
    public Node Generate(string[] pContext) {
        var lines = pContext.Select(line => line.Trim()).ToList();
        var root = ParseNode(lines[0], out _);
        
        Stack<Action<Node>> scope = new();
        scope.Push(root.SetRight);
        scope.Push(root.SetLeft);
        foreach (var line in lines.Skip(1)) {
            if(string.IsNullOrEmpty(line))
                continue;
            var setter = scope.Pop();
            var newNode = ParseNode(line, out var isTriangle);
            if (!isTriangle) {
                scope.Push(newNode.SetRight);
                scope.Push(newNode.SetLeft);
            }

            setter!(newNode);
        }
        return root; 

        Node ParseNode(string pLine, out bool isTriangle) {
            var result = new Node();
            if (pLine.StartsWith("Min")) {
                var match = Regex.Match(pLine, RangePattern);
                var rawMin = match.Groups["Min"].Value;
                var rawMax = match.Groups["Max"].Value;
                result.AABBMin = Parse(rawMin);
                result.AABBMax = Parse(rawMax);
                isTriangle = false;
            }
            else {
                var match = Regex.Match(pLine, TrianglePattern);
                var rawA = match.Groups["A"].Value;
                var rawB = match.Groups["B"].Value;
                var rawC = match.Groups["C"].Value;
                result.Triangle = new[] { Parse(rawA), Parse(rawB), Parse(rawC) };
                isTriangle = true;
            }

            return result;

            Vector3 Parse(string pText) {
                var args = pText.Split(',')
                    .Select(float.Parse)
                    .ToList();
                return new(args[0], args[1], args[2]);
            }
        }
    }
}