using System;
using System.Collections.Generic;
using UnityEngine;

public static class CSVLoader
{
    public static List<CourseData> LoadFromResources(string resourcePath = "CourseTable")
    {
        TextAsset csv = Resources.Load<TextAsset>(resourcePath);
        if (csv == null)
        {
            Debug.LogError($"[CsvCourseLoader] CSV 파일을 찾을 수 없습니다: Resources/{resourcePath}.csv ?");
            return new List<CourseData>();
        }

        return Parse(csv.text);
    }
    
    public static List<CourseData> Parse(string csvText)
    {
        var list = new List<CourseData>();
        string[] lines = csvText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1)
            return list;
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] cols = line.Split(',');
            if (cols.Length < 4)
                continue;
            int.TryParse(cols[0], out int id);
            int.TryParse(cols[1], out int semester);
            string subject = cols[2];
            string taskName = cols[3];

            var data = new CourseData
            {
                id = id,
                semester = semester,
                subject = subject,
                taskName = taskName,
                assignmentCount = 0,
                isExamCleared = false
            };

            list.Add(data);
        }

        return list;
    }
}