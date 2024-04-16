using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using System.Collections;
using DevExpress.XtraGrid;


namespace B2BServiceProject
{
    public partial class masterfrm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);
        DataTable dtmaster;
        DataTable dtcont;
        public static string id;
        public string regKey = "HBSERP_B2B";
        public string sipno;
        ArrayList siplist = new ArrayList();

        public masterfrm()
        {
            InitializeComponent();

        }




        public DataTable Yukle()
        {

            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select mc.cari_kodu,mc.cari_adi,fy.KODU,cm.* from CRM_OrderMaster cm left join m_Cari_kart mc on mc.kayit_id=cm.CARI_ID left join A_Form_Yetki_Tanimlari fy on fy.ID=mc.yetki_id  WHERE AKTARIM=0 order by SIP_TARIHI desc    ", con);
            DataTable table = new DataTable();
            adtr.Fill(table);
            return table;


        }
        public DataTable Yukle2()
        {

            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select mc.cari_kodu,mc.cari_adi,fy.KODU,cm.* from CRM_OrderMaster cm left join m_Cari_kart mc on mc.kayit_id=cm.CARI_ID left join A_Form_Yetki_Tanimlari fy on fy.ID=mc.yetki_id  WHERE AKTARIM=1 order by SIP_TARIHI desc    ", con);
            DataTable table = new DataTable();
            adtr.Fill(table);
            return table;


        }





        private void btnaktar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int sayac = 0;
            DialogResult sonuc;
            sonuc = MessageBox.Show("Sipariş Aktarılsın Mı?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                int[] selected = gridViewmaster.GetSelectedRows();

                foreach (int rowhandle in selected)

                {
                    //string sirkettip = (gridViewmaster.GetRowCellValue(rowhandle, "ENT_SIRKET_TIPI")).ToString();
                    sipno = (gridViewmaster.GetRowCellValue(rowhandle, "SIP_NO")).ToString();
                    //  if (gridViewmaster.GetRowCellValue(rowhandle, "UNVAN").ToString() != "" && gridViewmaster.GetRowCellValue(rowhandle, "ENT_SIRKET_TIPI").ToString() != "") //&& gridViewmaster.GetRowCellValue(rowhandle, "ENT_EMAIL").ToString() != ""
                    //   if (sirkettip == "1" || sirkettip == "2" && gridViewmaster.GetRowCellValue(rowhandle, "ENT_VNO").ToString() != "" && gridViewmaster.GetRowCellValue(rowhandle, "ENT_VERGIDAIRESI").ToString() != "")

                    {
                        sayac++;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        string id = (gridViewmaster.GetRowCellValue(rowhandle, "ID").ToString());
                        // SqlCommand komut = new SqlCommand("UPDATE HBS_B2C_ENT_DETAIL SET STOK_KODU=REPLACE(STOK_KODU, 'Ral_RAl_', '') where MASTER_ID=@id", con);
                        SqlCommand cmd = new SqlCommand("exec spB2BAddOrders @id", con);
                        SqlCommand cmd2 = new SqlCommand("update CRM_OrderMaster SET AKTARIM=1 WHERE ID= @id", con);
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd2.Parameters.Add(new SqlParameter("@id", id));
                        //komut.Parameters.Add(new SqlParameter("@id", id));
                        //komut.ExecuteNonQuery();
                        cmd.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        con.Close();
                    }


                    if (gridViewmaster.GetRowCellValue(rowhandle, "ADRES").ToString() != gridViewmaster.GetRowCellValue(rowhandle, "SEVKADRES").ToString())
                    {
                        MessageBox.Show(sipno + " " + "Sipariş Nolu Siparişin Sevk ve Fatura Adresleri Farklı! ", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        siplist.Add(sipno);
                    }
                }
            }
            if (sayac != 0)
            {
                MessageBox.Show("Sipariş Aktarımı Tamamlandı.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                gridmaster.DataSource = Yukle();
                aktarimgrid.DataSource = Yukle2();


            }
            else
            {

            }

        }



        private void gridmaster_Click_1(object sender, EventArgs e)
        {
            try
            {
                string id = (gridViewmaster.GetFocusedRowCellValue("ID")).ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd = new SqlCommand("Select * From CRM_OrderDetail WHERE MASTERID=@master_id", con);
                cmd.Parameters.AddWithValue("@master_id", id);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable(); 
                da.Fill(dt);
                griddetay.DataSource = dt;
            }
            catch (Exception)
            {


            }

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            string id = (gridViewaktarim.GetFocusedRowCellValue("ID")).ToString();
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand("Select * From CRM_OrderDetail WHERE MASTERID=@master_id", con);
            cmd.Parameters.AddWithValue("@master_id", id);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl2.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dtmaster = Yukle();
            gridmaster.DataSource = dtmaster;
            dtcont = Yukle2();
            aktarimgrid.DataSource = dtcont;
            gridViewmaster.RestoreLayoutFromRegistry(regKey);
            gridmaster.ForceInitialize();
            gridViewmaster.BestFitColumns();

         



        }




        private void excelaktarbtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string path = "";
            SaveFileDialog sv = new SaveFileDialog();

            sv.Filter = "Excel(.xls) | .xls | Excel(.xlsx) | *.xlsx | Pdf(.pdf) | *.pdf | Web(.html) | *.html | All Files(.) | *.";
            sv.ShowDialog();
            path = sv.FileName.ToString();
            //  MessageBox.Show(path);
            if (sv.FilterIndex == 1)
            {
                try
                {
                    gridViewmaster.ExportToXls(path);
                }
                catch (Exception)
                {


                }


            }
        }

        private void satirduzenlebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            id = (gridViewmaster.GetFocusedRowCellValue("ID")).ToString();
            detayfrm frm2 = new detayfrm();
            frm2.Show();
        }

        private void yenilebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            gridmaster.DataSource = Yukle();
            aktarimgrid.DataSource = Yukle2();
        }

        private void dizaynkytbtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            gridViewmaster.SaveLayoutToRegistry(regKey);

            MessageBox.Show("Dizayn Değişikliği Kaydedildi.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ondegerbtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(regKey);
            }
            catch (Exception)
            {

            }
            MessageBox.Show("Ön Dizayn Değerleri Kaydedildi.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aktarimisaret_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult sonuc;
            sonuc = MessageBox.Show("Sipariş Aktarıldı Olarak İşaretlensin Mi?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {

                int[] selected = gridViewmaster.GetSelectedRows();

                foreach (int rowhandle in selected)
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    string id = (gridViewmaster.GetRowCellValue(rowhandle, "ID").ToString());
                    SqlCommand cmd3 = new SqlCommand("update CRM_OrderMaster SET AKTARIM=1 WHERE ID= @id", con);
                    cmd3.Parameters.Add(new SqlParameter("@id", id));
                    cmd3.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("Sipariş Aktarıldı Olarak İşaretlendi.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            else
            {

            }



        }

        private void gridViewmaster_KeyDown(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn) != null && view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString() != String.Empty)
                    Clipboard.SetText(view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString());
                else
                    MessageBox.Show("Seçili alan boş!");
                e.Handled = true;
            }
        }

        private void gridViewaktarim_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {

                foreach (string item in siplist)
                {
                    if (gridViewaktarim.GetRowCellValue(e.RowHandle, gridViewaktarim.Columns["SIP_NO"]).ToString() == item)
                    {
                        e.Appearance.BackColor = Color.Yellow;
                    }
                }
            }
        }

        private void gridViewaktarim_KeyDown(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn) != null && view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString() != String.Empty)
                    Clipboard.SetText(view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString());
                else
                    MessageBox.Show("Seçili alan boş!");
                e.Handled = true;
            }
        }

        private void gridViewmaster_Click(object sender, EventArgs e)
        {
            griddetay.ForceInitialize();
            gridView3.BestFitColumns();
            

        }

        private void gridViewaktarim_Click(object sender, EventArgs e)
        {
            gridControl2.ForceInitialize();
            gridView6.BestFitColumns();
        }

        private void aktarimgrid_Load(object sender, EventArgs e)
        {
            aktarimgrid.ForceInitialize();
            gridViewaktarim.BestFitColumns();
            ribbonPageGroup1.Visible = false;
        }

        private void gridViewmaster_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            ribbonPageGroup1.Visible = true;
        }
    }
}