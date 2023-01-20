using Worms.engine.game_object;
using Worms.engine.game_object.components;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;
using Object = Worms.engine.game_object.Object;

namespace Worms.engine.core; 

public class GameObjectHandler {
    public List<Script> AllScripts { get; } = new();
    public List<Script> AllActiveGameObjectScripts { get; } = new();
    public List<TextureRenderer> AllActiveGameObjectTextureRenderers { get; } = new();

    private readonly Queue<GameObject> _instantiatedGameObjects = new();
    private readonly HashSet<Object> _destroyObjects = new();
    private readonly HashSet<GameObject> _activeStatusChangedGameObjects = new();

    public GameObjectHandler(GameObject root) {
        InstantiateGameObject(root);
        Transform.GameObjectInstantiateEvent += obj => _instantiatedGameObjects.Enqueue(obj);
        Object.ObjectDestroyEvent += obj => _destroyObjects.Add(obj);
        GameObject.GameObjectActiveEvent += obj => _activeStatusChangedGameObjects.Add(obj);
    }

    public void EndOfFrameCleanup() {
        while (_instantiatedGameObjects.Count > 0) {
            InstantiateGameObject(_instantiatedGameObjects.Dequeue());
        }
        while (_destroyObjects.Count > 0) {
            Object obj = _destroyObjects.First();
            DestroyObject(obj);
            _destroyObjects.Remove(obj);
        }
        while (_activeStatusChangedGameObjects.Count > 0) {
            GameObject obj = _activeStatusChangedGameObjects.First();
            ChangeActiveGameObject(obj);
            _activeStatusChangedGameObjects.Remove(obj);
        }
    }
    
    private void DestroyObject(Object obj) {
        switch (obj) {
            case GameObject gameObject:
                DestroyGameObject(gameObject);
                break;
            case ToggleComponent component:
                DestroyComponent(component);
                break;
        }
    }

    private void InstantiateGameObject(GameObject gameObject) {
        List<GameObject> allGameObjects = GetAllGameObjectsFromGameObject(gameObject, false).ToList();
        List<GameObject> allActiveGameObjects = GetAllGameObjectsFromGameObject(gameObject, true).ToList();
        
        AllActiveGameObjectTextureRenderers.AddRange(allActiveGameObjects.SelectMany(g => g.components).OfType<TextureRenderer>());
        AllScripts.AddRange(allGameObjects.SelectMany(g => g.components).OfType<Script>());
        AllActiveGameObjectScripts.AddRange(allActiveGameObjects.SelectMany(g => g.components).OfType<Script>());
    }

    private void ChangeActiveGameObject(GameObject gameObject) {
        if (!gameObject.IsActive) {
            DeActivateGameObject(gameObject);
        }
        else {
            ActivateGameObject(gameObject);
        }
    }

    private void DeActivateGameObject(GameObject gameObject) {
        HashSet<GameObject> active = GetAllGameObjectsFromGameObject(gameObject, true).ToHashSet();
        HashSet<GameObject> notActive = GetAllGameObjectsFromGameObject(gameObject, false)
            .Where(g => !active.Contains(g))
            .ToHashSet();

        AllActiveGameObjectTextureRenderers.RemoveAll(tr => notActive.Contains(tr.gameObject));
        AllActiveGameObjectScripts.RemoveAll(script => notActive.Contains(script.gameObject));
    }

    private void ActivateGameObject(GameObject gameObject) {
        List<GameObject> active = GetAllGameObjectsFromGameObject(gameObject, true).ToList();
        active.SelectMany(g => g.components).OfType<TextureRenderer>().ToList().ForEach(tr => {
            if (!AllActiveGameObjectTextureRenderers.Contains(tr)) {
                AllActiveGameObjectTextureRenderers.Add(tr);
            }
        });
        active.SelectMany(g => g.components).OfType<Script>().ToList().ForEach(script => {
            if (!AllActiveGameObjectScripts.Contains(script)) {
                AllActiveGameObjectScripts.Add(script);
            }
        });
    }

    private void DestroyGameObject(GameObject gameObject) {
        List<GameObject> all = GetAllGameObjectsFromGameObject(gameObject, false).ToList();
        AllActiveGameObjectTextureRenderers.RemoveAll(tr => all.Contains(tr.gameObject));
        AllActiveGameObjectScripts.RemoveAll(script => all.Contains(script.gameObject));
        AllScripts.RemoveAll(script => all.Contains(script.gameObject));
        
        gameObject.Transform.Parent!.children.Remove(gameObject.Transform);
    }
    
    private void DestroyComponent(ToggleComponent component) {
        component.gameObject.components.Remove(component);
        switch (component) {
            case Script script:
                AllScripts.Remove(script);
                AllActiveGameObjectScripts.Remove(script);
                break;
            case TextureRenderer tr:
                AllActiveGameObjectTextureRenderers.Remove(tr);
                break;
        }
    }

    private static IEnumerable<GameObject> GetAllGameObjectsFromGameObject(GameObject gameObject, bool active) {
        if (!gameObject.IsActive && active) {
            return new List<GameObject>();
        }
        List<GameObject> gameObjects = new() { gameObject };
        foreach (GameObject child in GetChildren(gameObject)) {
            gameObjects.AddRange(GetAllGameObjectsFromGameObject(child, active));
        }

        return gameObjects;
    }

    private static IEnumerable<GameObject> GetChildren(GameObject gameObject) {
        return gameObject.Transform.children.Select(static child => child.gameObject);
    }
}