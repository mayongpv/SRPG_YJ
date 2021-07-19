using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType // 4. 샘플에서 추출 - 우리가 정한 것- - 갈 수 있는 지역과 없는 지역 구분하기 위해 
{
    Walkable,
    Water, // water 프리팹 다 이걸로 지정
}
public class BlockInfo : MonoBehaviour // 갈 수 있는지역, 없는 지역을 나누기 위한 스크립트 -
{
    public BlockType blockType; //자기가 어떤 블럭인지 알 수 있는

    Vector3 downMousePosition;
    public float clickDistance = 1; //스크린에서 최소 1 이동했다는 의미로
     void OnMouseDown()
    {
        downMousePosition = Input.mousePosition;
    }
     void OnMouseUp()
    {
        var upMousePosition = Input.mousePosition;
        if (Vector3.Distance(downMousePosition , upMousePosition)> clickDistance)// 1보다 작으면 클릭이 아니라는뜻
        {
            return;
        }
        GroundManager.Instance.OnTouch(transform.position); 
    }
}
