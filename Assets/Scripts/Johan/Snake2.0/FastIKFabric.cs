using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class FastIKFabric : MonoBehaviour
{
    public int ChainLenght = 1;

    public Transform Target;

    public int Iterations = 10;

    public float Delta = 0.001f;
    public Transform Pole;
    protected float[] BonesLength;
    protected float CompleteLength;
    protected Transform[] Bones;
    protected Vector3[] Positions;
    [Range(0, 1)] public float SnapBackStrenght = 1f;

    protected Vector3[] StartDirectionSucc;

    protected Quaternion[] StartRotationBone;

    protected Quaternion StartRotationTarget;

    protected Quaternion StartRotationRoot;
    // Start is called before the first frame update

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ResolveIK();
    }

    void Init()
    {
        Bones = new Transform[ChainLenght + 1];
        Positions = new Vector3[ChainLenght + 1];
        BonesLength = new float[ChainLenght];
        StartDirectionSucc = new Vector3[ChainLenght + 1];
        StartRotationBone = new Quaternion[ChainLenght + 1];
        

        if (Target == null)
        {
            Target = new GameObject(gameObject.name + "Target").transform;
            Target.position = transform.position;
        }
        CompleteLength = 0;
        StartRotationTarget = Target.rotation;

        var current = transform;
        for (var i = Bones.Length - 1; i >= 0; i--)
        {
            Bones[i] = current;
            if (i == Bones.Length - 1)
            {
                StartDirectionSucc[i] = Target.position - current.position;
            }
            else
            {
                StartDirectionSucc[i] = Bones[i + 1].position - current.position;
                BonesLength[i] = StartDirectionSucc[i].magnitude;
                CompleteLength += BonesLength[i];
            }
            current = current.parent; 
        }
    }
    void ResolveIK()
    {
        if (Target == null) return;
        if (BonesLength.Length !=ChainLenght) Init();
        for (int i = 0; i < Bones.Length; i++) 
            Positions[i] = Bones[i].position;
        var RootRot = (Bones[0].parent != null) ? Bones[0].parent.rotation : Quaternion.identity;
        var RootRotDiff = RootRot * Quaternion.Inverse(StartRotationRoot);

        if ((Target.position - Bones[0].position).sqrMagnitude >= CompleteLength * CompleteLength)
        {
            var direction = (Target.position - Positions[0].normalized);
            for (int i = 1; i < Positions.Length; i++)
                Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
        }
        else
        {
            for (int i = 0; i < Positions.Length - 1; i++)
                Positions[i + 1] = Vector3.Lerp(Positions[i + 1], Positions[i] + RootRotDiff * StartDirectionSucc[i],
                    SnapBackStrenght);
            for (int iteration = 0; iteration < Iterations; iteration++)
            {
                //Backward
                for (int i = Positions.Length - 1; i > 0; i--)
                {
                    if (i == Positions.Length - 1) 
                        Positions[i] = Target.position;
                    else
                    {
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i];
                    }
                }
                //forward
                for (int i = 1; i < Positions.Length; i ++)
                    Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i-1];
                if((Positions[Positions.Length -1]- Target.position).sqrMagnitude < Delta * Delta) break;
            }
        }
        //Move toward the pole
        if (Pole != null)
        {
            for (int i = 1; i < Positions.Length - 1; i++)
            {
                var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(Pole.position);
                var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1],
                    plane.normal);
                Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) +
                               Positions[i - 1];
                
            }
        }

        for (int i = 0; i < Positions.Length; i++)
        {
            if (i == Positions.Length - 1)
                Bones[i].rotation = Target.rotation * Quaternion.Inverse(StartRotationTarget) * StartRotationBone[i];
            else
            {
                Bones[i].rotation = Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) *
                                    StartRotationBone[i];
                Bones[i].position = Positions[i];
            }
        }
    }

    
}
