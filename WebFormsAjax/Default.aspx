<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsAjax._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Webforms Ajax Demo</h1>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Recent Earthquakes</h2>
            <p>
                The data below is pulled from the USGS API.
            </p>
            <br />
            <div id="divEarthquakes"></div>
        </div>
        <div class="col-md-4">
            <h2>Random Image</h2>
            <p>
                The image below is pulled from <a href='http://lorempixel.com/' target='_blank'>lorempixel.com</a>
            </p>
            <div style="cursor: pointer;">
                Size:&nbsp;<a onclick="loadImage(100);">100</a>&nbsp;|&nbsp;<a onclick="loadImage(200);">200</a>&nbsp;|&nbsp;<a onclick="loadImage(300);">300</a>&nbsp;|&nbsp;<a onclick="loadImage(400);">400</a>
            </div>
            <br />
            <div id="divImage"></div>
        </div>
    </div>
   
    <script type = "text/javascript">

        $(document).ready(function () {
            // Disable AJAX Caching
            $.ajaxSetup({ cache: false });

            loadEarthquakes();
            loadImage();
        });
        
        function loadEarthquakes() {

            $.ajax({
                type: "POST",
                url: "/Default.aspx/GetRecentEarthquakes",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#divEarthquakes").html(data.d);
                },
                error: function (data) {
                    $("#divEarthquakes").html("<p>Error loading data!</p>");
                },
                complete: function (data) {
                    // Always runs regardless of success or fail
                }
            });

        }

        function loadImage(size) {

            size = size || 100;

            $.ajax({
                type: "POST",
                url: "/Default.aspx/GetRandomImage",
                data: '{size: ' + size + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#divImage").html(data.d);
                },
                error: function (data) {
                    $("#divImage").html("<p>Error loading data!</p>");
                },
                complete: function (data) {
                    // Always runs regardless of success or fail
                }
            });

        }
        
    </script>

</asp:Content>
