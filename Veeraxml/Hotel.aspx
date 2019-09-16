<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Hotel.aspx.cs" Inherits="Veeraxml.Hotel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<!-- Titlebar
================================================== -->
<div id="titlebar" class="gradient">
    <div class="container">
        <div class="row">
            <div class="col-md-12">

                <h2>Hotel Search</h2><span>Best Rates Ever</span>

                <!-- Breadcrumbs -->
                <nav id="breadcrumbs">
                    <ul>
                        <li><a href="#">Home</a></li>
                        <li>Listings</li>
                    </ul>
                </nav>

            </div>
        </div>
    </div>
</div>


<!-- Content
================================================== -->
<div class="container">
<div class="row">
		
<!-- Search -->
<div class="col-md-12">
    <div class="main-search-input gray-style margin-top-0 margin-bottom-10">

      

        <div class="main-search-input-item location">
            <div id="autocomplete-container">
                <input id="autocomplete-input" type="text" placeholder="Location">
            </div>
            <a href="#"><i class="fa fa-map-marker"></i></a>
        </div>

    
        
        <div class="main-search-input-item">
        <!-- Date Range -->
        <div id="booking-date-range" style="background-color: transparent;">
            <span></span>
        </div>
        </div>
        
        

        <button class="button" id="SearchHotel">Search</button>
    </div>
</div>
    
    
<!-- Room 1 -->
<div id="room1">
<div class="col-md-12">
    
    <div class="main-search-input gray-style margin-top-0 margin-bottom-10">
       
        <div class="main-search-input-item">
        Adults    
        </div> 
        <div class="main-search-input-item"> 
            <select data-placeholder="Adults" class="chosen-select" name="Adult" >
                <option>1</option>	
                <option>2</option>
                <option>3</option>
                <option>4</option>
                <option>5</option>
                <option>6</option>
            </select>
        </div>
        
        <%--        child 1 --%>
        <div class="main-search-input-item">
            Child #1
        </div> 
        <div class="main-search-input-item">
            <select data-placeholder="Child 1" class="chosen-select" name="child1" >
                <option>0</option>	
                <option>1</option>
                <option>2</option>
                <option>3</option>
            </select>
        </div>
        
        
        <%--        child 2 --%>
        <div class="main-search-input-item">
            Child 2
        </div> 
        <div class="main-search-input-item">
            <select data-placeholder="Children" class="chosen-select" name="Child2" >
                <option>0</option>	
                <option>1</option>
                <option>2</option>
                <option>3</option>
            </select>
        </div>
        
        <%--        child 3--%>
        <div class="main-search-input-item">
            Child #3
        </div> 
        <div class="main-search-input-item">
            <select data-placeholder="Children" class="chosen-select" name="Child3" >
                <option>0</option>	
                <option>1</option>
                <option>2</option>
                <option>3</option>
            </select>
        </div>
        
       
        

        
        
        <div class="main-search-input-item">
            <button class="button" id="Addroom"><i class="sl sl-icon-plus"></i></button>
        </div>

        <div class="main-search-input-item">
            <button class="button" id="Delroom"><i class="fa fa-minus"></i></button>
        </div>
        
        

    </div>
    </div>
    
    </div>
    
    
    
    


<!-- Search Params -->
 
<input type="hidden" id="SessionId" name="SessionId" value="" runat="server" />   
<input type="hidden" id="City" name="City" value="">
<input type="hidden" id="checkin" name="checkin" value="">
<input type="hidden" id="checkout" name="checkout" value="1">

<input type="hidden" id="RoomCount" name="RoomCount" value="1">

<input type="hidden" id="room1code" name="room1code" value="0">
<input type="hidden" id="room2code" name="room2code" value="0">
<input type="hidden" id="room3code" name="room3code" value="0">
<input type="hidden" id="room4code" name="room4code" value="0">
<input type="hidden" id="room5code" name="room5code" value="0">
    

<input type="hidden" id="Paging" name="Paging" value="10">
    

    
<!-- Search Params -->
   

<!-- Room Zero To Copy -->
<div class="col-md-12" id="room" style="display: none;">
    <div class="main-search-input gray-style margin-top-0 margin-bottom-10">
       
        <div class="main-search-input-item">
            Adults    
        </div> 
        <div class="main-search-input-item"> 
            <select data-placeholder="Adults" class="" name="Adult" >
                <option>1</option>	
                <option>2</option>
                <option>3</option>
                <option>4</option>
                <option>5</option>
                <option>6</option>
            </select>
        </div>
        
        
        <%--        child 1 --%>
        <div class="main-search-input-item">
            Child 1
        </div> 
        <div class="main-search-input-item">
            <select data-placeholder="Children" class="" name="Child1" >
                <option>0</option>	
                <option>1</option>
                <option>2</option>
                <option>3</option>
            </select>
        </div>
        
        <%--        child 2 --%>
        <div class="main-search-input-item">
            Child 2
        </div> 
        <div class="main-search-input-item">
            <select data-placeholder="Children" class="" name="Child2" >
                <option>0</option>	
                <option>1</option>
                <option>2</option>
                <option>3</option>
            </select>
        </div>
        
        <%--        child 3--%>
        <div class="main-search-input-item">
            Child #3
        </div> 
        <div class="main-search-input-item">
            <select data-placeholder="Children" class="" name="Child3" >
                <option>0</option>	
                <option>1</option>
                <option>2</option>
                <option>3</option>
            </select>
        </div>
        

        
    </div>
</div>
<!-- Room Zero To Copy -->

    
    

<div id="room2">
</div>    

<!--/ Room 3 /-->
<div id="room3">
</div>    
<!--/ Room 4 /-->
<div id="room4">
</div>    
    
