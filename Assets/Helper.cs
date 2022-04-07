using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class Helper
{
    public static string Serialize<T>(this T toSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, toSerialize);
        return writer.ToString();
    }
    public static T Deserialize<T>(this string toDeserialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(toDeserialize);
        return (T)xml.Deserialize(reader);
    }
}