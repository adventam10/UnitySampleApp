using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class PostXcodeBuild
{
#if UNITY_IOS
    private struct InfoPlistInfo
    {
        public string key;
        public string value;
        public InfoPlistInfo(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    };

    private struct LocalizationInfo
    {
        public string lang;
        public InfoPlistInfo[] infoPlist;
        public LocalizationInfo(string lang, InfoPlistInfo[] infoPlist)
        {
            this.lang = lang;
            this.infoPlist = infoPlist;
        }
    };

    private static LocalizationInfo[] localizationInfo = {
        new LocalizationInfo("en", new InfoPlistInfo[]
        {
            new InfoPlistInfo("CFBundleDisplayName", "Weather")
        }),
        new LocalizationInfo("ja", new InfoPlistInfo[]
        {
            new InfoPlistInfo("CFBundleDisplayName", "お天気")
        })
    };

    private static void createInfoPlistString(string pjDirPath, LocalizationInfo localizationInfo)
    {
        string dirPath = Path.Combine(pjDirPath, "Unity-iPhone Tests");
        if (!Directory.Exists(Path.Combine(dirPath, string.Format("{0}.lproj", localizationInfo.lang))))
        {
            Directory.CreateDirectory(Path.Combine(dirPath, string.Format("{0}.lproj", localizationInfo.lang)));
        }

        string plistPath = Path.Combine(dirPath, string.Format("{0}.lproj/InfoPlist.strings", localizationInfo.lang));
        StreamWriter writer = new StreamWriter(plistPath, false);
        foreach (InfoPlistInfo info in localizationInfo.infoPlist)
        {
            string convertedValue = System.Text.Encoding.UTF8.GetString(
                System.Text.Encoding.Convert(
                    System.Text.Encoding.Unicode,
                    System.Text.Encoding.UTF8,
                    System.Text.Encoding.Unicode.GetBytes(info.value)
                    )
            );
            writer.WriteLine(string.Format(info.key + " = \"{0}\";", convertedValue));
        }
        writer.Close();
    }

    private static void addKnownRegions(string pjDirPath, LocalizationInfo[] infoList)
    {
        string tmpString = "";
        string pjPath = PBXProject.GetPBXProjectPath(pjDirPath);

        foreach (LocalizationInfo info in infoList)
        {
            tmpString += "\t\t" + info.lang + ",\n";
        }
        tmpString += "\t\t);\n";

        StreamReader reader = new StreamReader(pjPath);
        string text = "";
        string tmpLine = "";
        while (reader.Peek() >= 0)
        {
            tmpLine = reader.ReadLine();
            if (tmpLine.IndexOf("knownRegions") != -1)
            {
                text += tmpLine + "\n";
                text += tmpString;
                while (true)
                {
                    tmpLine = reader.ReadLine();
                    if (tmpLine.IndexOf(");") != -1)
                    {
                        break;
                    }
                }
            } 
            else
            {
                text += tmpLine + "\n";
            }
        }
        reader.Close();
        StreamWriter writer = new StreamWriter(pjPath, false);
        writer.Write(text);
        writer.Close();
    }

    [PostProcessBuild]
    public static void SetXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS) {
            return;
        }

        foreach (LocalizationInfo info in localizationInfo)
        {
            createInfoPlistString(pathToBuiltProject, info);
        }
        addKnownRegions(pathToBuiltProject, localizationInfo);
    }
#endif
}