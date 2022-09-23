using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    private float _shooterXPos, _targetXPos, _dist, _nextXPos, _baseYPos;

    protected override void MoveToTarget()
    {
        base.MoveToTarget();

        //Set the X positions
        _shooterXPos = Shooter.transform.position.x;
        _targetXPos = Target.transform.position.x;

        _dist = _targetXPos - _shooterXPos; //Get the distance between shooter and target
        _nextXPos = Mathf.MoveTowards(transform.position.x, _targetXPos, ProjectileSO.speed * Time.deltaTime);

        _baseYPos = Mathf.Lerp(Shooter.transform.position.y, Target.transform.position.y, (_nextXPos - _shooterXPos) / _dist);
        var height = 2 * (_nextXPos - _shooterXPos) * (_nextXPos - _targetXPos) / (-0.25f * _dist * _dist);

        Vector3 movePos = new Vector3(_nextXPos, _baseYPos + height, transform.position.z);

        transform.rotation = LookAtTarget(movePos - transform.position); //Look at the target
        transform.position = movePos; //Set the new position
    }
}
