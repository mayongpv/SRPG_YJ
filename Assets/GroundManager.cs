using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GroundManager : SingletonMonoBehavior<GroundManager>
{
    public Vector2Int playerPos; //player 위치 
    public Dictionary<Vector2Int, int> map = new Dictionary<Vector2Int, int>();
    public List<int> passableValues = new List<int>();// 갈 수 있는 지역 //1.
    public Transform player;

    //context 메뉴 ; 코르틴 실행 안됨 

    internal void OnTouch(Vector3 position)
    {
        Vector2Int findPos = new Vector2Int((int)position.x, (int)position.z);
        FindPath(findPos);
    }


    void FindPath(Vector2Int goalPos)
    {
        StopAllCoroutines();
        StartCoroutine(FindPathCo(goalPos));
    }

    IEnumerator FindPathCo(Vector2Int goalPos)
    {
        passableValues = new List<int>();
        passableValues.Add((int)BlockType.Walkable); //3. 샘플에서 값 넣는거 그대로 함

        //자식의 모든 블록인포 찾자
        var blockInfos = GetComponentsInChildren<BlockInfo>(); // 자식들에서 블록 정보를 찾는다 
        //s 안붙여서 오류남, blockinfo 찾을수 없다고

        //맵을 채워넣자
        foreach (var item in blockInfos) //뭐터랑 워커블을 이용해서 맵을 채움. 순회하며 돌기 위해 foreach 사용
        {
            var pos = item.transform.position; //아이템들의 위치값 x랑 z 값을 쓸거임
            Vector2Int intPos = new Vector2Int((int) pos.x, (int)pos.z); //(int) 써준거는 강제로 변환해준 것)
            map[intPos] = (int)item.blockType; //맵에 
            //intPos가 KEY가 된다. 딕셔너리 : 앞부분키, 뒷부분 value <= 이 땅의 속성, 즉 블록타입 / 이걸 int형으로 넣어줌
            //이렇게 해서 Map 정보 초기화가 끝났다. 
        }
        playerPos.x = (int)player.position.x;
        playerPos.y = (int)player.position.z; // 따로 써준 이유 : 우리가 쓰려는 Vecor2 값에 x, y만 있어서 y값에 z 값을 넣어줄거임


        List<Vector2Int> path = PathFinding2D.find4(playerPos, goalPos, map, passableValues); //2.
        // Player에서 goal까지 찾겠다. / 맵 - 맵에대한 위치값, 위치, 갈수 있는지의 유무 등의 정보가 담김, / 패스 ; 갈 수 있는 지역
        if (path.Count == 0) // path를 이용해 player가 이동하는 코드 작성
            Debug.Log("길이 없다");
        else
        {
            Player.SelectPlayer.PlayAnimation("Walk");
            FollowTarget.Instance.SetTarget(Player.SelectPlayer.transform);
            foreach (var item in path)//path에는 길에 대한 정보만 있어서 x,y 값으로 플레이어가 갈 길 표시
            {
                Vector3 playerNewPos = new Vector3(item.x, 0, item.y); //좌표를 변환한 것
                player.LookAt(playerNewPos); 
                //player.position = playerNewPos; // 좌표를 원래 플레이어한테 할당해줬었음 
                player.DOMove(playerNewPos, moveTimePerUnit).SetEase(moveEase);
                yield return new WaitForSeconds(moveTimePerUnit); 
            }
            Player.SelectPlayer.PlayAnimation("Idle");
            FollowTarget.Instance.SetTarget(null);
        }
    }
       public Ease moveEase = Ease.InBounce;
    public float moveTimePerUnit = 0.3f;

    }

