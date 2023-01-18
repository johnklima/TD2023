using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOL : MonoBehaviour
{

    public GameObject baseobject;


    const int MAX_ROWS = 32;
    const int MAX_COLUMNS = 32;

    //2d array of ints
    int[,] cells;
    GameObject[,] objs;


    // Start is called before the first frame update
    void Start()
    {

        Random.seed = 1325152;

        //redimension our array
        cells = new int[MAX_ROWS, MAX_COLUMNS];
        objs = new GameObject[MAX_ROWS, MAX_COLUMNS];

        
        for (int row = 0; row < MAX_ROWS; row++)
        {
            for (int col = 0; col < MAX_COLUMNS; col++)
            {
                //create cells
                objs[row, col] = GameObject.Instantiate(baseobject, transform);
                float space = baseobject.transform.localScale.x;
                Vector3 pos = new Vector3(row * space, 0, col * space );
                objs[row, col].transform.position = pos;

                //init cells
                int state =  Random.Range(0, 2);
                if (state == 1)
                {
                    objs[row, col].SetActive(true);
                    cells[row, col] = 1;
                }                    
                else
                {
                    objs[row, col].SetActive(false);
                    cells[row, col] = 0;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        generateGOL();
        
    }

    //next generation game of life
    void generateGOL()
    {
        int[,] next = new int[MAX_ROWS, MAX_COLUMNS];

        // Loop through every spot in our 2D array and check spots neighbors
        for (int row = 1; row < MAX_ROWS - 1; row++)
        {

            for (int col = 1; col < MAX_COLUMNS - 1; col++)
            {

                // Add up all the states in a 3x3 surrounding grid, not including where i am now
                int neighbors = 0;

                neighbors += cells[row - 1, col];
                neighbors += cells[row + 1, col];
                neighbors += cells[row, col - 1];
                neighbors += cells[row, col + 1];
                neighbors += cells[row + 1, col + 1];
                neighbors += cells[row + 1, col - 1];
                neighbors += cells[row - 1, col + 1];
                neighbors += cells[row - 1, col - 1];


                // Rules of Life
                if ((cells[row, col] == 1) && (neighbors < 2))				// Loneliness 
                    next[row, col] = 0;
                else if ((cells[row, col] == 1) && (neighbors > 3))     // Overpopulation
                    next[row, col] = 0;
                else if ((cells[row, col] == 0) && (neighbors == 3))        // Reproduction 
                    next[row, col] = 1;
                else                                                        // Stasis         
                    next[row, col] = cells[row, col];
            }
        }

        //now swap new values for old
        for (int row = 1; row < MAX_ROWS - 1; row++)
        {
            for (int col = 1; col < MAX_COLUMNS - 1; col++)
            {

                cells[row, col] = next[row, col];

                if (cells[row, col] == 1)
                    objs[row, col].SetActive(true);
                else
                    objs[row, col].SetActive(false);

            }
        }

    }
}
