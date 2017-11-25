using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {
    
    Dictionary<string, ObjectPool> _pool;

    GameObject _player1;
    GameObject _player2;
    GameObject _computer;

    Player _black;
    Player _white;
    Matrix15x15 _matBlack;
    Matrix15x15 _matWhite;
    Renju _rule;

    int _blackCount;
    int _whiteCount;

    void Awake()
    {
        init();
        initUI();
    }

    void Update()
    {
        if (_black == null || _white == null) return;

        if (_black.state == Player.eState.victory || _white.state == Player.eState.victory)
            Debug.Log("??");

        if (_pool["Black"][_blackCount].activeSelf)
        {
            _black.state = Player.eState.waiting;
            _white.state = Player.eState.processingInput;
            _blackCount++;
        }

        if (_black.state == Player.eState.inputComplete)
        { 
            _pool["Black"][_blackCount].SetActive(true);
            _pool["Black"][_blackCount].transform.position = _black.position;
            
            _rule.Update(_black, _white, _black);
        }

        if (_pool["White"][_whiteCount].activeSelf)
        {
            _white.state = Player.eState.waiting;
            _black.state = Player.eState.processingInput;
            _whiteCount++;
        }

        if (_white.state == Player.eState.inputComplete)
        {
            _pool["White"][_whiteCount].SetActive(true);
            _pool["White"][_whiteCount].transform.position = _white.position;
            
            _rule.Update(_black, _white, _white);
            printMatrix(_white.matrix + _black.matrix);

        }
    }

    void createPool(GameObject prefab)
    {
        ObjectPool pool = new ObjectPool();
        pool.prefab = prefab;

        _pool.Add(prefab.name, pool);
    }

    void init()
    {
        var blackParent = new GameObject();
        blackParent.name = "BlackParent";
        blackParent.transform.SetParent(transform);

        var whiteParent = new GameObject();
        whiteParent.name = "WhiteParent";
        whiteParent.transform.SetParent(transform);

        var checkParent = new GameObject();
        checkParent.name = "Checkparent";
        checkParent.transform.SetParent(transform);

        _pool = new Dictionary<string, ObjectPool>();
        createPool(Resources.Load<GameObject>("Prefabs/Black"));
        createPool(Resources.Load<GameObject>("Prefabs/White"));
        createPool(Resources.Load<GameObject>("Prefabs/Check"));
        createPool(Resources.Load<GameObject>("Prefabs/Pan"));

        int stoneCount = 13 * 13 / 2;
        _pool["Black"].count = stoneCount;
        _pool["White"].count = stoneCount;
        _pool["Check"].count = stoneCount;
        _pool["Pan"].count = 1;

        _pool["Black"].CreateObjects(blackParent.transform, false);
        _pool["White"].CreateObjects(whiteParent.transform, false);
        _pool["Check"].CreateObjects(checkParent.transform, false);
        _pool["Pan"].CreateObjects(transform, false);
        
        _matBlack = new Matrix15x15();
        _matWhite = new Matrix15x15();

        _rule = new Renju();
        _rule.pool = _pool["Check"];

        _player1 = new GameObject();
        _player1.transform.SetParent(transform);
        _player1.name = "Player1";
        _player1.SetActive(false);
        _player1.AddComponent<Player>().rule = _rule;

        _player2 = new GameObject();
        _player2.transform.SetParent(transform);
        _player2.name = "Player2";
        _player2.SetActive(false);
        _player2.AddComponent<Player>().rule = _rule;

        _computer = new GameObject();
        _computer.transform.SetParent(transform);
        _computer.name = "Computer";
        _computer.SetActive(false);
        _computer.AddComponent<Computer>().rule = _rule;
    }
	
    void initUI()
    {
        var ui = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI"));
        ui.name = "UI";
        ui.transform.SetParent(transform);

        ui.transform.Find("Main/Play").GetComponent<Button>().onClick.AddListener(() => {
            ui.transform.Find("Main").gameObject.SetActive(false);
            initPlayOffline();
        });

        ui.transform.Find("Main/PlayWithComputer").GetComponent<Button>().onClick.AddListener(() => {
            ui.transform.Find("Main").gameObject.SetActive(false);
            initPlayWithComputer();
        });

        ui.transform.Find("Main/PlayWithAI").GetComponent<Button>().onClick.AddListener(() => {
        });
    }

    void initPlayOffline()
    {
        _black = _player1.GetComponent<Player>();
        _black.rule = _rule;
        _black.state = Player.eState.processingInput;
        _black.color = Matrix15x15.Cell.eState.black;
        _black.matrix = _matBlack;
        _black.matrix.Clear();

        _white = _player2.GetComponent<Player>();
        _white.rule = _rule;
        _white.state = Player.eState.waiting;
        _white.color = Matrix15x15.Cell.eState.white;
        _white.matrix = _matWhite;
        _white.matrix.Clear();

        _blackCount = 0;
        _whiteCount = 0;

        _pool["Pan"][0].SetActive(true);
        _black.gameObject.SetActive(true);
        _white.gameObject.SetActive(true);
    }

    void initPlayWithComputer()
    {
        _black = _player1.GetComponent<Player>();
        _black.rule = _rule;
        _black.state = Player.eState.processingInput;
        _black.color = Matrix15x15.Cell.eState.black;
        _black.matrix = _matBlack;
        _black.matrix.Clear();
        
         var computer = _computer.GetComponent<Computer>();
        computer.rule = _rule;
        computer.state = Player.eState.waiting;
        computer.color = Matrix15x15.Cell.eState.white;
        computer.matrix = _matWhite;
        computer.matrix.Clear();
        computer.other = _black;

        _white = computer;

        _blackCount = 0;
        _whiteCount = 0;

        _pool["Pan"][0].SetActive(true);
        _black.gameObject.SetActive(true);
        _white.gameObject.SetActive(true);
    }

    void printMatrix(Matrix15x15 mat)
    {
        string p = "";
        for(int i = 1; i < 14; ++i)
        {
            for(int j = 1; j < 14; ++j)
            {
                if(mat[j, i].state < Matrix15x15.Cell.eState.empty)
                    p += "-1 ";
                else
                    p += mat[j, i].weight + " ";

                if (mat[j, i].weight.ToString().Length == 1) p += " ";

            }
            p += "\n";
        }
        Debug.Log(p);
    }
}
