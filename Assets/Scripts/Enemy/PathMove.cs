using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathMove : MonoBehaviour
{
    [SerializeField] PathCreator pathCreator;
    [SerializeField] float speed = 6f;
    
    private Vector3 endPos;
    private float moveDistance;
    private int directionFlg = 1;

    void Start()
    {
        endPos = pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1);
    }

    void Update()
    {
        moveDistance += speed * directionFlg * Time.deltaTime;
        // if(Vector3.Distance(transform.position, pathCreator.path.GetPointAtDistance(moveDistance, EndOfPathInstruction.Stop)) < 0.001f) directionFlg *= -1;
        transform.position = pathCreator.path.GetPointAtDistance(moveDistance, EndOfPathInstruction.Stop);
        transform.rotation = pathCreator.path.GetRotationAtDistance(moveDistance, EndOfPathInstruction.Stop) * Quaternion.Euler(0, 0, 90);
    }
}
