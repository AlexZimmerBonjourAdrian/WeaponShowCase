using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "bullet Trail Config", menuName = "Scriptable Object/bullet Trail Config")]
public class BulletTrailScriptableObject : ScriptableObject
{
    public AnimationCurve widthCuve;
    public float TIme = 0.5f;
    public float MinVertexDistance = 0.1f;
    public Gradient ColorGardient;
    public Material Material;
    public int ConerVertices;
    public int EndCapVerices;

    public void SetupTrail(TrailRenderer trailRender)
    {
        trailRender.widthCurve = widthCuve;
        trailRender.time = TIme;
        trailRender.minVertexDistance = MinVertexDistance;
        trailRender.colorGradient = ColorGardient;
        trailRender.sharedMaterial = Material;
        trailRender.numCornerVertices = ConerVertices;
        trailRender.numCapVertices = EndCapVerices;
    }
}
