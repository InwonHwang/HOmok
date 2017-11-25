using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Comparer : IEqualityComparer<Vector3>
{
    public bool Equals(Vector3 x, Vector3 y)
    {
        return x.x == y.x && x.y == y.y && x.z == y.z;
    }


    public int GetHashCode(Vector3 v)
    {
        int hCode = (int)v.x ^ (int)v.y ^ (int)v.z;
        return hCode.GetHashCode();
    }
}

public class Renju
{
    class Pattern
    {
        public int[] pattern;
        public int offset1;
        public int offset2;
        public int type;

        public Pattern(int[] p, int o1, int o2, int t)
        {
            pattern = p;
            offset1 = o1;
            offset2 = o2;
            type = t;
        }
    }

    internal ObjectPool pool { get; set; }
    int count = 0;
    Dictionary<Vector3, GameObject> check = new Dictionary<Vector3, GameObject>(new Vector3Comparer());

    List<Pattern> black3 = new List<Pattern>();
    List<Pattern> white3 = new List<Pattern>();
    List<Pattern> black4 = new List<Pattern>();
    List<Pattern> white4 = new List<Pattern>();
    List<Pattern> black5 = new List<Pattern>();
    List<Pattern> white5 = new List<Pattern>();

    public bool IsDouble(int x, int y)
    {
        Vector2 key = new Vector2(x, y);

        if (check.ContainsKey(key)) return true;

        return false;
    }

