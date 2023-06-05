using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Ogrenci_Not_Kayit
{
    public partial class FrmOgretmen : Form
    {
        public FrmOgretmen()
        {
            InitializeComponent();
        }
        public string numara;
        SqlBaglanti bgl = new SqlBaglanti();

        //DataGridview1'de öğrencileri listelemeye yarıyor
        void OgrenciListele()
        {
            SqlCommand komut = new SqlCommand("Select * from Tbl_Ogrenci",bgl.baglanti());
            SqlDataAdapter da=new SqlDataAdapter(komut);
            DataTable dt =new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        //DataGridview2'de Notları listelemeye yarıyor
        void NotListele()
        {
            SqlCommand komut = new SqlCommand("Select * from Tbl_Notlar", bgl.baglanti());
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt =new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

       
        
        private void FrmOgretmen_Load(object sender, EventArgs e)
        {
            //Durum TextBox'unda değişiklik yapamama
            TxtDurum.Enabled = false;

            //Numarayı Label'a getirmek
            LblNumara.Text = numara;

            //Öğrenci Listeleme
            OgrenciListele();

            //Notları Listeleme
            NotListele();


            //Numaraya göre isim bilgisi getirme
            SqlCommand komut = new SqlCommand("Select * from Tbl_Ogretmen where Numara=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", numara);
            SqlDataReader sqlDataReader = komut.ExecuteReader();
            while(sqlDataReader.Read())
            {
                LblAdSoyad.Text = sqlDataReader[1] + " " +sqlDataReader[2];
            }
            bgl.baglanti().Close();
        }

        string fotograf;
        private void BtnFotografSec_Click(object sender, EventArgs e)
        {
            //Butona Basıldığında Fotoğraf Ekleme İşlemini Gerçekleştiriyor
            openFileDialog1.ShowDialog();
            fotograf=openFileDialog1.FileName;
            pictureBox1.ImageLocation = fotograf;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //Butona Basıldığında Ekleme İşlemini Gerçekleştiriyor
            
            SqlCommand komut = new SqlCommand("Insert into Tbl_Ogrenci (Ad,Soyad,Numara,Sifre,Fotograf) values (@p1,@p2,@p3,@p4,@p5)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
            komut.Parameters.AddWithValue("@p3",MskNumara.Text);
            komut.Parameters.AddWithValue("@p4",TxtSifre.Text);
            komut.Parameters.AddWithValue("@p5", fotograf);
            komut.ExecuteNonQuery();

            MessageBox.Show("Öğrenci Başarılı Bir Şekilde Eklendi","Başarılı",MessageBoxButtons.OK, MessageBoxIcon.Information);
            bgl.baglanti().Close();
            OgrenciListele();
            NotListele();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Tıklanılan Hücredeki Verileri Araçlara Taşıyan Komut

            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            MskNumara.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtSifre.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();

            pictureBox1.ImageLocation = dataGridView1.Rows[secilen].Cells[5].Value.ToString();

            //Seçilen Numaraya Ait Not Bilgilerini İlgili Araçlara Aktarma

            SqlCommand komut=new SqlCommand("Select * from Tbl_Notlar where Id=(Select Id from Tbl_Ogrenci where Numara=@p1)",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1",MskNumara.Text);
            SqlDataReader dr=komut.ExecuteReader();
            while (dr.Read())
            {
                TxtSınav1.Text = dr[1].ToString();
                TxtSınav2.Text= dr[2].ToString();
                TxtSınav3.Text= dr[3].ToString();
                TxtProje.Text = dr[4].ToString();
                TxtOrtalama.Text = dr[5].ToString();
                TxtDurum.Text = dr[6].ToString();
            }
            bgl.baglanti().Close();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            //Öğrenci Bilgilerini Güncelleme
            SqlCommand komut = new SqlCommand("update Tbl_Ogrenci set Ad=@p1,Soyad=@p2,Sifre=@p3,Fotograf=@p4 where Numara=@p5", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
            komut.Parameters.AddWithValue("@p3", TxtSifre.Text);
            komut.Parameters.AddWithValue("@p4", fotograf);
            komut.Parameters.AddWithValue("@p5", MskNumara.Text);
            komut.ExecuteNonQuery();
            MessageBox.Show("Öğrenci Bilgileri Başarılı Bir Şekilde Güncellendi!","Başarılı",MessageBoxButtons.OK,MessageBoxIcon.Information);
            bgl.baglanti().Close();
            OgrenciListele();

            //Not Bilgilerini Güncelleme
            SqlCommand komut2 = new SqlCommand("update Tbl_Notlar set Sınav1=@p1,Sınav2=@p2,Sınav3=@p3,Proje=@p4,Ortalama=@p5,Durum=@p6 where Id=(Select Id From Tbl_Ogrenci where Numara=@p7)", bgl.baglanti());
            komut2.Parameters.AddWithValue("@p1", TxtSınav1.Text);
            komut2.Parameters.AddWithValue("@p2", TxtSınav2.Text);
            komut2.Parameters.AddWithValue("@p3", TxtSınav3.Text);
            komut2.Parameters.AddWithValue("@p4",TxtProje.Text);
            komut2.Parameters.AddWithValue("@p5",Convert.ToDecimal(TxtOrtalama.Text));
            komut2.Parameters.AddWithValue("@p6",TxtDurum.Text);
            komut2.Parameters.AddWithValue("@p7",MskNumara.Text);
            komut2.ExecuteNonQuery();
            MessageBox.Show("Öğrenci Bilgileri Başarılı Bir Şekilde Güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            bgl.baglanti().Close();
            OgrenciListele();
            NotListele();

        }

        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            //Öğrenci Not Durumunu Hesaplama
            double sinav1, sinav2, sinav3, proje, ortalama;
            sinav1 = Convert.ToDouble(TxtSınav1.Text);
            sinav2=Convert.ToDouble(TxtSınav2.Text);
            sinav3 = Convert.ToDouble(TxtSınav3.Text);
            proje=Convert.ToDouble(TxtProje.Text);
            ortalama = (sinav1 + sinav2 + sinav3 + proje) / 4;
            TxtOrtalama.Text=ortalama.ToString();
            if (ortalama>=50)
            {
                TxtDurum.Text = "True";
            }
            else
            {
                TxtDurum.Text = "False";
            }

        }

        private void BtnDuyuruOlustur_Click(object sender, EventArgs e)
        {
            FrmDuyuruOlustur frm = new FrmDuyuruOlustur();
            frm.Show();
        }
    }
}
