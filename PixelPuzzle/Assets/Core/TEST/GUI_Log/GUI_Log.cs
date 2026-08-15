using System.Collections.Generic;
using UnityEngine;

public class GUI_Log : MonoBehaviour
{
    [SerializeField] private float _lineHeight;
    public int _maxLinesCount;

    private static List<string> _logs = new();

    public static void Log(string text)
    {
        _logs.Add($"Log: {text}");
    }

    private void FixedUpdate()
    {
        _maxLinesCount = (int)(Screen.height / _lineHeight);
    }

    private void OnGUI()
    {
        for (int i = 0; i < _maxLinesCount && i < _logs.Count; i++)
        {
            GUI.Label(new Rect(50f, i * _lineHeight, Screen.width, _lineHeight), _logs[_logs.Count - 1 - i]);
        }
    }
}