    public Renju()
    {
        int wall = 0;
        int black = 1;
        int white = 2;
        int empty = 3;

        black3.Add(new Pattern(new int[] { empty, empty, empty, black, black, empty, empty }, 1, 2, 3));
        black3.Add(new Pattern(new int[] { white, empty, empty, black, black, empty, empty }, 2, -1, 3));
        black3.Add(new Pattern(new int[] { empty, empty, empty, black, black, empty, white }, 2, -1, 3));
        black3.Add(new Pattern(new int[] { wall, empty, empty, black, black, empty, empty }, 2, -1, 3));
        black3.Add(new Pattern(new int[] { empty, empty, empty, black, black, empty, wall }, 2, -1, 3));
        black3.Add(new Pattern(new int[] { empty, empty, black, empty, black, empty, empty }, 3, -1, 3));
        black3.Add(new Pattern(new int[] { white, empty, black, empty, black, empty, empty }, 3, -1, 3));
        black3.Add(new Pattern(new int[] { empty, empty, black, empty, black, empty, white }, 3, -1, 3));
        black3.Add(new Pattern(new int[] { wall, empty, black, empty, black, empty, empty }, 3, -1, 3));
        black3.Add(new Pattern(new int[] { empty, empty, black, empty, black, empty, wall }, 3, -1, 3));
        black3.Add(new Pattern(new int[] { empty, empty, black, black, empty, empty, empty }, 4, 5, 3));
        black3.Add(new Pattern(new int[] { white, empty, black, black, empty, empty, empty }, 4, -1, 3));
        black3.Add(new Pattern(new int[] { empty, empty, black, black, empty, empty, white }, 4, -1, 3));
        black3.Add(new Pattern(new int[] { wall, empty, black, black, empty, empty, empty }, 4, -1, 3));
        black3.Add(new Pattern(new int[] { empty, empty, black, black, empty, empty, wall }, 4, -1, 3));
        black3.Add(new Pattern(new int[] { empty, black, empty, empty, black, empty }, 2, 3, 3));
        black3.Add(new Pattern(new int[] { empty, empty, black, empty, black, empty }, 1, -1, 3));
        black3.Add(new Pattern(new int[] { empty, black, empty, black, empty, empty }, 4, -1, 3));

        white3.Add(new Pattern(new int[] { empty, empty, empty, white, white, empty, empty }, 2, -1, 3));
        white3.Add(new Pattern(new int[] { black, empty, empty, white, white, empty, empty }, 2, -1, 3));
        white3.Add(new Pattern(new int[] { empty, empty, empty, white, white, empty, black }, 2, -1, 3));
        white3.Add(new Pattern(new int[] { wall, empty, empty, white, white, empty, empty }, 2, -1, 3));
        white3.Add(new Pattern(new int[] { empty, empty, empty, white, white, empty, wall }, 2, -1, 3));
        white3.Add(new Pattern(new int[] { empty, empty, white, empty, white, empty, empty }, 3, -1, 3));
        white3.Add(new Pattern(new int[] { black, empty, white, empty, white, empty, empty }, 3, -1, 3));
        white3.Add(new Pattern(new int[] { empty, empty, white, empty, white, empty, black }, 3, -1, 3));
        white3.Add(new Pattern(new int[] { wall, empty, white, empty, white, empty, empty }, 3, -1, 3));
        white3.Add(new Pattern(new int[] { empty, empty, white, empty, white, empty, wall }, 3, -1, 3));
        white3.Add(new Pattern(new int[] { empty, empty, white, white, empty, empty, empty }, 4, -1, 3));
        white3.Add(new Pattern(new int[] { black, empty, white, white, empty, empty, empty }, 4, -1, 3));
        white3.Add(new Pattern(new int[] { empty, empty, white, white, empty, empty, black }, 4, -1, 3));
        white3.Add(new Pattern(new int[] { wall, empty, white, white, empty, empty, empty }, 4, -1, 3));
        white3.Add(new Pattern(new int[] { empty, empty, white, white, empty, empty, wall }, 4, -1, 3));
        white3.Add(new Pattern(new int[] { empty, white, empty, empty, white, empty }, 2, 3, 3));
        white3.Add(new Pattern(new int[] { empty, empty, white, empty, white, empty }, 1, -1, 3));
        white3.Add(new Pattern(new int[] { empty, white, empty, white, empty, empty }, 4, -1, 3));

        black4.Add(new Pattern(new int[] { empty, empty, black, black, black, empty, empty }, 1, 5, 4));
        black4.Add(new Pattern(new int[] { white, empty, black, black, black, empty, empty }, 1, 5, 4));
        black4.Add(new Pattern(new int[] { empty, empty, black, black, black, empty, white }, 1, 5, 4));
        black4.Add(new Pattern(new int[] { white, empty, black, black, black, empty, white }, 1, 5, 4));
        black4.Add(new Pattern(new int[] { wall, empty, black, black, black, empty, empty }, 1, 5, 4));
        black4.Add(new Pattern(new int[] { empty, empty, black, black, black, empty, wall }, 1, 5, 4));
        black4.Add(new Pattern(new int[] { white, black, black, black, empty }, 4, -1, 4));
        black4.Add(new Pattern(new int[] { empty, black, black, black, white }, 0, -1, 4));
        black4.Add(new Pattern(new int[] { wall, black, black, black, empty }, 4, -1, 4));
        black4.Add(new Pattern(new int[] { empty, black, black, black, wall }, 0, -1, 4));

        black4.Add(new Pattern(new int[] { empty, empty, black, empty, black, black, empty }, 1, 3, 4));
        black4.Add(new Pattern(new int[] { white, empty, black, empty, black, black, empty }, 1, 3, 4));
        black4.Add(new Pattern(new int[] { empty, empty, black, empty, black, black, white }, 1, 3, 4));
        black4.Add(new Pattern(new int[] { white, empty, black, empty, black, black, white }, 1, 3, 4));
        black4.Add(new Pattern(new int[] { wall, empty, black, empty, black, black, empty }, 1, 3, 4));
        black4.Add(new Pattern(new int[] { empty, empty, black, empty, black, black, wall }, 1, 3, 4));

        black4.Add(new Pattern(new int[] { empty, black, empty, black, black, empty, empty }, 5, -1, 4));
        black4.Add(new Pattern(new int[] { white, black, empty, black, black, empty, empty }, 2, 5, 4));
        black4.Add(new Pattern(new int[] { empty, black, empty, black, black, empty, white }, 2, 5, 4));
        black4.Add(new Pattern(new int[] { white, black, empty, black, black, empty, white }, 2, 5, 4));
        black4.Add(new Pattern(new int[] { wall, black, empty, black, black, empty, empty }, 2, 5, 4));
        black4.Add(new Pattern(new int[] { empty, black, empty, black, black, empty, wall }, 2, 5, 4));

        black4.Add(new Pattern(new int[] { empty, empty, black, black, empty, black, empty }, 1, -1, 4));
        black4.Add(new Pattern(new int[] { white, empty, black, black, empty, black, empty }, 1, 4, 4));
        black4.Add(new Pattern(new int[] { empty, empty, black, black, empty, black, white }, 1, 4, 4));
        black4.Add(new Pattern(new int[] { white, empty, black, black, empty, black, white }, 1, 4, 4));
        black4.Add(new Pattern(new int[] { wall, empty, black, black, empty, black, empty }, 1, 4, 4));
        black4.Add(new Pattern(new int[] { empty, empty, black, black, empty, black, wall }, 1, 4, 4));

        black4.Add(new Pattern(new int[] { empty, black, black, empty, black, empty, empty }, 3, 5, 4));
        black4.Add(new Pattern(new int[] { white, black, black, empty, black, empty, empty }, 3, 5, 4));
        black4.Add(new Pattern(new int[] { empty, black, black, empty, black, empty, white }, 3, 5, 4));
        black4.Add(new Pattern(new int[] { white, black, black, empty, black, empty, white }, 3, 5, 4));
        black4.Add(new Pattern(new int[] { wall, black, black, empty, black, empty, empty }, 3, 5, 4));
        black4.Add(new Pattern(new int[] { empty, black, black, empty, black, empty, wall }, 3, 5, 4));

        black4.Add(new Pattern(new int[] { black, empty, black, empty, black}, 1, 3, 4));

        white4.Add(new Pattern(new int[] { empty, white, white, white, empty }, 0, 4, 4));
        white4.Add(new Pattern(new int[] { empty, empty, white, white, white }, 0, 1, 4));
        white4.Add(new Pattern(new int[] { white, empty, empty, white, white }, 1, 2, 4));
        white4.Add(new Pattern(new int[] { white, white, empty, empty, white }, 2, 3, 4));
        white4.Add(new Pattern(new int[] { white, white, white, empty, empty }, 3, 4, 4));
        white4.Add(new Pattern(new int[] { empty, white, empty, white, white }, 0, 2, 4));
        white4.Add(new Pattern(new int[] { white, white, empty, white, empty }, 2, 4, 4));

        black5.Add(new Pattern(new int[] { empty, empty, black, black, black, black, empty }, 1, -1, 5));
        black5.Add(new Pattern(new int[] { white, empty, black, black, black, black, empty }, 1, -1, 5));
        black5.Add(new Pattern(new int[] { empty, empty, black, black, black, black, white }, 1, -1, 5));
        black5.Add(new Pattern(new int[] { white, empty, black, black, black, black, white }, 1, -1, 5));
        black5.Add(new Pattern(new int[] { wall, empty, black, black, black, black, white }, 1, -1, 5));
        black5.Add(new Pattern(new int[] { white, empty, black, black, black, black, wall }, 1, -1, 5));
        black5.Add(new Pattern(new int[] { wall, empty, black, black, black, black, empty }, 1, -1, 5));
        black5.Add(new Pattern(new int[] { empty, empty, black, black, black, black, wall }, 1, -1, 5));

        black5.Add(new Pattern(new int[] { empty, black, empty, black, black, black, empty }, 2, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, empty, black, black, black, empty }, 2, -1, 5));
        black5.Add(new Pattern(new int[] { empty, black, empty, black, black, black, white }, 2, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, empty, black, black, black, white }, 2, -1, 5));
        black5.Add(new Pattern(new int[] { wall, black, empty, black, black, black, white }, 2, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, empty, black, black, black, wall }, 2, -1, 5));
        black5.Add(new Pattern(new int[] { wall, black, empty, black, black, black, empty }, 2, -1, 5));
        black5.Add(new Pattern(new int[] { empty, black, empty, black, black, black, wall }, 2, -1, 5));

        black5.Add(new Pattern(new int[] { empty, black, black, empty, black, black, empty }, 3, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, empty, black, black, empty }, 3, -1, 5));
        black5.Add(new Pattern(new int[] { empty, black, black, empty, black, black, white }, 3, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, empty, black, black, white }, 3, -1, 5));
        black5.Add(new Pattern(new int[] { wall, black, black, empty, black, black, white }, 3, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, empty, black, black, wall }, 3, -1, 5));
        black5.Add(new Pattern(new int[] { wall, black, black, empty, black, black, empty }, 3, -1, 5));
        black5.Add(new Pattern(new int[] { empty, black, black, empty, black, black, wall }, 3, -1, 5));

        black5.Add(new Pattern(new int[] { empty, black, black, black, empty, black, empty }, 4, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, black, empty, black, empty }, 4, -1, 5));
        black5.Add(new Pattern(new int[] { empty, black, black, black, empty, black, white }, 4, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, black, empty, black, white }, 4, -1, 5));
        black5.Add(new Pattern(new int[] { wall, black, black, black, empty, black, white }, 4, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, black, empty, black, wall }, 4, -1, 5));
        black5.Add(new Pattern(new int[] { wall, black, black, black, empty, black, empty }, 4, -1, 5));
        black5.Add(new Pattern(new int[] { empty, black, black, black, empty, black, wall }, 4, -1, 5));

        black5.Add(new Pattern(new int[] { empty, black, black, black, black, empty, empty }, 5, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, black, black, empty, empty }, 5, -1, 5));
        black5.Add(new Pattern(new int[] { empty, black, black, black, black, empty, white }, 5, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, black, black, empty, white }, 5, -1, 5));
        black5.Add(new Pattern(new int[] { wall, black, black, black, black, empty, white }, 5, -1, 5));
        black5.Add(new Pattern(new int[] { white, black, black, black, black, empty, wall }, 5, -1, 5));
        black5.Add(new Pattern(new int[] { wall, black, black, black, black, empty, empty }, 5, -1, 5));
        black5.Add(new Pattern(new int[] { empty, black, black, black, black, empty, wall }, 5, -1, 5));

        white5.Add(new Pattern(new int[] { empty, white, white, white, white }, 0, -1, 5));
        white5.Add(new Pattern(new int[] { white, empty, white, white, white }, 1, -1, 5));
        white5.Add(new Pattern(new int[] { white, white, empty, white, white }, 2, -1, 5));
        white5.Add(new Pattern(new int[] { white, white, white, empty, white }, 3, -1, 5));
        white5.Add(new Pattern(new int[] { white, white, white, white, empty }, 3, -1, 5));
    }

    void kmp(Matrix15x15 mat, Pattern p, int num, bool isRow)
    {
        int n = 15;
        int m = p.pattern.Length;

        int[] pi = getPi(p.pattern);

        int j = 0;
        for (int i = 0; i < n; ++i)
        {
            int state = isRow ? (int)mat[i, num].state : (int)mat[num, i].state;

            while (j > 0 && state != p.pattern[j])
                j = pi[j - 1];

            if (state == p.pattern[j])
            {
                if (j == m - 1)
                {
                    int x1 = isRow ? i - m + 1 + p.offset1 : num;
                    int y1 = isRow ? num : i - m + 1 + p.offset1;

                    int x2 = isRow ? i - m + 1 + p.offset2 : num;
                    int y2 = isRow ? num : i - m + 1 + p.offset2;
                    
                    if(p.type == 3) mat[x1, y1].count3++;
                    else if(p.type == 4) mat[x1, y1].count4++;
                    else mat[x1, y1].count5++;

                    if (p.offset2 != -1)
                    {
                        if (p.type == 3) mat[x2, y2].count3++;
                        else if (p.type == 4) mat[x2, y2].count4++;
                        else mat[x2, y2].count5++;
                    }

                    j = pi[j];
                }
                else
                {
                    j++;
                }
            }
        }
    }

    void kmp2(Matrix15x15 mat, Pattern p, int x, int y, bool isFrontDown)
    {
        int n = 15;
        int m = p.pattern.Length;

        int[] pi = getPi(p.pattern);

        int j = 0;

        while(x > 0 && y > 0 && x < n && y < n)
        {
            int state = (int)mat[x, y].state;

            while (j > 0 && state != p.pattern[j])
                j = pi[j - 1];

            if (state == p.pattern[j])
            {
                if (j == m - 1)
                {
                    int x1 = x - m + 1 + p.offset1;
                    int y1 = isFrontDown ? y - m + 1 + p.offset1 : y + m - 1 - p.offset1;
                    int x2 = x - m + 1 + p.offset2;
                    int y2 = isFrontDown ? y - m + 1 + p.offset2 : y + m - 1 - p.offset2;

                    if (p.type == 3) mat[x1, y1].count3++;
                    else if (p.type == 4) mat[x1, y1].count4++;
                    else mat[x1, y1].count5++;

                    if (p.offset2 != -1)
                    {
                        if (p.type == 3) mat[x2, y2].count3++;
                        else if (p.type == 4) mat[x2, y2].count4++;
                        else mat[x2, y2].count5++;
                    }

                    j = pi[j];
                }
                else
                {
                    j++;
                }
            }

            x++;
            if (isFrontDown) y++;            
            else y--;
        }
    }

    int[] getPi(int[] pattern)
    {
        int[] pi = new int[pattern.Length];

        int j = 0;

        for(int i = 1; i < pi.Length; ++i)
        {
            while (j > 0 && pattern[i] != pattern[j])
                j = pi[j - 1];

            if(pattern[i] == pattern[j])
            {
                pi[i] = ++j;
            }
        }

        return pi;
    }

    public void Update(Player black, Player white, Player inputPlayer)
    {
        int x = (int)inputPlayer.index.x;
        int y = (int)inputPlayer.index.y;
        
        black.matrix[x, y].weight = -1;
        black.matrix[x, y].state = inputPlayer.color;

        white.matrix[x, y].weight = -1;
        white.matrix[x, y].state = inputPlayer.color;

        check.Clear();
        count = 0;
        pool.SetActive(false);
        updateWeightByPolicy(black, white, inputPlayer);

        for (int i = 1; i < 14; ++i)
        {
            for (int j = 1; j < 14; ++j)
            {
                if (black.matrix[j, i].state == Matrix15x15.Cell.eState.restrict)
                {
                    Vector2 key = new Vector2(j - 7, i * -1 + 7);

                    check.Add(key, pool[count]);
                    pool[count].transform.position = new Vector3(key.x, key.y, -1);
                    pool[count++].SetActive(true);
                }
            }
        }
    }

    public void Update(Matrix15x15 matBlack, Matrix15x15 matWhite, int x, int y, Matrix15x15.Cell.eState color)
    {
        matBlack[x, y].weight = -1;
        matBlack[x, y].state = color;

        matWhite[x, y].weight = -1;
        matWhite[x, y].state = color;

        updateWeightByPolicy(matBlack, matWhite, x, y, color);
    }

    void updateWeightByPolicy(Player black, Player white, Player inputPlayer)
    {
        int x = (int)inputPlayer.index.x;
        int y = (int)inputPlayer.index.y;

        updateWeightByPolicy(black.matrix, white.matrix, x, y, inputPlayer.color);        
    }

    void updateWeightByPolicy(Matrix15x15 matBlack, Matrix15x15 matWhite, int x, int y, Matrix15x15.Cell.eState color)
    {
        Matrix15x15 toBeUpdated = color == Matrix15x15.Cell.eState.black ? matBlack : matWhite;
        
        for (int i = -1; i < 2; ++i)
        {
            for (int j = -1; j < 2; ++j)
            {
                if (i == 0 && j == 0) continue;

                if(toBeUpdated[x + j, y + i].weight != 50 &&
                    toBeUpdated[x + j, y + i].weight != 10)
                { 
                    toBeUpdated[x + j, y + i].weight++;
                }
                
            }
        }

        matBlack.ClearCount();
        matWhite.ClearCount();

        for (int i = 0; i < black3.Count; ++i)
        {
            for (int j = 1; j < 14; ++j)
            {
                kmp(matBlack, black3[i], j, true);
                kmp(matBlack, black3[i], j, false);
                kmp2(matBlack, black3[i], j, 1, true);
                kmp2(matBlack, black3[i], 1, j + 1, true);
                kmp2(matBlack, black3[i], 1, j, false);
                kmp2(matBlack, black3[i], j + 1, 13, false);

                kmp(matWhite, white3[i], j, true);
                kmp(matWhite, white3[i], j, false);
                kmp2(matWhite, white3[i], j, 1, true);
                kmp2(matWhite, white3[i], 1, j + 1, true);
                kmp2(matWhite, white3[i], 1, j, false);
                kmp2(matWhite, white3[i], j + 1, 13, false);
            }
        }

        for (int i = 0; i < black4.Count; ++i)
        {
            for (int j = 1; j < 14; ++j)
            {
                kmp(matBlack, black4[i], j, true);
                kmp(matBlack, black4[i], j, false);
                kmp2(matBlack, black4[i], j, 1, true);
                kmp2(matBlack, black4[i], 1, j + 1, true);
                kmp2(matBlack, black4[i], 1, j, false);
                kmp2(matBlack, black4[i], j + 1, 13, false);
            }
        }

        for (int i = 0; i < black5.Count; ++i)
        {
            for (int j = 1; j < 14; ++j)
            {
                kmp(matBlack, black5[i], j, true);
                kmp(matBlack, black5[i], j, false);
                kmp2(matBlack, black5[i], j, 1, true);
                kmp2(matBlack, black5[i], 1, j + 1, true);
                kmp2(matBlack, black5[i], 1, j, false);
                kmp2(matBlack, black5[i], j + 1, 13, false);
            }
        }

        if (color == Matrix15x15.Cell.eState.white)
        {
            for (int i = 0; i < white4.Count; ++i)
            {
                for (int j = 1; j < 14; ++j)
                {
                    kmp(matWhite, white4[i], j, true);
                    kmp(matWhite, white4[i], j, false);
                    kmp2(matWhite, white4[i], j, 1, true);
                    kmp2(matWhite, white4[i], 1, j + 1, true);
                    kmp2(matWhite, white4[i], 1, j, false);
                    kmp2(matWhite, white4[i], j + 1, 13, false);
                }
            }

            for (int i = 0; i < white5.Count; ++i)
            {
                for (int j = 1; j < 14; ++j)
                {
                    kmp(matWhite, white5[i], j, true);
                    kmp(matWhite, white5[i], j, false);
                    kmp2(matWhite, white5[i], j, 1, true);
                    kmp2(matWhite, white5[i], 1, j + 1, true);
                    kmp2(matWhite, white5[i], 1, j, false);
                    kmp2(matWhite, white5[i], j + 1, 13, false);
                }
            }
        }

        for (int i = 1; i < 14; ++i)
        {
            for (int j = 1; j < 14; ++j)
            {
                if (matBlack[j, i].weight != 10000 && matBlack[j, i].weight >= 50)
                {
                    matBlack[j, i].weight = matBlack[j, i].prevWeight;
                }

                if (matWhite[j, i].weight != 10000 && matWhite[j, i].weight >= 50)
                {
                    matWhite[j, i].weight = matWhite[j, i].prevWeight;
                }

                if (matBlack[j, i].count3 >= 1)
                {
                    matBlack[j, i].weight++;
                }

                if (matBlack[j, i].count4 >= 1)
                {
                    matBlack[j, i].prevWeight = matBlack[j, i].weight;
                    matBlack[j, i].weight = 50;
                }

                if (matWhite[j, i].count3 >= 1)
                {
                    matWhite[j, i].weight++;
                }

                if (matWhite[j, i].count4 >= 1)
                {
                    matWhite[j, i].prevWeight = matWhite[j, i].weight;
                    matWhite[j, i].weight = 50;
                }

                if (toBeUpdated[j, i].count5 == 1)
                {
                    toBeUpdated[j, i].weight = 10000;
                }

                //if (color == Matrix15x15.Cell.eState.black)
                {
                    if (matBlack[j, i].count3 == 1 && matBlack[j, i].count4 == 1)
                    {
                        matBlack[j, i].weight = 100;
                    }
                    else if ((matBlack[j, i].count3 > 1 || matBlack[j, i].count4 > 1))
                    {
                        matBlack[j, i].state = Matrix15x15.Cell.eState.restrict;
                    }
                }
                //else
                {
                    if (matWhite[j, i].count3 >= 1 && matWhite[j, i].count4 >= 1)
                    {
                        matWhite[j, i].weight = 100;
                    }
                }
            }
        }
    }
}


