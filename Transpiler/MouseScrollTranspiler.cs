using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Weapons;

namespace BattleImprove.Transpiler;

public class MouseScrollTranspiler {
#if DEBUG
    private static CodeMatcher codeMatcher;
#endif
    
    
    [HarmonyTranspiler, HarmonyPatch(typeof(InputReader), "SelectByScroll")]
    private static IEnumerable<CodeInstruction> ProjectileTranspiler(IEnumerable<CodeInstruction> instructions,
        ILGenerator generator, MethodBase original) {
        codeMatcher = new CodeMatcher(instructions).Start();

        var instruction = codeMatcher.Instructions()[4];
        if (instruction.opcode == OpCodes.Ble_Un_S || instruction.opcode == OpCodes.Ble_Un) {
            instruction.opcode = OpCodes.Bge_Un_S;
        }

        return codeMatcher.InstructionEnumeration();
    }
}