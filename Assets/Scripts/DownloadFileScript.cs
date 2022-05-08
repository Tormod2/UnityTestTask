using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;

public static class DownloadFileScript
{
    public static PathData GetTrajectoryData()
    {
        var path = Path.Combine(Application.dataPath, "Data.json");
        WebClient webClient = new WebClient();

        //Checks if file is already downloaded, and if not, downloads it from my Google Drive.
        if (!File.Exists(path))
        {
            webClient.DownloadFile("https://drive.google.com/uc?export=download&id=18LQ5O13A70Ahkbqz29eFzROCVtkXVPQj",
                path);
        }

        var pathData = JsonConvert.DeserializeObject<PathData>(File.ReadAllText(path));
        return pathData;
    }
}
