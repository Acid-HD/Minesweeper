using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int bombNumber;
    public bool isBomb;
    public int x;
    public int y;
    public bool isOpen;
    public TextMesh mesh;
    public GameObject blockObject;

    public Sprite zeroSprite;
    public Sprite bombSprite;

    private void Awake()
    {
        mesh = transform.GetComponent<TextMesh>();
        blockObject = transform.gameObject;
        isOpen = false;
        bombNumber = 0;
        x = 0;
        y = 0;
        isBomb = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {

            if (isBomb)
            {
                transform.GetComponentInChildren<SpriteRenderer>().sprite = bombSprite;
                mesh.text = "B";
            }
            else if(bombNumber == 0)
            {
                transform.GetComponentInChildren<SpriteRenderer>().sprite = zeroSprite;
                mesh.text = "";
            }
            else
            {
                transform.GetComponentInChildren<SpriteRenderer>().sprite = zeroSprite;
                mesh.text = bombNumber + "";
            }
        }
        else
        {
            mesh.text = "";
        }
    }

    private void OnMouseDown()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        if (isBomb)
        {
            Debug.Log("ITS BOMB");
        }
    }
}
