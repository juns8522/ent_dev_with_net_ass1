<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BlueConsultingUI.Supervisor_page.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />

    <title>Blue Consulting - Supervisors</title>

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
                            <li><a href="../Consultant_page/Default.aspx">Consultant Access</a></li>
                            <li class="active"><a href="../Supervisor_page/Default.aspx">Supervisor Access</a></li>
                        </ul>
                    </div>
                </div>
            </div><!-- /.navbar -->

        </div>

        <form id="form1" runat="server">
        <div>
            Supervisor&nbsp;&nbsp;
            <asp:Button ID="BtnLogout" runat="server" OnClick="BtnLogout_Click" Text="Logout" CausesValidation="False" CssClass="btn" />
            <div class="break"></div>
            <asp:Table ID="TableBudget" runat="server" BorderStyle="Solid">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>
                        Department
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                        Total Expenses
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell>
                        Budget Left
                    </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:TextBox ID="TbDeptName" runat="server" ReadOnly="True"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="TbExpenses" runat="server" ReadOnly="True"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="TbBudgetLeft" runat="server" ReadOnly="True"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <br />
        
        
            <asp:table ID="TbButtons" runat="server" Width="510px">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="BtnAll" runat="server" Text="All Reports"  Width="170px" OnClick="BtnAll_Click" CssClass="btn" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnRejected" runat="server" Text="Rejected Reports" Width="170px" OnClick="BtnRejected_Click" CssClass="btn" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:table>
            <br />
            <br />
            <asp:GridView ID="GridReports" runat="server" OnSelectedIndexChanged="GridReports_SelectedIndexChanged" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="View Expenses" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Button ID="BtnViewExpenses" runat="server" CausesValidation="False" CommandName="Select" Text="View" CssClass="btn" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id" HeaderText="Report ID" ReadOnly="true" />
                    <asp:BoundField DataField="Report_name" HeaderText="Report Name" ReadOnly="true" />
                    <asp:BoundField DataField="Total_amount" HeaderText="Total Amount" ReadOnly="True"/>
                    <asp:BoundField DataField="Supervisor_Approval" HeaderText="Supervisor Approval" ReadOnly="True"/>
                    <asp:BoundField DataField="Accounts_Approval" HeaderText="Accounts Approval" ReadOnly="True"/>
                    <asp:BoundField DataField="Department" HeaderText="Department" ReadOnly="True"/>
                    <asp:BoundField DataField="UserName" HeaderText="Consultant Username" ReadOnly="True"/>
                    <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="True"/>
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
            <div class="break"></div>
            <asp:table ID="Table2Buttons" runat="server" Width="340px" Visible="False">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="BtnApprove" runat="server" Text="Approve Report" Width="170px" OnClick="BtnApprove_Click" CssClass="btn"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnReject" runat="server" Text="Reject Report" Width="170px" OnClick="BtnReject_Click" CssClass="btn"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:table>
            <asp:Label ID="LbWarning" runat="server" Font-Bold="True" ForeColor="Red" Text="Approving this report will result in the department going over budget." Visible="False"></asp:Label>
        </div>
        </form>
    </div>
</body>
</html>
