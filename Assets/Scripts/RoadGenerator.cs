using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadGenerator : Singleton<RoadGenerator>
{
    public GameObject RoadPrefab; //List of roads on scene.
    private List<GameObject> roads = new List<GameObject>();
    // private List<GameObject> elements = new List<GameObject>();
    private float maxSpeed = 10; //Max speed in game.
    public float speed = 0;    //Current speed in game.
    private int maxRoadCount = 3;    //Max count of part road. 
    private int roadLenght = 50;    //Lenght of part road.

    void Start()
    {
        ResetLevel();
    }

    void Update()
    {
        if (speed == 0) { return; }//if speed = 0 do nothing.

        foreach (GameObject road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            if (speed < maxSpeed)
            {
                speed += 0.01f * Time.deltaTime; // Update move speed
            }


        }// speed != 0 move every road game object on z coordinate for speed * Time.deltaTime.

        if (roads[0].transform.position.z < -roadLenght)
        {
            Destroy(roads[0]); //Delete object.
            roads.RemoveAt(0); //Delete object from list.
            CreateNextRoad();
        }

    }

    public void StartLevel()
    {
        speed = maxSpeed;
        SwipeManager.Instance.enabled = true; //Turn on  Swipe manager
    }
    private void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero; //If List of gameObject is empty.

        if (roads.Count > 0)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, roadLenght);
        }// if roads count > 0 pos = position of last row in list + width of part road.

        GameObject go = Instantiate(RoadPrefab, pos, Quaternion.identity); //Create new object and put it in the center.
        go.transform.SetParent(transform);
        roads.Add(go); //add new part of road to the list

    }
    //Reset game, reset params,delete all old objects on scene and create new objects on scene.
    public void ResetLevel()
    {
        speed = 0;
        while (roads.Count > 0)
        {
            Destroy(roads[0]); //Delete object.
            roads.RemoveAt(0); //Delete object from list.
        }

        
        for (int i = 0; i < maxRoadCount; i++)
        {
            CreateNextRoad();
        }// Create new road. 
        SwipeManager.Instance.enabled = false; // Turn Swipe manager
        MapGenerator.Instance.ResetMaps();
    }
}
