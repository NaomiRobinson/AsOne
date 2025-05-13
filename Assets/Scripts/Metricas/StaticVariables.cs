using UnityEngine;

public class StaticVariables : MonoBehaviour
{
    public static class SessionData {
        public static bool helpviewed = false;
        public static int level = 1;
        public static int time;
        public static int death;
        public static string type;
        public static string name;
    }
    private void Awake()
  {
    DontDestroyOnLoad(gameObject);
  }
}


