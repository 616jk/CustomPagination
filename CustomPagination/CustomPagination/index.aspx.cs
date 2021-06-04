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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDropDownList();
            LoadGridView(true);
        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            ClearSearchInput();
            LoadDropDownList();
            LoadGridView(true);
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
            Debug.WriteLine(string.Format("Query: {0}", GetMockData(offset)));
            Debug.WriteLine("---------------------------");
        }

        private int ConvertInt(string input)
        {            
            int.TryParse(input, out int output);
            return output;
        }

        private void ClearSearchInput()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }

        private SearchInput GetSearchInput()
        {
            return new SearchInput()
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text
            };
        }

        private string WhereClause()
        {
            SearchInput searchInput = GetSearchInput();

            string whereClause = "WHERE 1=1 ";

            if (!string.IsNullOrEmpty(searchInput.FirstName))
                whereClause += string.Format("AND first_name LIKE '%{0}%' ", searchInput.FirstName);

            if (!string.IsNullOrEmpty(searchInput.LastName))
                whereClause += string.Format("AND last_name LIKE '%{0}%' ", searchInput.LastName);

            if (!string.IsNullOrEmpty(searchInput.Email))
                whereClause += string.Format("AND email LIKE '%{0}%' ", searchInput.Email);

            return whereClause;
        }

        private string CountMockData()
        {
            return string.Format("SELECT COUNT(id) FROM MOCK_DATA {0}", 
                WhereClause());
        }

        private string GetMockData(int offset = 0)
        {
            return string.Format("SELECT * FROM MOCK_DATA {0} ORDER BY id OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", 
                WhereClause(), 
                offset, 
                NumberOfRecords);
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

        class SearchInput
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }
    }
}