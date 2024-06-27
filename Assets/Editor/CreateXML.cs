using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Windows;

public class CreateXML : Editor
{
    [MenuItem("Assets/Create/XML Document")]
    public static void Create()
    {
        string xmlFile;

        if (Selection.activeObject != null)
        {
            xmlFile = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (UnityEngine.Windows.Directory.Exists(xmlFile))
            {
                xmlFile = Path.Combine(xmlFile, "Default.xml");
            }
            else if (UnityEngine.Windows.File.Exists(xmlFile))
            {
                xmlFile = Path.Combine(Path.GetDirectoryName(xmlFile), "Default.xml");
            }
        }
        else
        {
            xmlFile = "Assets/Default.xml";
        }

        xmlFile = AssetDatabase.GenerateUniqueAssetPath(xmlFile);

        XmlWriter XMLWriter = XmlWriter.Create(xmlFile);
        XMLWriter.WriteStartElement("root");
        XMLWriter.Close();

        AssetDatabase.ImportAsset(xmlFile);
    }
}
