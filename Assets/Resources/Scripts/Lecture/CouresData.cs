using System;
using UnityEngine;

[Serializable]
public class CourseData
{
    public int id;
    public int semester;
    public string subject;
    public string taskName;

    public int assignmentCount = 0;
    public bool isExamCleared = false;

    public int CurrentAssignmentCost
    {
        get => 200 + (assignmentCount * 200);
    }
}