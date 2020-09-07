using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Transactions;
using WebApp.Common;
using WebApp.Models;

namespace WebApp.Services
{
    public class MenuService
    {
        private AppConfig config;

        public MenuService(AppConfig config)
        {
            this.config = config;
        }

        public void Add(Menu menu)
        {
            //トランザクション開始
            using (TransactionScope scope = new TransactionScope())
            {
                //using (SqlConnection conn = new SqlConnection(DbHelper.getConnectionString(this.Configuration)))
                using (SqlConnection conn = new SqlConnection(config.ConnectionString))
                {
                    String sql = @"
                            INSERT INTO Menu(MenuId,Description,Unit)
                            VALUES (@MenuId,@Description,@Unit)
                            ";

                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql))
                    {

                        command.Connection = conn;
                        //System.Diagnostics.Debug.WriteLine("----------------");
                        command.Parameters.AddWithValue("@MenuId", menu.MenuId);
                        command.Parameters.AddWithValue("@Description", menu.Description);
                        command.Parameters.AddWithValue("@Unit", menu.Unit);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (var audio in menu.AudioFiles)
                {
                    //using (SqlConnection conn = new SqlConnection(DbHelper.getConnectionString(this.Configuration)))
                    using (SqlConnection conn = new SqlConnection(config.ConnectionString))
                    {
                        String sql = @"
                            INSERT INTO AudioFile(MenuId,FileName,Description,S3Url)
                            VALUES (@MenuId,@FileName,@Description,@S3Url)
                            ";

                        conn.Open();
                        using (SqlCommand command = new SqlCommand(sql))
                        {

                            command.Connection = conn;
                            //System.Diagnostics.Debug.WriteLine("----------------");
                            command.Parameters.AddWithValue("@MenuId", menu.MenuId);
                            command.Parameters.AddWithValue("@FileName", audio.FileName);
                            command.Parameters.AddWithValue("@Description", audio.Description);
                            command.Parameters.AddWithValue("@S3Url", ""); //ここでは設定しない。バッチでやるか？
                            command.ExecuteNonQuery();
                        }
                    }
                }

                scope.Complete();
            }
        }

        public void PutAudioFilesToS3(IFormFileCollection files)
        {
            using (var client = new AmazonS3Client())
            {
                // ファイルをS3にput
                foreach (var formFile in files)
                {
                    //using (var newMemoryStream = new MemoryStream())
                    using (var stream = formFile.OpenReadStream())
                    {
                        //formFile.CopyTo(newMemoryStream);
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = stream,
                            Key = formFile.FileName,
                            BucketName = "com.jurin-n.audio-files/webapp"
                        };

                        var fileTransferUtility = new TransferUtility(client);
                        fileTransferUtility.Upload(uploadRequest);
                    }
                }
            }
        }
    }
}
