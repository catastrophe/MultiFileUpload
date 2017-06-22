<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="MultiFileUpload.FileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblFile" Text="Select File:" runat="server"></asp:Label>
            <asp:FileUpload ID="fupload" runat="server" />

            <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="btnAdd_Click" />

            <br />
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <asp:Label ID="Label2" runat="server"></asp:Label>

            <br />
            <asp:GridView ID="grdFiles" runat="server" AutoGenerateColumns="false" OnRowDataBound="grdFiles_RowDataBound" OnRowDeleting="grdFiles_RowDeleting">
                <Columns>
                    <asp:BoundField HeaderText="Sl.No." DataField="ID" />
                    <asp:BoundField HeaderText="File Name" DataField="Name" />
                    <asp:BoundField HeaderText="Path" DataField="Path"/>

                    
                    
                    
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Link" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="btnUpload_Click" />
        <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="ID" DataField="id" />
                <asp:BoundField HeaderText="Name" DataField="Name" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton id="lnkDownlaod" runat="server" Text="Download" OnClick="lnkDownlaod_Click"
                             CommandArgument='<%#Eval("id") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
