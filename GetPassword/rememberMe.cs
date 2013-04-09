using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using SharpLauncher;
using System.Windows.Forms;

namespace Resolute_Launcher {
    public class rememberMe { //Functions provided by SirCmpwn

        private static readonly byte[] LastLoginSalt = new byte[] { 0x0c, 0x9d, 0x4a, 0xe4, 0x1e, 0x83, 0x15, 0xfc };
        private const string LastLoginPassword = "passwordfile";

        static String rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft/");
        static String lastLoginPath = Path.Combine(rootPath, "lastlogin");

        public String[] GetLastLogin() {
            try {
                byte[] encryptedLogin = File.ReadAllBytes(lastLoginPath);
                PKCSKeyGenerator crypto = new PKCSKeyGenerator(LastLoginPassword, LastLoginSalt, 5, 1);
                ICryptoTransform cryptoTransform = crypto.Decryptor;
                byte[] decrypted = cryptoTransform.TransformFinalBlock(encryptedLogin, 0, encryptedLogin.Length);
                short userLength = IPAddress.HostToNetworkOrder(BitConverter.ToInt16(decrypted, 0));
                byte[] user = decrypted.Skip(2).Take(userLength).ToArray();
                short passLength = IPAddress.HostToNetworkOrder(BitConverter.ToInt16(decrypted, userLength + 2));
                byte[] password = decrypted.Skip(4 + userLength).ToArray();
                String[] result = new String[2];
                result[0] = System.Text.Encoding.UTF8.GetString(user);
                result[1] = System.Text.Encoding.UTF8.GetString(password);
                return result;
            }
            catch (Exception e){
                MessageBox.Show(e.StackTrace);
                return null;
            }
        }
    }
}
