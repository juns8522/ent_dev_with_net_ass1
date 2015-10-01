<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BlueConsultingUI.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />

    <title>Blue Consulting</title>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <link rel="stylesheet" type="text/css" href="css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="css/style.css" />
</head>
<body class="login">
    <div class="container">
        <form id="form1" class="form-signin" runat="server">
            <div>
                <h2 class="form-signin-heading">Please sign in</h2>
                <asp:Login ID="Login1" runat="server"></asp:Login>
            </div>
        </form>
    </div>
</body>
</html>
