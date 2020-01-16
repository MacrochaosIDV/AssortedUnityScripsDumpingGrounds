using System.Collections;
using System.Collections.Generic;
using UnityEngine.Jobs;
using UnityEngine.Rendering;
using Unity.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
//using Unity.Rendering; 
//using Unity.Rendering.Hybrid; ///UNITY WHY U BROKE?
using Unity.Mathematics;
using UnityEngine;



[BurstCompile]
public struct seekBoid : IComponentData { }

[BurstCompile]
public struct Position3 : IComponentData
{
  public float3 Value;
}

[BurstCompile]
public struct TargetPosition : IComponentData
{
  public float3 Value;
}

[BurstCompile]
public struct Rotation : IComponentData
{
  public quaternion Value;
}

[BurstCompile]
public struct ScalarSpeed : IComponentData
{
  public float Value;
}
