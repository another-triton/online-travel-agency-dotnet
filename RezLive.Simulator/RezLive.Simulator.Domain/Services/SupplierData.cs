using System.Text;

namespace RezLive.Simulator.Domain.Services
{
    public static class SupplierData
    {
        public static int[] suppliers { get; set; } = null!;
        public static string[] xmlList { get; set; } = null!;

        public static void PopulateXmlList(string relativePath)
        {
            suppliers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
            xmlList = new string[suppliers.Length];
            StringBuilder sbXML = new();
            for (int i = 0; i < suppliers.Length; i++)
            {
                string[] supplierXmls = GetXMLPathBySupplier(Convert.ToInt32(suppliers[i]), relativePath);
                foreach (string path in supplierXmls)
                {
                    var xml = GetXml(path);                   
                    sbXML.AppendLine(xml[9..^11]);
                }

                xmlList[i] = $"<Hotels>{sbXML}</Hotels>";
                sbXML.Clear();
            }
        }

        private static string[] GetXMLPathBySupplier(int supplierId, string relativePath)
        {
            return Directory.GetFiles(relativePath, $"sup_{supplierId}_*.xml");
        }

        private static string GetXml(string path)
        {
            using FileStream stream = File.OpenRead(path);
            using StreamReader streamReader = new(stream);
            return streamReader.ReadToEnd();
        }
    }
}
