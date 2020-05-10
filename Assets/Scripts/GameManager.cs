using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject water_tile, sand_tile, mountain_tile, forest_tile, grass_tile, stone_tile;
    // Start is called before the first frame update
    
    float x_step = 17.321f;
    float y_step = 5;
    float line_offset = 8.661f;
    void Start()
    {
        Texture2D heightmap = Resources.Load("Heightmap_16", typeof(Texture2D)) as Texture2D; //Resources.Load("Assets/Textures/Heightmap_16") as Texture2D;
        //Texture2D heightmap = (Texture2D)Resources.LoadAssetAtPath("Assets/Textures/Heightmap_16", typeof(Texture2D));
        int x, y;

        // Loop through the images pixels to reset color.
        for (x = 0; x < heightmap.width; x++)
        {
            for (y = 0; y < heightmap.height; y++)
            {
                Color pixelColor = heightmap.GetPixel(x, y);
                Vector3 pos = new Vector3(y % 2 * line_offset + x * x_step, 0, y * y_step);

                if (pixelColor.b < 0.01)
                {
                    Instantiate(water_tile, pos, Quaternion.identity);
                }
                else if (pixelColor.b < 0.2)
                {
                    Instantiate(sand_tile, pos, Quaternion.identity);
                }
                else if (pixelColor.b < 0.4)
                {
                    Instantiate(grass_tile, pos, Quaternion.identity);
                }
                else if (pixelColor.b < 0.6)
                {
                    Instantiate(forest_tile, pos, Quaternion.identity);
                }
                else if (pixelColor.b < 0.8)
                {
                    Instantiate(stone_tile, pos, Quaternion.identity);
                }
                else
                {
                    Instantiate(mountain_tile, pos, Quaternion.identity);
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
