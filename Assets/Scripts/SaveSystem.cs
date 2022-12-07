using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveStage (GameManagerController manager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/stage" + manager.level.ToString() + ".fun";
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        StageData data = new StageData(manager);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static StageData LoadStage(string level)
    {
        string path = Application.persistentDataPath + "/stage" + level + ".fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StageData data = formatter.Deserialize(stream) as StageData;
            stream.Close();

            return data;
        } else
        {
            Debug.Log("파일 없음");
            return null;
        }
    }

    public static void SaveStars (int stars)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/stars.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        StarsData data = new StarsData(stars);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static StarsData LoadStars()
    {
        string path = Application.persistentDataPath + "/stars.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StarsData data = formatter.Deserialize(stream) as StarsData;
            stream.Close();

            return data;
        } else
        {
            StarsData data = new StarsData(0);
            SaveStars(0);
            Debug.Log("파일 없음");
            return data;
        }
    }

    public static void SaveMyCar (string resourceName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/mycar.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        MyCarData data = new MyCarData(resourceName);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static MyCarData LoadMyCar()
    {
        string path = Application.persistentDataPath + "/mycar.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            MyCarData data = formatter.Deserialize(stream) as MyCarData;
            stream.Close();

            return data;
        } else
        {
            Debug.Log("구매 이력 없음");
            return null;
        }
    }
}
