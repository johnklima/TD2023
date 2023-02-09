using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private Transform poofs;
    private bool firstHit = true;

    public float G = 9.8f;
    public Vector3 direction;

    public Transform start;
    public Transform end;

    public BallGravity grav;
    public bool inAir;
    public float launchAngle = 45;

    Camera cam;    
    [SerializeField] Transform player;
    [SerializeField] Transform playerCharacter;
    [SerializeField] Transform CannonRoot;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        grav = GetComponent<BallGravity>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !inAir)
        {
            //force player model to face forward
            playerCharacter.localRotation = Quaternion.identity;

            //take aim
            // follow the player as if I were a child
            CannonRoot.position = player.position;
            CannonRoot.rotation = player.rotation;

            //build exclusion layer mask
            int layerMask = (1 << 10);
            layerMask |= (1 << 5);
            layerMask |= (1 << 3);
            layerMask = ~layerMask; //not spore, not player, not UI

            Vector3 pos = cam.transform.position + cam.transform.forward;
            Vector3 fwd = cam.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(pos, fwd, out hit, 3000, layerMask))
            {
                //player facing is independent of player position
                Vector3 v1 = hit.point - player.position;
                v1.Normalize();


                if (Vector3.Dot(v1, fwd) > 0.3f && hit.distance > 1.0f)
                {
                    end.position = hit.point;
                    end.LookAt(cam.transform.position);
                }

            }






            firstHit = true;
            inAir = true;
            
            //lift up and forward
            transform.position = start.position;
            transform.position += Vector3.up + transform.forward;

            transform.LookAt(end);            
            grav.enabled = true;
            grav.impulse = fire(transform.position, end.position, launchAngle);

            
            //the puffball
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            end.gameObject.SetActive(false);
        }
    }


    public Vector3 fire(Vector3 startPos, Vector3 targPos, float angle)
    {

        direction = targPos - startPos;
        return calculateIterativeTrajectory(startPos, targPos, angle, false);


    }

    Vector3 calculateIterativeTrajectory(Vector3 startPoint, Vector3 endPoint, float desiredAngle, bool checkAngle)
    {

        Vector3 t;
        Vector3 c;

        t = endPoint;
        c = startPoint;

        //get a flat distance
        t.y = c.y = 0;
        float flatdistance = Vector3.Distance(t, c);

        Vector3 P1, P2, p2;
        p2 = P2 = endPoint - startPoint;

        //get just the flat direction without y component
        p2.y = 0;
        p2.Normalize();
        P1 = p2;

        //unitize the vector
        P2.Normalize();


        float angle = 0;

        //if we are asked to use a specific angle, this is our angle, for better or worse        
        angle = Mathf.Deg2Rad * desiredAngle;

        //add a bit of inclination just in case
        angle += Mathf.Acos(Vector3.Dot(P1, Vector3.up)) * 0.15f;

        //get direct angle in rads, this is a minimum launch angle
        float directAngle = Mathf.Acos(Vector3.Dot(P1, P2));

        if (angle < directAngle)
            angle = directAngle;

        //any angle less than 45 is 45 (optimal angle)
        //any angle greater is the angle, plus HALF the angle of the vector to y up.
        //with balistics the angle ALWAYS has to be greater than the direct angle from
        //point to point. so we add half of this angle to 90, splitting the difference
        if (checkAngle)
        {

            if (angle < 0.785398163f)
                angle = 0.785398163f;


            //if it is too close to pure vertical make it less than pure vertical
            if (angle > Mathf.PI / 2 - 0.05f)
                angle = Mathf.PI / 2 - 0.05f;

            //if we are going from up to down, use direct angle
            if (startPoint.y > endPoint.y)
                angle = directAngle;

        }

        float Y = endPoint.y - startPoint.y;


        // perform the trajectory calculation to arrive at the gun powder charge aka 
        // target velocity for the distance we desire, based on launch angle
        float rng = 0;
        float Vo = 0;
        float trydistance = flatdistance;
        int iters = 0;

        //now iterate until we find the correct proposed distance to land on spot.
        //this method is based on the idea of calculating the time to peak, and then
        //peak to landing at the height differential. we can then extract an XY distance 
        //achieved regardless of height differential. by iterating with a binary heuristic
        //we increase or decrease the initial velocity until we acheive our XY distance

        angle = Mathf.Abs(angle); // no negative numbers please!
        
        float f = (Mathf.Sin(angle * 2.0f));


        while (Mathf.Abs(rng - flatdistance) > 0.001f && iters < 64)
        {
            //make sure we dont squirt on a negative number. we can IGNORE that result
            if (trydistance > 0)
            {

                //---------------create an initial force-------------------------
                //-----------( / f seems to do nothing??? )----------------------
                //-it behaves as a constant and Vo is adjusted in relation to f?-
                //---------------------------------------------------------------
                
                float sqrtcheck = (trydistance * G) / f;

                if (sqrtcheck > 0)
                    Vo = Mathf.Sqrt(sqrtcheck);
                else
                    Debug.Log("sqrtcheck < 0 initial force");

                //find the vector of it in our trajectory planar space
                float Vy = Vo * Mathf.Sin(angle);
                float Vx = Vo * Mathf.Cos(angle);

                //get a height and thus time to peak
                float H = -Y + (Vy * Vy) / (2 * G);
                float upt = Vy / G;                      // time to max height

                //if again we squirt on a neg, but it is because our angle and force
                //are too accute. note: we handle up and down trajectory differently
                if (2 * H / G < 0)
                {
                    if (endPoint.y < startPoint.y)
                        rng = flatdistance;       //if going down we are done
                    else
                        rng = 0;       //if up we are not
                }
                else
                {
                    sqrtcheck = 2 * H / G;
                    if (sqrtcheck > 0)
                    {
                        float dnt = Mathf.Sqrt(sqrtcheck);           // time from max height to impact
                        rng = Vx * (upt + dnt);
                    }
                    else
                        Debug.Log("sqrtcheck < 0 H / Gravity");
                }

                if (rng > flatdistance)
                    trydistance -= (rng - flatdistance) / 2;		//using a binary zero-in, it takes about 8 iterations to arrive at target
                else if (rng < flatdistance)
                    trydistance += (flatdistance - rng) / 2;


                //Debug.Log("ITERS = " + iters);
            }
            else
            {

                iters = 64;
            }
            iters++;
        }

        Vector3 angV = new Vector3(direction.x, 0, direction.z);

        angV.Normalize();
        Vector3 side = Vector3.Cross(angV, Vector3.up);
        side.Normalize();


        //we need to rotate that by our actual launch angle
        angV = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, side) * angV;

        //multiply by calculated "powder charge"
        angV *= Vo;

        Debug.Log(Vo.ToString());

        return angV;

    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.layer != LayerMask.NameToLayer("Player")            &&
            obj.layer != LayerMask.NameToLayer("Ignore Raycast")    &&
            obj.layer != LayerMask.NameToLayer("Water")             &&
            obj.layer != LayerMask.NameToLayer("UI")                &&
            obj.layer != LayerMask.NameToLayer("Spore")             &&
            inAir                                                     )
        {
            Debug.Log("ball hit " + other.name);

            if (other.GetComponent<Enemy>())
                other.GetComponent<Enemy>().enemyHealthSystem.DealDamage(1);

            if (firstHit)
            {
                firstHit = false;
                end.gameObject.SetActive(true);
                grav.GetComponent<SphereCollider>().radius = 2f; //EXPLOSION!!!! size
                
                if (poofs)
                    StartCoroutine(Poof());
                if (transform.GetChild(0).GetComponent<MeshRenderer>())
                    StartCoroutine(FirstHit());
            }
        }
    }

    private IEnumerator FirstHit()
    {
        for (int i = 0; i < 2; i++)
            yield return null;
        
        grav.reset();
        grav.enabled = false;
        inAir = false;
        transform.localPosition = Vector3.zero;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;  //the puffball
        
    }

    private IEnumerator Poof()
    {
        for (int i = 0; i < poofs.childCount; i++)
        {
            GameObject puffPoof = poofs.GetChild(i).gameObject;
            if (!puffPoof.activeSelf)
            {
                puffPoof.SetActive(true);
                puffPoof.transform.position = grav.transform.position;
                
                yield return new WaitForSeconds(1);
                puffPoof.SetActive(false);

                break;
            }
        }
    }
}