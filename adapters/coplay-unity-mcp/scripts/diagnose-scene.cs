// Coplay IRunCommand example: read-only scene diagnostic.
// Adapt to the installed provider and Unity version before running.

using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

internal class CommandScript : IRunCommand
{
    public void Execute(ExecutionResult result)
    {
        result.Log("=== SCENE DIAGNOSTIC ===");

        var agents = Object.FindObjectsByType<NavMeshAgent>(
            FindObjectsSortMode.None);
        result.Log($"NavMeshAgents: {agents.Length}");
        foreach (var agent in agents)
        {
            var pathStatus = agent.isOnNavMesh
                ? agent.pathStatus.ToString()
                : "Unavailable (agent is off NavMesh)";
            result.Log(
                $"  {agent.name}: onNavMesh={agent.isOnNavMesh}, " +
                $"position={agent.transform.position}, pathStatus={pathStatus}");
        }

        var colliders = Object.FindObjectsByType<Collider>(
            FindObjectsSortMode.None);
        var triggerCount = 0;
        foreach (var collider in colliders)
        {
            if (!collider.isTrigger)
            {
                continue;
            }

            triggerCount++;
            var rigidbody = collider.attachedRigidbody;
            if (rigidbody == null)
            {
                result.LogWarning(
                    $"Trigger '{collider.name}' has no attached Rigidbody. " +
                    "Its collision counterpart may still own one.");
            }
            else
            {
                result.Log(
                    $"Trigger '{collider.name}': attached Rigidbody " +
                    $"kinematic={rigidbody.isKinematic}");
            }
        }
        result.Log($"Triggers: {triggerCount}");

        var canvases = Object.FindObjectsByType<Canvas>(
            FindObjectsSortMode.None);
        result.Log($"Canvases: {canvases.Length}");
        foreach (var canvas in canvases)
        {
            result.Log(
                $"  {canvas.name}: mode={canvas.renderMode}, " +
                $"order={canvas.sortingOrder}, " +
                $"active={canvas.gameObject.activeInHierarchy}");
        }

        var cameras = Object.FindObjectsByType<Camera>(
            FindObjectsSortMode.None);
        result.Log($"Cameras: {cameras.Length}");
        foreach (var camera in cameras)
        {
            result.Log(
                $"  {camera.name}: tag={camera.tag}, " +
                $"enabled={camera.enabled}, depth={camera.depth}");
        }

        var settings = AssetDatabase.LoadAllAssetsAtPath(
            "ProjectSettings/ProjectSettings.asset");
        if (settings.Length > 0)
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
                string[] modes = { "Legacy", "Input System", "Both" };
                var mode = activeInputHandler.intValue;
                var label = mode >= 0 && mode < modes.Length
                    ? modes[mode]
                    : "Unknown";
                result.Log($"Input Handling: {label} ({mode})");
            }
        }

        var controllers = Object.FindObjectsByType<CharacterController>(
            FindObjectsSortMode.None);
        foreach (var controller in controllers)
        {
            result.Log(
                $"CharacterController '{controller.name}': " +
                $"grounded={controller.isGrounded}, " +
                $"position={controller.transform.position}");
        }

        var gameObjects = Object.FindObjectsByType<GameObject>(
            FindObjectsSortMode.None);
        var missingCount = 0;
        foreach (var gameObject in gameObjects)
        {
            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (component != null)
                {
                    continue;
                }

                missingCount++;
                result.LogWarning(
                    $"Missing component on '{gameObject.name}'.");
            }
        }

        result.Log($"Missing components: {missingCount}");
        result.Log("=== DIAGNOSTIC COMPLETE ===");
    }
}
