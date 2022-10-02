using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 15.0f;
    private bool isTravelling;
    private Vector3 traveDirection;
    private Vector3 nextCollisionPosition;

    private int minSwipeRecognition = 500;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;

    private Color solveColor;
    // Start is called before the first frame update
    void Start()
    {
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (isTravelling)
        {
            rb.velocity = speed * traveDirection;
        }


        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        int i = 0;
        while (i < hitColliders.Length)
        {
           GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
           if (ground && !ground.isColored)
           {
               ground.ChangeColor(solveColor);
           }
          i++;
        }

        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTravelling = false;
                traveDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }
        if (isTravelling) return;

        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;
                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }
                currentSwipe.Normalize();

                // Up or Down

                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
                {
                    // GO UP/DOWN
                    Destination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5)
                {
                    // GO LEFT/RIGHT
                    Destination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }

            }
            swipePosLastFrame = swipePosCurrentFrame;

            if (Input.GetMouseButtonUp(0))
            {
                swipePosLastFrame = Vector2.zero;
                currentSwipe = Vector2.zero;
            }
        }


    }

    private void Destination(Vector3 direction)
    {
        traveDirection = direction;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }
        isTravelling = true;
    }
}
