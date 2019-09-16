<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotelList-Loader.aspx.cs" Inherits="Veeraxml.HotelList_Loader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- cdnjs -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.lazy/1.7.9/jquery.lazy.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.lazy/1.7.9/jquery.lazy.plugins.min.js"></script>
                
</head>
<body>
<form id="form1" runat="server">
       
    <div class="row">
        <span> <asp:Literal runat="server" id="Hotel_Count"></asp:Literal>  Hotels Found   </span>
        <asp:ListView ID="lvHotelsList" runat="server" OnItemDataBound="lvHotelsList_ItemDataBound" >
            <ItemTemplate>
                <!-- Listing Item / start -->

       
           

                <div class="col-lg-12 col-md-12">
                    <div class="listing-item-container list-layout">
                        <a href="Rooms.aspx?H=<%# Eval("Hotel_Name") %>&C=<%# Eval("Hotel_City_Name") %>" class="listing-item">
							
                            <!-- Image -->
                            <div class="listing-item-image">
                            
                         
                                <asp:Image runat="server" CssClass="lazy" ID="htlImg"/>                         

                                <span class="tag">MR</span>
                            </div>
							
                            <!-- Content -->
                            <div class="listing-item-content">
                                <div class="listing-badge now-open">Now Open</div>

                                <div class="listing-item-inner">
                                    <h3><%# Eval("Hotel_Name") %> <i class="verified-icon"></i></h3>
                                    <span><%# Eval("Hotel_Address") %></span>
                                
                              
                                

                                    <asp:Repeater runat="server" ID="Hotel_Category"></asp:Repeater>


                               


                                
                                    <h3><%#Eval("Room_Name") %></h3>
                                    <h4><%#Eval("Meal_Name") %></h4>
                                    <h5><%#Eval("Room_BasePrice") %> <%#Eval("Room_BasePrice_Currency") %></h5>


                                </div>

                                <span class="like-icon"></span>
                            </div>
                        </a>
                    </div>
                </div>
         
                <!-- Listing Item / End -->
    
            </ItemTemplate>
        </asp:ListView>


    </div>
    

     
        
        
        
        
        


    <script type="text/javascript" src="scripts/jquery-2.2.0.min.js"></script>

    
        
    <script type="text/javascript" src="scripts/custom.js"></script>


    <script>



         

          
         
            


    </script>
        
    
    
</form>
    
    


</body>
</html>

