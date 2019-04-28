using System.Collections;
using System.Collections.Generic;
using Bevel;
using UnityEngine;

public class MoveGizmo : MonoBehaviour {

    public enum Direction
    {
        X, Y, Z, XZ, Rotate
    }

    public GameObject movementParent;

    public GameObject planeFloor;
    public GameObject planeXY;
    public GameObject planeZY;

    public LayerMask planeLayer;

    private Vector3 previousLocation; // get rid of this
    private Vector3 previousRotationPoint;
    private Vector3 relativeHitPosition;

	// Use this for initialization
	void Start () {
		planeFloor.SetActive(false);
        planeXY.SetActive(false);
        planeZY.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        //if (!BevelInput.isSomethingStillPressed())
        //{
        //    StopMove();
        //}
	}

    public void StartMove(Direction direction)
    {
        Debug.Log("Starting Move");
        RaycastHit hit;
        if (Physics.Raycast(BevelInput.GetPrimaryControlRay(),out hit, 20f, planeLayer))
        {
            Debug.Log("Raycast has hit " + hit.point.ToString());
            relativeHitPosition = hit.point - movementParent.transform.position;
            previousRotationPoint = hit.point;
            StartCoroutine(MoveStuff(direction));
        }
        
    }

    public void StartMoveX()
    {
        Debug.Log("Start Move X");
        planeFloor.SetActive(true);
        planeXY.SetActive(true);

        StartMove(Direction.X);
    }

    public void StartMoveZ()
    {
        Debug.Log("Start Move Z");
        planeFloor.SetActive(true);
        planeZY.SetActive(true);
        StartMove(Direction.Z);
    }

    public void StartMoveXZ()
    {
        Debug.Log("Start Move XZ");
        planeFloor.SetActive(true);
        StartMove(Direction.XZ);
    }

    public void StartMoveY()
    {
        Debug.Log("Start Move Y");
        planeXY.SetActive(true);
        planeZY.SetActive(true);
        StartMove(Direction.Y);
    }

    public void StartRotate()
    {
        Debug.Log("Start Rotate");
        planeFloor.SetActive(true);
        StartMove(Direction.Rotate);
    }

    public void StopMove()
    {
        Debug.Log("Stopping Move");
        planeFloor.SetActive(false);
        planeZY.SetActive(false);
        planeXY.SetActive(false);

        StopAllCoroutines();

    }

    IEnumerator MoveStuff(Direction direction)
    {
        Debug.Log("coroutine started");
        while (true)
        {
            Debug.Log("coroustine iteration");
            RaycastHit hit;
            if (Physics.Raycast(BevelInput.GetPrimaryControlRay(), out hit, 20f, planeLayer))
            {
                Vector3 deltaPosition = hit.point - previousLocation;

                if (direction == Direction.X || direction == Direction.XZ)
                {
                    //movementParent.transform.Translate(new Vector3(deltaPosition.x, 0, 0));
                    movementParent.transform.position = new Vector3(
                        hit.point.x - relativeHitPosition.x,
                        movementParent.transform.position.y, 
                        movementParent.transform.position.z);
                }

                if (direction == Direction.Z || direction == Direction.XZ)
                {
                    //movementParent.transform.Translate(new Vector3(0,0,deltaPosition.z));
                    movementParent.transform.position = new Vector3(
                        movementParent.transform.position.x,
                        movementParent.transform.position.y,
                        hit.point.z - relativeHitPosition.z);

                }

                if (direction == Direction.Y)
                {
                    //movementParent.transform.Translate(new Vector3(0, deltaPosition.y, 0));
                    movementParent.transform.position = new Vector3(
                        movementParent.transform.position.x,
                        hit.point.y - relativeHitPosition.y,
                        movementParent.transform.position.z);

                }

                if (direction == Direction.Rotate)
                {
                    Vector3 previousAngleSource = previousRotationPoint - movementParent.transform.position;
                    Vector3 newAngleSource = hit.point - movementParent.transform.position;
                    //float angle = Vector3.Angle(newAngleSource, previousAngleSource);
                    float angle = hit.point.x - previousLocation.x ;

                    //movementParent.transform.eulerAngles = new Vector3(0,angle,0);
                    movementParent.transform.Rotate(this.transform.up, angle);

                    previousRotationPoint = hit.point;
                }

            }



            yield return null;
        }
    }

}
