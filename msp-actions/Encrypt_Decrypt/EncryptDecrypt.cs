using msp_actions.LOGGING;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;

namespace msp_actions.Encrypt_Decrypt
{
    internal class EncryptDecrypt
    {
        public EncryptDecrypt(IHostingEnvironment h)
        {
            SetEnv(h);
        }
        //private static IWebHostEnvironment ___hostingEnv;
        private static IHostingEnvironment? _hostingEnvironment;
        private static void SetEnv(IHostingEnvironment h)
        {
            _hostingEnvironment = h;
        }
        [XmlRoot(ElementName = "XML_EncryptDecrypt")]
        public class XML_EncryptDecrypt
        {
            [XmlElement("Public")]
            public RSAParameters Public { get; set; }

            [XmlElement("Private")]
            public RSAParameters Private { get; set; }
        }

        public class XMLTYPE<T>
        {
            [XmlAttribute]
            public T? Value { get; set; }
        }

        static int KeyLength;

        private static void CheckEncyptionSize(string key)
        {
            var key1 = Encoding.UTF8.GetBytes(key);
            //myAes.Key = Key; //ERROR
            KeySizes[] ks = Aes.Create().LegalKeySizes;
            foreach (KeySizes item in ks)
            {
                KeyLength = (int)key1.LongLength;
                Console.WriteLine(
                    "Legal min key size = " + item.MinSize + $" {key1.LongLength}"
                );
                Console.WriteLine(
                    "Legal max key size = " + item.MaxSize + $" {key1.LongLength}"
                );
            }
        }

        private static T? Deserialize<T>(string data) where T : class
        {
            if (data == null)
            {
                return null;
            }
            else
            {
                if (data.Trim().Length == 0)
                {
                    return null;
                }

                var ser = new XmlSerializer(typeof(T));

                using (var sr = new StringReader(data))
                {
                    return (T?)ser.Deserialize(sr);
                }
            }
        }

