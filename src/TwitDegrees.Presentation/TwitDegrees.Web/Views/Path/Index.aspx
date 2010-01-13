<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    $(function() {
        $("#searchBtn").click(function(e) {
            window.location.href = "/Path/Search/" + $('#startUser').val() + "/" + $('#destUser').val();
            e.preventDefault();
        });
    });    
</script>

    <h2>Index</h2>
    <fieldset>
        <legend>Search for a path from Start User to Destination User</legend>
        Start User: <%= Html.TextBox("startUser", "trasa") %><br />
        Destination User: <%= Html.TextBox("destUser") %> <br />
        <button id="searchBtn">Search</button>
    </fieldset>

</asp:Content>
