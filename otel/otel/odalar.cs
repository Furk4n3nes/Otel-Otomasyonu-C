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
    public partial class odalar : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False");

        private void musterigecmistablo()
        {
            try
            {
                baglanti.Open();

                // Boş girişleri kontrol et
                if (string.IsNullOrWhiteSpace(txtadi.Text) ||
                    string.IsNullOrWhiteSpace(txtsoyad.Text) ||
                    string.IsNullOrWhiteSpace(msktelefon.Text) ||
                    string.IsNullOrWhiteSpace(msktc.Text) ||
                    string.IsNullOrWhiteSpace(txtucret.Text) ||
                    string.IsNullOrWhiteSpace(txtoda.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // `decimal` ve `int` türündeki değerleri güvenli şekilde parse et
                if (!decimal.TryParse(txtucret.Text.Trim(), out decimal ucret))
                {
                    MessageBox.Show("Geçerli bir ücret girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtoda.Text.Trim(), out int odaId))
                {
                    MessageBox.Show("Geçerli bir oda numarası girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Veritabanına ekleme sorgusu
                string musteriEkleSorgu = @"
            INSERT INTO musterigecmis (musteri_ad, musteri_soyad, musteri_telefon, musteri_tc, ucret, giris, cikis, oda_id, temizlikucret)
            VALUES (@musteri_ad, @musteri_soyad, @musteri_telefon, @musteri_tc, @ucret, @giris, @cikis, @oda_id, @temizlikucret)";

                SqlCommand musteriEkleCmd = new SqlCommand(musteriEkleSorgu, baglanti);
                musteriEkleCmd.Parameters.AddWithValue("@musteri_ad", txtadi.Text.Trim());
                musteriEkleCmd.Parameters.AddWithValue("@musteri_soyad", txtsoyad.Text.Trim());
                musteriEkleCmd.Parameters.AddWithValue("@musteri_telefon", msktelefon.Text.Trim());
                musteriEkleCmd.Parameters.AddWithValue("@musteri_tc", msktc.Text.Trim());
                musteriEkleCmd.Parameters.AddWithValue("@ucret", ucret);
                musteriEkleCmd.Parameters.AddWithValue("@giris", giristrh.Value.Date);
                musteriEkleCmd.Parameters.AddWithValue("@cikis", cikistrh.Value.Date);
                musteriEkleCmd.Parameters.AddWithValue("@oda_id", odaId);
                musteriEkleCmd.Parameters.AddWithValue("@temizlikucret", label10.Text);

                musteriEkleCmd.ExecuteNonQuery();
                MessageBox.Show("Müşteri başarıyla geçmişe eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri eklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti.State == System.Data.ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void odadurumuguncelle()
        {
            
            string connectionString = "Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False";
            string query = "SELECT oda_id, oda_durum FROM odalar";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    // Her oda kaydını kontrol et
                    while (reader.Read())
                    {
                        int odaId = reader.GetInt32(0); // oda_id
                        bool odaDurum = reader.GetBoolean(1); // oda_durum

                        // GroupBox içindeki butona erişim sağla
                        Button odaButton = groupControl1.Controls["button" + odaId.ToString()] as Button;

                        if (odaButton != null)
                        {
                            // Eğer oda_durum true ise, butonun rengini kırmızı yap
                            if (odaDurum)
                            {
                                odaButton.BackColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                odaButton.BackColor = System.Drawing.Color.Green; // False ise farklı bir renk
                            }
                        }
                        else
                        {
                            // Buton bulunamazsa hata mesajı
                            MessageBox.Show("Button not found for oda_id: " + odaId.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısı başarısız: " + ex.Message);
            }
        }


        private void VerileriYukle()
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // admin tablosundaki tüm verileri seç
                string sql = "SELECT * FROM musteriler";
                SqlCommand komut = new SqlCommand(sql, baglanti);

                // Verileri almak için SqlDataAdapter kullan
                SqlDataAdapter da = new SqlDataAdapter(komut);

                // DataTable ile verileri tut
                DataTable dt = new DataTable();
                da.Fill(dt);

                // DataGridView'e veriyi ata
                data.DataSource = dt;
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

        public odalar()
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

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            anasayfa fr = new anasayfa();
            fr.Show();
            this.Hide();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Tıklanan hücrenin satır indexini al
            int rowIndex = e.RowIndex;

            // Eğer geçerli bir satıra tıklandıysa (örneğin, başlık değilse)
            if (rowIndex >= 0)
            {
                // Tıklanan satırın verilerini al
                DataGridViewRow selectedRow = data.Rows[rowIndex];

                // TextBox'lara satırdaki verileri ata
                label4.Text = selectedRow.Cells["musteri_id"].Value.ToString();
                txtadi.Text = selectedRow.Cells["musteri_ad"].Value.ToString();
                txtsoyad.Text = selectedRow.Cells["musteri_soyad"].Value.ToString();
                msktelefon.Text = selectedRow.Cells["musteri_telefon"].Value.ToString();
                msktc.Text = selectedRow.Cells["musteri_tc"].Value.ToString();
                txtoda.Text = selectedRow.Cells["oda_id"].Value.ToString();
                txtucret.Text = selectedRow.Cells["ucret"].Value.ToString();
                giristrh.Text = selectedRow.Cells["giris"].Value.ToString();
                cikistrh.Text = selectedRow.Cells["cikis"].Value.ToString();

            }
            
        }

        private void odalar_Load(object sender, EventArgs e)
        {
            VerileriYukle();
            odadurumuguncelle();

        }

        private void Btnguncelle(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                // Seçilen müşterinin ID'si
                int musteriId = int.Parse(label4.Text.Trim());

                // Yeni oda ID'si
                int yeniOdaId = int.Parse(txtoda.Text.Trim());

                // Mevcut oda ID'si (müşteri tablosundaki eski oda)
                string mevcutOdaSorgu = "SELECT oda_id FROM musteriler WHERE musteri_id = @musteri_id";
                SqlCommand mevcutOdaCmd = new SqlCommand(mevcutOdaSorgu, baglanti);
                mevcutOdaCmd.Parameters.AddWithValue("@musteri_id", musteriId);

                object mevcutOdaObj = mevcutOdaCmd.ExecuteScalar();

                if (mevcutOdaObj != null)
                {
                    int mevcutOdaId = (int)mevcutOdaObj;

                    // Eğer mevcut oda ile seçilen oda aynıysa güncellemeyin
                    if (mevcutOdaId == yeniOdaId)
                    {
                        // Müşteri bilgilerini güncelleme
                        string musteriGuncelle = @"
            UPDATE musteriler 
            SET musteri_ad = @musteri_ad, 
                musteri_soyad = @musteri_soyad, 
                musteri_telefon = @musteri_telefon, 
                musteri_tc = @musteri_tc, 
                ucret = @ucret, 
                giris = @giris, 
                cikis = @cikis 
            WHERE musteri_id = @musteri_id";

                        SqlCommand musteriCmd = new SqlCommand(musteriGuncelle, baglanti);
                        musteriCmd.Parameters.AddWithValue("@musteri_id", musteriId);
                        musteriCmd.Parameters.AddWithValue("@musteri_ad", txtadi.Text.Trim());
                        musteriCmd.Parameters.AddWithValue("@musteri_soyad", txtsoyad.Text.Trim());
                        musteriCmd.Parameters.AddWithValue("@musteri_telefon", msktelefon.Text.Trim());
                        musteriCmd.Parameters.AddWithValue("@musteri_tc", msktc.Text.Trim());
                        musteriCmd.Parameters.AddWithValue("@ucret", decimal.Parse(txtucret.Text.Trim()));
                        musteriCmd.Parameters.AddWithValue("@giris", giristrh.Value.Date);
                        musteriCmd.Parameters.AddWithValue("@cikis", cikistrh.Value.Date);

                        musteriCmd.ExecuteNonQuery();

                        MessageBox.Show("Müşteri bilgileri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Eski oda durumunu 0 (boş) yap
                        string eskiOdaGuncelle = "UPDATE odalar SET oda_durum = 0 WHERE oda_id = @oda_id";
                        SqlCommand eskiOdaCmd = new SqlCommand(eskiOdaGuncelle, baglanti);
                        eskiOdaCmd.Parameters.AddWithValue("@oda_id", mevcutOdaId);
                        eskiOdaCmd.ExecuteNonQuery();

                        // Yeni oda durumunu 1 (dolu) yap
                        string yeniOdaGuncelle = "UPDATE odalar SET oda_durum = 1 WHERE oda_id = @oda_id";
                        SqlCommand yeniOdaCmd = new SqlCommand(yeniOdaGuncelle, baglanti);
                        yeniOdaCmd.Parameters.AddWithValue("@oda_id", yeniOdaId);
                        yeniOdaCmd.ExecuteNonQuery();

                        // Müşteri bilgilerini güncelle
                        string musteriGuncelle = @"
            UPDATE musteriler 
            SET musteri_ad = @musteri_ad, 
                musteri_soyad = @musteri_soyad, 
                musteri_telefon = @musteri_telefon, 
                musteri_tc = @musteri_tc, 
                ucret = @ucret, 
                oda_id = @oda_id, 
                giris = @giris, 
                cikis = @cikis 
            WHERE musteri_id = @musteri_id";

                        SqlCommand musteriCmd = new SqlCommand(musteriGuncelle, baglanti);
                        musteriCmd.Parameters.AddWithValue("@musteri_id", musteriId);
                        musteriCmd.Parameters.AddWithValue("@musteri_ad", txtadi.Text.Trim());
                        musteriCmd.Parameters.AddWithValue("@musteri_soyad", txtsoyad.Text.Trim());
                        musteriCmd.Parameters.AddWithValue("@musteri_telefon", msktelefon.Text.Trim());
                        musteriCmd.Parameters.AddWithValue("@musteri_tc", msktc.Text.Trim());
                        musteriCmd.Parameters.AddWithValue("@ucret", decimal.Parse(txtucret.Text.Trim()));
                        musteriCmd.Parameters.AddWithValue("@oda_id", yeniOdaId);
                        musteriCmd.Parameters.AddWithValue("@giris", giristrh.Value.Date);
                        musteriCmd.Parameters.AddWithValue("@cikis", cikistrh.Value.Date);

                        musteriCmd.ExecuteNonQuery();

                        MessageBox.Show("Müşteri bilgileri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Mevcut müşteri bilgisi bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti.State == System.Data.ConnectionState.Open)
                {
                    baglanti.Close();
                    VerileriYukle();
                    odadurumuguncelle();
                }
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtoda.Text = "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtoda.Text = "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtoda.Text = "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtoda.Text = "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtoda.Text = "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtoda.Text = "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtoda.Text = "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtoda.Text = "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtoda.Text = "9";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtoda.Text = "10";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            txtoda.Text = "11";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            txtoda.Text = "12";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            txtoda.Text = "13";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            txtoda.Text = "14";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            txtoda.Text = "15";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            txtoda.Text = "16";
        }

        private void cikistrh_ValueChanged(object sender, EventArgs e)
        {
            int ucret;
            DateTime kucuktrh = Convert.ToDateTime(giristrh.Text);
            DateTime buyuktrh = Convert.ToDateTime(cikistrh.Text);

            TimeSpan sonuc = buyuktrh - kucuktrh;

            gunfark.Text = sonuc.TotalDays.ToString();

            ucret = Convert.ToInt32(gunfark.Text) * 500;

            txtucret.Text = ucret.ToString();
        }

        private void Btnsil(object sender, EventArgs e)
        {
            musterigecmistablo();

            try
            {
                baglanti.Open();
                

                // Seçilen müşterinin ID'si
                int musteriId = int.Parse(label4.Text.Trim());

                // Müşterinin oda ID'sini al
                string mevcutOdaSorgu = "SELECT oda_id FROM musteriler WHERE musteri_id = @musteri_id";
                SqlCommand mevcutOdaCmd = new SqlCommand(mevcutOdaSorgu, baglanti);
                mevcutOdaCmd.Parameters.AddWithValue("@musteri_id", musteriId);

                object mevcutOdaObj = mevcutOdaCmd.ExecuteScalar();

                if (mevcutOdaObj != null)
                {
                    int mevcutOdaId = (int)mevcutOdaObj;

                    // Eski oda durumunu 0 (boş) yap
                    string eskiOdaGuncelle = "UPDATE odalar SET oda_durum = 0 WHERE oda_id = @oda_id";
                    SqlCommand eskiOdaCmd = new SqlCommand(eskiOdaGuncelle, baglanti);
                    eskiOdaCmd.Parameters.AddWithValue("@oda_id", mevcutOdaId);
                    eskiOdaCmd.ExecuteNonQuery();
                }

                // Müşteriyi sil
                string musteriSil = "DELETE FROM musteriler WHERE musteri_id = @musteri_id";
                SqlCommand musteriSilCmd = new SqlCommand(musteriSil, baglanti);
                musteriSilCmd.Parameters.AddWithValue("@musteri_id", musteriId);
                musteriSilCmd.ExecuteNonQuery();

                MessageBox.Show("Müşteri başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Form alanlarını temizle
                txtadi.Clear();
                txtsoyad.Clear();
                msktelefon.Clear();
                msktc.Clear();
                txtucret.Clear();
                giristrh.Value = DateTime.Now;
                cikistrh.Value = DateTime.Now;
                txtoda.Clear();

                // DataGridView'i güncelle
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti.State == System.Data.ConnectionState.Open)
                {
                    baglanti.Close();
                    VerileriYukle();
                    odadurumuguncelle();
                    
                }
            }

        }

        
    }
}
