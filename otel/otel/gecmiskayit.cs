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
    public partial class gecmiskayit : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False");


        private void toplamodaucret()
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // SQL sorgusunu oluştur
                string toplamGelirSorgu = " SELECT SUM(ucret) FROM musterigecmis";

                // SQL komutunu hazırla
                SqlCommand cmd = new SqlCommand(toplamGelirSorgu, baglanti);

                // Sorguyu çalıştır ve sonucu al
                object sonuc = cmd.ExecuteScalar();

                // Eğer sonuç null ise 0 yazdır, aksi takdirde sonucu Label'a yazdır
                if (sonuc != DBNull.Value)
                {
                    label6.Text = sonuc.ToString() + " TL";
                }
                else
                {
                    label6.Text = "0 TL";
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını göster
                MessageBox.Show("Toplam gelir hesaplanırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                if (baglanti.State == System.Data.ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }



        private void toplamtemizlik()
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // SQL sorgusunu oluştur
                string toplamGelirSorgu = " SELECT SUM(temizlikucret) FROM musterigecmis";

                // SQL komutunu hazırla
                SqlCommand cmd = new SqlCommand(toplamGelirSorgu, baglanti);

                // Sorguyu çalıştır ve sonucu al
                object sonuc = cmd.ExecuteScalar();

                // Eğer sonuç null ise 0 yazdır, aksi takdirde sonucu Label'a yazdır
                if (sonuc != DBNull.Value)
                {
                    label4.Text = sonuc.ToString() + " TL";
                }
                else
                {
                    label4.Text = "0 TL";
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını göster
                MessageBox.Show("Toplam gelir hesaplanırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                if (baglanti.State == System.Data.ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }



        private void ToplamGelirHesapla()
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // SQL sorgusunu oluştur
                string toplamGelirSorgu = "SELECT SUM(ucret) - SUM(temizlikucret) FROM musterigecmis";

                // SQL komutunu hazırla
                SqlCommand cmd = new SqlCommand(toplamGelirSorgu, baglanti);

                // Sorguyu çalıştır ve sonucu al
                object sonuc = cmd.ExecuteScalar();

                // Eğer sonuç null ise 0 yazdır, aksi takdirde sonucu Label'a yazdır
                if (sonuc != DBNull.Value)
                {
                    label2.Text = sonuc.ToString() + " TL";
                }
                else
                {
                    label2.Text = "0 TL";
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını göster
                MessageBox.Show("Toplam gelir hesaplanırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                if (baglanti.State == System.Data.ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void Verileriyukle()
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // admin tablosundaki tüm verileri seç
                string sql = "SELECT * FROM musterigecmis";
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
        public gecmiskayit()
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

        private void gecmiskayit_Load(object sender, EventArgs e)
        {
            Verileriyukle();
            ToplamGelirHesapla();
            toplamtemizlik();
            toplamodaucret();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            anasayfa fr=new anasayfa();
            fr.Show();
            this.Hide();
        }
    }
}
