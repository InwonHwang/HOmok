using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum eState : int { waiting, processingInput, inputComplete, victory, }

    public eState state { get; set; }             // player 상태
    public Matrix15x15.Cell.eState color { get; set; }      // player 바둑돌 색
    public Matrix15x15 matrix { get; set; }       // matrix;
    public Renju rule { get; set; }

    public Vector3 position { get; set; } // player 바둑돌 놓는 위치
    public Vector2 index { get; set; }    // player 바둑돌 놓는 위치의 인덱스

    void Update()
    {
        if (state != eState.processingInput) return;

        input();        
    }

    protected virtual void input()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                int x = Mathf.RoundToInt(hitInfo.point.x);
                int y = Mathf.RoundToInt(hitInfo.point.y);

                position = new Vector3(x, y, -1);
                index = new Vector2(x + 7, y * -1 + 7);

                if (matrix[x + 7, y * -1 + 7].state == Matrix15x15.Cell.eState.empty &&
                    !rule.IsDouble(x, y))
                    state = eState.inputComplete;
            }
        }
    }
}
