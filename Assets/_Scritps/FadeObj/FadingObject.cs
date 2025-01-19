using System;
using UnityEngine;

public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
{
    [HideInInspector]
    public float InitialAlpha;

    private Material mat;
    public Material Material => mat;
    private Vector3 postion;

    private void Awake()
    {
        postion  = transform.position;
        mat = GetComponent<MeshRenderer>().material;
        InitialAlpha = mat.color.a;
    }

    public bool Equals(FadingObject other)
    {
        return postion.Equals(other.postion);
    }
}
