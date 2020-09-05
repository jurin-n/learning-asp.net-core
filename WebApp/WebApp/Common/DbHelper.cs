using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApp.Common
{
    public class DbHelper
    {
        public static String getConnectionString(IConfiguration configuration)
        {
            //暗号化パスワードがついたConnectionStringを取得し
            //SqlConnectionStringBuilderオブジェクト作成する。
            String connectionStringWithEncryptedPassword = configuration.GetConnectionString("MainDatabaseWithEncryptedPassword");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionStringWithEncryptedPassword);

            //パスワードのみ復号化
            String EncryptedPassword = builder.Password;
            builder.Password = decryptionPassword(EncryptedPassword);
            return builder.ToString();
        }

        private static string decryptionPassword(string encryptedPassword)
        {
            //TODO:復号ロジックを実装
            return encryptedPassword;
        }
    }
}
