// Coplay IRunCommand example: read-only Input System diagnostic.
// Adapt to the installed provider and Unity version before running.

using UnityEditor;
using UnityEngine;

internal class CommandScript : IRunCommand
{
    public void Execute(ExecutionResult result)
    {
        result.Log("=== INPUT SYSTEM DIAGNOSTIC ===");

        var settings = AssetDatabase.LoadAllAssetsAtPath(
            "ProjectSettings/ProjectSettings.asset");
        if (settings.Length == 0)
        {
            result.LogWarning("ProjectSettings asset was not available.");
        }
        else
        {
            var serializedSettings = new SerializedObject(settings[0]);
            var activeInputHandler =
                serializedSettings.FindProperty("activeInputHandler");

            if (activeInputHandler == null)
            {
                result.LogWarning(
                    "activeInputHandler was not found for this Unity version.");
            }
            else
            {
                var mode = activeInputHandler.intValue;
                string[] modes =
                {
                    "Legacy Input Manager",
                    "Input System Package",
                    "Both"
                };
                var label = mode >= 0 && mode < modes.Length
                    ? modes[mode]
                    : $"Unknown ({mode})";
                result.Log($"Active Input Handling: {label}");
            }
        }

        var actionAssets = AssetDatabase.FindAssets("t:InputActionAsset");
        result.Log($"InputActionAssets found: {actionAssets.Length}");
        foreach (var guid in actionAssets)
        {
            result.Log($"  {AssetDatabase.GUIDToAssetPath(guid)}");
        }

#if ENABLE_INPUT_SYSTEM
        var playerInputs =
            Object.FindObjectsByType<UnityEngine.InputSystem.PlayerInput>(
                FindObjectsSortMode.None);
        result.Log($"PlayerInput components in loaded scenes: {playerInputs.Length}");
        foreach (var playerInput in playerInputs)
        {
            result.Log(
                $"  {playerInput.name}: actions=" +
                $"{playerInput.actions?.name ?? "null"}, " +
                $"scheme={playerInput.currentControlScheme}");
        }
#else
        result.Log(
            "ENABLE_INPUT_SYSTEM is not defined in the current compilation.");
#endif

        var scriptGuids =
            AssetDatabase.FindAssets("t:MonoScript", new[] { "Assets" });
        var legacyCount = 0;
        foreach (var guid in scriptGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            string source;
            try
            {
                source = System.IO.File.ReadAllText(path);
            }
            catch (System.Exception exception)
            {
                result.LogWarning($"Could not read {path}: {exception.Message}");
                continue;
            }

            if (!source.Contains("Input.GetKey")
                && !source.Contains("Input.GetAxis")
                && !source.Contains("Input.GetButton"))
            {
                continue;
            }

            legacyCount++;
            if (legacyCount <= 10)
            {
                result.LogWarning($"  Legacy Input call: {path}");
            }
        }

        if (legacyCount > 10)
        {
            result.LogWarning(
                $"  ... and {legacyCount - 10} additional matching scripts.");
        }

        result.Log($"Scripts with common legacy Input calls: {legacyCount}");
        result.Log("=== DIAGNOSTIC COMPLETE ===");
    }
}
