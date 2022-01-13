using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
    [Header("Camera")]
    public GameObject cam;
    public GameObject camOrbit;
    public Vector3 camOffset = new Vector3(0, 0, 0);

    [Header("World Gen")]
    public int row;
    public int col;

    public GameObject tile1;
    public Vector3 tile1_rot;

    public GameObject tile2;
    public Vector3 tile2_rot;

    [Header("Scenery")]
    public GameObject borderDecor;
    public GameObject cornerDecor;
    public GameObject sceneryBrick;
    public int sceneryRange;

    public GameObject tree;
    public int treeCount, treeRange;
    
    public GameObject green;
    public int greenCount, greenRange;

    public GameObject mountains1, mounstains2;
    public int mountainsCount, mountainStartRange, mountainStopRange;

    // Start is called before the first frame update
    void Start()
    {
        GameObject worldMesh = new GameObject();
        GameObject boardGroup = new GameObject();
        GameObject boardDecor = new GameObject();
        GameObject boardScenery = new GameObject();
        GameObject boardTree = new GameObject();
        GameObject boardGreen = new GameObject();
        GameObject boardMountains = new GameObject();

        worldMesh.name = "World Mesh";
        boardGroup.name = "Game Board";
        boardDecor.name = "Decor";
        boardScenery.name = "Scenery";
        boardTree.name = "Trees";
        boardGreen.name = "Green";
        boardMountains.name = "Mountains";

        boardGroup.transform.parent = worldMesh.transform;
        boardDecor.transform.parent = worldMesh.transform;
        boardScenery.transform.parent = worldMesh.transform;
        boardTree.transform.parent = worldMesh.transform;
        boardGreen.transform.parent = worldMesh.transform;
        boardMountains.transform.parent = worldMesh.transform;

        GenerateBoard(row, col, boardGroup);
        GenerateDecor( row, col, sceneryRange, boardDecor);
        GenerateScenery(row, col, sceneryRange, boardScenery);
        GenerateTrees(row, col, treeCount, treeRange, boardTree);
        GenerateGreen(row, col, greenCount, greenRange, boardGreen);
        GenerateMountains(row, col, mountainsCount, mountainStartRange, mountainStopRange, boardMountains);
        CenterCam( boardGroup );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateBoard( int maxRow, int maxCol, GameObject group )
    {
        // 8Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
        bool flipColor = true;

        /*
         * Build game board
         */
        int i = 0;
        for (col = 0; col < maxCol; col++)
        {
            for (row = 0; row < maxRow; row++)
            {
                //  GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                // plane.transform.position = new Vector3(col, 0, row);
                GameObject tile;

                if (flipColor) {
                   tile = Commands.Instance.Spawn(tile1, new Vector3(col, 0, row), tile1_rot, "CanSpawn", null, group);
                }
                else {
                    tile = Commands.Instance.Spawn(tile2, new Vector3(col, 0, row), tile2_rot, "CanSpawn", null, group);
                }
                //tile.transform.localScale = scale;

               // tile.transform.parent = boardGroup.transform;
                tile.name = "BoardObject_" + i.ToString();
                //tile.tag = "CanSpawn";

                flipColor = !flipColor; // Alternate colors
                i++;
            }
        }
    }

    void GenerateDecor( int maxRow, int maxCol, int decorRange, GameObject group )
    {
        Vector3 spawnPos;
        for (col = -decorRange; col < maxCol; col++)
        {
            spawnPos = new Vector3(col, 0, -1);
            Commands.Instance.Spawn(borderDecor, spawnPos, new Vector3(-90, -90, 0), null, null, group);

            spawnPos = new Vector3(col, 0, maxRow);
            Commands.Instance.Spawn(borderDecor, spawnPos, new Vector3(-90, 90, 0), null, null, group);
        }

        spawnPos = new Vector3(maxCol, 0, -1);
        Commands.Instance.Spawn(cornerDecor, spawnPos, new Vector3(-90, 180, 0), null, null, group);

        spawnPos = new Vector3(maxCol, 0, maxRow);
        Commands.Instance.Spawn(cornerDecor, spawnPos, new Vector3(-90, -90, 0), null, null, group);

        for( int i = 1; i < decorRange; i++)
        {
            spawnPos = new Vector3(maxCol, 0, -1 - i);
            Commands.Instance.Spawn(borderDecor, spawnPos, new Vector3(-90, 180, 0), null, null, group);

            spawnPos = new Vector3(maxCol, 0, maxRow + i);
            Commands.Instance.Spawn(borderDecor, spawnPos, new Vector3(-90, 0, 0), null, null, group);
        }
    }

    void GenerateScenery( int maxRow, int maxCol, int sceneryRange, GameObject group )
    {
        int yRot = 0;
        Vector3 spawnPos;
        for (col = 0; col < maxCol; col++)
        {
            for( int row = maxRow + 1; row < maxRow + sceneryRange; row++)
            {
                if (Random.Range(0, 10) == 0)
                    yRot = 90;
                else
                    yRot = 0;

                spawnPos = new Vector3(col, 0, row);
                Commands.Instance.Spawn(sceneryBrick, spawnPos, new Vector3(-90, yRot, 0), null, null, group);
            }

            for (int row = 2; row < sceneryRange + 1; row++)
            {
                if (Random.Range(0, 10) == 0)
                    yRot = 90;
                else
                    yRot = 0;

                spawnPos = new Vector3(col, 0, -row);
                Commands.Instance.Spawn(sceneryBrick, spawnPos, new Vector3(-90, yRot, 0), null, null, group);
            }


            //spawnPos = new Vector3(col, 0, maxRow);
            //Commands.Instance.Spawn(sceneryBrick, spawnPos, new Vector3(-90, 90, 0), null, null, null);
        }
    }

    void GenerateTrees(int row, int col, int treeCount, int treeRange, GameObject group)
    {
        while( true )
        {
            if( treeCount == 0)
            {
                break;
            }
            float x = Random.Range(-treeRange, treeRange);
            float z = Random.Range(-treeRange, treeRange);
            if (z > row + sceneryRange || z < -4 - sceneryRange || x < 0 - sceneryRange || x > col)
            {
                float y_rot = Random.Range(0, 360);
                float scale = Random.Range(0.005f, 0.015f);

                Vector3 offset = new Vector3(0, y_rot, 0);
                Vector3 rotation = tree.transform.rotation.eulerAngles + offset;

                GameObject tree_go = Instantiate(tree, new Vector3(x, 0, z), Quaternion.Euler(rotation));

                tree_go.transform.parent = group.transform;
                tree_go.transform.localScale = new Vector3(scale, scale, scale);

                treeCount--;
            }
        }
    }
    
    void GenerateGreen(int row, int col, int greenCount, int greenRange, GameObject group)
    {
        while( true )
        {
            if( greenCount == 0)
            {
                break;
            }
            float x = Random.Range(-greenRange, greenRange);
            float z = Random.Range(-greenRange, greenRange);
            if (z > row + sceneryRange || z < -4 - sceneryRange || x < 0 - sceneryRange || x > col)
            {
                float y_rot = Random.Range(0, 360);
                float scale = Random.Range(0.5f, 1.5f);

                Vector3 offset = new Vector3(0, y_rot, 0);
                Vector3 rotation = green.transform.rotation.eulerAngles + offset;

                GameObject green_go = Instantiate(green, new Vector3(x, 0, z), Quaternion.Euler(rotation));

                green_go.transform.parent = group.transform;
                green_go.transform.localScale = new Vector3(scale, scale, scale);

                greenCount--;
            }
        }
    }   
    
    void GenerateMountains(int row, int col, int mountainsCount, int rangeStart, int rangeEnd, GameObject group)
    {
        int mountainRange = sceneryRange + rangeStart;
        while (true)
        {
            if (mountainsCount == 0)
            {
                break;
            }
            float x = Random.Range(-rangeEnd, rangeEnd);
            float z = Random.Range(-rangeEnd, rangeEnd);
            if (z > row + mountainRange || z < -4 - mountainRange || x < 0 - mountainRange || x > col)
            {
                float y_rot = Random.Range(0, 360);
                float scale = Random.Range(500f, 1500f);

                Vector3 offset = new Vector3(0, y_rot, 0);
                Vector3 rotation = mountains1.transform.rotation.eulerAngles + offset;

                GameObject green_go = Instantiate(mountains1, new Vector3(x, 0, z), Quaternion.Euler(rotation));

                green_go.transform.parent = group.transform;

                scale = (Mathf.Abs(x) + Mathf.Abs(z)) * 20;
                green_go.transform.localScale = new Vector3(scale, scale, scale);

                mountainsCount--;
            }
        }
    }

    void CenterCam(GameObject boardGroup )
    {
        float x = 0;
        float z = 0;
        int childCount = boardGroup.transform.childCount;

        foreach (Transform child in boardGroup.transform)
        {
            x += child.position.x;
            z += child.position.z;
        }

        var avgPos = new Vector3(x / childCount, 0, z / childCount);
        cam.transform.position = avgPos + camOffset;
        cam.transform.LookAt(avgPos);

        camOrbit.transform.position = avgPos;
    }
}
