using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Walkable,
    Water,
}
public class BlockInfo : MonoBehaviour
{
    public BlockType blockType;

    Vector3 downMousePosition;
    public float clickDistance = 1; //스크린에서 최소 1 이동했다는 의미로
    private void OnMouseDown()
    {
        downMousePosition = Input.mousePosition;
    }
    private void OnMouseUp()
    {
        var upMousePosition = Input.mousePosition;
        if (Vector3.Distance(downMousePosition , upMousePosition)> clickDistance)// 1보다 작으면 클릭이 아니라는뜻
        {
            Debug.Log($"downMousePosition : {downMousePosition }" + $"upMousePosition : {upMousePosition }");
            return; // 막힌듯! 이거 아니다!
        }
        GroundManager.Instance.OnTouch(transform.position); 
    }
}
