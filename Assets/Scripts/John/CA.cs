using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA : MonoBehaviour
{
    /*
    uniformity
    repetition
    random
    complexity
    */

    public GameObject baseObject;
    GameObject[] cellObjs;
    int[] cells;
    int generation;

    public int Rule;
    public int Binary;


    public int[] ruleset = {0,0,0,1,1,1,1,0 };  //rule 30

    int width = 64;

    float timer; 


    // Start is called before the first frame update
    void Start()
    {
   

        //deactivate the base object, it is there only for cloning,
        //and to see it in the editor
        baseObject.SetActive(false);

        Reset();

        timer = Time.time;
        generation = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timer > 0.1f)
        {
            timer = Time.time;
            //lets keep it square
            if (generation < width / 2)
            {
                generate();
            }            
            
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Reset();
            timer = Time.time;
        }

    }
    // The process of creating the new generation
    void generate()
    {

        //create the next row of cellObjs
        InstantiateGeneration();

        // First we create an empty array for the new values
        int[] nextgen = new int[width];
        // For every spot, determine new state by examing current state, and neighbor states
        
        // Ignore edges that only have one neighor
        for (int i = 1; i < width - 1; i++)
        {
            int left = cells[i - 1];   // Left neighbor state
            int me = cells[i];       // Current state
            int right = cells[i + 1];  // Right neighbor state
            nextgen[i] = Rules(left, me, right); // Compute next generation state based on ruleset
        }
        // The current generation is the new generation
        cells = nextgen;
        

        for(int i = 0; i < width; i++)
        {
            if (cells[i] == 1)
                cellObjs[i].SetActive(true);
            else
                cellObjs[i].SetActive(false);
        }

        generation++;
    }

    void InstantiateGeneration()
    {
        for (int i = 0; i < width; i++)
        {
            cellObjs[i] = Instantiate(baseObject);
            cellObjs[i].transform.parent = transform;
            cellObjs[i].transform.localPosition = Vector3.right * i + Vector3.forward * generation;
            cellObjs[i].SetActive(false);
        }
    }

    int Rules(int a, int b, int c)
    {
        if(false)
        {

            if (a == 1 && b == 1 && c == 1) return ruleset[7];
            if (a == 1 && b == 1 && c == 0) return ruleset[6];
            if (a == 1 && b == 0 && c == 1) return ruleset[5];
            if (a == 1 && b == 0 && c == 0) return ruleset[4];
            if (a == 0 && b == 1 && c == 1) return ruleset[3];
            if (a == 0 && b == 1 && c == 0) return ruleset[2];
            if (a == 0 && b == 0 && c == 1) return ruleset[1];
            if (a == 0 && b == 0 && c == 0) return ruleset[0];


        }
        else
        {
            if (a == 1 && b == 1 && c == 1) return ruleset[0];
            if (a == 1 && b == 1 && c == 0) return ruleset[1];
            if (a == 1 && b == 0 && c == 1) return ruleset[2];
            if (a == 1 && b == 0 && c == 0) return ruleset[3];
            if (a == 0 && b == 1 && c == 1) return ruleset[4];
            if (a == 0 && b == 1 && c == 0) return ruleset[5];
            if (a == 0 && b == 0 && c == 1) return ruleset[6];
            if (a == 0 && b == 0 && c == 0) return ruleset[7];

        }
        return 0;
    }

    private void Reset()
    {
        Random.seed = Time.frameCount;

        //delete old stuff
        foreach(Transform obj in transform)
        {
            Destroy(obj.gameObject);
        }

        cellObjs = new GameObject[width];
        cells = new int[width];

        //create the first generation
        generation = 0;
        InstantiateGeneration();
        generation = 1;

        //start with the center cellObj/cell alive
        cellObjs[width / 2].SetActive(true);
        cells[width / 2] = 1;
        
        //or start with random start positions
        if(false)
        {
            for (int i = 0; i < width; i++)
            {
                int dice = Random.Range(1, 11); //ten sided
                if (dice == 6)
                {
                    cells[i] = 1;
                    cellObjs[i].SetActive(true);
                }

            }
        }

        
        if (false)
        {
            //lets pick a random ruleset
            for (int i = 0; i < 8; i++)
            {
                ruleset[i] = Random.Range(0, 2);
            }
        }

        if (true)
        {

            //convert the decimal rule to binary and parse
            Binary = decimal_to_binary(Rule);
            //clear the array
            for (int i = 0; i < 8; i++)
            {
                ruleset[i] = 0;
            }
            string B = Binary.ToString();
            char[] C = B.ToCharArray();
            int d = 7;
            for(int i = C.Length - 1; i > -1 ; i--)
            {
                ruleset[d] = (int)char.GetNumericValue(C[i]);
                d--;
            }



        }

    }

    int decimal_to_binary(int n)
    {
        int binary = 0;
        int remainder, i, flag = 1;
        for (i = 1; n != 0; i = i * 10)
        {
            remainder = n % 2;
            n /= 2;
            binary += remainder * i;
        }
        return binary;
    }



}
