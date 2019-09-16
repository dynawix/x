<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rooms-List.aspx.cs" Inherits="Veeraxml.Rooms_List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       
        <div id="listing-pricing-list" class="listing-section">
            <h3 class="listing-desc-headline margin-top-70 margin-bottom-30">Pricing</h3>

                <div class="pricing-list-container">
						
                    <!-- Food List -->
                    <h4>Rooms</h4>
                    <ul>
                     
                        <asp:ListView runat="server" id="RoomsList">
                            <ItemTemplate>
                                <li>
                                    <h5><%#Eval("Room_Name") %> / <%#Eval("Supplier") %> </h5>
                                    <p><%#Eval("Meal_Name") %></p>
                                    <span><%#Eval("Room_BasePrice") %>  <%#Eval("Room_BasePrice_Currency") %></span>
                                    <p><a href="Book.aspx" class="button medium"><i class="sl sl-icon-layers"></i> Book Now</a></p>
                                </li>        
                            </ItemTemplate>
                        </asp:ListView>
                       
                      
                     
                    </ul>

                    <!-- Food List -->
             

                </div>
           
        </div>


    </form>
</body>
</html>
