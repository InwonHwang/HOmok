using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Player
{

    internal class Node
    {
        internal int v = 0;

        internal Vector2 index;
        internal Matrix15x15 matBlack;
        internal Matrix15x15 matWhite;

        internal List<Node> children = new List<Node>();
    }
    
    public Player other { get; set; }
    Node _root = new Node();
    int _depth = 5;

    void Update()
    {
        if (state != eState.processingInput) return;

        input();
    }

    protected override void input()
    {
        buildTree();

        int v = alphabeta(0, _root, int.MinValue, int.MaxValue, color);

        Node temp = null;
        foreach (var child in _root.children)
        {
            if (v == child.v)
            {
                temp = child;
                break;
            }
        }
        
        position = new Vector3(temp.index.x - 7 , temp.index.y * -1 + 7, -1);
        index = temp.index;

        state = eState.inputComplete;
    }

    void buildTree()
    {
        clearNode(_root);
        _root.matBlack = new Matrix15x15(other.matrix);
        _root.matWhite = new Matrix15x15(matrix);

        buildTree(0, _root, color);
    }

    void buildTree(int depth, Node node, Matrix15x15.Cell.eState color)
    {
        if (depth > _depth) return;

        if (color == Matrix15x15.Cell.eState.black) node.matBlack.Trick();
        else if(color == Matrix15x15.Cell.eState.white) node.matWhite.Trick();

        Matrix15x15 temp = node.matWhite + node.matBlack;

        List<Vector2> index = temp.AboveAgerageList;
        
        foreach (var idx in index)
        {
            if (temp[(int)idx.x, (int)idx.y].state == Matrix15x15.Cell.eState.restrict)
                continue; 

            Node newNode = new Node();
            clearNode(newNode);

            newNode.matBlack = new Matrix15x15(node.matBlack);
            newNode.matWhite = new Matrix15x15(node.matWhite);
            newNode.index = idx;

            if (depth == _depth)
            {
                newNode.v = temp[(int)idx.x, (int)idx.y].weight;
            }

            rule.Update(newNode.matBlack, newNode.matWhite, (int)idx.x, (int)idx.y,
                                            color == Matrix15x15.Cell.eState.black ?
                                            Matrix15x15.Cell.eState.black :
                                            Matrix15x15.Cell.eState.white);

            node.children.Add(newNode);
            buildTree(depth + 1, newNode, color == Matrix15x15.Cell.eState.black ?
                                                   Matrix15x15.Cell.eState.white :
                                                   Matrix15x15.Cell.eState.black);
        }
    }

    int alphabeta(int depth, Node node, int alpha, int beta, Matrix15x15.Cell.eState color)
    {
        if (depth > _depth) return node.v;

        if(this.color == color) // 최대값
        {
            node.v = int.MinValue;
            foreach (var child in node.children)
            {
                int temp = alphabeta(depth + 1, child, alpha, beta, color == Matrix15x15.Cell.eState.black ?
                                                                            Matrix15x15.Cell.eState.white :
                                                                            Matrix15x15.Cell.eState.black);

                node.v = Mathf.Max(temp, node.v);
                alpha = Mathf.Max(alpha, node.v);

                if (beta <= alpha) break;
            }

            return node.v;
        }
        else
        {
            node.v = int.MinValue;
            foreach (var child in node.children)
            {
                int temp = alphabeta(depth + 1, child, alpha, beta, color == Matrix15x15.Cell.eState.black ?
                                                                            Matrix15x15.Cell.eState.white :
                                                                            Matrix15x15.Cell.eState.black);

                node.v = Mathf.Max(temp, node.v);
                beta = Mathf.Max(beta, node.v);

                if (beta <= alpha) break;
            }

            return node.v;
        }
    }

    void clearNode(Node node)
    {
        node.matBlack = null;
        node.matWhite = null;
        node.index = Vector2.zero;
        node.v = int.MinValue;
        
        node.children.Clear();
    }

    void printMatrix(Matrix15x15 mat)
    {
        string p = "";
        for (int i = 12; i > -1; --i)
        {
            for (int j = 0; j < 13; ++j)
            {
                p += mat[j, i].weight + "  ";

                if (mat[j, i].weight.ToString().Length == 1) p += " ";

            }
            p += "\n";
        }

        Debug.Log(p);
    }
}
