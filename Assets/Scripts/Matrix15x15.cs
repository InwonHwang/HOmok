using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix15x15 {

    public class Cell
    {
        public enum eState : int { wall, black, white, empty, restrict }

        public eState state;
        public int weight;
        public int prevWeight;
        public int count3;
        public int count4;
        public int count5;

        public Cell(eState s, int w, int p, int c3, int c4, int c5)
        {
            state = s;
            weight = w;
            prevWeight = p;
            count3 = c3;
            count4 = c4;
            count5 = c5;
        }
    }

    public Cell this[int x, int y]
    {
        get {
            return _cells[x, y];
        }
        set {
            _cells[x, y] = value;
        }
    }

    public static Matrix15x15 operator +(Matrix15x15 mat0, Matrix15x15 mat1)
    {
        Matrix15x15 ret = new Matrix15x15();
        for (int i = 1; i < 14; ++i)
        {
            for (int j = 1; j < 14; ++j)
            {
                ret[i, j].state = mat0[i, j].state;
                if(mat1[i, j].state == Cell.eState.restrict) ret[i, j].state = mat1[i, j].state;

                ret[i, j].weight = mat0[i, j].weight + mat1[i, j].weight;
            }
        }
        return ret;
    }

    public int MinValue
    {
        get
        {
            int minValue = int.MaxValue;

            for (int x = 1; x < 14; ++x)
            {
                for (int y = 1; y < 14; ++y)
                {
                    if (_cells[x, y].state == Cell.eState.empty && _cells[x, y].weight < minValue)
                    {
                        minValue = _cells[x, y].weight;
                    }
                }
            }
            return minValue;
        }
    }

    public int MaxValue
    {
        get
        {
            int maxValue = int.MinValue;

            for (int x = 1; x < 14; ++x)
            {
                for (int y = 1; y < 14; ++y)
                {
                    if (_cells[x, y].state == Cell.eState.empty && _cells[x, y].weight > maxValue)
                    {
                        maxValue = _cells[x, y].weight;
                    }
                }
            }
            return maxValue;
        }
    }

    public float Average
    {
        get
        {
            float sum = 0;

            for (int x = 1; x < 14; ++x)
            {
                for (int y = 1; y < 14; ++y)
                {
                    sum += _cells[x, y].weight;
                }
            }
            return (float)(sum / (13 * 13));
        }
    }

    public List<Vector2> MinList
    {
        get
        {
            _minList.Clear();
            int minValue = MinValue;

            for (int y = 1; y < 14; ++y)
            {
                for (int x = 1; x < 14; ++x)
                {
                    if (_cells[x, y].state != Cell.eState.empty || _cells[x, y].weight != minValue)
                        continue;
                    
                    Vector2 temp = new Vector2(x, y);
                    _minList.Add(temp);                    
                }
            }

            return _minList;
        }
    }

    public List<Vector2> MaxList
    {
        get
        {
            _maxList.Clear();
            int maxValue = MaxValue;

            for (int y = 1; y < 14; ++y)
            {
                for (int x = 1; x < 14; ++x)
                {
                    if (_cells[x, y].state != Cell.eState.empty || _cells[x, y].weight != maxValue)
                        continue;
                    
                    Vector2 temp = new Vector2(x, y);
                    _maxList.Add(temp);
                }
            }

            return _maxList;
        }
    }

    public List<Vector2> AboveAgerageList
    {
        get
        {
            _aboveAverageList.Clear();
            float average = (float)(MinValue + MaxValue) / 10.0f * 8.0f;
            
            for (int y = 1; y < 14; ++y)
            {
                for (int x = 1; x < 14; ++x)
                {
                    if (_cells[x, y].state != Cell.eState.empty || _cells[x, y].weight < average)
                        continue;
                    
                    Vector2 temp = new Vector2(x, y);
                    _aboveAverageList.Add(temp);
                }
            }

            return _aboveAverageList;
        }
    }

    Cell[,] _cells = new Cell[15, 15];
    List<Vector2> _minList = new List<Vector2>();
    List<Vector2> _maxList = new List<Vector2>();
    List<Vector2> _aboveAverageList = new List<Vector2>();

    public Matrix15x15()
    {
        for (int x = 0; x < 15; ++x)
        {
            for (int y = 0; y < 15; ++y)
            {
                _cells[x, y] = new Matrix15x15.Cell(Cell.eState.empty, 0, 0, 0, 0, 0);
            }
        }

        for (int i = 0; i < 15; ++i)
        {
            _cells[0, i].state = Cell.eState.wall;
            _cells[i, 0].state = Cell.eState.wall;
            _cells[14, i].state = Cell.eState.wall;
            _cells[i, 14].state = Cell.eState.wall;
        }
    }

    public Matrix15x15(Matrix15x15 temp)
    {
        for (int x = 0; x < 15; ++x)
        {
            for (int y = 0; y < 15; ++y)
            {
                _cells[x, y] = new Matrix15x15.Cell(temp[x, y].state,
                                                    temp[x, y].weight,
                                                    temp[x, y].prevWeight,
                                                    temp[x, y].count3,
                                                    temp[x, y].count4,
                                                    temp[x, y].count5);
            }
        }
    }

    public void Clear()
    {
        for(int x = 1; x < 14; ++x)
        {
            for(int y = 1; y < 14; ++y)
            {
                _cells[x, y].state = Cell.eState.empty;
                _cells[x, y].weight = 0;
                _cells[x, y].prevWeight = 0;
                _cells[x, y].count3 = 0;
                _cells[x, y].count4 = 0;
                _cells[x, y].count5 = 0;
            }                
        }
    }

    public void ClearCount()
    {
        for (int x = 1; x < 14; ++x)
        {
            for (int y = 1; y < 14; ++y)
            {
                _cells[x, y].count3 = 0;
                _cells[x, y].count4 = 0;
                _cells[x, y].count5 = 0;

                if (_cells[x, y].state == Cell.eState.restrict)
                {
                    _cells[x, y].state = Cell.eState.empty;
                }
            }
        }
    }

    public void Trick()
    {
        for (int x = 1; x < 14; ++x)
        {
            for (int y = 1; y < 14; ++y)
            {
                if(_cells[x, y].weight == 50 || _cells[x, y].weight == 100)
                    _cells[x, y].weight++;
            }
        }
    }
}
