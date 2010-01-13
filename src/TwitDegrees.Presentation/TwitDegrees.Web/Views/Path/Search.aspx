<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SearchModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Search</h2>

    <p>Searching from <%= Html.Encode(Model.StartUser) %> to <%= Html.Encode(Model.DestinationUser) %></p>
    <% 
        if (!Model.PathFound)
        {
            Response.Write("No Path Found.");
        } else {
            Response.Write(Html.UnorderedList<string>(Model.Path, p => p, "path", null, null));
        } %>
</asp:Content>
