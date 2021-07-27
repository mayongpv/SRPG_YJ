using UnityEngine;

public class FollowTarget : SingletonMonoBehavior<FollowTarget>
{
    Transform target;
    public Vector3 offset = new Vector3(0, 0, -7);

    public void SetTarget(Transform target)
    {
        this.target = target;
        if (target) //타겟이 있을 때
        {
            var pos = target.position;
            pos.y = transform.position.y;
            transform.position = pos;

        }
    
    }
    void LateUpdate()
    {
        if (target == null)
            return;
        var newPos = target.position + offset;
        newPos.y = transform.position.y; // y만 내 원래  위치의 y
        newPos.x = transform.position.x;
        transform.position = newPos;

    }
}
