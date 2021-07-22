using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    static public Player SelectPlayer;
    Animator animator;
     void Start()
    {
        SelectPlayer = this;
        animator = GetComponentInChildren<Animator>();
        GroundManager.Instance.AddBlockInfo(transform.position, BlockType.Player);
    }
    public void PlayAnimation(string nodName)
    {
        animator.Play(nodName, 0, 0);
    }

    internal void OnTouch(Vector3 position)
    {
        Vector2Int findPos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        FindPath(findPos);
    }

    void FindPath(Vector2Int goalPos)
    {
        StopAllCouroutines();
        StartCourtine();
    }
}
