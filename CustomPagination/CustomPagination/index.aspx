<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CustomPagination.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">        
        <div style="padding-left:20px;">
            <h2>GridView - Custom Pagination</h2>
            <!-- Textbox -->
            <asp:Table ID="tblSearchInput" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table><br />           
            
            <!-- Search & Clear Button -->
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
            <asp:Button ID="btnViewAll" runat="server" Text="View All" OnClick="btnViewAll_Click" /><br /><br />
            
            <!-- Grid View -->
            <asp:GridView ID="gridViewMockData" runat="server" CellPadding="5" EmptyDataText="No Data"></asp:GridView><br />
            
            <!-- Custom Pagination -->
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" OnClick="btnPrevious_Click" Width="70px"/>
            <asp:DropDownList 
                ID="ddlCustomPagination" 
                runat="server" 
                AutoPostBack="True" 
                OnSelectedIndexChanged="ddlCustomPagination_SelectedIndexChanged" 
                Width="70px">
            </asp:DropDownList>
            <asp:Label ID="lblTotalPages" runat="server"></asp:Label>
            <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" Width="70px"/>
        </div>
    </form>
</body>
</html>
