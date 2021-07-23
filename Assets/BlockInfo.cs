using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum BlockType // 4. 샘플에서 추출 - 우리가 정한 것- - 갈 수 있는 지역과 없는 지역 구분하기 위해 
{ 
    // 비트연산 - > 메모리가 겹치지 않게 하기 위해 , 중복으로 선택이 가능하다. 
    None  =       0,
    Player =      1<<0,
    Monster =   1 << 1,
    Walkable =  1 << 2,
    Water =       1 << 3, // water 프리팹 다 이걸로 지정, 
}
public class BlockInfo : MonoBehaviour // 갈 수 있는지역, 없는 지역을 나누기 위한 스크립트 -
{
    public BlockType blockType; //자기가 어떤 블럭인지 알 수 있는

    Vector3 downMousePosition;
    public float clickDistance = 1; //스크린에서 최소 1 이동했다는 의미로

    //이동을 할 때 -> 다운과 업의 위치가 차이 없거나 아주 작을 때만 클릭한걸로 간주

    void OnMouseDown()
    {
        downMousePosition = Input.mousePosition;
    }
    void OnMouseUp()
    {
        var upMousePosition = Input.mousePosition;
        if (Vector3.Distance(downMousePosition, upMousePosition) > clickDistance)// 1보다 작으면 클릭이 아니라는뜻
        {
            return;
        }
        Player.SelectPlayer.Ontouch(transform.position);
    }
    private void SelectPlayer()
    {

        Player player = (Player)actor;

        if (Player.CompleteTurn)
        {
            CenterNotifyUI.Instance;


        }
    
    }





    string debugTextPrefab = "DebugTextPrefab";
    GameObject debugTextGos;
    internal void UpdateDebugInfo()
    {
        if (debugTextGos == null)
        {
            GameObject textMeshGo = Instantiate((GameObject)Resources.Load(debugTextPrefab), transform);
            debugTextGos = textMeshGo;
            textMeshGo.transform.localPosition = Vector3.zero;
        }

        StringBuilder debugText = new StringBuilder();// $"{item.blockType}:{intPos.y}";
                                                      //ContaingText(debugText, item, BlockType.Walkable);
        ContaingText(debugText, BlockType.Water);
        ContaingText(debugText, BlockType.Player);
        ContaingText(debugText, BlockType.Monster);

        GetComponentInChildren<TextMesh>().text = debugText.ToString();
    }
    private void ContaingText(StringBuilder sb, BlockType walkable)
    {
        if (blockType.HasFlag(walkable))
        {
            sb.AppendLine(walkable.ToString());
        }
    }
