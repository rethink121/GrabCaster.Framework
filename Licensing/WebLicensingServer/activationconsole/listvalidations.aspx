<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="listvalidations.aspx.cs" Inherits="LicService.ListValidations" %>
<%@ Register src="DashBoard.ascx" tagname="DashBoard" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>License Service Activation Console - List Serial Validations</title>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:DashBoard ID="DashBoard1" runat="server" />
    <div>
        <b>Select Settings File:</b> 
        <asp:DropDownList ID="cmbSettingFiles" runat="server" AutoPostBack="True" Width="231px">
        </asp:DropDownList>
        <br />
        <br />
        <b>Enter Serial Code:</b>
        <asp:TextBox ID="txtLicenseCode" runat="server" Width="757px"></asp:TextBox>
        <br />
        <asp:Button ID="btnGetValidationData" runat="server" 
            OnClick="btnGetValidationData_Click" Text="Get Validations" /><br />
        <br />
        <strong>Validation Records (Machine Codes):             <asp:CheckBox ID="chkUseHashedMachineCodes" runat="server" OnCheckedChanged="chkUseHashedMachineCodes_CheckedChanged"
                Text="Use Hashed Machine Codes" AutoPostBack="True" Checked="True" /></strong><br />
        <asp:ListBox ID="lstRecords" runat="server" Height="173px" Width="770px"></asp:ListBox>
        <br />
        <br />
        <asp:Label ID="lblMessage" runat="server"></asp:Label></div>
    </form>
</body>
</html>
