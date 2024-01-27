using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Enemy1Move : MonoBehaviour
{
    [SerializeField] private Enemy1 enemy1;
    public PathCreator pathCreator;
    [SerializeField] private float speed = 6f;
    
    private Vector3 endPos;
    private float moveDistance;
    private int directionFlg = 1;
    private bool endFlg = false;

    void Start()
    {
        endPos = pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1);
    }

    void Update()
    {
        if(endFlg) return;
        moveDistance += speed * directionFlg * Time.deltaTime;
        if(Vector3.Distance(transform.position, endPos) < 0.001f)
        {
            endFlg = true;
        }
        transform.position = pathCreator.path.GetPointAtDistance(moveDistance, EndOfPathInstruction.Stop);
        transform.rotation = pathCreator.path.GetRotationAtDistance(moveDistance, EndOfPathInstruction.Stop) * Quaternion.Euler(0, 0, 90);
    }
}
