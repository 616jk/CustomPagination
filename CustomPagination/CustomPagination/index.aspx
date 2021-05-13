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
