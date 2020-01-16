using System.Collections;
using System.Collections.Generic;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;

public class csECS_SeekBall : JobComponentSystem
{
  [BurstCompile]
  struct seekBallJobSys : IJobForEach<Translation, TargetPosition, ScalarSpeed, seekBoid>
  {
    [ReadOnly]
    public float deltaT;
    public void Execute(ref Translation _translation,
                        [ReadOnly] ref TargetPosition _target,
                        [ReadOnly] ref  ScalarSpeed _speed,
                        [ReadOnly] ref seekBoid sboid) {
      _translation.Value += (_target.Value - _translation.Value) * _speed.Value * deltaT;
    }
  }

  protected override JobHandle OnUpdate(JobHandle inputDependencies) {
    seekBallJobSys job = new seekBallJobSys();
    job.deltaT = UnityEngine.Time.deltaTime;
    return job.Schedule(this, inputDependencies);
  }
}
