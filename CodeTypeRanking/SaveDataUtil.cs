using System.IO;

namespace CodeTypeRanking
{
    static class SaveDataUtil
    {
        const string FileName = "myObjectId.txt";

        /// <summary>
        /// ObjectId の読み込み
        /// </summary>
        /// <returns></returns>
        public static string LoadObjectId()
        {
            if (!File.Exists(FileName))
            {
                return null;
            }

            string objectId = null;
            using (StreamReader sr = new StreamReader(FileName))
            {
                objectId = sr.ReadLine();
            }

            return objectId;
        }

        /// <summary>
        /// ObjectId の保存
        /// </summary>
        public static void SaveObjectId(string objectid)
        {
            using (StreamWriter sw = new StreamWriter(FileName))
            {
                sw.Write(objectid);
            }
        }
    }
}
