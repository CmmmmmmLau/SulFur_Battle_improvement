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
#if DEBUG
    private static CodeMatcher codeMatcher;
#endif
    
    [HarmonyTranspiler,HarmonyPatch(typeof(Projectile), "HandleHit")]
    private static IEnumerable<CodeInstruction> ProjectileTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original) {
        var codeMatcher = new CodeMatcher(instructions).End();
        
        // Make a label that just jump to the end of the code
        var endLabel = generator.DefineLabel();
        codeMatcher.Instructions()[codeMatcher.Length - 1].labels.Add(endLabel);
        
        // Match the target code in Line:201 → if (this.fHitsLeft <= 0)
        codeMatcher.MatchBack(true, new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(PerfectRandom.Sulfur.Core.Weapons.Projectile), "fHitsLeft")));

        if (codeMatcher.IsValid) {
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
        } else {
            Plugin.instance.LoggingInfo("Failed to find the target code in Projectile.HandleHit");
        }

        
        
        return codeMatcher.InstructionEnumeration();
    }
    
    [HarmonyTranspiler,HarmonyPatch(typeof(ProjectileSystem), "Update")]
    private static IEnumerable<CodeInstruction> ProjectileSystemTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original) {
        // Define a variable to store the hitbox index
        // !!! This variable must be checked in every game broken update !!!
        var hitboxVariable = Convert.ToByte(11);
        
        var codeMatcher = new CodeMatcher(instructions).End();
    
        // Match the target code in Line: 192 → ptr.displacement = 0f;
        // And add a jump label to this code
        var label1 = generator.DefineLabel();
        var match = new[] {
            new CodeMatch(OpCodes.Ldloc_2),
            new CodeMatch(i => i.opcode == OpCodes.Ldc_R4 && i.operand.Equals(0f))
        };
        codeMatcher.MatchBack(false,match)
            .ThrowIfInvalid("Failed to find the target code in in Line: 192 -> ptr.displacement = 0f;")
            .Instruction.WithLabels(label1);
        
        // Match the target code in Line: 191  → ptr.velocity = Vector3.Reflect(ptr.velocity, raycastHit.normal) * num4;
        match = new[] {
            new CodeMatch(OpCodes.Ldloc_2),
            new CodeMatch(OpCodes.Ldloc_2)
        };
        
        // Push hitbox to the stack first
        // and insert new instruction code with the label
        var instructionsList = new[] {
            new CodeInstruction(OpCodes.Ldloc_S, hitboxVariable),
            Transpilers.EmitDelegate(CheckHitboxAlive),
            new CodeInstruction(OpCodes.Brfalse_S, label1)
        };
        
        // The code now should be like this:
        // if (BulletDirection(hitbox)) {
        //  ptr.velocity = Vector3.Reflect(ptr.velocity, raycastHit.normal) * num4;
        // }
        codeMatcher.MatchBack(false,match)
            .ThrowIfInvalid("Failed to find the target code in Line: 191 -> ptr.velocity = Vector3.Reflect(ptr.velocity, raycastHit.normal) * num4;")
            .Insert(instructionsList);
        
        // Match the target code  in Line: 148 → ptr.position = raycastHit.point;
        // And do the same thing as above
        // The code now should be like this:
        // if (BulletDirection(hitbox)) {
        //  ptr.position = raycastHit.point;
        // }
        
        // 1. Add label fisrt
        var label2 = generator.DefineLabel();
        match = new[] {
            new CodeMatch(OpCodes.Ldloc_2),
            new CodeMatch(OpCodes.Ldloca_S),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Stfld),
            new CodeMatch(OpCodes.Ldloc_3),
        };
        codeMatcher.MatchBack(true, match)
            .ThrowIfInvalid("Failed to find the target code in Line: 148 -> ptr.position = raycastHit.point;")
            .Instruction.WithLabels(label2);
        
        instructionsList = new[] {
            new CodeInstruction(OpCodes.Ldloc_S, hitboxVariable).MoveLabelsFrom(codeMatcher.MatchBack(false, match[0]).Instruction),
            Transpilers.EmitDelegate(CheckHitboxAlive),
            new CodeInstruction(OpCodes.Brfalse_S, label2)
        };
        
        codeMatcher.Insert(instructionsList);
        
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