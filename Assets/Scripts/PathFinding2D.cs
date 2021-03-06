using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public static class PathFinding2D
{
    /**
     * find a path in grid tilemaps
     */
    public static List<Vector2Int> find4(Vector2Int from, Vector2Int to, Dictionary<Vector2Int, BlockType> map, BlockType passableValues)
    {
        Func<Vector2Int, Vector2Int, float> getDistance = delegate (Vector2Int a, Vector2Int b)
        {
            float xDistance = Mathf.Abs(a.x - b.x);
            float yDistance = Mathf.Abs(a.y - b.y);
            return xDistance * xDistance + yDistance * yDistance;
        };
        Func<Vector2Int, List<Vector2Int>> getNeighbors = delegate (Vector2Int pos)
        {
            var neighbors = new List<Vector2Int>();
            neighbors.Add(new Vector2Int(pos.x, pos.y + 1));
            neighbors.Add(new Vector2Int(pos.x, pos.y - 1));
            neighbors.Add(new Vector2Int(pos.x + 1, pos.y));
            neighbors.Add(new Vector2Int(pos.x - 1, pos.y));
            return neighbors;
        };

        return astar(from, to, map, passableValues, getDistance, getNeighbors);
    }

    static List<Vector2Int> astar(Vector2Int from, Vector2Int to, Dictionary<Vector2Int, BlockType> map ,BlockType passableValues,
                      Func<Vector2Int, Vector2Int, float> getDistance, Func<Vector2Int, List<Vector2Int>> getNeighbors)
    {
        var result = new List<Vector2Int>();
        if (from == to)
        {
            result.Add(from);
            return result;
        }
        Node finalNode;
        List<Node> open = new List<Node>();
        if (findDest(new Node(null, from, getDistance(from, to), 0), open, map, to, out finalNode, passableValues, getDistance, getNeighbors))
        {
            while (finalNode != null)
            {
                result.Add(finalNode.pos);
                finalNode = finalNode.preNode;
            }
        }
        result.Reverse();
        return result;
    }

    static bool findDest(Node currentNode, List<Node> openList,
                         Dictionary<Vector2Int, BlockType> map, Vector2Int to, out Node finalNode, BlockType passableValues,
                      Func<Vector2Int, Vector2Int, float> getDistance, Func<Vector2Int, List<Vector2Int>> getNeighbors)
    {
        if (currentNode == null)
        {
            finalNode = null;
            return false;
        }
        else if (currentNode.pos == to)
        {
            finalNode = currentNode;
            return true;
        }
        currentNode.open = false;
        openList.Add(currentNode);

        foreach (var item in getNeighbors(currentNode.pos))
        {
            if (map.ContainsKey(item) && passableValues.HasFlag(map[item]))
            {
                findTemp(openList, currentNode, item, to, getDistance);
            }
        }
        /*1. 열려있는 노드의 주변 노드를 모은다.
         * 2. 열려있는 노드 중에서 다장 뎁스가 낮은 것들의 주변을 모두 모은다.
         * 3. 열려있는 노드 중에 목표지점까지 예상거리가 가장 작은 것 부터 계산한다.(이건 A*)
        */
        //오픈 리스트 중에서 가장 작은애, 즉 이동 거리와 횟수가 가장 적은애를 next라는 변수로 히턴해서 사용하겠다 - Min 함수 / 
        var next = openList.FindAll(obj => obj.open).Min(); 
        return findDest(next, openList, map, to, out finalNode, passableValues, getDistance, getNeighbors); 
        //open List; : 아직 검사 안한 애들 - 아직open되어 있는 애들

    }

    static void findTemp(List<Node> openList, Node currentNode, Vector2Int from, Vector2Int to, Func<Vector2Int, Vector2Int, float> getDistance)
    {

        Node temp = openList.Find(obj => obj.pos == (from));
        if (temp == null)
        {
            temp = new Node(currentNode, from, getDistance(from, to), currentNode.gScore + 1);
            openList.Add(temp);
        }
        else if (temp.open && temp.gScore > currentNode.gScore + 1)
        {
            temp.gScore = currentNode.gScore + 1;
            temp.fScore = temp.hScore + temp.gScore;
            temp.preNode = currentNode;
        }
    }

    class Node : IComparable
    {
        public Node preNode; // 바로 전의 Node
        public Vector2Int pos; // 자기 위치
        public float fScore; // h + g
        public float hScore; //예상거리
        public float gScore; //Step, 몇번째 이동인지
        public bool open = true;

        public Node(Node prePos, Vector2Int pos, float hScore, float gScore)
        {
            this.preNode = prePos;
            this.pos = pos;
            this.hScore = hScore;
            this.gScore = gScore;
            this.fScore = hScore + gScore;
        }

        public int CompareTo(object obj)
        {
            Node temp = obj as Node;

            if (temp == null) return 1;

            if (Mathf.Abs(this.fScore - temp.fScore) > 0.01f)
            {
                return this.fScore > temp.fScore ? 1 : -1;
            }

            if (Mathf.Abs(this.hScore - temp.hScore) > 0.01f)
            {
                return this.hScore > temp.hScore ? 1 : -1;
            }
            return 0;
        }
    }
}