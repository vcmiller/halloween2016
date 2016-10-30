using UnityEngine;
using System.Collections.Generic;

public class Generator : MonoBehaviour {
    Player player;

    int lastPlacement = -10;
    int lastWallPlacement = -15;

    Queue<FloorChain> chains;

    int lightCountdown = 0;
    int calciumCountdown = 50;

    public GameObject lightPrefab;
    public GameObject floorPrefab;
    public GameObject floorGapPrefab;
    public GameObject wallPrefab;
    public GameObject wallUpperPrefab;
    public GameObject wallLowerPrefab;
    public GameObject calciumPrefab;

    public int wallOff = 2;

    public int leadValue = 3;
    public int wallLeadValue = 8;


    // Use this for initialization
    void Start () {
        chains = new Queue<FloorChain>();
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        while (player.transform.position.x > lastPlacement * 2 - leadValue) {
            UpdateChains();
            Place();
        }

        while (player.transform.position.x > lastWallPlacement * 2 - wallLeadValue) {
            PlaceWalls();
        }
	}

    void UpdateChains() {
        lastPlacement++;

        if (chains.Count == 0 || chains.Peek().end < lastPlacement) {
            bool previousGap = true;

            if (chains.Count > 0) {
                previousGap = false;

                foreach (FloorChain chain in chains) {
                    if (chain.isGap) {
                        previousGap = true;
                    }
                }

                chains.Clear();
            }

            if (lastPlacement < 10) {
                previousGap = true;
            }

            float rand = Random.value;
            if (rand < .2f && !previousGap) { // Gap
                FloorChain gap;
                gap.end = lastPlacement + Random.Range(2, 3);
                gap.height = player.platformHeight;
                gap.isGap = true;
                chains.Enqueue(gap);
            } else if (rand < .6f || previousGap) { // Single platform
                FloorChain platform;
                platform.end = lastPlacement + Random.Range(5, 6);
                if (previousGap) {
                    platform.height = player.platformHeight;
                } else {
                    platform.height = player.platformHeight + 2.7f;
                }
                platform.isGap = false;
                chains.Enqueue(platform);
            } else { // Double platform
                FloorChain lower;
                lower.end = lastPlacement + Random.Range(5, 6);
                lower.height = player.platformHeight - 2;
                lower.isGap = false;

                FloorChain upper = lower;
                upper.height = player.platformHeight + 2.7f;

                chains.Enqueue(upper);
                chains.Enqueue(lower);
            }
        }
    }

    void PlaceWalls() {
        float height = 0;

        if (chains.Count > 0) {
            height = chains.Peek().height;
        }

        lastWallPlacement++;

        GameObject obj;


        int min = (int)(height / 4) - wallOff - 8;
        int max = (int)(height / 4) + wallOff;

        for (int i = min; i < max + 1; i++) {
            if (i == 0) {
                obj = Instantiate(wallLowerPrefab);
            } else if (i > 0) {
                obj = Instantiate(wallLowerPrefab);
            } else {
                obj = Instantiate(wallLowerPrefab);
            }

            obj.transform.position = new Vector3(lastWallPlacement * 2, i * 4, 3);
        }
    }

    void Place() {
        if (chains.Count == 0) {
            return;
        }

        if (calciumCountdown > 1) {
            calciumCountdown--;
        }
        
        GameObject obj;

        foreach (FloorChain chain in chains) {

            float height = chain.height;
            
            obj = Instantiate(chain.isGap ? floorGapPrefab : floorPrefab);
            obj.transform.position = new Vector3(lastPlacement * 2, height, 1);
            
            if (!chain.isGap) {
                lightCountdown--;
                if (lightCountdown <= 0) {
                    obj = Instantiate(lightPrefab);
                    obj.transform.position = new Vector3(lastPlacement * 2, height + .1f, 1);
                    lightCountdown = 5;
                }
            }
        }

        if (chains.Count > 1) {
            if (calciumCountdown <= 2) {
                calciumCountdown--;
            }

            if (calciumCountdown <= 0) {
                calciumCountdown = 90;

                float height = chains.ToArray()[Random.Range(0, chains.Count)].height;
                obj = Instantiate(calciumPrefab);
                obj.transform.position = new Vector3(lastPlacement * 2, height + 1f, 0);
            }
        } else {
            if (calciumCountdown <= 1) {
                calciumCountdown = 2;
            }
        }
    }
}
