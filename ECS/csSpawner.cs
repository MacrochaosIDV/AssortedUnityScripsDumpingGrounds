using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

public class csSpawner : MonoBehaviour
{
  #region PublicData
  [Range(100U, 10000U)]
  public int m_count;
  public float m_sphrSpeed;
  public GameObject m_sphere;
  #endregion

  #region PrivateData
  [SerializeField] private GameObject[] sphereArr;
  [SerializeField] private GameObject[] targetArr;
  private int m_sphrCount;
  #endregion

  void Start() {
    sphereArr = new GameObject[m_count];
    targetArr = new GameObject[m_count];
    for (uint i =0; i< m_count; ++i) {
      sphereArr[i] = Instantiate(m_sphere, transform.position + 
                       new Vector3(UnityEngine.Random.Range(-1.0f, 2.0f), 
                                   UnityEngine.Random.Range(-1.0f, 2.0f), 
                                   UnityEngine.Random.Range(-1.0f, 2.0f) * i * 0.1f), 
                                 Quaternion.identity, 
                                 transform);

      targetArr[i] = sphereArr[(int)UnityEngine.Random.Range(0, i+1)];
    }
    m_sphrCount = m_count;
  }

  void Update() {
    NativeArray<float3> I_Pos = new NativeArray<float3>(m_sphrCount, Allocator.TempJob);
    NativeArray<float3> T_Pos = new NativeArray<float3>(m_sphrCount, Allocator.TempJob);
    NativeArray<float3> O_Pos = new NativeArray<float3>(m_sphrCount, Allocator.TempJob);

    for (int i = 1; i < m_sphrCount; ++i) {
      I_Pos[i] = sphereArr[i].transform.position;
      T_Pos[i] = targetArr[i].transform.position;
    }
    
    jobSeekBall job = new jobSeekBall() {
      inPos = I_Pos,
      inTargetPos = T_Pos,
      outPos = O_Pos,
      inSpeed = m_sphrSpeed,
      deltaT = Time.deltaTime
    };
    JobHandle jH = job.Schedule(sphereArr.Length, 1);
    jH.Complete();
    

    for (int i = 1; i < m_sphrCount; ++i) {
      sphereArr[i].transform.position += arrToVec(O_Pos[i]);
    }
    I_Pos.Dispose();
    T_Pos.Dispose();
    O_Pos.Dispose();
  }

  [BurstCompile(CompileSynchronously = true)]
  public struct jobSeekBall : IJobParallelFor
  {
    [ReadOnly]
    public NativeArray<float3> inPos;

    [ReadOnly]
    public NativeArray<float3> inTargetPos;

    public NativeArray<float3> outPos;

    public float inSpeed;
    public float deltaT;
    public void Execute(int index) {
      outPos[index] = inTargetPos[index] - inPos[index];
      outPos[index] *= inSpeed * deltaT;
    }
  }

  public static Vector3 arrToVec(float3 arr) {
    return new Vector3(arr.x, arr.y, arr.z);
  }
}
