using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class Map
{
    //public static String fileMapPath4 = "map4";
    //public static String fileMapPath6 = "map6";
    //public static String fileMapPath8 = "map8";
    public static List<ObjectMap> listObjectMap4;
    public static List<ObjectMap> listObjectMap6;
    public static List<ObjectMap> listObjectMap8;

    public static void LoadMap4()
    {
        if (listObjectMap4 == null)
        {
            listObjectMap4 = new List<ObjectMap>();
        }
        else listObjectMap4.Clear();
        TextAsset mydata = Resources.Load("Data/Map4") as TextAsset;
        StringReader stringReader = new StringReader(mydata.text);
        using (stringReader)
        {
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                listObjectMap4.Add(new ObjectMap(line));
            }
        }
        stringReader.Close();
    }

    public static void LoadMap6()
    {
        if (listObjectMap6 == null)
        {
            listObjectMap6 = new List<ObjectMap>();
        }
        else listObjectMap6.Clear();
        TextAsset mydata = Resources.Load("Data/Map6") as TextAsset;
        StringReader stringReader = new StringReader(mydata.text);
        using (stringReader)
        {
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                listObjectMap6.Add(new ObjectMap(line));
            }
        }
        stringReader.Close();
    }

    public static void LoadMap8()
    {
        if (listObjectMap8 == null)
        {
            listObjectMap8 = new List<ObjectMap>();
        }
        else listObjectMap8.Clear();
        TextAsset mydata = Resources.Load("Data/Map8") as TextAsset;
        StringReader stringReader = new StringReader(mydata.text);
        using (stringReader)
        {
            string line = "";
            while ((line = stringReader.ReadLine()) != null)
            {
                listObjectMap8.Add(new ObjectMap(line));
            }
        }
        stringReader.Close();
    }

    public static ObjectMap getObjectMap(int index, int map)
    {
        switch (index)
        {
            case 4:
                if (listObjectMap4 == null)
                {
                    listObjectMap4 = new List<ObjectMap>();
                }
                if (listObjectMap4.Count == 0)
                {
                    LoadMap4();
                }
                if (map < listObjectMap4.Count)
                {
                    return listObjectMap4[map];
                }
                else return listObjectMap4[0];
                break;
            case 6:
                if (listObjectMap6 == null)
                {
                    listObjectMap6 = new List<ObjectMap>();
                }
                if (listObjectMap6.Count == 0)
                {
                    LoadMap6();
                }
                if (map < listObjectMap6.Count)
                {
                    return listObjectMap6[map];
                }
                else return listObjectMap6[0];
                break;
            default:
                if (listObjectMap8 == null)
                {
                    listObjectMap8 = new List<ObjectMap>();
                }
                if (listObjectMap8.Count == 0)
                {
                    LoadMap8();
                }
                if (map < listObjectMap8.Count)
                {
                    return listObjectMap8[map];
                }
                else return listObjectMap8[0];
                break;

        }

    }
}
public class ObjectMap
{
    public int[] listMap;
    public ObjectMap(string stringMap)
    {
        string[] strRowSplist = stringMap.Split(' ');

        listMap = new int[strRowSplist.Length];
        for (int i = 0; i < strRowSplist.Length - 1; i++)
        {
            if ("-1".Equals(strRowSplist[i]))
            {
                listMap[i] = -1;
            }
            else if ("0".Equals(strRowSplist[i]))
            {
                listMap[i] = 0;
            }
            else
            {
                listMap[i] = int.Parse(strRowSplist[i]);
            }
        }
    }
}
