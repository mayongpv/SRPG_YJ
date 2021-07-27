using UnityEngine;

public class StageManager : MonoBehaviour
{
    public enum GameStateType
    {
        NotInit,
        SelectPlayer,               // 조정할 아군 선택, 선택된 플레이어가 갈 수 있는 영역과 공격 가능한 영역 표시.
        SelectedPlayerMoveOrAct,     // 선택된 플레이어가 이동하거나 행동을 할 차례
        IngPlayerMove,              // 플레이어 이동중
        SelectToAttackTarget,       // 이동후에 공격할 타겟을 선택. 공격할 타겟이 없다면 SelectPlayer로 변경
        AttackToTarget,
        // 모든 플레이어 선택했다면 MonsterTurn을 진행 시킨다.
        MonsterTurn,
    }

    public class StageManager : SingletonMonoBehavior<StageManager>
    {



        [SerializeField] GameStateType m_gmaeState;
        get => Instance.m_gameState;
        set {
            Debug.Log($"{Instance.m_gameState} => {value}");
            NotifyUI.Instance.Show(value.ToString(), 10);
            Instance.m_gameState = value;
        }

    public void Start()
    {
        OnStartTurn();
    }
    private void Update()
    {
        int turn = 1;
        private void ProcessNextTurn()
        {
            //// 턴 정보 초기화 해주자.

            turn++;

            OnStartTurn();
        }

        private void OnStartTurn()
        {
            FollowTarget.Instance.SetTarget(Player.Players[0].transform);

            // 몇번째 턴인지 보여주자.
            ShowCurrentTurn();

            // 게임상태를 SelectPlayer
            GameState = GameStateType.SelectPlayer;
        }


        private static void OnStartTurn()
        {
            FollowTarget.Instance.SetTarget(Player.players[0].transform);

            //몇번째 턴인지 보여주자
            ShowCurrentTurn();

            GameState = GameStateType.SelectPlayer;

        }
        private void ShowCurrentTurn()
        {
            CenterNotifyUI.Instance.Show($"{turn}이 시작되었습니다");
        }
    }
