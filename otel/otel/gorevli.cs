using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace otel
{
    public partial class gorevli : Form
    {
        private bool sifreGorunur = false;
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False");

        private void VerileriYenidenYukle()
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Verileri çekmek için SQL sorgusu
                string sql = "SELECT * FROM admin";
                SqlCommand komut = new SqlCommand(sql, baglanti);
                SqlDataAdapter da = new SqlDataAdapter(komut);

                // Verileri DataTable'e doldur
                DataTable dt = new DataTable();
                da.Fill(dt);

                // DataGridView'e verileri bağla
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }



        public gorevli()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            anasayfa fr = new anasayfa();
            fr.Show();
            this.Hide();
        }

        private void btnGosterGizle_Click(object sender, EventArgs e)
        {
            if (sifreGorunur)
            {
                // Şifreyi gizle
                txtsifre.PasswordChar = '*';
                sifreGorunur = false;
            }
            else
            {
                // Şifreyi göster
                txtsifre.PasswordChar = '\0'; // Şifreyi açık hale getirir
                sifreGorunur = true;
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            // Parametreli sorgu ile şifreyi güncelle
            SqlCommand komut = new SqlCommand("UPDATE admin SET kullanici_sifre = @yeniSifre WHERE kullanici_id = @kullaniciAdi", baglanti);
            komut.Parameters.AddWithValue("@yeniSifre", txtsifre.Text);
            komut.Parameters.AddWithValue("@kullaniciAdi", txtkullanici.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();

            VerileriYenidenYukle();

            MessageBox.Show("Görevli Bilgileri Güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Silme sorgusu: kullanici_id'ye göre kayıt sil
                string sql = "DELETE FROM admin WHERE kullanici_id = @kullaniciAdi";

                // SQL komutunu oluştur ve parametreleri ata
                SqlCommand komut = new SqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@kullaniciAdi", txtkullanici.Text);

                // Sorguyu çalıştır
                int result = komut.ExecuteNonQuery();
                

                // İşlem sonucuna göre mesaj göster
                if (result > 0)
                {
                    MessageBox.Show("Görevli Bilgileri Silindi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Silinecek Kayıt Bulunamadı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                VerileriYenidenYukle();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                // Bağlantıyı kapat
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            
        }

        private void gorevli_Load(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // admin tablosundaki tüm verileri seç
                string sql = "SELECT * FROM admin";
                SqlCommand komut = new SqlCommand(sql, baglanti);

                // Verileri almak için SqlDataAdapter kullan
                SqlDataAdapter da = new SqlDataAdapter(komut);

                // DataTable ile verileri tut
                DataTable dt = new DataTable();
                da.Fill(dt);

                // DataGridView'e veriyi ata
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Tıklanan hücrenin satır indexini al
            int rowIndex = e.RowIndex;

            // Eğer geçerli bir satıra tıklandıysa (örneğin, başlık değilse)
            if (rowIndex >= 0)
            {
                // Tıklanan satırın verilerini al
                DataGridViewRow selectedRow = dataGridView1.Rows[rowIndex];

                // TextBox'lara satırdaki verileri ata
                txtkullanici.Text = selectedRow.Cells["kullanici_id"].Value.ToString();
                txtsifre.Text = selectedRow.Cells["kullanici_sifre"].Value.ToString();
            }
        }

    }
}
