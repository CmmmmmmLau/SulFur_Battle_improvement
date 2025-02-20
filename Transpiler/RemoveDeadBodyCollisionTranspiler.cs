using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Units;
using PerfectRandom.Sulfur.Core.Weapons;
using UnityEngine;

namespace BattleImprove.Transpiler;

public class RemoveDeadBodyCollisionTranspiler {
    [HarmonyTranspiler,HarmonyPatch(typeof(Projectile), "HandleHit")]
    private static IEnumerable<CodeInstruction> ProjectileTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original) {
        var codeMatcher = new CodeMatcher(instructions).End();
        
        // Make a label that just jump to the end of the code
        var endLabel = generator.DefineLabel();
        codeMatcher.Instructions()[codeMatcher.Length - 1].labels.Add(endLabel);
        
        // Match the target code in Line:201 → if (this.fHitsLeft <= 0)
        codeMatcher.MatchBack(true, new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(PerfectRandom.Sulfur.Core.Weapons.Projectile), "fHitsLeft")));

        // Push hitbox to the stack first
        // and insert new instruction code with the label
        var insertCode = new[] {
            new CodeInstruction(OpCodes.Ldarg_2),
            Transpilers.EmitDelegate(CheckHitboxAlive),  
            new CodeInstruction(OpCodes.Brfalse_S, endLabel)
        };
        codeMatcher.Advance(3).InsertAndAdvance(insertCode); 
        // The code now should be like this:
        // if (this.fHitsLeft <= 0) {
        //  if (BulletDirection(hitbox)) {
        //      .......
        //  }
        // }
        
        
        return codeMatcher.InstructionEnumeration();
    }
    
    [HarmonyTranspiler,HarmonyPatch(typeof(ProjectileSystem), "Update")]
    private static IEnumerable<CodeInstruction> ProjectileSystemTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original) {
        var codeMatcher = new CodeMatcher(instructions).End();
    
        // Match the target code in Line: 175 → if (math.lengthsq(ptr.velocity) > 0.0001f)
        // And add a jump label to this code
        var label1 = generator.DefineLabel();
        var match = new[] {
            new CodeMatch(OpCodes.Ldloc_3),
            new CodeMatch(OpCodes.Ldfld),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Ldc_R4)
        };
        codeMatcher.MatchBack(false,match).Instruction.labels.Add(label1);
        
        // Match the target code in Line: 174  → ptr.velocity = Vector3.Reflect(ptr.velocity, raycastHit.normal) * num5;
        match = new[] {
            new CodeMatch(OpCodes.Ldloc_3),
            new CodeMatch(OpCodes.Ldloc_3)
        };
        codeMatcher.MatchBack(false,match);

        // Copy the label from the target code
        var labels = codeMatcher.Instruction.labels;
        // Push hitbox to the stack first
        // and insert new instruction code with the label
        var instructionsList = new[] {
            new CodeInstruction(OpCodes.Ldloc_S, Convert.ToByte(10)).WithLabels(labels),
            Transpilers.EmitDelegate(CheckHitboxAlive),
            new CodeInstruction(OpCodes.Brfalse_S, label1)
        };
        // Dont forget to clear the label that only original code has
        codeMatcher.InsertAndAdvance(instructionsList).Labels.Clear();
        // The code now should be like this:
        // if (BulletDirection(hitbox)) {
        //  ptr.velocity = Vector3.Reflect(ptr.velocity, raycastHit.normal) * num5;
        // }
        
        // Match the target code  in Line: 142 → ptr.position = raycastHit.point;
        // And do the same thing as above
        // The code now should be like this:
        // if (BulletDirection(hitbox)) {
        //  ptr.position = raycastHit.point;
        // }
        var label2 = generator.DefineLabel();
        match = new[] {
            new CodeMatch(OpCodes.Ldloc_3),
            new CodeMatch(OpCodes.Ldloca_S),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Stfld),
            new CodeMatch(OpCodes.Ldloc_S),
        };
        codeMatcher.MatchBack(true, match).Labels.Add(label2);
        labels = codeMatcher.MatchBack(false, match[0]).Labels;
        
        instructionsList = new[] {
            new CodeInstruction(OpCodes.Ldloc_S, Convert.ToByte(10)).WithLabels(labels),
            Transpilers.EmitDelegate(CheckHitboxAlive),
            new CodeInstruction(OpCodes.Brfalse_S, label2)
        };
        codeMatcher.InsertAndAdvance(instructionsList).Labels.Clear();
        
        return codeMatcher.InstructionEnumeration();
    }

    private static bool CheckHitboxAlive(Hitbox hitbox) {
        if (hitbox == null) {
            return true;
        }
        return !hitbox.GetOwner().isNpc || hitbox.GetOwner().IsAlive;
    }
    
    // [HarmonyDebug,HarmonyPatch(typeof(ProjectileSystem), "Update")]
    // private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original) {
    //     codeMatcher = new CodeMatcher(instructions).End();
    //     var skipLabel = generator.DefineLabel();
    //     var dataType = AccessTools.Inner(typeof(ProjectileSystem), "Data");
    //     
    //
    //     // Match the target code
    //     var match = new[] {
    //         new CodeMatch(OpCodes.Ldloc_S),
    //         new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(UnityEngine.Component), "get_transform")),
    //         new CodeMatch(OpCodes.Ldloc_3),
    //         new CodeMatch(OpCodes.Ldfld, AccessTools.Field(dataType, "velocity")),
    //         new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Unity.Mathematics.float3), "op_Implicit", new[] {typeof(Unity.Mathematics.float3)})),
    //         new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(UnityEngine.Transform), nameof(UnityEngine.Transform.forward)))
    //     };
    //     codeMatcher.MatchBack(false,match);
    //     
    //     // Remove some instructions
    //     codeMatcher.RemoveInstructions(6);
    //
    //     // Push hitbox and projectile to the stack
    //     // var instructionsList = new[] {
    //     //     // new CodeInstruction(OpCodes.Ldloc_3),
    //     //     // new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(dataType, "velocity")),
    //     //     new CodeInstruction(OpCodes.Ldloc_S, Convert.ToByte(10)),
    //     //     new CodeInstruction(OpCodes.Ldloc_S, Convert.ToByte(4)),
    //     //     Transpilers.EmitDelegate(BulletDirection)
    //     // };
    //     // codeMatcher.InsertAndAdvance(instructionsList);
    //     
    //     return codeMatcher.InstructionEnumeration();
    // }
    // static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original) {
    //     codeMatcher = new CodeMatcher(instructions).End();
    //     var skipLabel = generator.DefineLabel();
    //     var dataType = AccessTools.Inner(typeof(ProjectileSystem), "Data");
    //
    //     // Match the target code
    //     var match = new[] {
    //         new CodeMatch(OpCodes.Ldloc_S),
    //         new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(UnityEngine.Component), "get_transform")),
    //         new CodeMatch(OpCodes.Ldloc_3),
    //         new CodeMatch(OpCodes.Ldfld, AccessTools.Field(dataType, "velocity")),
    //         new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Unity.Mathematics.float3), "op_Implicit", new[] {typeof(Unity.Mathematics.float3)})),
    //         new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(UnityEngine.Transform), nameof(UnityEngine.Transform.forward)))
    //     };
    //     codeMatcher.MatchBack(true,match);
    //     
    //     // Add a new label and back the beginning of the target code
    //     codeMatcher.Instructions()[codeMatcher.Pos + 1].labels.Add(skipLabel);
    //     codeMatcher.MatchBack(false, match);
    //
    //     var instructionsList = new[] {
    //         new CodeInstruction(OpCodes.Ldloc_S, 10),
    //         new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Hitbox), "get_Owner")),
    //         new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Unit), "get_IsAlive")),
    //         new CodeInstruction(OpCodes.Brfalse_S, skipLabel)
    //     };
    //     codeMatcher.InsertAndAdvance(instructionsList);
    //     
    //     return codeMatcher.InstructionEnumeration();
    // }
}