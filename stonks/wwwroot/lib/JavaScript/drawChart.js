
// Load the Visualization API and the corechart package.
//google.charts.load('current', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.
//google.charts.setOnLoadCallback(function () { drawChart('chart_div', @Html.Raw(Json.Serialize(Model.StockPrices))) });

// Callback that creates and populates a data table,
// instantiates the chart, passes in the data and
// draws it.
function drawChart(chartId, json, selectHandler) {

    var datas = [];

    if (document.getElementById(chartId) != null) {
        var chart = new google.visualization.CandlestickChart(document.getElementById(chartId));
        var errorId = "error";

        function chartSelectHandler() {
            var selectedItem = chart.getSelection()[0];

            if (selectedItem) {
                var aDateTime = new Date(data.getValue(selectedItem.row, 0));

                const offset = aDateTime.getTimezoneOffset();
                aDateTime = new Date(aDateTime.getTime() + (offset * 60 * 1000));
                aDateTime = new Date(aDateTime.getTime() + (offset * 60 * 1000));
                var aDate = aDateTime.toISOString().split('T')[0];

                document.getElementById("date").value = aDate
                document.getElementById("price").value = data.getValue(selectedItem.row, 2)
            }
        }

        // If the user has no stocks in their portfolio, tell them
        if (json == null || json[0] == null) {
            $("#" + chartId).append("<div id='" + errorId + "' class='alert alert-danger mt-3' role='alert'>You have no stocks in your portfolio!</div>")
        }
        else {
            $("#" + chartId).remove("#" + errorId);

            datas.push(['Date', 'Data', 'Data', 'Data', 'Data']);

            for (let i = 0; i < json.length; i++) {
                datas.push([new Date(json[i].date), json[i].low, json[i].open, json[i].close, json[i].high]);
            }

            // Create the data table.
            var data = new google.visualization.arrayToDataTable(datas);

            var options = {
                legend: 'none',
                explorer: { axis: 'horizontal' },
                width: "100%",
                height: "100%",
                chartArea: {
                    left: 50,
                    width: "90%",
                    height: "90%"
                },
                vAxis: {
                    gridlines: { count: 1 }
                },
                hAxis: {
                    gridlines: { color: 'transparent' }
                },
                colors: ['black'],
                bar: { groupWidth: '100%' }, // Remove space between bars.
                candlestick: {
                    fallingColor: { strokeWidth: 0, fill: '#a52714' }, // red
                    risingColor: { strokeWidth: 0, fill: '#0f9d58' }   // green
                }
            };

            if (selectHandler) {
                google.visualization.events.addListener(chart, 'select', chartSelectHandler);
            }

            chart.draw(data, options);
        }

        
    }
    
}