using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CustomPagination
{
    public partial class index : System.Web.UI.Page
    {
        private static readonly string MockDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MOCK_DBConnectionString"].ConnectionString;
        private static readonly int NumberOfRecords = 20; //default set to 20 rows of record

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
            LoadGridView(); //simply go to LoadGridView again, it will base on the dropdownlist selection to pull the data
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (ddlCustomPagination.SelectedValue != "0" && ddlCustomPagination.SelectedValue != "1")
                ddlCustomPagination.SelectedValue = (ConvertInt(ddlCustomPagination.SelectedValue) - 1).ToString(); //minus 1 => go to previous page

            ddlCustomPagination_SelectedIndexChanged(sender, e); //trigger the ddlCustomPagination_SelectedIndexChanged event
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ddlCustomPagination.SelectedValue != "0" && ddlCustomPagination.SelectedValue != ddlCustomPagination.Items.Count.ToString())
                ddlCustomPagination.SelectedValue = (ConvertInt(ddlCustomPagination.SelectedValue) + 1).ToString(); //plus 1 => go to next page
            
            ddlCustomPagination_SelectedIndexChanged(sender, e); //trigger the ddlCustomPagination_SelectedIndexChanged event
        }

        private void LoadDropDownList()
        {
            DataTable dtTotalRecords = ExecuteSqlCommand(MockDBConnectionString, CountMockData()); //get the data

            int totalRecords = ConvertInt(dtTotalRecords.Rows[0][0].ToString()); //read the returned result
            int totalPages = (int)Math.Ceiling((double)totalRecords / NumberOfRecords); //calculate total how many pages, based on the total records

            lblTotalPages.Text = string.Format(" of {0} Pages", totalPages.ToString()); //set the label

            if (totalRecords > 0)
            {
                ddlCustomPagination.Items.Clear();
                for (int i = 1; i <= totalPages; i++)
                {
                    ddlCustomPagination.Items.Add(i.ToString()); //add the page selection
                }
            }
            else
            {
                ddlCustomPagination.Items.Add("0");
            }
        }

        private void LoadGridView(bool isInitial = false)
        {
            int offset = (ConvertInt(ddlCustomPagination.SelectedValue) - 1) * NumberOfRecords; //how many record need to offset
            
            if (isInitial || offset < 0)
                offset = 0;

            DataTable dtRecords = ExecuteSqlCommand(MockDBConnectionString, GetMockData(offset)); //get the data
            gridViewMockData.DataSource = dtRecords; //set the returned result as gridview data source
            gridViewMockData.DataBind();

            //for debug purpose, from here we can see how many records is pull from database
            Debug.WriteLine("---------------------------");
            Debug.WriteLine(string.Format("Page: {0}", ddlCustomPagination.SelectedValue));
            Debug.WriteLine(string.Format("Total Records Loaded: {0}", dtRecords.Rows.Count));
            Debug.WriteLine(string.Format("Query: {0}", GetMockData(offset)));
            Debug.WriteLine("---------------------------");
        }

        private int ConvertInt(string input)
        {            
            int.TryParse(input, out int output);
            return output;
        }

        private string CountMockData()
        {
            return "SELECT COUNT(id) FROM MOCK_DATA"; //query to count total how many records
        }

        private string GetMockData(int offset = 0)
        {
            return string.Format("SELECT * FROM MOCK_DATA ORDER BY id OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, NumberOfRecords); //query to select the data and using offset & fetch next
        }

        private DataTable ExecuteSqlCommand(string connectionString, string queryString)
        {
            //sql command to get the data and return in datatable
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