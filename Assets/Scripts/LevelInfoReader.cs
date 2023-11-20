using UnityEngine;
using System.IO;

public class LevelInfoReader : MonoBehaviour
{
    public static int GetRoomCount(string levelName, string roomType, string roomSize)
    {
        string fileName = levelName + "_info";
        TextAsset levelInfo = Resources.Load<TextAsset>(fileName);

        if (levelInfo != null)
        {
            string[] lines = levelInfo.text.Split('\n');
            bool isInRoomTypeSection = false;

            foreach (string line in lines)
            {
                if (line.StartsWith(roomType))
                {
                    isInRoomTypeSection = true;
                }
                else if (isInRoomTypeSection && line.Contains(roomSize))
                {
                    string countString = line.Replace(roomSize + ":", "").Trim();
                    if (int.TryParse(countString, out int count))
                    {
                        return count;
                    }
                }
                
                if (line.StartsWith("Final_Rooms") && roomType == "Final_Rooms")
                {
                    string countString = line.Replace("Final_Rooms:", "").Trim();
                    if (int.TryParse(countString, out int count))
                    {
                        return count;
                    }
                }
            }
        }

        // Valor predeterminado si no se encuentra el archivo o el formato es incorrecto
        return 1;
    }
}