        public static string v2_EncryptText(string text)
        {
            string Return = "";
            try
            {
                using (var rsa = new RSACryptoServiceProvider() { KeySize = 1024 * 2 })
                {
                    var path = _hostingEnvironment?.ContentRootPath;
                    RSAParameters? pub;
                    RSAParameters? priv;
                    //    first we read from the xml to check if RSAParams exist already
                    using (
                        Stream reader = new FileStream(
                            path + "/Content/Encrypt_Decrypt.xml",
                            FileMode.Open
                        )
                    )
                    {
                        var xs = new System.Xml.Serialization.XmlSerializer(
                            typeof(XML_EncryptDecrypt)
                        );
                        //get the object back from the stream
                        var ret = (XML_EncryptDecrypt?)xs.Deserialize(reader);
                        pub = ret?.Public;
                        priv = ret?.Private;
                        // Write out the properties of the object.

                        if (pub == null)
                        {
                            Console.WriteLine("It is empty");
                            pub = rsa.ExportParameters(false);
                            priv = rsa.ExportParameters(true);
                            using (
                                Stream writer = new FileStream(
                                    path + "/Content/Encrypt_Decrypt.xml",
                                    FileMode.Open,
                                    FileAccess.ReadWrite
                                )
                            )
                            {
                                var c = new XML_EncryptDecrypt();
                                c.Public = (RSAParameters)pub;
                                c.Private = (RSAParameters)priv;
                                var xs2 = new System.Xml.Serialization.XmlSerializer(
                                    typeof(XML_EncryptDecrypt)
                                );

                                //get the object back from the stream
                                xs2.Serialize(writer, c);
                            }
                        }

                        rsa.ImportParameters((RSAParameters)pub);
                        var byteData = Encoding.Unicode.GetBytes(text);
                        var encryptedData = rsa.Encrypt(byteData, false);
                        Return = Convert.ToBase64String(encryptedData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { }
            //Logging.Log("Encrypting", "Item", text, Return);
            return Return;
        }

        public static byte[] v2_EncryptText_ToBytes(string text)
        {
            byte[] Return = new byte[] { };
            try
            {
                using (var rsa = new RSACryptoServiceProvider() { KeySize = 1024 * 2 })
                {
                    var path = _hostingEnvironment?.ContentRootPath;
                    RSAParameters? pub;
                    RSAParameters? priv;
                    //    first we read from the xml to check if RSAParams exist already
                    using (
                        Stream reader = new FileStream(
                            path + "/Content/Encrypt_Decrypt.xml",
                            FileMode.Open
                        )
                    )
                    {
                        var xs = new System.Xml.Serialization.XmlSerializer(
                            typeof(XML_EncryptDecrypt)
                        );
                        //get the object back from the stream
                        var ret = (XML_EncryptDecrypt?)xs.Deserialize(reader);
                        pub = ret?.Public;
                        priv = ret?.Private;
                        // Write out the properties of the object.

                        if (pub == null)
                        {
                            Console.WriteLine("It is empty");
                            pub = rsa.ExportParameters(false);
                            priv = rsa.ExportParameters(true);
                            using (
                                Stream writer = new FileStream(
                                    path + "/Content/Encrypt_Decrypt.xml",
                                    FileMode.Open,
                                    FileAccess.ReadWrite
                                )
                            )
                            {
                                var c = new XML_EncryptDecrypt();
                                c.Public = (RSAParameters)pub;
                                c.Private = (RSAParameters)priv;
                                var xs2 = new System.Xml.Serialization.XmlSerializer(
                                    typeof(XML_EncryptDecrypt)
                                );

                                //get the object back from the stream
                                xs2.Serialize(writer, c);
                            }
                        }

                        rsa.ImportParameters((RSAParameters)pub);
                        var byteData = Encoding.Unicode.GetBytes(text);
                        var encryptedData = rsa.Encrypt(byteData, false);
                        Return = encryptedData;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { }
            //Logging.Log("Encrypting", "Item", text, Return.Length.ToString());
            return Return;
        }

        public static string v2_DecryptText(string? text)
        {
            if (text == null || text == "" || text?.Length < 100)
            {
                return "Empty, Null, or non encrypted string";
            }
            string Return = "";
            try
            {
                using (var rsa = new RSACryptoServiceProvider() { KeySize = 1024 * 2 })
                {
                    var path = _hostingEnvironment?.ContentRootPath;
                    RSAParameters? pub;
                    RSAParameters? priv;
                    //first we read from the xml to check if RSAParams exist already
                    using (
                        Stream reader = new FileStream(
                            path + "/Content/Encrypt_Decrypt.xml",
                            FileMode.Open
                        )
                    )
                    {
                        var xs = new System.Xml.Serialization.XmlSerializer(
                            typeof(XML_EncryptDecrypt)
                        );
                        //get the object back from the stream
                        var ret = (XML_EncryptDecrypt?)xs.Deserialize(reader);
                        pub = ret?.Public;
                        priv = ret?.Private;
                        // Write out the properties of the object.

                        var privateRSA = (RSAParameters?)priv;
                        if (privateRSA == null)
                        {
                            privateRSA = rsa.ExportParameters(true);
                        }

                        rsa.ImportParameters((RSAParameters)privateRSA);
                        byte[] bi = Convert.FromBase64String(text.Trim());
                        byte[] bdecr = rsa.Decrypt(bi, false);
                        Return = Encoding.Unicode.GetString(bdecr);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            Logging.Log("Decrypting", "Item", text, Return);
            return Return;
        }

        public static string v2_DecryptText_FromBytes(byte[]? text)
        {
            string Return = "";
            if (text == null || text.Length < 1)
            {
                return Return;
            }
            try
            {
                using (var rsa = new RSACryptoServiceProvider() { KeySize = 1024 * 2 })
                {
                    var path = _hostingEnvironment?.ContentRootPath;
                    RSAParameters? pub;
                    RSAParameters? priv;
                    //first we read from the xml to check if RSAParams exist already
                    using (
                        Stream reader = new FileStream(
                            path + "/Content/Encrypt_Decrypt.xml",
                            FileMode.Open
                        )
                    )
                    {
                        var xs = new System.Xml.Serialization.XmlSerializer(
                            typeof(XML_EncryptDecrypt)
                        );
                        //get the object back from the stream
                        var ret = (XML_EncryptDecrypt?)xs.Deserialize(reader);
                        pub = ret?.Public;
                        priv = ret?.Private;
                        // Write out the properties of the object.

                        var privateRSA = (RSAParameters?)priv;
                        if (privateRSA == null)
                        {
                            privateRSA = rsa.ExportParameters(true);
                        }

                        rsa.ImportParameters((RSAParameters)privateRSA);
                        byte[] bdecr = rsa.Decrypt(text ?? new byte[] { }, false);
                        Return = Encoding.Unicode.GetString(bdecr ?? new byte[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally { }
            Logging.Log("Decrypting", "Item", text, Return);
            return Return;
        }

    }
}
