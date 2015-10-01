<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddExpenses.aspx.cs" Inherits="BlueConsultingUI.Consultant_page.AddExpenses" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />

    <title>Blue Consulting - Consultants</title>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <link rel="stylesheet" type="text/css" href="../css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
</head>
<body class="consultants">
    <div class="container">
        <div class="masthead">
            <h3 class="muted">Blue Consulting</h3>

            <div class="navbar">
                <div class="navbar-inner">
                    <div class="container">
                        <ul class="nav">
                            <li><a href="../Default.aspx">Home</a></li>
                            <li><a href="../AccountStaff_page/Default.aspx">Accountant Access</a></li>
                            <li class="active"><a href="../Consultant_page/Default.aspx">Consultant Access</a></li>
                            <li><a href="../Supervisor_page/Default.aspx">Supervisor Access</a></li>
                        </ul>
                    </div>
                </div>
            </div><!-- /.navbar -->

        </div>

        <form id="form1" runat="server">
        <div>
            <h3>Add Expenses</h3>   
            Location
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TbLocation" runat="server" Width="400px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="LocationValidator" runat="server" ControlToValidate="TbLocation" ErrorMessage="Location required!" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <br />
            <br />
            Description
            <asp:TextBox ID="TbDescription" runat="server" Width="400px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="DescriptionValidator1" runat="server" ControlToValidate="TbDescription" ErrorMessage="Description required!" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <br />
            <asp:Calendar ID="CalendarConsultant" runat="server" Width="526px" ShowNextPrevMonth="False" OnSelectionChanged="CalendarConsultant_SelectionChanged"></asp:Calendar>
            <br />
            Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TbDate" runat="server" ReadOnly="true"></asp:TextBox>
            <asp:RequiredFieldValidator ID="DateValidator1" runat="server" ControlToValidate="TbDate" ErrorMessage="Date required!" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <br />
            <br />
            Amount
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TbAmount" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="DDLCurrency" runat="server">
                <asp:ListItem>AUD</asp:ListItem>
                <asp:ListItem>CNY</asp:ListItem>
                <asp:ListItem>Euros</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="AmountValidator1" runat="server" ControlToValidate="TbAmount" ErrorMessage="Amount required!" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
        
            <br />
            <br />
            Receipt&nbsp;&nbsp;
            <asp:FileUpload ID="FuPdf" runat="server" />
            <asp:RequiredFieldValidator ID="FuPdfValidator1" runat="server" ControlToValidate="FuPdf" ErrorMessage="Please select a file" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="PdfValidator" runat="server" ErrorMessage="Not a pdf!" ControlToValidate="FuPdf" ForeColor="Red" OnServerValidate="PdfValidator_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
            <br />
            <br />
            <br />
            <asp:Button ID="BtnAddMoreExpenses" runat="server" Text="Save Expense" OnClick="BtnAddMoreExpenses_Click" CssClass="btn" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="BtnSubmitReport" runat="server" Text="Submit Report" OnClick="BtnSubmitReport_Click" CausesValidation="False" CssClass="btn" />
            <br />
            <asp:Label ID="LbWarning" runat="server" Font-Bold="False" ForeColor="Red"></asp:Label>
        </div>
        </form>
    </div>
</body>
</html>
