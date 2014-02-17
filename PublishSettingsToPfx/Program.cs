using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace PublishSettingsToPfx
{
    class Program
    {
        static void Main(string[] args)
        {
            string publishSettingsFile = args[0];
            string pfxPassword = args[1];

            string certString = GetCertStringFromPublishSettings(publishSettingsFile);

            var cert = new X509Certificate2(Convert.FromBase64String(certString));
            
            byte[] certData = cert.Export(X509ContentType.Pfx, pfxPassword);
            File.WriteAllBytes(Path.ChangeExtension(publishSettingsFile, ".pfx"), certData);
        }

        static string GetCertStringFromPublishSettings(string path)
        {
            var document = XDocument.Load(path);
            var profile = document.Descendants("PublishProfile").First();

            return profile.Attribute("ManagementCertificate").Value;
        }
    }
}
