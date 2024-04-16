using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using System.Data.SqlClient;

namespace B2BServiceProject
{
    public partial class detayfrm : DevExpress.XtraEditors.XtraForm
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);
        public detayfrm()
        {
            InitializeComponent();
        }

        private void detayfrm_Load(object sender, EventArgs e)
        {

            SqlCommand cmd = new SqlCommand("SELECT SIPARIS_TUT,SIP_NO,SIP_TARIHI,ENT_YERI,UNVAN,ENT_SIRKET_TIPI,ENT_VERGIDAIRESI,ENT_VNO,ENT_TCKN,ENT_EMAIL,FAT_ADRES,FAT_SEHIR,FAT_ILCE,FAT_SEMT_MAHALLE,FAT_GSM,SEVK_ADRES,SEVK_SEHIR,SEVK_SEMT_MAHALLE,SEVK_GSM FROM HBS_B2C_ENT_MASTER WHERE AKTARIM=0 AND ID= @id", con);
            cmd.Parameters.Add(new SqlParameter("@id", masterfrm.id));
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataReader sonuc = cmd.ExecuteReader();
            if (sonuc.Read())
            {
                siptuttxtbox.Text = sonuc["SIPARIS_TUT"].ToString();
                sipnotxtbox.Text = sonuc["SIP_NO"].ToString();
                siptarihtxtbox.Text = sonuc["SIP_TARIHI"].ToString();
                entyericmtxtbox.Text = sonuc["ENT_YERI"].ToString();
                unvantxtbox.Text = sonuc["UNVAN"].ToString();
                sirkettipicmbox.Text = sonuc["ENT_SIRKET_TIPI"].ToString();
                vdairetxtbox.Text = sonuc["ENT_VERGIDAIRESI"].ToString();
                vnotxtbox.Text = sonuc["ENT_VNO"].ToString();
                tcnotxtbox.Text = sonuc["ENT_TCKN"].ToString();
                emailtxtbox.Text = sonuc["ENT_EMAIL"].ToString();
                fatadrestxtbox.Text = sonuc["FAT_ADRES"].ToString();
                fatsehirtxtbox.Text = sonuc["FAT_SEHIR"].ToString();
                fatilcetxtbox.Text = sonuc["FAT_ILCE"].ToString();
                fatsemttxtbox.Text = sonuc["FAT_SEMT_MAHALLE"].ToString();
                fatgsmtxtbox.Text = sonuc["FAT_GSM"].ToString();
                sevkadrestxtbox.Text = sonuc["SEVK_ADRES"].ToString();
                sevksehirtxtbox.Text = sonuc["SEVK_SEHIR"].ToString();
                sevksemttxtbox.Text = sonuc["SEVK_SEMT_MAHALLE"].ToString();
                sevkgsmtxtbox.Text = sonuc["SEVK_GSM"].ToString();
            }
            con.Close();
        }




        private void kaydetbtn_Click_1(object sender, EventArgs e)
        {
            string sirkettipi = sirkettipicmbox.EditValue.ToString();
            if (unvantxtbox.Text != "")
            {
                if ((sirkettipi == "1" && tcnotxtbox.Text != "") || (sirkettipi == "2" && vnotxtbox.Text != "" && vdairetxtbox.Text != ""))
                {

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlCommand sqlcmd2;
                    sqlcmd2 = new SqlCommand("UPDATE HBS_B2C_ENT_MASTER SET UNVAN=@unvan,ENT_SIRKET_TIPI=@sirkettipi,ENT_VERGIDAIRESI=@vdairesi,ENT_VNO=@vno,ENT_TCKN=@tckn,ENT_EMAIL=@mail,FAT_ADRES=@fatadres,FAT_SEHIR=@fatsehir,FAT_ILCE=@fatilce,FAT_SEMT_MAHALLE=@fatsemt,FAT_GSM=@fatgsm,SEVK_ADRES=@sevkadres,SEVK_SEHIR=@sevksehir,SEVK_SEMT_MAHALLE=@sevksemt,SEVK_GSM=@sevkgsm where ID=@id ", con);
                    sqlcmd2.Parameters.AddWithValue("@unvan", unvantxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@id", masterfrm.id);
                    sqlcmd2.Parameters.AddWithValue("@sirkettipi", sirkettipi);
                    sqlcmd2.Parameters.AddWithValue("@vdairesi", vdairetxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@vno", vnotxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@tckn", tcnotxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@mail", emailtxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@fatadres", fatadrestxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@fatsehir", fatsehirtxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@fatilce", fatilcetxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@fatsemt", fatsemttxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@fatgsm", fatgsmtxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@sevkadres", sevkadrestxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@sevksehir", sevksehirtxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@sevksemt", sevksemttxtbox.Text);
                    sqlcmd2.Parameters.AddWithValue("@sevkgsm", sevkgsmtxtbox.Text);
                    try
                    {
                        sqlcmd2.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {

                        MessageBox.Show(ex.Message.ToString(), "Error Message");
                    }

                    DialogResult sonuc;
                    sonuc = MessageBox.Show("Değişiklikler Kaydedildi.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (sonuc == DialogResult.OK)
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("TC ve Vergi No Bilgilerini Kontrol Ediniz.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
          
        
             else
             {
                MessageBox.Show("Unvan Bilgisi Boş Kaydedilemez.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
     
         }

     
    }
    }

    

