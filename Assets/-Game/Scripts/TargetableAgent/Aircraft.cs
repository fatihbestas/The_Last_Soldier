using System.Collections;
using UnityEngine;

public class Aircraft : TargetableAgent
{
    public override void TakeDamage(float damage, Vector3 hitPoint)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamageByLaser()
    {
        throw new System.NotImplementedException();
    }

    protected override void Death()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Move()
    {
        throw new System.NotImplementedException();
    }

    public override void MoveAgain()
    {
        throw new System.NotImplementedException();
    }

    protected override void Stop()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsHuman()
    {
        throw new System.NotImplementedException();
    }

    protected override void LevelEnd(bool isLevelPassed)
    {
        throw new System.NotImplementedException();
        // idle
        // if health < 0 return
    }

    protected override void LevelStart()
    {
        gameObject.SetActive(false);
    }
}
