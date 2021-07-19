using UnityEngine;

public class FollowTarget : SingletonMonoBehavior<FollowTarget>
{
    Transform target;
    public Vector3 offset;
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    void LateUpdate()
    {
        if (target == null)
            return;
        var newPos = target.position + offset;
        newPos.y = transform.position.y; // y만 내 원래  위치의 y
        transform.position = newPos;

    }
}
