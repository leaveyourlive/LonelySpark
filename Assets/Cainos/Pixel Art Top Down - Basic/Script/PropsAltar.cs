using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;
        public int goldCost = 10;
        public float delayBeforeSceneLoad = 2f;

        private Color curColor;
        private Color targetColor;

        private void Awake()
        {
            targetColor = runes[0].color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor.a = 1.0f;

            if (other.CompareTag("Player"))
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                if (playerStats != null && playerStats.money >= goldCost)
                {
                    playerStats.money -= goldCost;
                    StartCoroutine(LoadSceneWithDelay(3));
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            targetColor.a = 0.0f;
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);
            foreach (var r in runes)
            {
                r.color = curColor;
            }
        }

        private IEnumerator LoadSceneWithDelay(int sceneIndex)
        {
            yield return new WaitForSeconds(delayBeforeSceneLoad);
            ClearDontDestroyOnLoadScene();
            SceneManager.LoadScene(3);
        }

        private void ClearDontDestroyOnLoadScene()
        {
            // Находим все объекты, которые принадлежат к "пустой" сцене DontDestroyOnLoad
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if ((LayerMask.LayerToName(obj.layer) == "UI") || (LayerMask.LayerToName(obj.layer) == "DontDestroy"))
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}