<!--/ Room 5 /-->
<div id="room5">
</div>    

    
  
<!-- Search Section / End -->


<div class="col-md-12">

<!-- Sorting - Filtering Section -->
<div class="row margin-bottom-25 margin-top-30">

    <div class="col-md-6">
        <!-- Layout Switcher -->
        <div class="layout-switcher">
            <a href="#" class="grid"><i class="fa fa-th"></i></a>
            <a href="#" class="list active"><i class="fa fa-align-justify"></i></a>
        </div>
    </div>

    <div class="col-md-6">
        <div class="fullwidth-filters">
						

            <!-- Panel Dropdown-->
            <div class="panel-dropdown float-right">
                <a href="#">Hotel Name</a>
                <div class="panel-dropdown-content">
                    <div class="search-blog-input">
                        <div class="input"><input class="search-field" type="text" placeholder="Hotel Name " value="" id="Filter_HotelName"></div>
                    </div>
                    <div class="panel-buttons">
                        <button class="panel-cancel" id="Filter_HotelName_Clear" >Clear</button>
                        <button class="panel-apply" id="Filter_HotelName_Apply" >Apply</button>
                    </div>
                </div>
            </div>
            <!-- Panel Dropdown / End -->

        
        </div>
    </div>

</div>
<!-- Sorting - Filtering Section / End -->

<div id="loadData">
    
    <div id='loadingDiv' style="display:none;position: fixed;height: 5em; width: 5em;overflow:show;margin: auto;top: 0;left: 0;bottom: 0;right: 0;">
     <img src="images/loader1.gif" style="height: 75px; width: 75px;" /> 
    </div> 
   
</div>
    
    <div id="NewLoad">
    
        <div id='NewLoader' style="display:none;position: fixed;height: 5em; width: 5em;overflow:show;margin: auto;top: 0;left: 0;bottom: 0;right: 0;">
            <img src="images/loader1.gif" style="height: 75px; width: 75px;" /> 

        </div> 
    

    </div>

    


    
    <!-- Pagination -->
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-md-12">
            <!-- Pagination -->
            <div class="pagination-container margin-top-20 margin-bottom-40">
                
                <button class="button" id="LoadMore">Load More</button>

            </div>
        </div>
    </div>
    <!-- Pagination / End -->
        
        
    
    

</div>

</div>
</div>
    
  
    

    


</asp:Content>


<asp:Content ID="Scripts" runat="server" ContentPlaceHolderID="jQScript">
    
    
    

    <script>


        //Search Loader Waiting 

        $(document).ajaxStart(function () {
            $("#loadingDiv").css("display", "block");
            $("#NewLoader").css("display", "block");

            //$("#SearchHotel").prop('disabled', true);

        });
        $(document).ajaxComplete(function () {
            $("#loadingDiv").css("display", "none");
            $("#NewLoader").css("display", "none");

            //$("#SearchHotel").prop('disabled', false);

        });


        //Multiple Rooms Function

        setInterval("GetRoom(1);", 500);
        setInterval("GetRoom(2);", 500);
        setInterval("GetRoom(3);", 500);
        setInterval("GetRoom(4);", 500);
        setInterval("GetRoom(5);", 500);

        //Get Each Room to Its Contatiner 

        function GetRoom(xdiv) {
            //Get Room Data Logic
            var valu = $('#room' + xdiv).find('select[name="Adult"]').val();
            var valuChld1 = $('#room' + xdiv).find('select[name="child1"]').val();
            var valuChld2 = $('#room' + xdiv).find('select[name="child2"]').val();
            var valuChld3 = $('#room' + xdiv).find('select[name="child3"]').val();

            $("#room" + xdiv + "code").val("[" + valu + "," + valuChld1 + "," + valuChld2 + "," + valuChld3 + "]");
        }

       

        // Add Room Logic
        $("#Addroom").click(function () {
            var x = parseInt($("#RoomCount").val()) + 1;

            $("#RoomCount").val(x) ; 

            $('#' + 'room').clone().prependTo('#' +  'room' + (x));
            $('#' + 'room' + (x)).children().show();

        });

        // Delete Room Logic
        $("#Delroom").click(function () {

            var y = parseInt(($("#RoomCount").val()));


            if  (y === 1) {
                return;
            }


            var x = parseInt($("#RoomCount").val()) - 1;
            $("#RoomCount").val(x);
            //Clear Associated Room Code Hidden Input
            $("#room" + x + "code").val("0");
            //Remove All Child But Keep Div
            $('#' + 'room' + x).empty();
 
        });






        //Do Initial Search Logic
        $("#SearchHotel").click(function () {

            var cityname = $("#City").val();
            var checkin = $("#checkin").val();
            var checkout = $("#checkout").val();
            var room1 = $("#room1code").val();
            var room2 = $("#room2code").val();
            var room3 = $("#room3code").val();
            var room4 = $("#room4code").val();
            var room5 = $("#room5code").val();






            $('#loadData').load('HotelList.aspx?cityname=' + cityname + '&checkin=' + checkin + '&checkout=' + checkout + '&room1=' + room1
                + '&room2=' + room2 + '&room3=' + room3 + '&room4=' + room4 + '&room5=' + room5);

        });



        //Load More

        $("#LoadMore").click(function () {
            alert("Hello there from Click");

            var y = parseInt(($("#Paging").val())) + 10;
            var x = $("#SessionId").val();
            $("#Paging").val(y); 



            $('#NewLoad').load('HotelList-Loader.aspx?Load=' + y + '&sessionid=' + x );
        });


        $("#Filter_HotelName_Apply").click(function () {

            var hotelName = $("#Filter_HotelName").val();

            $('#loadData').load('HotelList.aspx?H=' + hotelName  );

        });


    


    </script>

</asp:Content>

