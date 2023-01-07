namespace Worms.engine.game_object; 

public static class GameObjectHelper {
    public static IEnumerable<T> GetAllComponentsOfTypeFromGameObject<T>(GameObject root, bool active) {
        List<T> components = new();
        GetAllGameObjectsFromGameObject(root, active)
            .ToList()
            .ForEach(gameObject => {
                if (gameObject.TryGetComponent(out T component)) {
                    components.Add(component);
                }
            });
        return components;
    }
    
    public static IEnumerable<GameObject> GetAllGameObjectsFromGameObject(GameObject gameObject, bool active) {
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
        return gameObject.Transform.children.Select(GetGameObjectFromTransform);
    }

    private static GameObject GetGameObjectFromTransform(Transform transform) {
        return transform.GameObject;
    }
}