
using TMPro;
using UnityEngine;

public class BF5MessageController : MessageController {
    public KillStreakController headshotStreak;
    public KillStreakController killStreak;

    protected override GameObject GetDamageInfo(string type, int damage, GameObject placeholder) {
        var info =  base.GetDamageInfo(type, damage, placeholder);
        
        info.transform.localPosition = new Vector3(0, 0, 0);
        var rect = info.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f, 0.5f);
        
        var tmp = info.GetComponent<TMP_Text>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 15;

        return info;
    }

    public override void OnEnemyKill(string enemyName, string weaponName, string exp, bool isHeadShot, bool isFar = false) {
        base.OnEnemyKill(enemyName, weaponName, exp, isHeadShot, isFar);
        
        if (isHeadShot) {
            headshotStreak.AddKillStreak();
        } else {
            killStreak.AddKillStreak();
        }
        
        headshotStreak.ResetTimer();
        killStreak.ResetTimer();
    }
}