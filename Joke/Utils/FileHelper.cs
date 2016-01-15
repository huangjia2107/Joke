using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Joke.Utils
{
    public class FileHelper
    {
        public static readonly string CredentialSource = "JokeUserAndPassword";

        public static void SaveCredential(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return;

            //用于保证只存在一个用户信息
            RemoveCredential();

            PasswordVault vault = new PasswordVault();
            PasswordCredential cred = new PasswordCredential(CredentialSource, username, password);
            vault.Add(cred);
        }

        public static PasswordCredential RetrieveCredential(string username)
        {
            try
            {
                PasswordVault vault = new PasswordVault();
                return vault.Retrieve(CredentialSource, username);
            }
            catch
            {
                return null;
            }
        }

        public static IReadOnlyList<PasswordCredential> RetrieveAllCredential()
        {
            try
            {
                PasswordVault vault = new PasswordVault();
                return vault.FindAllByResource(CredentialSource);
            }
            catch
            {
                return null;
            }
        }

        public static void RemoveCredential()
        {
            var pcList = RetrieveAllCredential();
            if (pcList == null)
                return;

            PasswordVault vault = new PasswordVault();
            foreach (var pc in pcList)
            {
                vault.Remove(pc);
            }
        }
    }
}
