using System;
using System.Runtime.InteropServices;
using System.Text;

namespace wp.dll.iniManager
{
    public class IniManager
    {
        private string Path { get; } = AppDomain.CurrentDomain.BaseDirectory + @"config.ini";

        public IniManager(string aPath) => Path = aPath;

        public IniManager() { }

        public string Get(string aSection, string aKey) => GetPrivateString(aSection, aKey);

        public void Set(string aSection, string aKey, string aValue)
        {
            Create();
            Set(aSection, aKey, aValue as object);
        }

        public void Set(string aSection, string aKey, object aValue)
        {
	        Create();
	        WritePrivateString(aSection, aKey, aValue.ToString().Replace("False", "0").Replace("True", "1"));
        }

        public bool CheckExist() => System.IO.File.Exists(Path);

        public void Create()
        {
            if (!CheckExist())
            {
                System.IO.File.Create(Path).Close();
            }
        }

        #region private
        private string GetPrivateString(string aSection, string aKey)
        {
            var buffer = new StringBuilder(Size);
            GetPrivateString(aSection, aKey, null, buffer, Size, Path);
            return buffer.ToString();
        }

        private void WritePrivateString(string aSection, string aKey, string aValue) =>
            WritePrivateString(aSection, aKey, aValue, Path);

        private const int Size = 1_024; //Максимальный размер (для чтения значения из файла)

        //Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        //Импорт функции WritePrivateProfileString (для записи значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);
        #endregion
    }
}
