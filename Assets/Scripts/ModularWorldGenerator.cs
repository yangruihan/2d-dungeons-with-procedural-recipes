using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModularWorldGenerator : MonoBehaviour
{
    public Module[] Modules;
    public Module StartModule;
    public int Iterations = 5;

    private void Start()
    {
        var startModule = Instantiate(StartModule, transform.position, Quaternion.identity).GetComponent<Module>();
        startModule.gameObject.SetActive(true);
        var pendingExits = new List<ModuleConnector>(startModule.GetExits());

        for (int i = 0; i < Iterations; i++)
        {
            var newExits = new List<ModuleConnector>();

            foreach (var pendingExit in pendingExits)
            {
                var newTag = GetRandom(pendingExit.Tags);
                var newModulePrefab = GetRandomWithTag(Modules, newTag);
                var newModule = Instantiate(newModulePrefab, transform.position, Quaternion.identity).GetComponent<Module>();
                var newModuleExits = newModule.GetExits();
                var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                MatchExits(pendingExit, exitToMatch);
                newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
            }

            pendingExits = newExits;
        }
    }

    private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
    {
        var newModule = newExit.transform.parent;
        var forwardVectorToMatch = -oldExit.transform.up;
        var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.up);
        newModule.RotateAround(newExit.transform.position, Vector3.forward, correctiveRotation);
        var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
        newModule.transform.position += correctiveTranslation;
        newModule.gameObject.SetActive(true);
    }

    private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
    {
        var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
        return GetRandom(matchingModules);
    }

    private static T GetRandom<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    private static float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.up, vector) * Mathf.Sign(-vector.x);
    }
}
