using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyableUnitController : UnitController
{
    List<Transform> _movePoints = new List<Transform>();

    protected override void UpdateMove()
    {
        if (_lockTarget == null)
            return;

        Vector3 dir = _lockTarget.transform.position - transform.position;
        dir.y = 0f;

        if (dir.magnitude <= _attackInterval && IsAttackable)
        {
            State = Define.State.Attack_To_Idle;
            return;
        }

        transform.position += dir.normalized * MoveSpeed * Time.deltaTime;
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
    }
}
