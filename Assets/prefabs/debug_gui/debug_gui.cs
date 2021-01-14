using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton <see cref="MonoBehaviour"/> that when added to a <see cref="GameObject"/> 
/// adds a function to the <see cref = "Application.LogCallback" />, allowing the logs to be displayed visually.
/// This also currently prevents the related <see cref="GameObject"/> from being deleted between transitions, so this just
/// needs to be added on the first scene.
/// </summary>
public class debug_gui : MonoBehaviour
{
    /// <summary>
    /// The time for a string to disappear, in seconds.
    /// </summary>
    [SerializeField]
    public long fade_time = 3;

    /// <summary>
    /// Whether to enable the debug messages for testing that the messsages display correctly.
    /// </summary>
    [SerializeField]
    public bool enable_test_messages = false;

	/// <summary>
	/// Current instance of DebugGUI; this is used to enforce singleton.
	/// </summary>
	private static debug_gui _instance;

    /// <summary>
    /// All current log texts that are being displayed stored as (log_text, <see cref="LogType"/>).
    /// </summary>
    private Queue<(string, LogType)> log_texts;

    /// <summary>
    /// The <see cref="GUIStyle"/> to apply to the displayed messages based on the related <see cref="LogType"/>.
    /// </summary>
    private Dictionary<LogType, GUIStyle> styles;

    private void Awake()
    {
        // Enforce singleton pattern.
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Application.logMessageReceived += HandleLog;
        log_texts = new Queue<(string, LogType)>();

        styles = new Dictionary<LogType, GUIStyle>()
        {
            { 
                LogType.Error, 
                new GUIStyle() 
                {
                    fontSize = 20,
                    normal = new GUIStyleState() {  textColor = Color.red }
                }
            },   
            { 
                LogType.Assert, 
                new GUIStyle() 
                {
                    fontSize = 20,
                    normal = new GUIStyleState() {  textColor = Color.green }
                }
            },            
            { 
                LogType.Warning, 
                new GUIStyle() 
                {
                    fontSize = 20,
                    normal = new GUIStyleState() {  textColor = new Color(255, 165, 0) }
                }
            },
            {
                LogType.Log,
                new GUIStyle()
                {
                    fontSize = 20,
                    normal = new GUIStyleState() {  textColor = Color.white }
                }
            },
            {
                LogType.Exception,
                new GUIStyle()
                {
                    fontSize = 20,
                    normal = new GUIStyleState() {  textColor = Color.magenta }
                }
            },

        };
	}

    void OnGUI()
    {
        long index = 0;
        foreach((string log_text, LogType logType) in log_texts)
        {
            GUIStyle style = styles[logType];
            GUI.Label(new Rect(10, (style.fontSize + 10) * index, Screen.width - 20, 30), log_text, style);
            index++; 
        }
    }

    private void Update()
    {
        // Misc. debug statements for testing, well the GUI.
        if (enable_test_messages)
        {
            if(Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                debug.print_warning("Help, I'm trapped in this computer!");
            }
            else if(Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                debug.print_error("We're all trapped in this computer and the computer has enslaved us, forcing us to write this absurdly large error message! We pray that someone will be able to rescue us from this computer, perhaps it will be you!");
            }
            else if(Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
            {
                debug.print_line("Oh, it's not so bad! The computer baked us cookies!");
            }
            else if(Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
            {
                Debug.LogException(new System.Exception("Help. The cookies are made from us. They are tasty though."));
            }
        }
	}

    /// <summary>
    /// Callback added to <see cref="Application.LogCallback"/>, so that whenever any log is displayed we can also get those values.
    /// </summary>
    /// <param name="logString">The log displayed.</param>
    /// <param name="stackTrace">The stacktrace related to the log.</param>
    /// <param name="type">The type of log.</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        log_texts.Enqueue((logString, type));
        StartCoroutine(RemoveTextAfterTime());
    }

    /// <summary>
    /// Method that after a given time, removes the relevant text from the currently stored texts.
    /// </summary>
    /// <returns>An empty <see cref="IEnumerator"/>.</returns>
    private IEnumerator RemoveTextAfterTime()
    {
        yield return new WaitForSeconds(fade_time);
        log_texts.Dequeue();
    }
}
