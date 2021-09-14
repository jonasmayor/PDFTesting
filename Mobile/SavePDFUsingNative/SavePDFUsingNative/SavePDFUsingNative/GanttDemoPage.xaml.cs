using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SavePDFUsingNative
{
    public partial class GanttDemoPage : ContentPage
    {
        public GanttDemoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var source = new HtmlWebViewSource();
            source.Html = "<html><head>" +
            "<script src=\"https://cdn.syncfusion.com/ej2/dist/ej2.min.js\" type=\"text/javascript\"></script>" +
            "<link href=\"https://cdn.syncfusion.com/ej2/material.css\" rel=\"stylesheet\"><link href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css\" rel=\"stylesheet\">"  +        
            "<style>body{touch-action:none;}</style>" +

            "<script src=\"https://ej2.syncfusion.com/javascript/demos/gantt/default/datasource.js\" type=\"text/javascript\"></script>" +
            "<script> function script() {ej.base.enableRipple(!0);var ganttChart=new ej.gantt.Gantt({dataSource:window.projectNewData,height:\"450px\",allowSelection:!0,highlightWeekends:!0,taskFields:{id:\"TaskID\",name:\"TaskName\",startDate:\"StartDate\",endDate:\"EndDate\",duration:\"Duration\",progress:\"Progress\",dependency:\"Predecessor\",child:\"subtasks\"},labelSettings:{leftLabel:\"TaskName\"},projectStartDate:new Date(\"03/24/2019\"),projectEndDate:new Date(\"07/06/2019\")});ganttChart.appendTo(\"#Default\"); }</script>" +
            "</head><body onload=\"script();\"><div class=\"stackblitz-container material\"><div class=\"control-section\"> <div class=\"content-wrapper\"><div id=\"Default\"></div></div></div></div></body></html>";
            test.Source = source;
        }
    }
}
