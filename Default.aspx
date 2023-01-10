<%@ Page Title="SPYRO" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Spyro_Web_App_v1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    
<table>
       <caption>Experiment Information</caption>

        <tr>
            <td>Logged In User</td>
            <td>Input File</td>
        </tr>
       
        <tr>
            <td style="width:30%"><asp:Label runat="server" class="form-control" id="lblUser"></asp:Label></td>
            <td rowspan="7">
                <asp:TextBox runat="server" class="form-control" id="txtTst" rows="20" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td>Experiment</td>
        </tr>

        <tr>
            <td><asp:Label runat="server" class="form-control" id="lblTitle"></asp:Label></td>
        </tr> 
        <tr class="labels">
            <td>CH4</td>
        </tr>
        <tr> 
           <td><asp:Label runat="server" class="form-control" id="lblCH4"></asp:Label></td>
        </tr>

        <tr>
            <td>T Adjust</td>
       </tr>  
        <tr>
            <td><asp:TextBox runat="server" class="form-control" id="txtTAdjust" rows="1" TextMode="Singleline"></asp:TextBox></td>
        </tr>
        
        <tr style="background-color:#fce9e9"><td colspan="3"></td><asp:Label ForeColor="Red" runat="server" class="form-control" id="lblmsg"></asp:Label></tr>
        <tr style="background-color:#ccb3b3">
            <td colspan="3">
                <asp:Button class="btn btn-primary btn-lg btn-block" runat="server" id="Button1" Text="RUN SPYRO" OnClick="ExecuteTAdjust_Click" />
            </td>
        </tr>  
</table>



</asp:Content>
