using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MultiFileUpload
{
    public partial class FileUpload : System.Web.UI.Page
    {
        string CS = @"Data Source=.\SQLEXPRESS;Initial Catalog=DbFiles;Integrated Security=true";
        int counter = 0;
        DataTable dt=new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack != true)
            {
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(Int16));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Path", typeof(string));

                Session["myTable"] = dt;
            }

        }
              

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            foreach(GridViewRow row in grdFiles.Rows)
            {
                string filename = row.Cells[1].Text;
                string filepath = row.Cells[2].Text;
                Stream strm = fupload.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(strm);
                Byte[] filesize = GetFileByte(filepath);
                string filetype = Path.GetExtension(filename);
                switch (filetype)
                {
                    case ".doc":
                        filetype = "application/word";
                        break;
                    case ".docx":
                        filetype = "application/word";
                        break;
                    case ".pdf":
                        filetype = "application/pdf";
                        break;
                    case ".xls":
                        filetype = "application/xls";
                        break;
                    case ".xlsx":
                        filetype = "application/xls";
                        break;
                    case ".txt":
                        filetype = "application/text";
                        break;
                    case ".msg":
                        filetype = "application/message";
                        break;
                    case ".jpg":
                        filetype = "image/jpg";
                        break;
                    case ".PNG":
                        filetype = "image/PNG";
                        break;
                }
                UploadFile(filename, filetype, filesize);
                DeleteFile(filepath);
                Label1.ForeColor = System.Drawing.Color.Green;
                
            }

            Label2.Text = "successfully uploaded..!!";
            grdFiles.Visible = false;
            BindGridview();
            
        }
               

        public static Byte[] GetFileByte(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] fileBytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();
            return fileBytes;
        }

        public void UploadFile(string filename, string filetype, byte[] filesize)
        {
            
            
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "insert into [tblFiles](Data,ContentType,Name) values(@Data,@ContentType,@Name)";
            cmd.Parameters.AddWithValue("@Name", filename);
            cmd.Parameters.AddWithValue("@ContentType", filetype);
            cmd.Parameters.AddWithValue("@Data", filesize);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["myTable"];

            if (fupload.HasFile)
            {
                string filename = Path.GetFileName(fupload.PostedFile.FileName);
                string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), "Sid_" + filename);
                
                //string filepath = fupload.PostedFile.FileName;

                fupload.SaveAs(path);
               
                if(dt.Rows.Count==0)
                { 
                    counter = 1;
                }
                else
                {
                    counter = dt.Rows.Count+1;
                }

                dt.Rows.Add(counter, filename, path);

                grdFiles.DataSource = dt;
                grdFiles.DataBind();


            }
        }

        protected void grdFiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    e.Row.Cells[2].Visible = false;
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[2].Visible = false;
                string item = e.Row.Cells[0].Text;
                foreach (LinkButton link in e.Row.Cells[3].Controls.OfType<LinkButton>())
                {
                    if (link.CommandName == "Delete")
                    {
                        link.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                    }
                }
            }
        }

        protected void grdFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = (DataTable)Session["myTable"] as DataTable;
            string path = dt.Rows[index]["Path"].ToString();
            dt.Rows[index].Delete();
            DeleteFile(path);
            ViewState["dt"] = dt;
            grdFiles.DataSource = dt;
            grdFiles.DataBind();
        }

        private void BindGridview()
        {

            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select id,Name from tblfiles";
                    cmd.Connection = con;
                    con.Open();
                    GridView1.DataSource = cmd.ExecuteReader();
                    GridView1.DataBind();
                    con.Close();
                }
            }
        }

        protected void lnkDownlaod_Click(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName;
            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select * from tblfiles where id=@Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["data"];
                        fileName = sdr["Name"].ToString();
                    }
                    con.Close();
                }
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        public static bool DeleteFile(string path)
        {
            bool isFileExist = false;
            try
            {
                File.Delete(path);
                isFileExist = true;
            }
            catch (IOException objEx)
            {
                //Log for File delete error
            }
            return isFileExist;
        }

    }
}