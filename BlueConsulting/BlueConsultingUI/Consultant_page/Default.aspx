<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BlueConsultingUI.Consultant_page.Default" %>

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
            Consultant&nbsp;&nbsp;
            <asp:Button ID="BtnLogout" runat="server" OnClick="BtnLogout_Click" Text="Logout" CausesValidation="False" CssClass="btn" />
            <br />
            <br />
            <asp:Label ID="LbReportName" runat="server" Text="Report Name"></asp:Label>
            &nbsp; &nbsp;
            <asp:TextBox ID="TbReportName" runat="server" Width="400px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reportNameValidator" runat="server" ControlToValidate="TbReportName" ErrorMessage="Report name required!" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>

            <asp:Button ID="BtnAddReport" runat="server" Text="Add Report" OnClick="BtnAddReport_Click" CssClass="btn" />

            <br />
            <br />
            <br />
        
            <asp:table runat="server" Width="510px">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="BtnAll" runat="server" Text="All Reports"  Width="170px" OnClick="BtnAll_Click" CausesValidation="False" CssClass="btn" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnApproved" runat="server" Text="Approved Reports" Width="170px" OnClick="BtnApproved_Click" CausesValidation="False" CssClass="btn btn-success" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnNotApproved" runat="server" Text="Not Approved Reports" Width="170px" OnClick="BtnNotApproved_Click" CausesValidation="False" CssClass="btn btn-danger" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:table>
            <br />
        
            <br />
            <asp:GridView ID="GridReports" runat="server" OnSelectedIndexChanged="GridReports_SelectedIndexChanged"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="View Expenses" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Button ID="BtnViewExpenses" runat="server" CausesValidation="False" CommandName="Select" Text="View" CssClass="btn" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id" HeaderText="Report ID" ReadOnly="True" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Report_name" HeaderText="Report Name" ReadOnly="True" />
                    <asp:BoundField DataField="Department" HeaderText="Department" ReadOnly="True" />
                    <asp:BoundField DataField="Supervisor_approval" HeaderText="Supervisor Approval" ReadOnly="True" />
                    <asp:BoundField DataField="Accounts_approval" HeaderText="Accounts Approval" ReadOnly="True" />
                    <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="True" />
                    <asp:BoundField DataField="Total_amount" HeaderText="Total Amount" ReadOnly="True" />

                </Columns>
            </asp:GridView>
            <br />
            <asp:GridView ID="GridExpenses" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridExpenses_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Expense ID" ReadOnly="true" />
                    <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="true" />
                    <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="True"/>
                    <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True"/>
                    <asp:BoundField DataField="Amount" HeaderText="Amount" ReadOnly="True"/>
                    <asp:BoundField DataField="Currency" HeaderText="Currency" ReadOnly="True"/>
                    <asp:BoundField DataField="Amount_aud" HeaderText="Amount(AUD)" ReadOnly="True"/>
                    <asp:BoundField DataField="Report_id" HeaderText="Report ID" ReadOnly="True"/>
                    <asp:TemplateField HeaderText="View Receipt" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Button ID="BtnViewReceipt" runat="server" CausesValidation="False" CommandName="Select" Text="View" CssClass="btn" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        </form>
    </div>
</body>
</html>
