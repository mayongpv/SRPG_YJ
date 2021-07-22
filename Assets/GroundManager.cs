using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : SingletonMonoBehavior<GroundManager> //싱글턴으로 가져옴
{
    public Vector2Int playerPos; //player 위치 
    public Dictionary<Vector2Int, BlockType> map = new Dictionary<Vector2Int, BlockType>(); //A*에서 사용
    public Dictionary<Vector2Int, BlockInfo> blockInfoMap = new Dictionary<Vector2Int, BlockInfo>();
    public BlockType passbleValues = BlockType.Walkable | BlockType.Water; // | 한개만 넣으면 두개 다 사용한다는 의미 (비트 연산에서)
    public Transform player;

    public bool useDebugMode = true;
    public GameObject debugTextPrefab;
    new private void Awake()
    {
        base.Awake();

        //자신의 모든 BlockInfo 찾자
        var blockInfos = GetComponentInChildren<BlockInfo>();

        debugTextGos.ForEach(x => Destroy(x));
        // 블럭에 기존에 있던 디버그용 텍스트 삭제
        debugTextGos.Clear();

        foreach (var item in blockInfos) //뭐터랑 워커블을 이용해서 맵을 채움. 순회하며 돌기 위해 foreach 사용
        {
            var pos = item.transform.position; //아이템들의 위치값 x랑 z 값을 쓸거임
            Vector2Int intPos = new Vector2Int((int)pos.x, (int)pos.z); //(int) 써준거는 강제로 변환해준 것)
            map[intPos] = item.blockType; //맵에 
                                          //intPos가 KEY가 된다. 딕셔너리 : 앞부분키, 뒷부분 value <= 이 땅의 속성, 즉 블록타입 / 이걸 int형으로 넣어줌
                                          //이렇게 해서 Map 정보 초기화가 끝났다. 

            if (useDebugMode)
            {
                item.UpadateDebufInfo();
            }
            blockInfoMap[intPos] = item;


        }
    }
    public List<GameObject> debugTextGos = new List<GameObject>();

    public void AddBlockInfo(Vector3 position, BlockType addBlockType, Actor actor)
    {
        Vector2Int pos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        if (map.ContainsKey(pos) == false)
        {
            Debug.LogError($"{ pos} 위치에 맵이 없다");

        }
        map[pos] |= addBlockType;
        blockInfoMap[pos].blockType |= addBlockType;
        if (useDebugMode)
            blockInfoMap[pos].UpdateDebufInfo.info();
    }

        internal void OnTouch(Vector3 position, BlockType addBlcokType)
    {
        Vector2Int pos = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        if (map.ContainsKey(pos) == false)
        {
            Debug.LogError($"{pos}위치에 맵이 없다");
        }
        map[pos] |= addBlcokType;
        blockInfoMap[pos].blockType |= addBlcokType;
        if (useDebugMode)
            blockInfoMap[pos].UpdateDebufInfo();
    }

    public void RemoveBlockinfo(Vector3 position, BlockType removeBlockType)
    {
        Vector2Int pos = new Vector2Int(Mathf.RoundToInt(position.x),
                Mathf.RoundToInt(position.z));
        if (map.ContainsKey(pos) == false)
        {
            Debug.LogError($"위치에 맵이 없다.");
        }
        map[pos] &= ~removeBlockType;
        //blockInfoMap[pos].blockType &= 


    }

    void FindPath(Vector2Int goalPos)
    {
        StopAllCoroutines();
        StartCoroutine(FindPathCo(goalPos));
    }


    IEnumerator FindPathCo(Vector2Int goalPos)
    {

        playerPos.x = Mathf.RoundToInt(player.position.x);
        playerPos.y = Mathf.RoundToInt(player.position.z); // 따로 써준 이유 : 우리가 쓰려는 Vecor2 값에 x, y만 있어서 y값에 z 값을 넣어줄거임


        List<Vector2Int> path = PathFinding2D.find4(playerPos, goalPos, map, passableValues); //2.
        // Player에서 goal까지 찾겠다. / 맵 - 맵에대한 위치값, 위치, 갈수 있는지의 유무 등의 정보가 담김, / 패스 ; 갈 수 있는 지역
        if (path.Count == 0) // path를 이용해 player가 이동하는 코드 작성
            Debug.Log("길이 없다");
        else
        {
            RemoveBlockInfo(player.SelectPlayer.transform.position, BlockType.Player);
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
            //이동한 위치에는 플레이어 정보 추가
            AddBlockInfo(Player.SelectPlayer.transform.position, BlockType.Player);
        }
    }
    public Ease moveEase = Ease.InBounce;
    public float moveTimePerUnit = 0.3f;

  

        } 

