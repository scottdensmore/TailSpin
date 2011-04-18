<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TailSpin.Web.Survey.Shared.Models.SurveyAnswer>" %>
<div style="font-size: 16px; padding-bottom:12px;">
    Survey Location:
    <% if (!string.IsNullOrEmpty(this.Model.StartLocation) || !string.IsNullOrEmpty(this.Model.CompleteLocation)) { %>
    
    <div id='map' style="position:relative; width:400px; height:300px;"></div>
    
    <script type="text/javascript" src="https://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2&s=1"></script>
    <script type="text/javascript">
        if (typeof VEMap == 'undefined') { 
            this.document.getElementById('map').style.height = 0;
            this.document.write('Start location: <%:this.Model.StartLocation%> // Completed location: <%:this.Model.CompleteLocation%>');
        } else {
            var centerLocation = null;
            var startLocation = null;
            var completedLocation = null;
            <% if (!string.IsNullOrEmpty(this.Model.StartLocation)) { %>
            var startLocation = new VELatLong(<%: this.Model.StartLocation.Split(',')[0] %>, <%: this.Model.StartLocation.Split(',')[1] %>);
            var centerLocation = startLocation;
            <% } %>

            <% if (!string.IsNullOrEmpty(this.Model.CompleteLocation)) { %>
            var completedLocation = new VELatLong(<%: this.Model.CompleteLocation.Split(',')[0] %>, <%: this.Model.CompleteLocation.Split(',')[1] %>);
            var centerLocation = completedLocation;
            <% } %>

            var map = null;
            map = new VEMap('map');
            map.SetDashboardSize(VEDashboardSize.Tiny);
            map.LoadMap();
            map.SetCenterAndZoom(centerLocation, 12);

            if (startLocation != null)
            {
                var startPushpin = new VEShape(VEShapeType.Pushpin, startLocation);
                startPushpin.SetTitle('Start Location');
                startPushpin.SetDescription('Location where the survey was started.');
                map.AddShape(startPushpin);
            }
        
            if (completedLocation != null)
            {
                var completedPushpin = new VEShape(VEShapeType.Pushpin, completedLocation);
                completedPushpin.SetTitle('Completed Location');
                completedPushpin.SetDescription('Location where the survey was completed.');
                map.AddShape(completedPushpin);
            }
        }
    </script>

    <% } else { %>
    <span style="font-style: italic">Unknown.</span>
    <% } %>
</div>
