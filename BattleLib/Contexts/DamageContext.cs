using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleLib.Contexts;

public class DamageContext : EventContext {
    public float DamageAmount;
    public DamageTypes DamageType;
    public DamageSourceData DamageSourceData;
    public Hitmesh.Data Hitbox;
    public Vector3 HitPoint;
}