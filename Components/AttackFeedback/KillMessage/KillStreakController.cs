using System.Linq;
using PerfectRandom.Sulfur.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillStreakController : MonoBehaviour {
    public GameObject FirstStreak;
    
    public TMP_Text streakCounter;
    public GameObject iconPrefab;
    public GameObject container;
    
    private float timer;
    private int streakCount;
    
    private void Start() {
        streakCounter.text = "";
        
        timer = 0;
    }

    private void Update() {
        if (FirstStreak) {
            timer += Time.deltaTime;

            if (timer > 10f) {
                foreach (var icon in this.container.GetComponentsInChildren<StreakIcon>()) {
                    icon.Fade();
                }
                timer = 0;
                streakCount = 0;
                
                streakCounter.text = "";
            }
        }
        
#if DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            this.AddKillStreak();
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            Enumerable.Range(0,10).ToList().ForEach(x => this.AddKillStreak());
        }
#endif
    }

    public void AddKillStreak() {
        var placeholder = GetPlaceholder();
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
        
        var streak = Instantiate(iconPrefab, this.container.transform, false);
        streak.GetComponent<StreakIcon>().InitStrekIcon(placeholder, this);
        
        streakCount++;
        
        if (!FirstStreak) {
            FirstStreak = streak;
        }

        if (streakCount % 5 == 0) {
            streakCounter.gameObject.SetActive(true);
            foreach (var icon in this.container.GetComponentsInChildren<StreakIcon>()) {
                if (icon.gameObject != FirstStreak && !icon.NeedDestroyWhenOnPosition) {
                    icon.TargetPosition = FirstStreak;
                    icon.NeedDestroyWhenOnPosition = true;
                }
            }
            streakCounter.text = "×" + streakCount;
        }
        
        timer = 0;
    }
    
    protected virtual GameObject GetPlaceholder() {
        var placeholder = new GameObject("Placeholder", typeof(RectTransform));
        placeholder.transform.SetParent(this.transform);
        placeholder.transform.SetAsFirstSibling();
        var rec = placeholder.GetComponent<RectTransform>();
        rec.sizeDelta = new Vector2(25, 25);

        return placeholder;
    }
}