using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AbdullahErolEticaret.App_Classes
{
    public class Baglanti
    {
        // Bağlantı nesnesi
        private MySqlConnection connection;
        // Bağlantı özelliği
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        // Bağlantı class constructor => Baglanti nesnemi türetmek için ve kodlarımda kullanmak için "Yapılandırıcı" nesnesi oluşturuyorum.
        public Baglanti()
        {
            // Bağlantı stringi 
            string connectionString = ConfigurationManager.ConnectionStrings["EticaretDB"].ConnectionString;
            // Bağlantı nesnesini oluştur
            connection = new MySqlConnection(connectionString);
        }

        // Bağlantıyı açan metot
        public MySqlConnection Open()
        {
            try
            {
                // Bağlantıyı aç
                connection.Open();
                // Bağlantı başarılı ise true döndür
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Hata olursa false döndür
                return connection;
            }
        }

        // Bağlantıyı kapatan metot
        public MySqlConnection Close()
        {
            try
            {
                // Bağlantıyı kapat
                connection.Close();
                // Bağlantı başarılı ise true döndür
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Hata olursa false döndür
                return connection;
            }
        }
    }
}