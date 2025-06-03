using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampfireManager : MonoBehaviour
{
    public static CampfireManager Instance;

    [System.Serializable]
    public class CampfireData
    {
        public string campfireName;
        public string sceneName;
        public Vector3 position;
    }

    public List<CampfireData> activatedCampfires = new List<CampfireData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCampfire(string name, string sceneName, Vector3 position)
    {
        if (!activatedCampfires.Exists(c => c.campfireName == name && c.sceneName == sceneName))
        {
            CampfireData newCampfire = new CampfireData()
            {
                campfireName = name,
                sceneName = sceneName,
                position = position
            };
            activatedCampfires.Add(newCampfire);
        }
    }
}