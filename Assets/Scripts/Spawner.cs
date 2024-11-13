using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int length = 5;
    public int width = 5;
    public int height = 5;

    // Reference to the cube prefab.
    public GameObject spawnPrefab;
    public Mesh modelMesh;

    // Size for spacing.
    public float prefabSize = 1f;

    // prefab counter
    public int maxSpawned = 0;
    private int spawnedCount = 0;

    [ContextMenu("SpawnMesh")]
    public void SpawnToVertecies()
    {
        Vector3[] verts = modelMesh.vertices;
        for (int i = 0; i < verts.Length; i++)
            Instantiate(spawnPrefab, verts[i] + transform.position, Quaternion.identity, transform);
    }

    [ContextMenu("SpawnCubes")]
    public void SpawnCubes()
    {
        if (spawnPrefab == null)
            return;

        // Reset spawner counter
        spawnedCount = 0;

        // Remove old cubes
        RemoveCubes();

        // Create a hollow rectangular prism.
        for (int h = 0; h < height; h++)
        {
            for (int l = 0; l < length; l++)
            {
                for (int w = 0; w < width; w++)
                {
                    // Only spawn cubes at the edges, leaving the inside empty.
                    // Adjusted condition: if h == 0 or h == height - 1, only spawn if it's also an edge cube in the x or z direction.
                    var isEdge = l == 0 || l == length - 1 || w == 0 || w == width - 1 || (h > 0 && h < height && (l == 0 || l == length - 1 || w == 0 || w == width - 1));

                    if (isEdge)
                    {
                        Instantiate(spawnPrefab, new Vector3(l * prefabSize, h * prefabSize, w * prefabSize), Quaternion.identity, transform);
                        spawnedCount++;
                    }

                    if (spawnedCount == maxSpawned)
                        return;
                }
            }
        }
    }

    public void RemoveCubes()
    {
        // Get the current child count.
        int numChildren = transform.childCount;

        // Iterate backwards through the child objects.
        // This is important as removing children changes the order.
        for (int i = numChildren - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject); // Destroy each child object.
    }
}
