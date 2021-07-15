using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    private int itemSpace = 15; //space between traps
    private int itemCountInMap = 5;
    protected float laneOffset = 2.5f;
    public float _laneOffset { get { return laneOffset; } }

    private int coinsCountInItem = 10;// Count coins in one iteam
    private float coinsHeight = 0.5f;// coins height
    private int mapSize;// Size of map

    private enum TrackPos { Left = -1, Center = 0, Right = 1 };
    private enum CoinsStyle { Line, Jump, Ramp, None };


    [SerializeField] private GameObject ObstacleTopPrefab;
    [SerializeField] private GameObject ObstacleBottomPrefab;
    [SerializeField] private GameObject ObstacleFullPrefab;
    [SerializeField] private GameObject ObstacleRampPrefab;
    [SerializeField] private GameObject ObstacleCoinPrefab;
    [SerializeField] private GameObject ObstacleDoubleScorePrefab;
    [SerializeField] private GameObject ObstacleDoubleJumpPrefab;
    [SerializeField] private GameObject ObstacleDoubleCoinPrefab;
    [SerializeField] private GameObject ObstacleCoinMagnetPrefab;

    private List<GameObject> maps = new List<GameObject>();
    private List<GameObject> activeMaps = new List<GameObject>();

    struct MapItem
    {
        public void SetValues(GameObject obstacle, TrackPos trackPos, CoinsStyle coinsStyle)
        {
            this.obstacle = obstacle;
            this.trackPos = trackPos;
            this.coinsStyle = coinsStyle;
        }
        public GameObject obstacle;
        public TrackPos trackPos;
        public CoinsStyle coinsStyle;
    }
    private void Awake()
    {
        mapSize = itemCountInMap * itemSpace;

        maps.Add(MakeMap1());
        maps.Add(MakeMap2());
        maps.Add(MakeMap1());
        // maps.Add(MakeMap2());
        // maps.Add(MakeMap1());
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (RoadGenerator.Instance.speed == 0)
        {
            return;
        }
        foreach (GameObject map in activeMaps)
        {
            map.transform.position -= new Vector3(0, 0, RoadGenerator.Instance.speed * Time.deltaTime);
        }
        if (activeMaps[0].transform.position.z < -mapSize)
        {
            RemoveFirstActiveMap();

            AddActiveMap();
        }
    }

    private void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }

    public void ResetMaps()
    {
        while (activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }
        AddActiveMap();
        AddActiveMap();
    }
    void AddActiveMap()
    {
        int r = Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true);
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(true);

        }
        go.transform.position = activeMaps.Count > 0 ? activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * mapSize :
        new Vector3(0, 0, 10);
        maps.RemoveAt(r);
        activeMaps.Add(go);

    }
    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        MapItem item = new MapItem();
        for (int i = 0; i < itemCountInMap; i++)
        {
            item.SetValues(null, TrackPos.Center, CoinsStyle.Line);
            if (i == 1)
            {
                item.SetValues(ObstacleDoubleJumpPrefab, TrackPos.Left, CoinsStyle.None);
            }
            else if (i == 2)
            {
                item.SetValues(ObstacleRampPrefab, TrackPos.Left, CoinsStyle.Ramp);

            }
            else if (i == 3)
            {
                item.SetValues(ObstacleRampPrefab, TrackPos.Right, CoinsStyle.Ramp);
            }
            else if (i == 4)
            {
                item.SetValues(ObstacleTopPrefab, TrackPos.Center, CoinsStyle.Line);
            }


            Vector3 obstaclePos = new Vector3((int)item.trackPos * laneOffset, 0.5f, i * itemSpace);
            CreateCoins(item.coinsStyle, obstaclePos, result);

            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);

            }
        }
        return result;
    }
    GameObject MakeMap2()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        MapItem item = new MapItem();
        for (int i = 0; i < itemCountInMap; i++)
        {
            item.SetValues(null, TrackPos.Center, CoinsStyle.Line);
            if (i == 1)
            {
                item.SetValues(ObstacleCoinMagnetPrefab, TrackPos.Right, CoinsStyle.None);
            }
            else if (i == 2)
            {
                item.SetValues(ObstacleBottomPrefab, TrackPos.Left, CoinsStyle.Jump);
            }
            else if (i == 3)
            {
                item.SetValues(ObstacleFullPrefab, TrackPos.Center, CoinsStyle.None);
            }
            else if (i == 4)
            {
                item.SetValues(ObstacleTopPrefab, TrackPos.Left, CoinsStyle.Line);
            }


            Vector3 obstaclePos = new Vector3((int)item.trackPos * laneOffset, 0.5f, i * itemSpace);
            CreateCoins(item.coinsStyle, obstaclePos, result);

            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);

            }
        }
        return result;
    }
    void CreateCoins(CoinsStyle style, Vector3 pos, GameObject parentObject)
    {
        Vector3 coinPos = Vector3.zero;
        if (style == CoinsStyle.Line)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = coinsHeight;
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(ObstacleCoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinsStyle.Jump)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Max(-1 / 2f * Mathf.Pow(i, 2) + 3, coinsHeight);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(ObstacleCoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinsStyle.Ramp)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Min(Mathf.Max(1.0f * (i + 2), coinsHeight), 3.0f);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(ObstacleCoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinsStyle.None)
        {
            coinPos = Vector3.zero;
        }
    }
}
