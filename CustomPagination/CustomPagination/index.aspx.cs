using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CustomPagination
{
    public partial class index : System.Web.UI.Page
    {
        private static readonly string MockDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MOCK_DBConnectionString"].ConnectionString;
        private static readonly int NumberOfRecords = 20; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDropDownList();
                LoadGridView(true);
            }
        }

        protected void ddlCustomPagination_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridView();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (ddlCustomPagination.SelectedValue != "0" && ddlCustomPagination.SelectedValue != "1")
                ddlCustomPagination.SelectedValue = (ConvertInt(ddlCustomPagination.SelectedValue) - 1).ToString();

            ddlCustomPagination_SelectedIndexChanged(sender, e);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ddlCustomPagination.SelectedValue != "0" && ddlCustomPagination.SelectedValue != ddlCustomPagination.Items.Count.ToString())
                ddlCustomPagination.SelectedValue = (ConvertInt(ddlCustomPagination.SelectedValue) + 1).ToString();
            
            ddlCustomPagination_SelectedIndexChanged(sender, e);
        }

        private void LoadDropDownList()
        {
            DataTable dtTotalRecords = ExecuteSqlCommand(MockDBConnectionString, CountMockData());

            int totalRecords = ConvertInt(dtTotalRecords.Rows[0][0].ToString());
            int totalPages = (int)Math.Ceiling((double)totalRecords / NumberOfRecords);

            lblTotalPages.Text = string.Format(" of {0} Pages", totalPages.ToString());

            if (totalRecords > 0)
            {
                ddlCustomPagination.Items.Clear();
                for (int i = 1; i <= totalPages; i++)
                {
                    ddlCustomPagination.Items.Add(i.ToString());
                }
            }
            else
            {
                ddlCustomPagination.Items.Add("0");
            }
        }

        private void LoadGridView(bool isInitial = false)
        {
            int offset = (ConvertInt(ddlCustomPagination.SelectedValue) - 1) * NumberOfRecords;
            
            if (isInitial || offset < 0)
                offset = 0;

            DataTable dtRecords = ExecuteSqlCommand(MockDBConnectionString, GetMockData(offset));
            gridViewMockData.DataSource = dtRecords;
            gridViewMockData.DataBind();

            Debug.WriteLine("---------------------------");
            Debug.WriteLine(string.Format("Page: {0}", ddlCustomPagination.SelectedValue));
            Debug.WriteLine(string.Format("Total Records Loaded: {0}", dtRecords.Rows.Count));
            Debug.WriteLine("---------------------------");
        }

        private int ConvertInt(string input)
        {            
            int.TryParse(input, out int output);
            return output;
        }

        private string CountMockData()
        {
            return "SELECT COUNT(id) FROM MOCK_DATA";
        }

        private string GetMockData(int offset = 0)
        {
            return string.Format("SELECT * FROM MOCK_DATA ORDER BY id OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, NumberOfRecords);
        }

        private DataTable ExecuteSqlCommand(string connectionString, string queryString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataTable datatable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(queryString, connection);
                adapter.Fill(datatable);
                return datatable;
            }
        }
    }
}