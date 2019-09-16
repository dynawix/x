<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scrap.aspx.cs" Inherits="Veeraxml.Scrap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox runat="server" ID="XHTML" TextMode="MultiLine"></asp:TextBox>
            <asp:Button runat="server" ID="Scrapit" OnClick="Scrap_OnClick"/>
        </div>
    </form>
</body>
</html>
