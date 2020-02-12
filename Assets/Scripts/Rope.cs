using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject ropeSegmentPrefab;

    List<GameObject> ropeSegments = new List<GameObject>();

    public bool isIncreasing { get; set; }
    public bool isDecreasing { get; set; }

    public Rigidbody2D connectedObject;

    public float maxRopeSegmentLenght = 1.0f;

    public float ropeSpeed = 4.0f;

    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        ResetLength();
    }

    void ResetLength()
    {
        foreach (GameObject segment in ropeSegments)
        {
            Destroy(segment);     
        }

        ropeSegments = new List<GameObject>();

        isIncreasing = false;
        isDecreasing = false;

        CreateRopeSegment();
    }

    void CreateRopeSegment()
    {
        GameObject segment = (GameObject)Instantiate(
            ropeSegmentPrefab,
            this.transform.position,
            Quaternion.identity);

        segmen.transform.setParent(this.transform, true);

        Rigidbody2D segmentBody = segment.GetComponent<Rigidbody2D>;

        SpringJoint2D segmentJoint = segment.GetComponent<SpringJoint2D>();

        if (segmentBody == null || segmentJoint == null)
        {
            Debug.LogError("Rope segment body prefab has no Rigidbody and/or SpringJoint2D!");

            return;
        }

        ropeSegments.Insert(0, segment);

        if (ropeSegments.Count == 1)
        {
            SpringJoint2D
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
