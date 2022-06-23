using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;



    public class CryptographyManager
    {
        public static readonly Encoding Encoder = Encoding.UTF8;
        public enum HashName
        {
            SHA1 = 1,
            MD5 = 2,
            SHA256 = 4,
            SHA384 = 8,
            SHA512 = 16
        }
        public  string ComputeHash(string plainText, HashName hashName)
        {
            var sb = new StringBuilder();
            // Make sure hashing algorithm is specified.
            HashAlgorithm hash;

            // Initialize appropriate hashing algorithm class.
            switch (hashName)
            {
                case HashName.SHA1:
                    hash = new SHA1Managed();
                    break;
                case HashName.SHA256:
                    hash = new SHA256Managed();
                    break;
                case HashName.SHA384:
                    hash = new SHA384Managed();
                    break;
                case HashName.SHA512:
                    hash = new SHA512Managed();
                    break;
                case HashName.MD5:
                    hash = new MD5CryptoServiceProvider();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hashName), hashName, null);
            }

            var result = hash.ComputeHash(Encoder.GetBytes(plainText));

       
            foreach (var b in result)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public  bool VerifyHash(string plainText, string hashValue, HashName hashName)
        {
            var computedHash = ComputeHash(plainText, hashName);
            //WebLog.Log("Correct Hashvalue: "+ computedHash);
           // WebLog.Log("wrong Hashvalue: " + hashValue);
            return hashValue.ToLower().ToString() == computedHash.ToLower().ToString();
        }


        public static string TripleDesEncrypt(string plainText, string Key)
        {
            var des = CreateDes(Key);
            var ct = des.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(plainText);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(output);
        }

        public static string TripleDesDecrypt(string cypherText, string Key)
        {
            var des = CreateDes(Key);
            var ct = des.CreateDecryptor();
            var input = Convert.FromBase64String(cypherText);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(output);
        }

        public static TripleDES CreateDes(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            return des;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }


