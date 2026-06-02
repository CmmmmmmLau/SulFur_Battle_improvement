using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleLib.Context;

public class DamagedContext {
    public Unit Unit { get; }
    public float Damage { get; }
    public DamageTypes DamageType { get; }
    public DamageSourceData sourceData { get; }
    public Hitmesh.Data hitbox { get; }
    public Vector3 hitPosition { get; }
    
    public DamagedContext(Unit unit, float damage, DamageTypes damageType, DamageSourceData sourceData, Hitmesh.Data hitbox, Vector3 hitPosition) {
        Unit = unit;
        Damage = damage;
        DamageType = damageType;
        this.sourceData = sourceData;
        this.hitbox = hitbox;
        this.hitPosition = hitPosition;
    }
}