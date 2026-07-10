// Coplay IRunCommand example: read-only NavMesh inspection.
// This does not add components, bake data, or modify the scene.

using UnityEngine;
using UnityEngine.AI;

internal class CommandScript : IRunCommand
{
    public void Execute(ExecutionResult result)
    {
        result.Log("=== NAVMESH INSPECTION ===");

        var triangulation = NavMesh.CalculateTriangulation();
        result.Log(
            $"Triangulation: vertices={triangulation.vertices.Length}, " +
            $"indices={triangulation.indices.Length}, " +
            $"areas={triangulation.areas.Length}");

        var agents = Object.FindObjectsByType<NavMeshAgent>(
            FindObjectsSortMode.None);
        var onMesh = 0;
        var offMesh = 0;
        foreach (var agent in agents)
        {
            if (agent.isOnNavMesh)
            {
                onMesh++;
                result.Log(
                    $"  {agent.name}: on NavMesh, " +
                    $"agentType={agent.agentTypeID}, areaMask={agent.areaMask}");
            }
            else
            {
                offMesh++;
                result.LogWarning(
                    $"  {agent.name}: off NavMesh at " +
                    $"{agent.transform.position}, " +
                    $"agentType={agent.agentTypeID}, areaMask={agent.areaMask}");
            }
        }

        result.Log(
            $"Agents: {onMesh} on NavMesh, {offMesh} off NavMesh.");
        result.Log("No scene or NavMesh data was modified.");
        result.Log("=== INSPECTION COMPLETE ===");
    }
}
