using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Minesweeper : MonoBehaviour
{
    public int mapHeight = 30;
    public int mapWidth = 30;
    public int totalBomb = 186;
    public bool defaultRatio = false;

    public GameObject blockPrefab;
    public Transform point;

    List<Block> map = new List<Block>();

    private Camera camera;
    private bool isFirst = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        setUpMap();
        //setUpBomb();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Block b = hit.collider.gameObject.GetComponent<Block>();
                if (!isFirst)
                {
                    isFirst = true;
                    setUpBomb(b);
                }

                if (b.bombNumber == 0)
                {
                    RecursionTrigger(new List<Block>(),b);
                }
                
            }
        }
    }
    
    void setUpMap()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                GameObject tempObject = GameObject.Instantiate(blockPrefab, new Vector2(point.position.x+(float)i, point.position.y + (float)j), Quaternion.identity, point);
                Block tempBlock = tempObject.GetComponent<Block>();
                tempBlock.x = i;
                tempBlock.y = j;
                map.Add(tempObject.GetComponent<Block>());
            }
        }
    }

    void setUpBomb(Block firstBlock)
    {
        isFirst = true;
        if (defaultRatio)
            totalBomb = (int)(0.20625 * (float)mapWidth * (float)mapHeight) ;

        for (int i = 0; i < totalBomb; i++)
        {
            int x = Random.Range(0, 29);
            int y = Random.Range(0, 29);

            if((map[getIndexFromPosition(x, y)].isBomb)
                || (x == firstBlock.x && y == firstBlock.y)
                )
            {
                i--;
            }
            else
            {
                map[getIndexFromPosition(x, y)].isBomb = true;
            }
        }
        setNumber();
    }

    void setNumber()
    {
        foreach(Block b in map)
        {
            b.bombNumber = getTotalBomb(b);
        }
    }

    int getTotalBomb(Block b)
    {
        int total = 0;

        for (int i = -1; i <= 1; i++)
        {
            if(
                (b.x == 0 && i == -1)
                || (b.x == mapHeight-1 && i == 1)
                )
            {
                continue;
            }
            for (int j = -1; j <= 1; j++)
            {
                if(
                    (i == 0 && j == 0)
                    || (b.y == 0 && j == -1)
                    || (b.y == mapWidth-1 && j == 1)
                  )
                {
                    continue;
                }

                if (map[getIndexFromPosition(b.x+i, b.y + j)].isBomb)
                {
                    total++;
                }
            }
        }

        return total;
    }

    void RecursionTrigger(List<Block> openedBlock, Block b)
    {
        openedBlock.Add(b);
        for (int i = -1; i <= 1; i++)
        {
            if (
                (b.x == 0 && i == -1)
                || (b.x == mapHeight - 1 && i == 1)
                )
            {
                continue;
            }
            for (int j = -1; j <= 1; j++)
            {
                if (
                    (i == 0 && j == 0)
                    || (b.y == 0 && j == -1)
                    || (b.y == mapWidth - 1 && j == 1)
                  )
                {
                    continue;
                }

                Block mB = map[getIndexFromPosition(b.x + i, b.y + j)];

                mB.isOpen = true;

                if(openedBlock.Any<Block>((e)=>e.x == mB.x && e.y == mB.y))
                {
                    continue;
                }

                if (mB.bombNumber == 0)
                {
                    RecursionTrigger(openedBlock,mB);
                }
            }
        }
    }

    int getIndexFromPosition(int x, int y)
    {
        return x * mapWidth + y;
    }
}
