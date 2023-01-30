using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomGrowthEffectGOL : MonoBehaviour
{
    [SerializeField] private Transform objectToSpawn;
    [SerializeField] private int maxRows, maxCol;
    private float objectSize;
    public bool startGrowing;


    //seed 528 worked alrighty
    [SerializeField] private int seed;
    private float timer, timerMax = .2f;

    private int[,] cellsArray;
    [SerializeField] private Transform[,] objectsArray;


    private void Start()
    {
        Random.seed = seed;
        timer = timerMax;

        objectSize = objectToSpawn.localScale.x;
        cellsArray = new int[maxRows, maxCol];
        objectsArray = new Transform[maxRows, maxCol];

        //SLETT SENERE!!!!!!!!!!!!!
        Initialize();
        //SLETT SENERE!!!!!!!!!!!!!
    }

    private void Update()
    {
        if (startGrowing)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                InitNextGeneration();
                timer = timerMax;
            }
        }
        
    }

    private void InitNextGeneration()
    {
        //the new generation array will
        int[,] nextGenerationArr = new int[maxRows, maxCol];

        for (int row = 1; row < maxRows - 1; row++)
        {

            for (int col = 1; col < maxCol - 1; col++)
            {
                int neighboursAmount = 0;

                //neighbour counting, check every nearby cell and check if that cell is active or not, if the cell is active (meaning it is 1) then it will
                //add 1 to the neighbourAmount
                neighboursAmount += cellsArray[row - 1, col];
                neighboursAmount += cellsArray[row + 1, col];
                neighboursAmount += cellsArray[row, col - 1];
                neighboursAmount += cellsArray[row, col + 1];
                neighboursAmount += cellsArray[row + 1, col + 1];
                neighboursAmount += cellsArray[row + 1, col - 1];
                neighboursAmount += cellsArray[row - 1, col + 1];
                neighboursAmount += cellsArray[row - 1, col - 1];


                if (cellsArray[row, col] == 1 && (neighboursAmount < 2 || neighboursAmount > 3))
                    nextGenerationArr[row, col] = 0;
                else if (cellsArray[row, col] == 0 && neighboursAmount == 3)
                    nextGenerationArr[row, col] = 1;
                else
                    nextGenerationArr[row, col] = cellsArray[row, col];
            }
        }


        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {

                cellsArray[row, col] = nextGenerationArr[row, col];

                if (cellsArray[row, col] == 1)
                    objectsArray[row, col].gameObject.SetActive(true);
                else
                    objectsArray[row, col].gameObject.SetActive(false);

            }
        }
    }

    public void Initialize()
    {
        //create a grid based on the cellsArray by using a 2 for-loops, to access each element in the 2Dimensional array 2 loops are needed

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                //instantiate and store an object in the objectsArray list

                objectsArray[row, col] = Instantiate(objectToSpawn, transform);
                Vector3 newPosition = new Vector3(row * objectSize, 0f, col * objectSize);
                objectsArray[row, col].transform.position = newPosition;

                //randomly determine which ones will be active at start
                int state = Random.Range(0, 2);
                if (state == 1)
                {
                    objectsArray[row, col].gameObject.SetActive(true);
                    cellsArray[row, col] = 1;
                }
                else
                {
                    objectsArray[row, col].gameObject.SetActive(false);
                    cellsArray[row, col] = 0;
                }

            }
        }
    }

}
