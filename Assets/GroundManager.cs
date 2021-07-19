using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GroundManager : SingletonMonoBehavior<GroundManager>
{
    public Vector2Int playerPos;
    //public Vector2Int goalPos;
    public Dictionary<Vector2Int, int> map = new Dictionary<Vector2Int, int>();

    internal void OnTouch(Vector3 position)
    {
        Vector2Int findPos = new Vector2Int((int)position.x, (int)position.z);
        FindPath(findPos);
        ;
    }

    public List<int> passableValues = new List<int>();// 갈 수 있는 지역

    public Transform player;

    void FindPath(Vector2Int goalPos)
    {
        StopAllCoroutines();
        StartCoroutine(FindPathCo(goalPos));
    }
    public Transform goal;

    //[ContextMenu("길찾기 테스트")]

    IEnumerator FindPathCo(Vector2Int goalPos)
    {
        passableValues = new List<int>();
        passableValues.Add((int)BlockType.Walkable);

        //자식의 모든 블록인포 찾자
        var blockInfos = GetComponentsInChildren<BlockInfo>(); //s 안붙여서 오류남, blockinfo 찾을수 없다고

        //맵을 채워넣자
        foreach (var item in blockInfos)
        {
            var pos = item.transform.position;
            Vector2Int inPos = new Vector2Int((int)pos.x, (int)pos.z);
            map[inPos] = (int)item.blockType;

        }
        playerPos.x = (int)player.position.x;
        playerPos.y = (int)player.position.z;


        List<Vector2Int> path = PathFinding2D.find4(playerPos, goalPos, map, passableValues);
        if (path.Count == 0)
            Debug.Log("길이 없다");
        else
        {
            Player.SelectPlayer.PlayAnimation("Walk");
            FollowTarget.Instance.SetTarget(Player.SelectPlayer.transform);
            foreach (var item in path)
            {
                Vector3 playerNewPos = new Vector3(item.x, 0, item.y);
                player.LookAt(playerNewPos);
                //player.position = playerNewPos;
                player.DOMove(playerNewPos, moveTImePerUnit).SetEase(moveEase);
                yield return new WaitForSeconds(moveTImePerUnit);
            }
            Player.SelectPlayer.PlayAnimation("Idle");
            FollowTarget.Instance.SetTarget(null);
        }
    }
       public Ease moveEase = Ease.InBounce;
    public float moveTImePerUnit = 0.3f;

    }

