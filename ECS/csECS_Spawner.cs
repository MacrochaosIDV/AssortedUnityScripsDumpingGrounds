using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class csECS_Spawner : MonoBehaviour
{
  #region PrivateData
  [Range(100U, 10000U)]
  [SerializeField] public int m_count;
  private int m_sphrCount;
  public float m_sphrSpeed;
  
  [SerializeField] private GameObject m_prefab;

  private static EntityManager em;
  private EntityArchetype m_sphrArcheType;

  private GameObject[] m_arrSpheres;
  private NativeArray<Entity> m_nArrEntities;
  private float3[] m_targetPosition;
  private int[] m_relationIndex; 
  private EntityQuery m_sphrQuery;

  EntityQuery sphrQry;
  #endregion

  void Start()
  {
    #region Start
    m_sphrCount = m_count;
    float3[] spherePosition = new float3[m_sphrCount];
    m_targetPosition = new float3[m_sphrCount];
    m_relationIndex = new int[m_sphrCount];
    m_arrSpheres = new GameObject[m_sphrCount];

    em = World.Active.EntityManager;
    //rm = new RenderMesh() {mesh = m_prefab.GetComponent<MeshFilter>().sharedMesh, 
    //                       material = m_prefab.GetComponent<MeshRenderer>().sharedMaterial, 
    //                       castShadows = UnityEngine.Rendering.ShadowCastingMode.On, 
    //                       receiveShadows = true};

    m_sphrArcheType = em.CreateArchetype(typeof(Translation), 
                                         typeof(TargetPosition), 
                                         typeof(ScalarSpeed), 
                                         typeof(seekBoid));

    for (int i = 0; i < m_sphrCount; ++i) {
      Entity ent = em.CreateEntity(m_sphrArcheType);
      spherePosition[i] = new float3(UnityEngine.Random.Range(-1.0f, 2.0f),
                                     UnityEngine.Random.Range(-1.0f, 2.0f),
                                     UnityEngine.Random.Range(-1.0f, 2.0f) * i * 0.1f);
      m_relationIndex[i] = (int)UnityEngine.Random.Range(0, i + 1);
      m_targetPosition[i] = spherePosition[m_relationIndex[i]];

      em.SetComponentData(ent, new Translation() { Value = spherePosition[i] });
      em.SetComponentData(ent, new TargetPosition() { Value = m_targetPosition[i] });
      em.SetComponentData(ent, new ScalarSpeed() { Value = m_sphrSpeed });

      //em.SetSharedComponentData(ent, rm);
      m_arrSpheres[i] = Instantiate(m_prefab, 
                                    (float3)transform.position + spherePosition[i], 
                                    Quaternion.identity, 
                                    transform);
    }
    #endregion
  }

  void Update()
  {
    /// Entity Method
    m_sphrQuery = em.CreateEntityQuery(typeof(seekBoid));
    NativeArray<Entity> nEnts = m_sphrQuery.ToEntityArray(Allocator.TempJob);
    for (int i = 0; i < nEnts.Length; ++i) {

      m_targetPosition[i] = em.GetComponentData<Translation>(nEnts[i]).Value;
      em.SetComponentData(nEnts[i], 
                          new TargetPosition() { 
                            Value = m_targetPosition[m_relationIndex[i]] });
      m_arrSpheres[i].transform.position = m_targetPosition[m_relationIndex[i]];
    }
    nEnts.Dispose();
    m_sphrQuery.Dispose();
  }
}
