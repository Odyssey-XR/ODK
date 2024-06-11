namespace Shared.GameObjects.UI
{
  using UnityEngine;
  using System.Collections.Generic;
  using System.Text;
  using TMPro;
  
  public class DebugConsole : MonoBehaviour
  {
    private const    uint         _queueSize = 15;
    private readonly List<string> _logQueue  = new();

    [SerializeField]
    private TextMeshProUGUI _textField;

    void OnEnable()
    {
      Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
      Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
      _logQueue.Add("[" + type + "] : " + logString);
      if (type == LogType.Exception)
        _logQueue.Add(stackTrace);
      
      while (_logQueue.Count > _queueSize)
        _logQueue.RemoveAt(0);

      StringBuilder builder = new();
      foreach (string log in _logQueue)
        builder.AppendLine(log);
      _textField.text = builder.ToString();
    }
  }
}